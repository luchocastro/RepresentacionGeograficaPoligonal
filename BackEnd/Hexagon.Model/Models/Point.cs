using Hexagon.Model.Helper;
using Hexagon.Model.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hexagon.Model
{
    public class Point  : IPackable, IComparable<Point>, IComparer<Point> 
    {

        private float _X, _Y;
        private long _Row =-1;
        public Point()  
        {

        }
        
        public Point(float x, float y) 
        {
            _X = x;
            _Y = y;
        }
        public Point(double x, double y) 
        {
            _X = (float)x;
            _Y = (float)y;
        }
        public float X { get { return _X; }  set { _X = value; } }

        public long Row { get { return _Row; } 
            set { _Row = value; } }
        public float Y { get { return _Y; }  set { _Y = value; } }
        


        public object FromString(string Point)
        {
            if (Point == null || Point == "")
                return null;
            var Parsed = JsonSerializer.Deserialize<float []>(Point);
            
            X = Parsed[0] ;
            Y = Parsed[1];
            if (Parsed.Length==3)
                Row =  (long)(Parsed[2]);
            return this;

        }
        public override string ToString()
        {
            return ObjectToString();
        }
        public string ObjectToString()
        {

            return JsonSerializer.Serialize(new float[] { this.X, this.Y, this.Row });
        }

        public Point(string x, string y)
        {
            X = Utils.FloatFromString(x);
            Y = Utils.FloatFromString(y);
        }
        [JsonIgnore]
        [ModelSaveAtributes(InPackage = true, PropertyOrder = 0)]
        public string XText { get { return Utils.FloatFromFloat(this.X); } set { this.X = Utils.FloatFromString(value); } }
        [JsonIgnore]
        [ModelSaveAtributes(InPackage = true, PropertyOrder = 1)]
        public string YText { get { return Utils.FloatFromFloat(this.Y); } set { this.Y = Utils.FloatFromString(value); } }

        public string PointText
        {
            get
            { return Utils.FloatFromFloat(this.X) + ":" + Utils.FloatFromFloat(this.Y); }
            set { this.X = Utils.FloatFromString(value.Split(":")[0]); this.Y = Utils.FloatFromString(value.Split(":")[1]); }
        }

        public int Compare(object x, object y)
        {
            var xCoor = (Point )x;
            var yCoor = (Point)y;
            return new Point().Compare(xCoor, yCoor );


        }

        public int CompareTo(object obj)
        {

            return this.Compare(this, obj);
        }

        public int CompareTo([AllowNull] Point other)
        {
            if (this == null)
                return -1;
            else
                return new Point().Compare(this, other);
        }

        public int Compare([AllowNull] Point xCoor, [AllowNull] Point yCoor)
        {
            if (xCoor == null && yCoor == null)
                return 0;
            if (yCoor == null)
                return 1;
            if (xCoor == null)
                return -1;
            else
            {
                if (xCoor.X > yCoor.X)
                    return 1;
                if (xCoor.X < yCoor.X)
                    return -1;
                if (xCoor.Y > yCoor.Y)
                    return 1;
                if (xCoor.Y < yCoor.Y)
                    return -1;
                if (xCoor.Row  > yCoor.Row)
                    return 1;
                if (xCoor.Row < yCoor.Row)
                    return -1;

                return 0;
            }
                
        }
        public static IComparer<Point> TComparer()
        {
            return new Point();
        }
        public static IComparer Comparer()
        {
            return (IComparer)new Point();
        }
    }
    public class PointEnumerator : IEnumerator<KeyValuePair<float, SortedSet<float>>>
    {
        private bool disposedValue;
        Point _Current;
        Column ColumnPoints;
        KeyValuePair<float, SortedSet<float>> ActualLine = new KeyValuePair<float, SortedSet<float>>();
        public KeyValuePair<float, SortedSet<float>> GetActualLine() { return ActualLine; }
        Points Points;
        long size = 0;



        public PointEnumerator(Column ColumnPoints, Points Points )
        {
            this.ColumnPoints = ColumnPoints;
            this.Points = Points;   
            

        }
        
        
        private int _pos;
        private long  _Index =-1;


        KeyValuePair<float, SortedSet<float>> IEnumerator<KeyValuePair<float, SortedSet<float>>>.Current { get { return ActualLine; } }
        public  long Index { get { return _Index; } }

        public object Current { get { return this.Current; } }

        public bool MoveNext1()
        {
            if (_Current == null)
            {
                _pos = 0; 
                   var col = ColumnPoints.RWDataColumn.GetNextString;
                if (col == null)
                    return false;
                ActualLine = JsonSerializer.Deserialize<KeyValuePair<float, SortedSet<float>>>(col );
                _Current = new Point(ActualLine.Key, ActualLine.Value.ElementAt(_pos));
            }
            else
            {

                _pos++;
                if (ActualLine.Value.Count () <= _pos )
                {
                    _pos = 0;

                    var col = ColumnPoints.RWDataColumn.GetNextString;

                    if (col==null)
                    {
                        _Current = null;
                        return false;
                    }
                 
                    ActualLine = JsonSerializer.Deserialize<KeyValuePair<float, SortedSet<float>>>(col );
                    _Current = new Point(ActualLine.Key, ActualLine.Value.ElementAt(_pos));
                }
                else
                {

                    _Current = new Point (ActualLine.Key ,  ActualLine.Value.ElementAt(_pos));
                    
                }

            }
            return true;
        }
        public Point Add(float X, float Y, long Row = 0)
        {
            return null;
        }


        public bool MoveNext()
        {
            
            if (_Index ==-1)
            {
                size = Points.Count;
            }
            _Index++;
            if (size == 0 || _Index>=size )
            {
                _Index = -1;
                ActualLine = default;
                return false;
            }
            ActualLine = Points.ElementAt((int)_Index );

            return true;
        }
        public void Reset1()
        {
            ColumnPoints.RWDataColumn.CloseReader();
            _Current = null;
        }
        public void Reset ()
        {
            _Index = -1;
            _pos = 0;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ColumnPoints.Dispose();
                }

                // TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
                // TODO: establecer los campos grandes como NULL
                disposedValue = true;
            }
        }

        // // TODO: reemplazar el finalizador solo si "Dispose(bool disposing)" tiene código para liberar los recursos no administrados
        // ~Points()
        // {
        //     // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }


    }
    public class PointKeyPair  : IPackable
    {
        float _X;
        SortedSet<float> _Ys =new SortedSet<float>();
        public float X {get { return _X; } }
        public SortedSet<float>  Ys { get { return Ys; } }
        public PointKeyPair(float X, SortedSet<float> Ys)
        {
            _Ys = Ys;
            _X = X;
        }
        public KeyValuePair<float, SortedSet<float>> KeyPair
        {
            get
            {
                return new KeyValuePair<float, SortedSet<float>>(_X, _Ys)
                        ;
            }
        }

        public static PointKeyPair GetCleanListYes(PointKeyPair Pair, List <float> Y) 
        {
             Pair.Ys.UnionWith(Y);
            return Pair;     

         }
        public static  SortedSet<float>  GetEmptySortedSet ( )
        {
            return  new  SortedSet<float> ( );
                
        }
        public static  PointKeyPair GetJustXKeyPair(float x)
        {
            
            return new PointKeyPair (x,  GetEmptySortedSet());
             
        }
        public object FromString(string ObjectPackaged)
        {
            return  JsonSerializer.Deserialize<KeyValuePair<float, SortedSet<float>>>(ObjectPackaged);  
        }

        public string ObjectToString()
        {
            return JsonSerializer.Serialize  (KeyPair);
        }
    }
    public class Points : IDictionary<float, SortedSet<float>>
    {

        Column ColumnPoints;
        //PointEnumerator Enumerator;
        KeyValuePair<float, SortedSet<float>> ActualLine = new KeyValuePair<float, SortedSet<float>>();
        SortedDictionary<float, int> Xs = new SortedDictionary<float, int>();
        long Actual = -1;
        SortedDictionary<float, int> Ys = new SortedDictionary<float, int>();
        SortedList<float, SortedSet<float>> XEtYs = new SortedList<float, SortedSet<float>>();
        SortedList<float, SortedSet<float>> YEtXs = new SortedList<float, SortedSet<float>>();
        SortedList<Coordinate, List<Tuple<long, int>>> RowPoints = new SortedList<Coordinate, List<Tuple<long, int>>>();
        PointEnumerator  PointEnumerator ;
        private bool SesionWrite = true; 
        
        public Points(Column ColumnPoints)
        {
            this.ColumnPoints = ColumnPoints;


            this.ColumnPoints.FieldType = new FieldType { OwnIndexInData = true, FieldTypeName = "" };
            PointEnumerator = new PointEnumerator(this.ColumnPoints, this);
        }

        public class ListToSave
        {
            public string Name;
            public Type Type;

        }
        public List<string> PointListsToSave
        {
            get
            {
                var ret = new List<string>();
                ret.Add("XIndex");
                ret.Add("YIndex");
                ret.Add("XWithYFamily");
                ret.Add("YWithXFamily");
                ret.Add("XYFileIndex");
                ret.Add("FileIndexXY");
                return ret;
            }
        }
        private bool _IsReadOnly = false;
        public int Count { get { return (int)ColumnPoints.NumberOfRows; } }

        List<float> keys = new List<float>();
        ICollection<SortedSet<float>> values = new List<SortedSet<float>>();

        public bool IsReadOnly { get { return _IsReadOnly; } }
        public long IndexOf(float key)
        {
            var index =  -1;
            while (true)
            {
                index++;
                var actual = ColumnPoints.RWDataColumn.GetNextString;
                if (actual != null)
                {
                    ActualLine = JsonSerializer.Deserialize<KeyValuePair<float, SortedSet<float>>>(actual);
                    if (ActualLine.Key == key)
                    {
                        ColumnPoints.RWDataColumn.CloseReader();
                    
                    return index++;
                    }
                    ActualLine = new KeyValuePair<float, SortedSet<float>>();
                }
                else
                    break;
            }
            ColumnPoints.RWDataColumn.CloseReader();

            return -1;


        }
        public  KeyValuePair<float, SortedSet<float>> ElementAt(int indexOfKey)
        {
            var index = -1;
            while (true)
            {
                index++;
                var actual = ColumnPoints.RWDataColumn.GetNextString;
                if (actual != null)
                {
                    ActualLine = JsonSerializer.Deserialize<KeyValuePair<float, SortedSet<float>>>(actual);
                    if (index == indexOfKey)
                    {
                        ColumnPoints.RWDataColumn.CloseReader();
                        return ActualLine;
                    }
                    
                    ActualLine = new KeyValuePair<float, SortedSet<float>>();
                }
                else
                    break;
            }
            ColumnPoints.RWDataColumn.CloseReader();

            return default;


        }
        public ICollection<float> Keys
        {
            get
            {

                keys.Clear();
                while (true)
                {
                    var actual = ColumnPoints.RWDataColumn.GetNextString;
                    if (actual != null)
                    {
                        ActualLine = JsonSerializer.Deserialize<KeyValuePair<float, SortedSet<float>>>(actual);
                        keys.Add(ActualLine.Key);
                    }
                    else
                        break;
                }

                    ColumnPoints.RWDataColumn.CloseReader();
                    return keys;
            }

        }
        

        public ICollection<SortedSet<float>> Values
        {
            get
            {
                values.Clear();
                    while (true)
                    {
                        var actual = ColumnPoints.RWDataColumn.GetNextString;
                    if (actual != null)
                    {
                        ActualLine = JsonSerializer.Deserialize<KeyValuePair<float, SortedSet<float>>>(actual);
                        values.Add(ActualLine.Value);
                    }
                    else
                        break;
                    }
                ColumnPoints.RWDataColumn.CloseReader();
                return values;
            }
        }
        public SortedSet<float> this[float key] { get
            {
                
                    while (true)
                    {
                        var actual = ColumnPoints.RWDataColumn.GetNextString;
                    if (actual != null)
                    {
                        ActualLine = JsonSerializer.Deserialize<KeyValuePair<float, SortedSet<float>>>(actual);
                        if (ActualLine.Key == key)
                        {
                            ColumnPoints.RWDataColumn.CloseReader();
                            return ActualLine.Value;
                        }

                    }
                    else
                        break;
                    }
                values = null;
                    ColumnPoints.RWDataColumn.CloseReader();


                    return null;
            }
            set {
                ;
            }
        }
        public string PointListsType(string PoiintList)
        {
            switch (PoiintList)
            {

                case "XIndex":
                    return Xs.GetType().FullName;
                case "YIndex":
                    return Ys.GetType().FullName;
                case "Points":
                    return XEtYs.GetType().FullName;
                case "FileIndexXY":

                default:
                    return "";
            }
        }
        public int PointListsTot(string PoiintList)
        {
            switch (PoiintList)
            {

                case "XIndex":
                    return Xs.Count();
                case "YIndex":
                    return Ys.Count();
                case "XWithYFamily":
                    return XEtYs.Count();
                case "YWithXFamily":
                    return YEtXs.Count();
                case "XYFileIndex":

                case "FileIndexXY":

                default:
                    return 0;
            }
        }
        public string PointListsvalue(string PoiintList, int Value)
        {
            switch (PoiintList)
            {

                case "XIndex":
                    return JsonSerializer.Serialize(Xs.ElementAt(Value));
                case "YIndex":
                    return JsonSerializer.Serialize(Ys.ElementAt(Value));
                case "XWithYFamily":
                    return JsonSerializer.Serialize(XEtYs.ElementAt(Value));

                case "YWithXFamily":
                    return JsonSerializer.Serialize(YEtXs.ElementAt(Value));



                default:
                    return "";
            }
        }

        public Points RestoreFromFile(string PathFields)
        {
            //if (Xs.Count() == 0)
            //{

            //    using StreamReader StreamReaderXYDist = new StreamReader(Path.Combine(PathFields, "XIndex.Hex.Json"));
            //    bool read = true;
            //    while (read)
            //    {
            //        var ActualRead = "";
            //        ActualRead = StreamReaderXYDist.ReadLine();

            //        if (ActualRead == null)
            //            break;
            //        var kp = JsonSerializer.Deserialize<KeyValuePair<float, float>>(ActualRead);
            //        Xs.Add(kp.Key, kp.Value);
            //    }

            //}

            //if (Ys.Count() == 0)
            //{
            //    using StreamReader StreamReaderXYDist = new StreamReader(Path.Combine(PathFields, "YIndex.Hex.Json"));

            //    bool read = true;
            //    while (read)
            //    {
            //        var ActualRead = "";
            //        ActualRead = StreamReaderXYDist.ReadLine();

            //        if (ActualRead == null)
            //            break;
            //        var kp = JsonSerializer.Deserialize<KeyValuePair<int, float>>(ActualRead);
            //        Ys.Add(kp.Key, kp.Value);

            //    }
            //}
            //if (RowPointFromXEtYs.Count() == 0)
            //{

            //    using StreamReader StreamReaderXYDist = new StreamReader(Path.Combine(PathFields, "XYFileIndex.Hex.Json"));
            //    bool read = true;
            //    while (read)
            //    {
            //        var ActualRead = "";
            //        ActualRead = StreamReaderXYDist.ReadLine();

            //        if (ActualRead == null)
            //            break;
            //        var kp = JsonSerializer.Deserialize<KeyValuePair<Coordinate, SortedSet<long>>>(ActualRead);
            //        RowPointFromXEtYs.Add(kp.Key, kp.Value);
            //    }

            //}
            //if (PointRowFromXEtYs.Count() == 0)
            //{

            //    using StreamReader StreamReaderXYDist = new StreamReader(Path.Combine(PathFields, "FileIndexXY.Hex.Json"));
            //    bool read = true;
            //    while (read)
            //    {
            //        var ActualRead = "";
            //        ActualRead = StreamReaderXYDist.ReadLine();

            //        if (ActualRead == null)
            //            break;
            //        var kp = JsonSerializer.Deserialize<KeyValuePair<long, SortedSet<Coordinate>>>(ActualRead);
            //        PointRowFromXEtYs.Add(kp.Key, kp.Value);
            //    }

            //}
            return null;
        }

        public List<PointsRows> RestoreFromFile(string PathFields, string VAlue)
        {
            //RestoreFromFile(PathFields);
            //var XEtYsKP = JsonSerializer.Deserialize<KeyValuePair<int, SortedSet<int>>>(VAlue);


            //var ret = new List<PointsRows>();
            //var x = Xs[XEtYsKP.Key];
            //foreach (var item in XEtYsKP.Value)
            //{
            //    var y = Ys[item];
            //    var coord = new Coordinate { Row = XEtYsKP.Key, Col = item };
            //    var point = new Point(x, y);
            //    var rows = RowPointFromXEtYs[coord];
            //    var PointRows = new PointsRows();
            //    PointRows.Points.Add(point);
            //    PointRows.Rows.AddRange(rows.Select(x=>x).ToList());

            //    ret.Add(PointRows);
            //}
            //return ret;
            return null;
        }




        public SortedList<long, List<Tuple<int, Coordinate>>> GetPointRowFromXEtYs()
        { return null; }
        public string ToRowsRepited(KeyValuePair<Coordinate, SortedSet<long>> item)
        {
            return System.Text.Json.JsonSerializer.Serialize(item);

        }

        
        public void Add(Point item)
        {
            this.Add(item.X, item.Y, item.Row);
        }
        public Point GetPoint(int row, int col)
        {
            float x = Xs.FirstOrDefault(X => X.Value == row).Key;
            float y = Ys.FirstOrDefault(X => X.Value == col).Key;
            return new Point(x, y);
        }/// <summary>
        /// ASume que la columena va a estar ordena x Y R
        /// </summary>
        /// <param name="ColumnXY"></param>
        public void Add(Column ColumnXY)
        {
            XWithYRows ActuallineWRV = null;
            var FileToWrite = Path.GetTempFileName();
            using var Writer = new StreamWriter(ColumnPoints.PathFields, true);
            var Xact = float.MinValue;
            var yact = float.MinValue;
            var first = true;
            ActuallineWRV = new XWithYRows();
            var YRows = new  YRows ();
            while (true)
            {
                var line = ColumnXY.RWDataColumn.GetNextString; 
                if (line == null)
                {
                    Writer.WriteLine(JsonSerializer.Serialize(ActuallineWRV));
                    break;
                }

                    var pointToW = new Point().FromString( line);
                if (pointToW == null)
                    continue;  
                var point = (Point)pointToW;
                if (Xact != point.X  )
                {
                    if (!first)
                    {
                        Writer.WriteLine(JsonSerializer.Serialize(ActuallineWRV));
                    }
                    else
                    {
                        first = false ;
                    } 
                        
                    ActuallineWRV = new XWithYRows(point.X);
                }
                if(yact!=point.Y || Xact != point.X)
                {

                    YRows = new YRows(point.Y) ;
                }

                    YRows.Add(point.Row);
                
                Xact = point.X;
                yact = point.Y;



                ActuallineWRV.AddYSetOFRow(point.Y, point.Row);



            }


            ColumnXY.RWDataColumn.CloseReader();

            Writer.Flush();
            Writer.Close();
        }


        public int[] Add(float X, float Y, long row = 0)
        {
            XWithYRows ActuallineWRV = null ;
            var FileToWrite = Path.GetTempFileName();
            using var Writer = new StreamWriter(FileToWrite, true);
            try
            {
                if (File.Exists(ColumnPoints.PathFields))
                {
                    using var Reader = new StreamReader(ColumnPoints.PathFields);
                    try
                    {
                        int XIndex = 0;
                        bool YExists = false;
                        bool XExists = false;
                        while (true)
                        {
                            var line = Reader.ReadLine();
                            if (line == null)
                                break;

                            ActuallineWRV = JsonSerializer.Deserialize<XWithYRows> (line);
                            if (ActuallineWRV.Value == X)
                            {
                                ActuallineWRV.AddYSetOFRow(Y, row);


                                XExists = true;

                                Writer.WriteLine(JsonSerializer.Serialize(ActuallineWRV));
                            }
                            else
                                Writer.WriteLine(line);
                        }
                        if (!XExists)
                        {
                            ActuallineWRV = new XWithYRows(X);
                            ActuallineWRV.AddYSetOFRow(Y, row);
                            Writer.WriteLine(JsonSerializer.Serialize(ActuallineWRV));

                        }
                    }
                    finally
                    {
                        Reader.Close();
                        Reader.Dispose();
                    }
                }
                else
                {

                    ActuallineWRV = new XWithYRows(X);
                    ActuallineWRV.AddYSetOFRow(Y, row);

                    Writer.WriteLine(JsonSerializer.Serialize(ActuallineWRV));
                }
                Writer.Flush();
                Writer.Close();
                if (File.Exists(ColumnPoints.PathFields))
                    File.Delete(ColumnPoints.PathFields);
                File.Copy(FileToWrite, ColumnPoints.PathFields,true);
                if (File.Exists(FileToWrite))
                    File.Delete(FileToWrite);
            }
            finally { 
                Writer.Dispose();
            }
             








            return new int[] { 0, 0 };
        }

        public void Clear()
        {
            Xs.Clear();
            Ys.Clear();
            YEtXs.Clear();
            XEtYs.Clear();
        }

        

        public bool Remove(Point item)
        {


            var Point = PointIndexbyPoint(item);
            try
            {
                if (Point[0] == -1 || Point[1] == -1)
                {
                    XEtYs[Point[0]].Remove(Point[1]);
                    if (XEtYs[Point[0]].Count == 0)
                        XEtYs.Remove(Point[0]);

                    YEtXs[Point[1]].Remove(Point[0]);
                    if (YEtXs[Point[1]].Count == 0)
                        YEtXs.Remove(Point[1]);

                    var XInYEtXs = YEtXs.Values.SelectMany(x => x).Where(x => x == Point[0]).Count();
                    if (XInYEtXs == 0)
                        Xs.Remove(Point[0]);
                    if (XInYEtXs == 0)
                        Xs.Remove(Point[0]);

                    var YInXEtYs = XEtYs.Values.SelectMany(x => x).Where(x => x == Point[1]).Count();
                    if (YInXEtYs == 0)
                        Ys.Remove(Point[1]);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;

            }



            throw new System.NotImplementedException();
        }





        private int IndexByPoint(Point point)
        {
            try
            {
                var xPoint = Xs.Where(x => x.Value == point.X).First();
                var anteriores = 0;
                try
                {
                    XEtYs.Where(x => x.Key < xPoint.Key).SelectMany(Z => Z.Value).Count();
                }
                catch { };
                var y = Ys.First(x => x.Value == point.Y).Key;
                var Ypos = -1;
                if (XEtYs[xPoint.Key].Where(w => w < y).Count() > 0)
                {
                    Ypos = XEtYs[xPoint.Key].Where(w => w < y).Count();
                }
                else
                    return -1;
                return anteriores + Ypos;
            }
            catch { return -1; }
        }

        private int[] PointIndexbyPoint(Point item)
        {
            try {
                var indexX = Xs.First(x => x.Key == item.X).Value;
                var indexY = XEtYs[item.X].Where(Y => Y <= item.Y).Count();
                if (indexX < 0 || indexY < 0)
                    return new int[] { -1, -1 };
                return new int[] { indexX, indexY };
            }
            catch
            {
                return new int[] { -1, -1 };


            }
        }
        public void AddNew(KeyValuePair<float, SortedSet<float>> keyValue)
        { }
        public void AddAt(KeyValuePair<float, SortedSet<float>> keyValue )
        { }
        public string ObjectToString()
        {
            throw new NotImplementedException();
        }

        public Coordinate FromString(string ObjectPackaged)
        {
            throw new NotImplementedException();
        }




        public void Add(float key, SortedSet<float> value)
        {
            var keyValuePair = new KeyValuePair<float, SortedSet<float>>(key, value);
            Add(keyValuePair);
            
        }

        public bool ContainsKey(float key)
        {
            return this[key] == null; 
        
        }

        public bool Remove(float key)
        {

            var index = this.IndexOf(key);
            if (index < 0)
                return false; 
            return true;

        }

        public bool TryGetValue(float key, [MaybeNullWhen(false)] out SortedSet<float> value)
        {
            throw new NotImplementedException();
        }

        public void Add(KeyValuePair<float, SortedSet<float>> item)
        {
            
            var index = this.IndexOf(item.Key);
            if (index == -1)
            { 
                ColumnPoints.RWDataColumn.Add(JsonSerializer.Serialize(item));
            }
            else
            {
                
                 ActualLine.Value.UnionWith(item.Value ) ;

            }

            ColumnPoints.RWDataColumn.CloseWriter();
        }

        public int[] Add2(float X, float Y, long row = 0)
        {


            if (File.Exists(ColumnPoints.PathFields))
            {
                using var Reader = File.Open(ColumnPoints.PathFields, FileMode.Open, FileAccess.ReadWrite);
                try
                {
                    int XIndex = 0;
                    bool YExists = false;
                    bool XExists = false;

                    int ActualPosition = 0;
                    while (!XExists)
                    {
                        int LenLine = 0;
                        string line = null;
                        while (line == null || !line.Contains("\n"))
                        {

                            LenLine++;
                            int ByteRead = Reader.ReadByte();
                            if (ByteRead == -1)
                                break;

                            line = (line == null ? "" : line);
                            line += Char.ConvertFromUtf32(ByteRead);


                        }
                        if (line == null)
                            break;

                        ActualLine = JsonSerializer.Deserialize<KeyValuePair<float, SortedSet<float>>>(line);
                        if (ActualLine.Key == X)
                        {
                            XExists = true;
                            if (!ActualLine.Value.Contains(Y))
                            {
                                ActualLine.Value.Add(Y);
                                var LinetTo = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(ActualLine) + "\n");

                                Reader.Write(LinetTo, 0, LinetTo.Length);
                                break;
                            }

                        }
                    }
                    if (!XExists)
                    {
                        var Sorted = new SortedSet<float>();

                        Sorted.Add(Y);
                        ActualLine = new KeyValuePair<float, SortedSet<float>>(X, Sorted);
                        var LinetTo = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(ActualLine) + "\n");

                        Reader.Write(LinetTo, 0, LinetTo.Length);



                    }
                }
                finally
                {
                    Reader.Close();
                    Reader.Dispose();
                }
            }
            else
            {
                var FileToWrite = ColumnPoints.PathFields;
                using var Writer = new StreamWriter(FileToWrite, true);
                var Sorted = new SortedSet<float>();

                Sorted.Add(Y);
                ActualLine = new KeyValuePair<float, SortedSet<float>>(X, Sorted);
                Writer.WriteLine(JsonSerializer.Serialize(ActualLine));

                Writer.Flush();
                Writer.Close();
            }











            return new int[] { 0, 0 };
        }
        public bool Contains(KeyValuePair<float, SortedSet<float>> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<float, SortedSet<float>>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<float, SortedSet<float>> item)
        {
            return Remove(item.Key);
        }

        IEnumerator<KeyValuePair<float, SortedSet<float>>> IEnumerable<KeyValuePair<float, SortedSet<float>>>.GetEnumerator()
        {
            return this.PointEnumerator;
        }

        public IEnumerator GetEnumerator()
        {
            return this.PointEnumerator;
        }
    }
}

