using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Services 
{
    public class GetFileServiceOptions : IFileServiceOptions
    {


            private readonly IOptions<FileServiceOptions> Options;
            public GetFileServiceOptions(IOptions<FileServiceOptions> _options)
            {
                Options = _options;
            }



            public FileServiceOptions Get()
            {
                return Options.Value;
            }
        
    }
}
