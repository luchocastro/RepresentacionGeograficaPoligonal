using Hexagon.IO;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Services
{    public class GetSplitOptions : ISplitOptions
    {


            private readonly IOptions<SplitOptions> Options;
            public GetSplitOptions(IOptions<SplitOptions> _options)
            {
                Options = _options;
            }



            public SplitOptions Get()
            {
                return Options.Value;
            }
        
    }
}
