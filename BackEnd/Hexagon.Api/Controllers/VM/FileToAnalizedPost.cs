using Hexagon.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hexagon.Api.Controllers.VM
{
    public class FileToAnalizePost
    {
        public List<string> FuncionsNameColumns { get; set; }
        public  string  FuncionName { get; set; }
        public List<string > ColumnsToKeep { get; set; }
        public string DataTypeForColumns { get; set; } 

    }
}
