using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hexagon.IO
{
    class ChunkReader
    {
        string ActualFile = null;
        string[] ActualReading = null;

        StreamReader ActualReader = null;
        List<string[]> FilesChunk = null;
        SplitOptions SplitOptions;
        string Extension, PathFile, FileNameWithoutExtension;
        public ChunkReader(SplitOptions SplitOptions, string PathFile, string FileNameWithoutExtension, string Extension = "*")
        {
            string[] paths = Directory.GetFiles(PathFile, "*." + Extension);
            int chunks = paths.Length; // Number of chunks  
            FilesChunk = FilesOrdered();
            ActualReading = FilesChunk.Where(x => x[0] == "1").First();
            ActualReader = new StreamReader(ActualReading[1]);
            this.SplitOptions = SplitOptions;
            this.Extension = Extension;
            this.FileNameWithoutExtension = FileNameWithoutExtension;
            this.PathFile = PathFile;
        }
        public List<string[]> FilesOrdered()
        {
            var MaskFile = String.Format(SplitOptions.MaskSplitFile + Extension,
                    FileNameWithoutExtension, "&Num&", SplitOptions.MaxFile.ToString());
            var NumInit = MaskFile.IndexOf("&Num&");
            var InitStrig = NumInit > 0 ? MaskFile.Substring(0, NumInit) : "#";
            var FinalString = MaskFile.Substring(NumInit + 5);
            string FilterExpresion = String.Format(SplitOptions.MaskSplitFile + Extension,
                    FileNameWithoutExtension, "*", "*");

            List<string[]> Names = new List<string[]>();

            if (Directory.Exists(PathFile))
            {
                Names = new DirectoryInfo(PathFile).GetFiles(FilterExpresion).Select(x => new string[] { x.FullName, x.Name.Replace(InitStrig, "").Replace(FinalString, "") }).OrderBy(x => int.Parse(x[1])).ToList();

            }

            return Names;
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
                        ActualReading = FilesChunk.Where(x => x[0] == (int.Parse(ActualReading[0]) + 1).ToString()).First();

                        ActualReader = new StreamReader(ActualReading[1]);

                        Readed = ActualReader.ReadLine();

                    }
                }
            }
            return false;

        }
    }
}
