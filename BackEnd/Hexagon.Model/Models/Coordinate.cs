namespace Hexagon.Model
{
    public struct Coordinate
        {
            public Coordinate(float col, float row )
            {
                Row = row;
                Col = col;
            }

            public float Row { get; }
            public float Col { get; }
            
            public override string ToString() => $"({Col}, {Row} )";
        }
    }
    
