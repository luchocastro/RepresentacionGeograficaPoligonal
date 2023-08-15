using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.FileDataManager
{
    public class FileDataManagerOption : IFileDataManagerOptions
    {

        private readonly IOptions<FileDataManagerOptions> Options;
        public FileDataManagerOption(IOptions<FileDataManagerOptions> _options)
        {
            Options = _options;
        }


    
        public FileDataManagerOptions Get()
        {
            return Options.Value;
         }
    }
}
