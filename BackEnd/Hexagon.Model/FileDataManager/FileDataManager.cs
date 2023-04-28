using Hexagon.Model.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Hexagon.Model;
using Hexagon.Model.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Hexagon.AsyncIO;

namespace Hexagon.Model.FileDataManager
{
    public class contextFactoryFile  
    { }
    public class FileDataManager<G, TEntity>:  IDataRepository<G, TEntity> where TEntity : Base
    {
        private readonly IMapper Mapper;
        private readonly IConfiguration  IConfiguration;
        private readonly IFileDataManagerOptions FileDataManagerOptions;
        private readonly string User;
        private PersistEntity<TEntity> persistEntity =null;
        string parentDirectory;
        string _DefaultExtension;
        public string DefaultExtension { get { return _DefaultExtension; } }
        public FileDataManager(IMapper Mapper, IConfiguration Configuration, IFileDataManagerOptions IFileDataManagerOptions )
        {
            this.Mapper = Mapper;
            this.IConfiguration = Configuration;
            this.FileDataManagerOptions = IFileDataManagerOptions;
            persistEntity = new PersistEntity<TEntity>(this.FileDataManagerOptions);
            parentDirectory = persistEntity.ParentDirectory;
            _DefaultExtension = persistEntity.DefaultExtention;

        }
        public  PersistEntity<TEntity> PersistEntity
        {
            get
            {
                if (persistEntity == null)
                    persistEntity = new PersistEntity<TEntity>(this.FileDataManagerOptions);
                parentDirectory = persistEntity.ParentDirectory;
                return persistEntity;
            }
        }
        public string Head { get; set; }
        public bool Open(string Head)
        {
            if (Head != null && Head != "")
                this.Head = Head;
            return true;

        }
        public  string DefaultMaskID { get { 
                return
                    persistEntity.DefaultMaskID ; 
            } }
        public virtual IFileDataManagerOptions IFileDataManagerOptions
        {
            get
            {
                return
                    this.FileDataManagerOptions   ;
            }
        }


        
        public string ParentDirectory()
        {
            return parentDirectory;
        }
        public virtual async Task<G> AddAsync(TEntity entity)
        {
            var ID = GenerateFullID(entity);
            entity.ID = ID;
            entity.Path = Path.Combine(this.ParentDirectory(), entity.ID + DefaultExtension);
            PersistEntity.Entity = entity;

            await Task<TEntity>.Run(() => { entity =  persistEntity.Save().Result; });
            
            return Mapper.Map<G>(entity);
        }

