using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Hexagon.Model.Models;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;
using Hexagon.Shared.CommonFunctions;
using Hexagon.Model;
using System.Threading.Tasks;
using Hexagon.Services.Helpers;
using Hexagon.Model.FileDataManager;
using Hexagon.Shared.DTOs;
using Hexagon.Model.Repository;

namespace Hexagon.Services.ConvertSourceFileToJsonStrategy
{
    public class ConvertDelimitedFileToJsonStrategy : IConvertSourceFileToJsonStrategy
    {
        public IDataRepository<ColumnDTO, Column> ColumnRepository { get; set; }
        public NativeJsonFile Do(string Base64File, DataFileConfiguration FileData )
        {            
            var Base64EncodedBytes = System.Convert.FromBase64String(Base64File);
            MemoryStream SourceFile = new MemoryStream(Base64EncodedBytes);
            object Delimiter = null;
            FileData.FileProperties.TryGetValue("Delimiter", out Delimiter) ;
            if (Delimiter == null)
                throw new Exception( ServicesConstants.DelimiterMissing);
            object HasTitleInDefinition = null;
            FileData.FileProperties.TryGetValue("HasTitle", out HasTitleInDefinition) ;
            bool HasTitle = HasTitleInDefinition != null && Convert.ToBoolean(HasTitleInDefinition.ToString());
            string []Columns  = null;
            int Step = 0;
            List<JObject> JObjectList = new List<JObject>();
            using (StreamReader StreamReader = new StreamReader(SourceFile))
            {
                String ActualLine = "";
                 while ((ActualLine = StreamReader.ReadLine()) != null)
                {
                    string[] DataInLine = ActualLine.Split(Delimiter.ToString());
                    int ColumnsQuantity = DataInLine.Length;
                    Step++;
                    if (Step == 1 )
                    {
                        if (HasTitle)
                        {
                            Columns = DataInLine;
                        }
                        else
                        {                            
                            Columns = new string[ColumnsQuantity];
                            for (int i = 0; i < ColumnsQuantity; i++)
                            {
                                Columns[i] = "Column-"+ i.ToString();
                            }
                        }
                       
                        
                    }
                    dynamic JObjectLine = new JObject();
                    if (Step > 1 || !HasTitle)
                    {
                        int i = 0;
                        foreach (string Data in DataInLine)
                        {

                            JObjectLine[Columns[i]] = new JValue(TypeFunction.GetTyped(Data)); ;
                            i++;
                        }
                        JObjectList.Add(JObjectLine);
                    }
                    
                }
            }
            var JSonFileConverted = Newtonsoft.Json.JsonConvert.SerializeObject(JObjectList);
            NativeJsonFile NativeJsonFile = new NativeJsonFile();

            var ColumnsForModel=new List<Column>();
            int pos = 0;
            foreach (var item in Columns)
            {
                ColumnsForModel.Add(new Column(item, pos, EnumActionToDoWithUncasted.DeleteData, EnumAlowedDataType.Character ));
                pos++;
            }
            NativeJsonFile.Content = JSonFileConverted;

            return NativeJsonFile;
        }

