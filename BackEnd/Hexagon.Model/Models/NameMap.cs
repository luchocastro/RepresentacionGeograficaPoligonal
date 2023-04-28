using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
namespace Hexagon.Model.Models
{
    public class NameMap  
    {
        private Base Entity;
        public NameMap(Base Entity)
        {
            this.Entity = Entity;
            OriginalName = Entity.ID;
        }
        public NameMap( )
        {
            }
        public string OriginalName { get ; set ;  } 
        public string FileName { get; set; }
        public override string ToString() => JsonSerializer.Serialize<NameMap>(this);

        public bool InDictionay { get; set; } = false;
    }
}
