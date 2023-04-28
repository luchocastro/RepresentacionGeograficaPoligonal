using Hexagon.Model.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.Models
{
    public class AnalizedFile :  Equatable<AnalizedFile>  
    {
        string name;
        string nicname;
        public AnalizedFile()
        {

        }
        public string PathFiles { get; set; }


    }
}

