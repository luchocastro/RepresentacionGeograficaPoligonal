using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Shared.DTOs
{
    public struct LineDTO
    {

        public LineDTO(UInt64 Number,  string[] Fields)
        {
            this.Number = Number;
            this.Fieds = Fields ;
        }
        public string[] Fieds { get; set; }
        public UInt64 Number { get; set; }

    }
}