        public virtual G Add(TEntity entity)
        {
            var ID = GenerateFullID(entity);
            entity.ID = ID;
            var FileToWrite = Path.Combine(this.ParentDirectory(), entity.ID + DefaultExtension);
            if (!Directory.Exists(Path.GetDirectoryName  (FileToWrite)))
                Directory.CreateDirectory(Path.GetDirectoryName(FileToWrite));
            entity.Path = FileToWrite;

            var ToWrite = JsonConvert.SerializeObject (entity);
            Write(FileToWrite, ToWrite);
            return Mapper.Map<G>(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<G> FindByConditionDTO(Expression<Func<TEntity, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public virtual string GetID(string Name, string PararentID, Type EntityType)
        {

            string Mask = "";
            FileDataManagerOptions.Get().Settings.TryGetValue("Mask" + EntityType.Name, out Mask);
            return GetID(Name, PararentID, EntityType, Mask);
        }
        private string GetID(string Name, string PararentID, Type EntityType, string Mask)
        { 
          string[] MaskSplited = (Mask != "" ? Mask : this.DefaultMaskID).Split("/");

            var ret = "";
            foreach (var item in MaskSplited)
            {
                var toDo = "";
                switch (item )
                {
                    case "ID":
                        toDo = "";
                        break;
                    case "Type":
                        toDo = EntityType.Name;
                        break;
                    case "ParentID":
                        toDo = PararentID;
                        break;
                    default:

                        break;
                }
                ret = Path.Combine(ret, toDo);
            }
            return Path.Combine(ret, Name);
        }
        public virtual G Get(string id)
        {   
            
            var PathToRead = Path.Combine(this.ParentDirectory(), id + DefaultExtension);
            if (!File.Exists(PathToRead))
                return default(G);
            var Ret = Read(PathToRead);

            return Mapper.Map<G>( Ret);
            

        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public string ClassLocation (G DTOEntity)
        {
            var  ID = GenerateFullID(Mapper.Map<TEntity>(DTOEntity));

            return Path.Combine( ID.Substring(0, ID.LastIndexOf("\\")));

        }
        public virtual IEnumerable<G> GetAllDTO()
        {
            throw new NotImplementedException();
        }
        public TEntity Get (G EntityDTO)
        {
            var id = GenerateFullID(Mapper.Map<TEntity>(EntityDTO));

            var PathToRead = Path.Combine(this.ParentDirectory(), id + DefaultExtension);
            if (!File.Exists(PathToRead))
                return default(TEntity);

            return   Read(PathToRead);

            return Mapper.Map<TEntity>(Get(id));

        }
        public async Task<TEntity> GetAsync(G EntityDTO)
        {
            var id = GenerateFullID(Mapper.Map<TEntity>(EntityDTO));
            TEntity entity = null;
            var PathToRead = Path.Combine(this.ParentDirectory(), id + DefaultExtension);
            if (!File.Exists(PathToRead))
                return default(TEntity);
            
            await Task<TEntity>.Run(() => { entity = new AsyncIO<TEntity>(PathToRead).ReadJsonAsync().Result  ; })  ;

            return entity;

        }
        public string GenerateFullID (TEntity Entity)
        {
            string Mask = "";
            FileDataManagerOptions.Get().Settings.TryGetValue("Mask" + Entity.GetType().Name , out Mask);
            string [] MaskSplited = (Mask != "" ? Mask : this.DefaultMaskID ).Split("/");

            var ret = "";
            foreach (var item in MaskSplited)
            {
                var toDo = "";
                switch (item )
                {
                    case "ID":
                        toDo = Entity.ID;
                        break;
                    case "Type":
                        toDo = Entity.GetType().Name;
                        break;
                    default:
                        Type objtype = Entity.GetType();

                        PropertyInfo prop = objtype.GetProperty(item);
                        if (prop != null)
                        {
                            object list = prop.GetValue(Entity);
                            toDo = list.ToString();
                        }
                        break;
                }
                if(toDo!="")
                ret = Path.Combine(ret, toDo);
            }
                return ret;

        }

        public string GetPath(TEntity Entity )
        {

            return  Path.GetFullPath( Path.Combine(ParentDirectory(), Entity.ID + DefaultExtension));

        }
        public IEnumerable<G> GetColectionFromParent(string ParentID)
        {
             string Mask = "";
            FileDataManagerOptions.Get().Settings.TryGetValue("Mask" + typeof(TEntity).Name, out Mask);
            Mask = (Mask != "" ? Mask : this.DefaultMaskID);
            Mask = Mask.Substring(0, Mask.LastIndexOf("/")); ;


            
            string PathToRead = Path.Combine(ParentDirectory(), GetID("",ParentID, typeof(TEntity ),Mask));
            var ret = new List<G>();
            foreach (var File in Directory.GetFiles(PathToRead, "*"+ DefaultExtension))
            {
                var enti= Read(File);
                this.PersistEntity.Entity = enti;
                
                enti.IdTraslated = true;

                ret.Add(Mapper.Map<G>(enti));
            }
            return ret;

        }
        public void SaveChanges(TEntity entity)
        {
            var ToSave = JsonConvert.SerializeObject(entity);

            Write(GetPath(entity), ToSave);
        }

        public virtual G Update(TEntity dbEntity, TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task UpdateAsync(TEntity dbEntity, TEntity entity)
        {
            throw new NotImplementedException();
        }
        public void Write(string Path, string ToSave)
        {
            using (StreamWriter StreamWriter = new StreamWriter(Path))
            {
                StreamWriter.Write(ToSave);
                StreamWriter.Close();

            }
        }

        public TEntity Read(string Path)
        {

            
            using (StreamReader file = File.OpenText(Path))
            {
                var Tclass = file.ReadToEnd();
                file.Dispose();
                return System.Text.Json.JsonSerializer.Deserialize<TEntity>(Tclass);
                
            }  
        }
        

        public virtual TEntity GetFromFile(string Path)
        {
            throw new NotImplementedException();
        }

        public virtual string  SetToFile(TEntity TEntity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Base> GetChild(TEntity Parent, Type ChildType)
        {
            var ret = new List<Base>();
            var Mask = TypeMaskID(ChildType );
            
            string PathParent = Path.GetFullPath( Path.GetDirectoryName( Path.Combine(ParentDirectory(), Parent.ID + DefaultExtension) ) );
            var dirs = Directory.GetDirectories(PathParent, ChildType.Name);
            foreach (var item in dirs)
            {
                if (Directory.GetFiles(item, "*" + DefaultExtension ).Count() > 0)
                {
                    var Files = Directory.GetFiles(item, "*" + DefaultExtension);
                    foreach (var FilePath in Files)
                    {
                        var ToRead = File.ReadAllText(FilePath);
                        var Child = JsonConvert.DeserializeObject<Base> (ToRead);
                        ret.Add( Child) ;
                    }
                }
            }
            return ret;
        }

        public IConfiguration Configuration { get {return this.IConfiguration; } }
       public IMapper IMapper { get { return this.Mapper; } }
        public string TypeMaskID(Type Type)
        {
            string Mask = "";
            FileDataManagerOptions.Get().Settings.TryGetValue("Mask" + Type.Name, out Mask);
            return Mask;

        }
    }
}
