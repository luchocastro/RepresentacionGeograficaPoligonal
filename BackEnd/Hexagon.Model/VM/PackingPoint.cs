using Hexagon.Model.Helper;
using Hexagon.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hexagon.Model
{
    public  class Packing<TEntity>  where TEntity :  Package 
    {

            public Point Point { get; private set; }
            public TEntity Entity { get; private set; }

            public Packing(TEntity TEntity)
            {
            this.Entity = TEntity;
            
            }
            public TEntity Get (string PointFromString)
            {
                var  Num = JsonSerializer.Deserialize<TEntity>(PointFromString);
                return Num;

            }
            public string Serialized(TEntity TEntity)
            {
                return JsonSerializer.Serialize(TEntity);
            }
            public override string ToString()
            {
                return base.ToString();
            }
        }
        
}
