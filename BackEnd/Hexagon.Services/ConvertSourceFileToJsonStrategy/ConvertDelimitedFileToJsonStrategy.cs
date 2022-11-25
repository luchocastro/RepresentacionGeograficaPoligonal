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
                ColumnsForModel.Add(new Column(item, pos, EnumActionToDoWithUncasted.DeleteData));
                pos++;
            }
            NativeJsonFile.Content = JSonFileConverted;
           NativeJsonFile.Columns = ColumnsForModel;
            return NativeJsonFile;
        }
    }
}