        public async Task<NativeFile> DoFromFileAsync(string PathFileOrigen, DataFileConfiguration FileData, int FirstNRows = 0, NativeFile NativeFile = null)
        {

            object Delimiter = FileData.DecimalSeparator;
            FileData.FileProperties.TryGetValue("Delimiter", out Delimiter);
            if (Delimiter == null)
                throw new Exception(ServicesConstants.DelimiterMissing);
            object HasTitleInDefinition = null;
            object NumberConcatenator = null;
            FileData.FileProperties.TryGetValue("HasTitle", out HasTitleInDefinition);
            FileData.FileProperties.TryGetValue("NumberConcatenator", out NumberConcatenator);
            bool HasTitle = HasTitleInDefinition != null && Convert.ToBoolean(HasTitleInDefinition.ToString());
            string[] Columns = null;
            long Step = 0;
            List<Line> Lineas = new List<Line>();
            List<Column> ColumnsForModel = new List<Column>();

            bool SetCols = true;
            var Coma = "";
            var ActualLine = "";
            var ColumnsBegin = "";
            string[] DataInLine = null;

            int ColumnsQuantity = 0;
            var Line = new Line();
            var TieneFecha = (FileData.DatetimeFormart != null && FileData.DatetimeFormart != "");
            var TieneParDenumeros = NumberConcatenator != null;
            var TiposPermitidosColumas = new Dictionary<int, Dictionary<EnumAlowedDataType, int>>();
            var PathColums = ColumnRepository.ClassLocation(new ColumnDTO() { ParentID = NativeFile.ParentID });

            using (StreamReader StreamReader = new StreamReader(PathFileOrigen))
            {

                ActualLine = StreamReader.ReadLine();

                DataInLine = ActualLine.Split(Delimiter.ToString());
                ColumnsQuantity = DataInLine.Length;


                if (HasTitle)
                {

                    Columns = DataInLine;
                }
                else
                {
                    Columns = new string[ColumnsQuantity];
                    for (int i = 0; i < ColumnsQuantity; i++)
                    {
                        Columns[i] = "Column-" + i.ToString();
                    }
                }
                for (int i = 0; i < ColumnsQuantity; i++)
                {


                    var PathColumsInfo = Path.Combine(PathColums, Columns[i] + ".Hex.Json");
                    var PathColumsField = Path.Combine(PathColums, typeof(Field).Name, Columns[i] + ".Hex.Json");

                    if (!Directory.Exists(Path.GetDirectoryName(PathColumsField)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(PathColumsField));
                    }

                    var TiposPermitidos = new Dictionary<EnumAlowedDataType, int>();
                    TiposPermitidos.Add(EnumAlowedDataType.Character, 0);
                    TiposPermitidos.Add(EnumAlowedDataType.Position, 0);
                    TiposPermitidos.Add(EnumAlowedDataType.DataTime, 0);
                    TiposPermitidos.Add(EnumAlowedDataType.GenericNumber, 0);
                    TiposPermitidos.Add(EnumAlowedDataType.NullOrEmpty, 0);
                    TiposPermitidosColumas.Add(i, TiposPermitidos);


                    var ColumnToPersist = new Column()
                    {
                        Name = Columns[i],
                        OriginalPosition = i,
                        PathFields = PathColumsField,
                        ParentID = NativeFile.ParentID,
                        IdTraslated = false
                    };
                    ColumnToPersist.ID = ColumnRepository.GenerateFullID(ColumnToPersist);
                    ColumnsForModel.Add(ColumnToPersist);

                    NativeFile.Columns.Add(Columns[i]);

                }
                StreamReader.Dispose();
            }

            var Cols = ColumnsForModel;
            var t = Task.Run(() =>
            {
              for (int i = 0; i < ColumnsForModel.Count(); i++)
              {
                  using StreamReader StreamReader = new StreamReader(PathFileOrigen);
                  ColumnsBegin = "";
                  bool Last = false;
                  SetCols = true;
                  Step = 0;
                    var Muestra = 0;
                  using var StreamWriter = new StreamWriter(ColumnsForModel[i].PathFields, true, Encoding.UTF8);
                    var rand = new Random();
                   
                  while (true)
                  {
                      var ActualLine = StreamReader.ReadLineAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                      if (ActualLine == null) Last = true;
                      
                      if ( (!HasTitle || !SetCols))
                      {
                          string value = null;
                          if (!Last)
                          {
                              DataInLine = ActualLine.Split(Delimiter.ToString());

                              if (i < DataInLine.Count())
                              {

                                  value = DataInLine[i];

                              }
                          }
                          else
                          { ColumnsBegin = ""; }
                          var Field = new Field();
                          Field.Index = Step;
                          Field.Value = value;
                            
                          var FieldText = ColumnsBegin;
                            if (!Last)
                            {

                                FieldText += Field.ToString();
                                StreamWriter.WriteAsync(FieldText);
                                StreamWriter.Flush();
                                var num = 0d  ;
                                var numrnd = 0 ;
                                long rem = 0;
                                if (Step >1)
                                {
                                    num = Math.Ceiling( (Math.Log((double ) Step  , 2d)));
                                    numrnd = rand.Next(1, (int)num+1);
                                    Math.DivRem( (long)numrnd, (long)num, out rem);
                                    
                                }
                                if (num < 1 || rem == 0 )
                                {
                                    Muestra++; 
                                    if (value!=null)
                                    {
                                        var str = TiposPermitidosColumas[i][EnumAlowedDataType.Character];
                                        str++;
                                        TiposPermitidosColumas[i][EnumAlowedDataType.Character] = str;
                                    }
                                    if (value == null || value.ToString() == "")
                                    {
                                        var vacio = TiposPermitidosColumas[i][EnumAlowedDataType.NullOrEmpty];
                                        vacio++;
                                        TiposPermitidosColumas[i][EnumAlowedDataType.NullOrEmpty] = vacio;
                                    }
                                    else if (FindTypesAllowed.IsNumber(value, FileData.DecimalSeparator))
                                    {
                                        var q = TiposPermitidosColumas[i][EnumAlowedDataType.GenericNumber];
                                        q++;
                                        TiposPermitidosColumas[i][EnumAlowedDataType.GenericNumber] = q;
                                    }
                                    else
                                    {
                                        var EncontroFecha = false;
                                        if (TieneFecha)
                                        {
                                            if (FindTypesAllowed.IsDate(value, FileData.DatetimeFormart))
                                            {
                                                EncontroFecha = true;
                                                var Esfecha = TiposPermitidosColumas[i][EnumAlowedDataType.DataTime];
                                                Esfecha++;
                                                TiposPermitidosColumas[i][EnumAlowedDataType.DataTime] = Esfecha;
                                            }
                                        }

                                        if (TieneParDenumeros && !EncontroFecha)
                                        {
                                            if (FindTypesAllowed.IsManyNumber(value, FileData.DecimalSeparator, NumberConcatenator.ToString()))
                                            {
                                                var q = TiposPermitidosColumas[i][EnumAlowedDataType.Position];
                                                q++;
                                                TiposPermitidosColumas[i][EnumAlowedDataType.Position] = q;
                                            }
                                        }
                                    }
                                }
                            }

                      }

                      if (Step == FirstNRows)
                      {
                          break;
                      }
                        if (SetCols && HasTitle)
                            Step = -1;
                      if (!SetCols)
                          ColumnsBegin = "\n";
                      else
                          SetCols = false;
                      if (Last)
                          break;
                      
                            Step++;


                    }



              }

              for (int i = 0; i < TiposPermitidosColumas.Count(); i++)
              {
                  var col = ColumnsForModel[i];
                  col.DictionaryEnumAlowedDataType = TiposPermitidosColumas[i];
                  col.NumberOfRows = Step +1;
                  ColumnRepository.Add(col);



              }
          }).ConfigureAwait(false);


            NativeFile.DataFileConfiguration = FileData;

            return NativeFile;
        }

