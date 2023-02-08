using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.FileDataManager
{
    public interface IForFile<Entity>
    {
        public Entity GetFromFile(string Path);
        public string SetToFile(Entity Entity);
    }
}
