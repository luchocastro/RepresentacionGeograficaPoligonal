using Hexagon.Model.Models;
using Hexagon.Model.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Hexagon.Model.FileDataManager
{
    public class RWDataColumn :  IDisposable
    {
        string TempFile = "";
        StreamWriter Temp = null;
        public RWDataColumn(Column column) { Column = column;
             


        }
        StreamReader _Reader = null;
        StreamWriter _Writer = null;
        public Column Column { get; }
        private long CurrentPosition = -1;
        public long GetCurrent() { return CurrentPosition; }
        public String Linea { get; set; }
        public void CloseReader()
        {
            if (_Reader != null)
            {
                _Reader.Close();
                _Reader.Dispose();
                _Reader = null;
            }
        }
        public void CloseWriter()
        {
            if (_Writer != null)
            {

                _Writer.Close();
                _Writer.Dispose();
            }
        }
        public bool _WasDeleted = false;

        private  bool _DeleteFile = false;
        private bool Save = false;
        public bool ToWrite { get; set; } = false;

        public bool DeleteFile { get { return _DeleteFile; } set { _DeleteFile = value; } }
        public StreamReader Reader 
        {
            get
            {
                if (!System.IO.File.Exists(Column.PathFields))
                    return null;
                if (_Reader == null)
                {
                    _Reader = new StreamReader(Column.PathFields);
                    ToWrite = true;

                }
                return _Reader;
            }
        }

        public StreamWriter Writer
        {
            get
            {

                if (_Writer == null)
                {

                    _Writer = new StreamWriter(Column.PathFields, true);
                    
                }
                return _Writer;
            }
        }

        public Field GetNext
        {
            get {
                var data = GetNextString;
                if (data == null)
                    return null;
                return Column.ObjectFromString(Linea);
            }

        } 
            public string GetNextString {  
            get
            {

                        Linea = Reader.ReadLine();

                    if (Linea==null) return null;
                
                        return Linea;
            }
        }


        public void Add (String field)
        {

            Writer.WriteLine( field  );


        }
        public void Add(Field field)
        {
            
            Writer.WriteLine( field.ObjectToString()  );
            

        }

        public void Dispose()
        {
            if (_Reader != null)
            {

                _Reader.Close();
                _Reader.Dispose();
            }
            if(_Writer != null )
            {
                
                _Writer.Close();
                _Writer.Dispose();
            }
        }

        public bool FileExists { get { return System.IO.File.Exists(Column.PathFields); } }
    public bool EOF { get { return _Reader.EndOfStream; } }



    }
}
