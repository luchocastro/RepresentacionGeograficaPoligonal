using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hexagon.IO
{
    public class OrderByChunk
    {

            SplitOptions SplitOptions;
        string Extension, PathFile, FileNameWithoutExtension;
        public OrderByChunk(SplitOptions SplitOptions, string PathFile, string FileNameWithoutExtension, string Extension = "*")
        {

            this.SplitOptions = SplitOptions;
            this.Extension = Extension;
            this.FileNameWithoutExtension = FileNameWithoutExtension;
            this.PathFile = PathFile;
        }
        public bool SortTheChunks(Type TypeToSort)
        {
            string TempFolder, ChunkFolder;
            TempFolder = Path.Combine(PathFile, this.SplitOptions.TempFolder);
            ChunkFolder = Path.Combine(PathFile, this.SplitOptions.SplitFolder);
            return SortTheChunks(TempFolder, ChunkFolder, TypeToSort);
        }
        public bool SortTheChunks(string TempFolder, string ChunkFolder, Type TypeToSort)
        {
            string FilterExpresion = "*" + SplitOptions.SplitExtension;
            if (!Directory.Exists(TempFolder))
                Directory.CreateDirectory(TempFolder);
            var di = new DirectoryInfo(TempFolder).GetFiles("*" + SplitOptions.SplitSortedExtension);
            foreach (var item in di)
            {
                item.Delete();
            }
            foreach (string path in Directory.GetFiles(ChunkFolder, FilterExpresion))
            {
                var Fieldlist = new List<GenericPackage>();
                using var read = new StreamReader(path);
                while (true)
                {

                    var Pac = new GenericPackage(TypeToSort);
                    var line = read.ReadLine();
                    if (line == null)
                    {
                        read.Close();
                        read.Dispose();
                        break;
                    }
                    var lineDes = Pac.FromString(line);
                    Pac.Value = lineDes;
                    Fieldlist.Add(Pac);
                }
                // Read all lines into an array

                Fieldlist.Sort();

                // Create the 'sorted' filename
                string newpath = Path.Combine(TempFolder, Path.GetFileName(path)) + SplitOptions.SplitSortedExtension;
                // Write it
                File.WriteAllLines(newpath, Fieldlist.Select(x => x.ObjectToString()));
                // Delete the unsorted chunk
                // Free the in-memory sorted array

                GC.Collect();
            }
            return true;
        }
        public void Order(Type TypeOnFile)
        {
            string TempFolder, PathToWrite;
            TempFolder = Path.Combine(PathFile, this.SplitOptions.TempFolder);
            PathToWrite = Path.Combine(PathFile, this.SplitOptions.SortedFolder);

            SortTheChunks(TypeOnFile);
            Order(TempFolder,
             PathToWrite, this.FileNameWithoutExtension, TypeOnFile);
        }

        public void Order(string TempFolder,
            string PathToWrite, string FileNameToWrite, Type TypeOnFile)
        {

            string Filter = "*" + SplitOptions.SplitSortedExtension;
            if (!Directory.Exists(PathToWrite))
                Directory.CreateDirectory(PathToWrite);
            var Writer = new ChunkWriter(SplitOptions, PathToWrite, FileNameToWrite, SplitOptions.SplitSortedExtension, false);


            string[] paths = Directory.GetFiles(TempFolder, Filter);
            int chunks = paths.Length; // Number of chunks  

            // Open the files
            StreamReader[] readers = new StreamReader[chunks];
            bool[] Ended = new bool[chunks];
            var Lines = new string[chunks];
            var Values = new GenericPackage[chunks];
            var TotLineas = 0;
            var LinesXFiles = 20000;
            var part = 1;

            string FilePartName = String.Format(SplitOptions.MaskSplitFile +SplitOptions.OrderedExtension,
        FileNameToWrite, part.ToString(), LinesXFiles.ToString());
            FilePartName = Path.Combine(PathToWrite, FilePartName);

            ;
            for (int i = 0; i < chunks; i++)
            {
                readers[i] = new StreamReader(paths[i]);
                Lines[i] = readers[i].ReadLine();
                if (Lines[i] == null)
                {
                    Ended[i] = true;
                    readers[i].Close();
                    readers[i].Dispose();
                }
                else
                {
                    Values[i] = new GenericPackage(TypeOnFile);
                    var val = Values[i].FromString(Lines[i]);
                    Values[i].Value = val;
                }
            }
            GenericPackage ValueToCompare = null;
            bool first = true;
            var ToWrite = 0;


            //using var Wri = new StreamWriter(column.PathFields, true);
            while (true)
            {

                bool End = true;
                for (int i = 0; i < chunks; i++)
                {
                    if (Ended[i])
                        continue;
                    if (Values[i] == null)
                        continue;
                    if (ValueToCompare == null && first)
                    {
                        ValueToCompare = Values[i];
                        ToWrite = 0;
                        first = false;
                        continue;

                    }
                    int ord = Values[i].CompareTo(Values[ToWrite]);
                    if (ord < 1)
                    {
                        ToWrite = i;
                    }


                    End = false;
                }
                if (End)
                {

                    break;
                }

                Writer.WriteChunk(Values[ToWrite].ObjectToString());


                Lines[ToWrite] = readers[ToWrite].ReadLine();

                if (Lines[ToWrite] == null)
                {

                    Ended[ToWrite] = true;
                    readers[ToWrite].Close();
                    readers[ToWrite].Dispose();
                    Values[ToWrite] = null;
                    for (int i = 0; i < Values.Length; i++)
                    {
                        if (Values[i] != null)
                        {
                            ValueToCompare = Values[i];
                            ToWrite = i;
                            break;
                        }
                    }
                }
                else
                {
                    Values[ToWrite] = new GenericPackage(TypeOnFile);
                    var val = Values[ToWrite].FromString(Lines[ToWrite]);
                    Values[ToWrite].Value = val;
                    ValueToCompare = Values[ToWrite];
                }

            }
            Writer.CloseChunkWriter();

        }
    }
}
