using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model
{
    public class Function
    {
        public Function() { }
        
            public Function(string Path, string FullClassName, string FunctionName, Dictionary<string, string> Types)
        {
            this.Path = Path;
            this.FullClassName = FullClassName;
            this.FunctionName = FunctionName;
            this.Types = Types;

        }
        public string Path { get; }
        public string FullClassName { get; }
        public string FunctionName { get; }
        public Dictionary<string, string> Types { get; }

    }
}



