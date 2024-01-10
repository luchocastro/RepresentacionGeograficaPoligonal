using Hexagon.Model.Models;
using System.Collections;
using System.Text.Json;
using Hexagon.IO;
namespace Hexagon.Model
{

        public class Coordinate : IComparer, System.IComparable, IPackable
        {
            public Coordinate(float col, float row)
            {
                Row = row;
                Col = col;
            }

            public float Row { get; set; }
            public float Col { get; set; }

        public int Compare(object x, object y)
            {
                var xCoor = (Coordinate)x;
                var yCoor = (Coordinate)y;
                return xCoor.Row > yCoor.Row ? 1 :
                    xCoor.Row < yCoor.Row ? -1 :
                        xCoor.Col > yCoor.Col ? 1 :
                            xCoor.Col < yCoor.Col ? -1 : 0;



            }

            public int CompareTo(object obj)
            {

                return this.Compare(this, obj);
            }

            public object FromString(string ObjectPackaged)
            {
                var RET = JsonSerializer.Deserialize<int[]>(ObjectPackaged);

                Row = RET[0];
                Col = RET[1];
                return this;
            }

            public string ObjectToString()
            {
                return JsonSerializer.Serialize(new float  [] { Row, Col });
            }
        
    }
}
