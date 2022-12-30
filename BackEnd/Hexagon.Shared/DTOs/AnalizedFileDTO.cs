using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Shared.DTOs
{
    public struct AnalizedFileDTO
    {
        public List<List<string>> Lines { get; set; }
        public List<ColumnDTO> Columns { get; set; }
        public AnalizedFileDTO(List<List<string>> Lines, List<ColumnDTO> Columns)
        {
            this.Columns = Columns;
            this.Lines = Lines;
        }

    }
}
