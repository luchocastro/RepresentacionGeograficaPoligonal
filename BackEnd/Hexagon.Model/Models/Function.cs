using Hexagon.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model
{
    public class Function : Base
    {
        public Function() { }
        
        public Function(string PathFunctionDLL, string FullClassName, string FunctionName, Dictionary<string, string> Types)
        {
            this.PathFunctionDLL = PathFunctionDLL;
            this.FullClassName = FullClassName;
            this.FunctionName = FunctionName;
            this.Types = Types;

        }
        public string PathFunctionDLL { get; set; }
        public string FullClassName { get; set; }
        public string FunctionName { get; set; }
        public Dictionary<string, string> Types { get; set; }

    }
}



