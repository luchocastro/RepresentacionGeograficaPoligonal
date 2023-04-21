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

namespace Hexagon.Services.ConvertSourceFileToJsonStrategy
{
    public class ConvertDelimitedFileToJsonStrategy : IConvertSourceFileToJsonStrategy
    {
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

        public async  Task<NativeFile> DoFromFileAsync(string PathFileOrigen, DataFileConfiguration FileData, int FirstNRows = 0, AnalizedFile AnalizedFile)
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
            var Coma = "";
             using (StreamReader StreamReader = new StreamReader(PathFileOrigen))
            {
                bool Continue = true;
                var ActualLine = "";
                
                while ((ActualLine = await StreamReader.ReadLineAsync().ConfigureAwait(false)) != null)
                {

                    string[] DataInLine = ActualLine.Split(Delimiter.ToString());
                    int ColumnsQuantity = DataInLine.Length;
                    var ColumnsBegin = "";
                    var Line = new Line();
                    

                    if (SetCols)
                    {
                        
                        ColumnsBegin = "[";
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
                            var PathColums = Path.Combine(Path.GetDirectoryName(AnalizedFile.Path), "Columns");

                            if (!Directory.Exists(PathColums))
                            {
                                Directory.CreateDirectory(PathColums);
                            }
                            PathColums += Columns[i] + ".Hex.Json";
                            ColumnsForModel.Add(new Column { Name = Columns[i], OriginalPosition = i, PathFields= PathColums });
                        }
                        
                    }
                    if (AnalizedFile.Path != "" && (!HasTitle || !SetCols  ))
                    {

                        Line = Helpers.FilesHelper.LineToField(DataInLine, (long)Step);

                        for (int i = 0; i < ColumnsForModel.Count(); i++)
                        {
                            using (var StreamWriter = new StreamWriter(ColumnsForModel[i].PathFields,true,Encoding.UTF8 ))
                            {
                                var FieldText = ColumnsBegin;
                                FieldText += System.Text.Json.JsonSerializer.Serialize(new Field() { Index = Line.Number, Value = Line.Fieds[i] });

                                await StreamWriter.WriteAsync(FieldText).ConfigureAwait(false);
                            }
                            
                        }

                        if (Step == FirstNRows)
                        {
                            break;
                        }
                        Step++;
                        ColumnsBegin = ",";
                    }

                    SetCols = false;
                    //if (Step < 100)
                    //{
                    //    for (int i = 0; i < ColumnsQuantity; i++)
                    //    {
                    //        var Parsed = FindTypesAllowed.GetTypesAllows(DataInLine[i], FileData);

                    //        ColumnsArray[i].DataTypeFinded.AddRange(Parsed);
                    //    }

                    //}

                }
                for (int i = 0; i < ColumnsForModel.Count(); i++)
                {
                    using (var StreamWriter = new StreamWriter(ColumnsForModel[i].PathFields, true, Encoding.UTF8))
                    {
                        var FieldText = "]";
                        await StreamWriter.WriteAsync(FieldText).ConfigureAwait(false);
                    }

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

            NativeFile.Columns = ColumnsForModel;
            NativeFile.DataFileConfiguration = FileData;

            return  NativeFile;
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
            NativeFile.Content = Lineas;
            //for (int i = 0; i < ColumnsForModel.Count(); i++)
            //{
            //    var col = ColumnsForModel[i];
            //    var FieldsEv = Step > 100 ? 100 : Step;
            //    var datatypes = FindTypesAllowed.TypesPrincipals(ColumnsArray[i].DataTypeFinded, FieldsEv);
            //    ColumnsForModel[i].DataTypeFinded.AddRange(  datatypes);
            //}

            NativeFile.Columns = ColumnsForModel;
            NativeFile.DataFileConfiguration = FileData;

            return NativeFile;
        }
    }
}
