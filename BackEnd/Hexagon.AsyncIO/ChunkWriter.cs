using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hexagon.IO
{
    public  class ChunkWriter
    {
        bool NewFile = false;
        FileStream ActualWriter = null;
        StreamReader ActualReader = null;
        bool Flush = false ;
        bool FirstWrite = false;
        string ActualParentFile = null;
        string ActualFile = null;
        string[] ActualReading = null;
        SplitOptions SplitOptions;
        long  ActualFileLen = 0;
        int FileNum = 0;
        string _ParentDirectory;
        string _FileNameWithoutExtension;
        string Mask;
        long MaskSize;
        string Extension;
        string FilterExpresion;
        List<string[]> FileOrdered = new List<string[]>();
        bool Append = true;
         public ChunkWriter(
            SplitOptions SplitOptions, string ParentDirectory, string FileNameWithoutExtension, string Extension, bool Append )
        {

            this.SplitOptions = SplitOptions;
            _ParentDirectory = ParentDirectory;
            _FileNameWithoutExtension = FileNameWithoutExtension;
            Mask = SplitOptions.MaskSplitFile;
            MaskSize = SplitOptions.MaxFile;
            this.Extension = Extension;
             FilterExpresion = String.Format(Mask + Extension,
                    _FileNameWithoutExtension, "*", "*");
            ActualFile = String.Format(Mask + Extension,
                   _FileNameWithoutExtension, FileNum.ToString(), SplitOptions.MaxFile.ToString ());

            ActualParentFile = Path.Combine(_ParentDirectory, _FileNameWithoutExtension);
            Prepare(Append);

        }
        public void DeleteFilesForlder()
        {
            if (Directory.Exists(_ParentDirectory))
            {
                var di = new DirectoryInfo(_ParentDirectory).GetFiles(FilterExpresion);
                foreach (var item in di)
                {
                    item.Delete();
                }
            }
        }
        public void Prepare(bool Append)
        {
            if (!Directory.Exists(_ParentDirectory))
                Directory.CreateDirectory(_ParentDirectory);
            if (!Append)
            {
                NewFile = true;
                DeleteFilesForlder();
            }
            else
            {
                if (FilesOrdered().Count > 0)
                {
                    var file = new FileInfo(FilesOrdered().Last()[0]);

                    ActualFile = file.FullName;
                    FileNum = int.Parse(FilesOrdered().Last()[1]);
                    ActualFileLen = file.Length;

                }
            }

        } 

        
        public List<string[]> FilesOrdered()
        {
            var MaskFile = String.Format(Mask + Extension,
                    _FileNameWithoutExtension, "&Num&", SplitOptions.MaxFile.ToString ());
            var NumInit = MaskFile.IndexOf("&Num&");
            var InitStrig = NumInit > 0 ? MaskFile.Substring(0, NumInit) : "#";
            var FinalString = MaskFile.Substring(NumInit + 5);
            
            List<string[]> Names = new List<string[]> ();
     
            if (Directory.Exists(_ParentDirectory))
            {
                Names = new DirectoryInfo(_ParentDirectory).GetFiles(FilterExpresion).Select(x => new string[] { x.FullName, x.Name.Replace (InitStrig,"").Replace(FinalString,"")}).OrderBy(x=>int.Parse(x[1])).ToList();

            }
            return Names;
        }
        public void CloseChunkWriter()
        {
            if (ActualWriter != null)
            {
                if(Flush )
                { 
                ActualWriter.Flush();
                    Flush = false;
                }
                ActualWriter.Dispose();

            } 
        }

        public bool WriteChunk( string Line )
        {
            try
            {

                string FilePartName = "";
                

                if (ActualFileLen + ASCIIEncoding.UTF8.GetByteCount(Line) > SplitOptions.MaxFile)
                {
                    if (ActualWriter != null)
                    {
                        ActualWriter.Flush();
                        ActualWriter.Dispose();
                    }
                    NewFile = true;

                }
                if (NewFile)
                {
                    ActualFileLen = 0;
                    FileNum++;
                    ActualFile = String.Format(Mask + Extension,
                _FileNameWithoutExtension, FileNum.ToString(), SplitOptions.MaxFile.ToString());

                    ActualFile = Path.Combine(_ParentDirectory, ActualFile );
                    ActualWriter = File.OpenWrite(ActualFile);
                    NewFile = false;
                }
                ActualFileLen += ASCIIEncoding.UTF8.GetByteCount(Line);
                ChunkWriter.Write(ActualWriter, Line);
                Flush = true;
                return true;

            }
            catch
            {
                return false ;
            }
            finally
            {
            }
        }
        public static void Write(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value + "\r\n");
            fs.Write(info, 0, info.Length);
        }

        public static void Write(StreamWriter sw, string value)
        {
            sw.WriteLine(value);
        }
        ~ChunkWriter ()
        {
            if (ActualWriter != null)
            {
                if (Flush)
                    ActualWriter.Flush();
                ActualWriter.Dispose();
            }
        }
    }

}
