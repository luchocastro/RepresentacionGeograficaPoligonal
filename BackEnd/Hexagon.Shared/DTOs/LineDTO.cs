using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Shared.DTOs
{
    public struct LineDTO
    {

        public LineDTO(long Number,  string[] Fields)
        {
            this.Number = Number;
            this.Fieds = Fields ;
        }
        public string[] Fieds { get; set; }
        public long Number { get; set; }

    }
}
