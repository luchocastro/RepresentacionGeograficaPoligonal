using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Shared.DTOs
{
    public class FunctionDTO
    {
        public FunctionDTO( )
        {
        }
        public FunctionDTO(string Path, string FullClassName, string FunctionName, Dictionary<string, string> Types)
        {
            this.Path = Path;
            this.FullClassName = FullClassName;
            this.FunctionName = FunctionName;
            this.Types = Types;

        }
        public string Path { get; set; }
        public string FullClassName { get; set; }
        public string FunctionName { get; set; }
        public Dictionary<string, string> Types { get; set; }

    }
}
