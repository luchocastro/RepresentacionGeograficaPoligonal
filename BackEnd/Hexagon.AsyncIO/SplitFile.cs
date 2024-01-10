
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hexagon.IO
{
    public class SplitFile 
    {
        FileStream ActualWriter = null;
        StreamReader ActualReader = null;
        bool Flush = true;
        bool FirstWrite = false;
        string ActualFile = null;
        string[] ActualReading = null;
        SplitOptions  SplitOptions;
        int PartNum=0;
        int FileNum =0;
        List <GenericPackage> Chunked = null;
        string _ParentDirectory;
        string _FileNameWithoutExtension;
        ChunkWriter ChunkWriter = null;
        public SplitFile(ISplitOptions ISplitOptions, string ParentDirectory ="", string FileNameWithoutExtension="")
        {
            this.SplitOptions = ISplitOptions.Get();
            Mask = SplitOptions.MaskSplitFile;
            _ParentDirectory = ParentDirectory;
            _FileNameWithoutExtension = FileNameWithoutExtension;

           var ChunkFolder = Path.Combine(_ParentDirectory, this.SplitOptions.SplitFolder); 
             }
        ~SplitFile()
        {
            if(ActualWriter!=null )
            {
                if (Flush )
                    ActualWriter.Flush();
                ActualWriter.Dispose();
            }
        }
        public string ParentDirectory { get { return _ParentDirectory; } set { _ParentDirectory = value; } }
        public string FileNameWithoutExtension { get { return _FileNameWithoutExtension; } set { _FileNameWithoutExtension = value; } }
        string Mask = ""; 



        public void DeleteChunk(String Folder, string Extension ="*")
        {


            if (Directory.Exists(Folder))
            {
                var di = new DirectoryInfo(Folder).GetFiles("*." + Extension);
                if (Directory.Exists(Folder))
                {
                    foreach (var item in di)
                    {
                        item.Delete();
                    }
                }
            }

        } 
            public bool WriteChunk(string Line )
        {
            String Folder, FileName,  Extension ;
            Folder = Path.Combine(_ParentDirectory, this.SplitOptions.SplitFolder);
            FileName = this.FileNameWithoutExtension  ;
            Extension = SplitOptions.SplitExtension;
            return WriteChunk(Folder, FileName, Line, Extension);
        }
        public bool WriteChunk(String Folder, string FileName, string Line , String Extension="" )
        {

            string FilePartName = "";
            bool NewFile = false;
            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);
            if (FirstWrite)
            { 
                DeleteChunk(Folder,Extension );
                FirstWrite = false;
            }
            if (PartNum > SplitOptions.MaxFile)
            {
                ActualWriter.Dispose();
                NewFile = true;
                PartNum = 0;

            }
            if (ActualFile == null || ActualFile != Path.Combine(Folder, FileName))
            {

                NewFile = true;


                if (ActualWriter != null)
                {
                    ActualWriter.Dispose();
                }

                FileNum = 0;

                ActualFile = Path.Combine(Folder, FileName);
            }
            if (NewFile )
            {
                PartNum = 0;
                FileNum ++;
                FilePartName = String.Format(Mask + Extension,
        FileName, FileNum.ToString(), SplitOptions.MaxFile.ToString () );

                FilePartName = Path.Combine(Folder, FilePartName);
                ActualWriter =  File.OpenWrite(FilePartName);
                
            }
            PartNum+= ASCIIEncoding.UTF8.GetByteCount(Line);  
           SplitFile.Write( ActualWriter,Line);

            return true;
        }
        List<string[]> FilesChunk = null;
        public void GetChunkReader()
        {
            string PathFile = Path.Combine(this.ParentDirectory, this.FileNameWithoutExtension);
            string[] paths = Directory.GetFiles(PathFile );
            int chunks = paths.Length; // Number of chunks  
            FilesChunk = paths.Select(x => new string[] {   x .Substring(0, x.IndexOf(".")), x }).ToList();
            ActualReading = FilesChunk.Where(x => x[0] == "1").First();
            ActualReader = new StreamReader(ActualReading[1]);

        }

        public void GetChunkReader (string PathFile, string OriginalFileName, string Extension = "*")
        {
            string[] paths = Directory.GetFiles(PathFile, "*." + Extension);
            int chunks = paths.Length; // Number of chunks  
            FilesChunk = paths.Select(x => new string[] { x.Replace(PathFile, "").Replace(OriginalFileName, "").Replace(@"\", "").Replace(".part_", ""), x }).Select(x => new string[] { x[0].Substring(0, x[0].IndexOf(".")), x[1] }).ToList();
            ActualReading = FilesChunk.Where(x => x[0] == "1") .First();
            ActualReader = new StreamReader(ActualReading[1]);

        }
        public bool ReadFromChunk(ref string Readed)
        {


            Readed = null;
            if (ActualReader != null)
            {
                Readed = ActualReader.ReadLine();
                if (Readed == null)
                {
                    ActualReader = null;

                    if (ActualReading[0] == FilesChunk.Count().ToString())
                    {
                        return false;
                    }
                    else
                    {
                        ActualReading = FilesChunk.Where(x => x[0] == (int.Parse (ActualReading[0])+1).ToString()).First();

                        ActualReader = new StreamReader(ActualReading[1]);

                        Readed = ActualReader.ReadLine(); 

                    }
                }
            }
            return false;
        }

                    public void Dispose()
        {
            throw new NotImplementedException();
        }

            public static void Write(FileStream fs, string value)
            {
                byte[] info = new UTF8Encoding(true).GetBytes(value + "\r\n");
                fs.Write(info, 0, info.Length);
            }

        public static void Write(StreamWriter sw, string value)
        {
            sw.WriteLine (value );
        }

        
    }
    
}