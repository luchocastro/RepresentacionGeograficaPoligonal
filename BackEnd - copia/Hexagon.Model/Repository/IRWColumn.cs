using Hexagon.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Hexagon.Model.Repository
{
    public interface IRWData
    {

        public StreamReader Reader { get; }
        public StreamWriter Writer { get; }
        public Field GetNext { get; }
        public void Add(Field Field); 
     public bool EOF { get; }


    }
}