        public NativeFile DoFromFile(string PathFileOrigen, DataFileConfiguration FileData, int FirstNRows = 0)
        {
            
            object Delimiter = FileData.DecimalSeparator;
            FileData.FileProperties.TryGetValue("Delimiter", out Delimiter);
            if (Delimiter == null)
                throw new Exception(ServicesConstants.DelimiterMissing);
            object HasTitleInDefinition = null;
            FileData.FileProperties.TryGetValue("HasTitle", out HasTitleInDefinition);
            bool HasTitle = HasTitleInDefinition != null && Convert.ToBoolean(HasTitleInDefinition.ToString());
            string[] Columns = null;
            int Step = 0;
            List<Line> Lineas = new List<Line>();
            List<Column> ColumnsForModel = new List<Column>();
            bool SetCols = true;
            using (StreamReader StreamReader = new StreamReader(PathFileOrigen))
            {
                String ActualLine = "";
                while ((ActualLine = StreamReader.ReadLine()) != null)
                {
                    string[] DataInLine = ActualLine.Split(Delimiter.ToString());
                    int ColumnsQuantity = DataInLine.Length;

                    
                    if (SetCols)
                    {
                        if (HasTitle)
                        {

                            Columns = DataInLine;
                        }
                        else
                        {
                            Columns = new string[ColumnsQuantity];
                            for (int i = 0; i < ColumnsQuantity; i++)
                            {
                                Columns[i] = "Column-" + i.ToString();
                            }
                            Lineas.Add(Helpers.FilesHelper.LineToField(DataInLine, (long)Step));
                            Step++;
                        }
                        for (int i = 0; i < ColumnsQuantity ; i++)
                        {
                            ColumnsForModel.Add(new Column { Name = Columns[i], OriginalPosition = i });
                        }
                        SetCols = false;
                    }
                    else
                    { 

                    Lineas.Add(Helpers.FilesHelper.LineToField(DataInLine, (long)Step));
                        
                        if (Step==FirstNRows)
                        { break; }
                        Step++;
                    }
                    //if (Step < 100)
                    //{
                    //    for (int i = 0; i < ColumnsQuantity; i++)
                    //    {
                    //        var Parsed = FindTypesAllowed.GetTypesAllows(DataInLine[i], FileData);

                    //        ColumnsArray[i].DataTypeFinded.AddRange(Parsed);
                    //    }
                       
                    //}
                     
                }
            }

            NativeFile NativeFile = new NativeFile();
           
            //for (int i = 0; i < ColumnsForModel.Count(); i++)
            //{
            //    var col = ColumnsForModel[i];
            //    var FieldsEv = Step > 100 ? 100 : Step;
            //    var datatypes = FindTypesAllowed.TypesPrincipals(ColumnsArray[i].DataTypeFinded, FieldsEv);
            //    ColumnsForModel[i].DataTypeFinded.AddRange(  datatypes);
            //}

            NativeFile.DataFileConfiguration = FileData;

            return  NativeFile ;
        }
    }
}
