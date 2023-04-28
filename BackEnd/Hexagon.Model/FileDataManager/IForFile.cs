using Hexagon.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.FileDataManager
{
    public interface IPersistEntity<TEntity> where TEntity : Base
    {
        
        public TEntity GetFromFile();
        public TEntity SetToFile();
        public TEntity Entity { get; set; }

        public string DefaultExtention { get;  }
        public string ParentDirectory { get;  }
        public string DictionaryParentDirectory { get;  }
        public string PathToSave();

    }

}

