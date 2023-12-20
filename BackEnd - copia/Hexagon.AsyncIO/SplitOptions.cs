using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.IO 
{
    public class SplitOptions 
    {


        public long MaxFile { get; set; }
        public string MaskSplitFile { get; set; }
        public string NumberIdentifier { get; set; }
        public string TempFolder { get; set; }
        public string SplitFolder { get; set; }
        public string SortedFolder { get; set; }
        public string SplitSortedExtension { get; set; }
        public string SplitExtension { get; set; }
        public string OrderedExtension { get; set; }


    }
}
