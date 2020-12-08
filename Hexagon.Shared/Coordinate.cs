namespace Hexagon.Shared
{
    public struct Coordinate
        {
            public Coordinate(decimal col, decimal row )
            {
                Row = row;
                Col = col;
            }

            public decimal Row { get; }
            public decimal Col { get; }
            
            public override string ToString() => $"({Col}, {Row} )";
        }
    }
    
