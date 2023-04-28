using Hexagon.AsyncIO;
using Hexagon.Model.Models;
using Hexagon.Model.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hexagon.Model.FileDataManager
{
    public class PersistEntity<TEntity> where TEntity : Base
    {
        
        
        private readonly string NameDictionary;
        private readonly IFileDataManagerOptions FileDataManagerOptions;

        private  string defaultExtention;
        private readonly string parentDirectory;
        private readonly string ParentDictionaryClass;
        private readonly string DictionaryDirectoryName;

        private TEntity entity;
        private string defaultMaskID;
        private string dictionaryDirectory;
        public PersistEntity(IFileDataManagerOptions FileDataManagerOptions) 
        {
            this.FileDataManagerOptions = FileDataManagerOptions;
            var opt = this.FileDataManagerOptions.Get() ;
            DefaultExtention = opt.DefaultExtension;
            ParentDictionaryClass = opt.ParentDictionaryClass;
            defaultMaskID = opt.DefaultMask;
            this.NameDictionary = "MapFiles" + this.DefaultExtention;
            this.DictionaryDirectoryName = "Dictionary";
            parentDirectory = Path.Combine(Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)).FullName, AppDomain.CurrentDomain.FriendlyName);
        }
        private string ParentDictionary ()
        {
            if (Entity.ID == null || Entity.ID == "")
                return "";
            var ParentPos = Entity.ID.IndexOf(ParentDictionaryClass);
            if (ParentPos < 0)
                return "";
            var ParentPosName = Entity.ID.IndexOf(@"\", ParentPos +ParentDictionaryClass.Length+1);
            if (ParentPosName<0)
            {
                Entity.IdTraslated = false;
                return "";
            }
            return  Entity.ID.Substring(0,ParentPosName)  ;

        }
        public TEntity  Entity { get { return entity; }
            set { 
                entity = value;
                var _ParentDictionary = ParentDictionary();
                DictionaryDirectory = Path.Combine(ParentDirectory, _ParentDictionary,DictionaryDirectoryName);
            }
        }
        public string DefaultExtention
        {
            get { return defaultExtention; }
            private set { defaultExtention = value; }

        }
        public string DefaultMaskID
        {
            get { return defaultMaskID; }
            private set { defaultMaskID = value; }

        }

        public string ParentDirectory
        {
            get
            {
                return parentDirectory ;
                ;
            }
        }
        public string DictionaryDirectory
        {
            get
            {

                return dictionaryDirectory;

            }
            private set { dictionaryDirectory = value; }
        }
        public string PathToSave
        {
            get
            {
                return Path.Combine(DictionaryDirectory, typeof(TEntity).Name);
            }

        }
        private string DictionaryFile
        {

            get
            {
                return Path.Combine(DictionaryDirectory, NameDictionary);
            }
        }
        private string Compress(string ToCompress)
        {
            return CompressHelper.Compress(ToCompress);
        }
        private string Decompress(string ToDecompress)
        {
            return CompressHelper.Decompress(ToDecompress);
        }
        public NameMap  Get ()
        {
             if (!Entity.IdTraslated|| DictionaryFile == "")
            {
                var NotMap = new NameMap (Entity);

               NotMap.FileName= Entity.Path ;
                return NotMap;
            }
            if (File.Exists(DictionaryFile))
            {
                using var StreamReader = new StreamReader(DictionaryFile);
                var line = "";
                try
                {
                    while ((line = StreamReader.ReadLine()) != null)
                    {
                        if (line != "")
                        {
                            var NameMapInFile = JsonSerializer.Deserialize<NameMap>(line);
                            if (NameMapInFile.OriginalName == Entity.ID)
                            {
                                NameMapInFile.InDictionay = true;
                                return NameMapInFile;

                            }
                        }

                    }

                }
                finally
                {
                    StreamReader.Dispose();
                }
            }
            NameMap NameMap = new NameMap(Entity); ;

            NameMap.FileName = Path.Combine(PathToSave, Guid.NewGuid().ToString() + DefaultExtention);
            NameMap.InDictionay = false;
            Entity.Path = NameMap.FileName;
            return NameMap;

        }
        public NameMap  NameMapSereialized
        {
            get {

                return Get();
                }

        }
        public async Task< TEntity> Save ()
        {
            
            
            var Name = NameMapSereialized;
            if (Name.FileName == "")
                throw new ArgumentNullException("FileName");
            if (Entity.IdTraslated && !Name.InDictionay )
            {
                if (!Directory.Exists(DictionaryDirectory))
                    Directory.CreateDirectory(DictionaryDirectory);

                using (var StreamWriter = new StreamWriter(DictionaryFile, true, System.Text.Encoding.UTF8))
                {
                    Name.InDictionay = true;
                    var ToSave =  Name.ToString()+ "\n";

                    try
                    {
                        StreamWriter.Write(ToSave);
                    }
                    finally
                    {
                        StreamWriter.Dispose();
                    }
                    Entity.Path = Name.FileName;
                    

                }
            }

                if (!Directory.Exists(Path.GetDirectoryName (Entity.Path)))
                    Directory.CreateDirectory(Path.GetDirectoryName(Entity.Path));
            using (var StreamWriter = new StreamWriter(Entity.Path, false, System.Text.Encoding.UTF8))
            {
                var EntitySaved = Entity;
                var ListBlob = new List<PropertyInfo>();
                foreach(var prop in Entity.GetType().GetProperties())
                {
                    if (ModelSaveAtributes.ProperIsBlobs(prop))
                    { 
                        prop.SetValue (Entity, null);
                        ListBlob.Add(prop);
                    }
                }
                var optjson = new JsonSerializerOptions(JsonSerializerDefaults.Web) ;
                optjson.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                var ToSave = JsonSerializer.Serialize< TEntity>(this.Entity,optjson);

                try
                {
                    StreamWriter.WriteLine(ToSave);
                 }
                finally
                {
                    StreamWriter.Dispose();
                }

                foreach(var prop in ListBlob)
                {   
                    
                    var FileToWrite = Path.Combine(Path.GetDirectoryName(Entity.Path), prop.Name+ Path.GetFileName(Entity.Path));
                    Type T = prop.PropertyType;
                    var Value = prop.GetValue(EntitySaved);



                    var AsyncIO = new AsyncIO<Object>(FileToWrite);
                    await AsyncIO.WriteJsonAsync(Value);
                }
            }
            return Entity;
        }
        public  TEntity SaveAll()
        {


            var Name = NameMapSereialized;
            if (Name.FileName == "")
                throw new ArgumentNullException("FileName");
            if (Entity.IdTraslated && !Name.InDictionay)
            {
                if (!Directory.Exists(DictionaryDirectory))
                    Directory.CreateDirectory(DictionaryDirectory);

                using (var StreamWriter = new StreamWriter(DictionaryFile, true, System.Text.Encoding.UTF8))
                {
                    Name.InDictionay = true;
                    var ToSave = Name.ToString() + "\n";

                    try
                    {
                        StreamWriter.Write(ToSave);
                    }
                    finally
                    {
                        StreamWriter.Dispose();
                    }
                    Entity.Path = Name.FileName;


                }
            }

            if (!Directory.Exists(Path.GetDirectoryName(Entity.Path)))
                Directory.CreateDirectory(Path.GetDirectoryName(Entity.Path));
            using (var StreamWriter = new StreamWriter(Entity.Path, false, System.Text.Encoding.UTF8))
            {
                var EntitySaved = Entity;
                            var optjson = new JsonSerializerOptions(JsonSerializerDefaults.Web);
                optjson.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                var ToSave = JsonSerializer.Serialize<TEntity>(this.Entity, optjson);

                try
                {
                    StreamWriter.Write (ToSave);
                }
                finally
                {
                    StreamWriter.Dispose();
                }



            }
            return Entity;
        }
        public TEntity SetToFile()
        {
            Save();
            return this.Entity;
        }
        public TEntity GetFromFile()
        {
            var ToSave = Get();
            if (!File.Exists(ToSave.FileName))
                return default;

            using var StreamReader = new StreamReader(ToSave.FileName);
            var line = "";
            try
            {
                line = StreamReader.ReadToEnd(); 
                return JsonSerializer.Deserialize<TEntity>(line);
            }
            finally
            {
                StreamReader.Dispose();
            }
        }
    }

}
