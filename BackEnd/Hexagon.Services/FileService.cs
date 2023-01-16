using Hexagon.Model.Models;
using Hexagon.Services.ConvertSourceFileToJsonStrategy;
using Hexagon.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using Hexagon.Services.Helpers;
using Hexagon.Services.Interfaces;
using System.Reflection;
using Newtonsoft.Json;
using Hexagon.Model;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Hexagon.Shared.Enums;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Hexagon.Services
{
    public class FileService : IFileService
    {
        private IConfiguration _Configuration;
        public FileService(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }
        public NativeJsonFileDTO ConvertFileBase64(string Base64File, DataFileConfigurationDTO FileData )
        {
            NativeJsonFile NativeJsonFile = new NativeJsonFile();

            NativeJsonFile = GetJsonSerializedFile(Base64File, FileData);
            //return new NativeJsonFileDTO { Content = NativeJsonFile.Content, Columns = NativeJsonFile.Columns };

            var columns = NativeJsonFile.Columns.Select(x => new ColumnDTO(x.Name, x.OriginalPosition, 
                (EnumActionToDoWithUncastedDTO) Enum.Parse(typeof(EnumActionToDoWithUncasted),  x.ActionToDoWithUncasted.ToString()),
                (EnumAlowedDataTypeDTO) Enum.Parse(typeof(EnumAlowedDataType), x.DataTypeSelected.ToString()) ));


            return new NativeJsonFileDTO { Content = NativeJsonFile.Content, Columns = columns.ToList()};

        }
        public NativeFileDTO ConvertFile (string PathFile, DataFileConfigurationDTO FileData, LayoutDto Layout)
        {
            NativeFileDTO NativeFile = new NativeFileDTO();


            NativeFile = GetJsonSerializedFileFromFile(PathFile, FileData, new Model.Layout(Layout.Flat, Layout.Size,Layout.Origin,1000 ));

            //return new NativeJsonFileDTO { Content = NativeJsonFile.Content, Columns = NativeJsonFile.Columns };

            


            return NativeFile;

        }
        public List<DataFileConfigurationDTO> GetDataFileConfiguration(string Path)
        {
            
            return FilesHelper.ReadDataFileConfiguration(Path );
        }

        public List<ColumnDTO> GetFileColumsFromFile(string PathFile)
        {
            
            var Destination = Path.GetDirectoryName(PathFile) + @"\Def" + Path.GetFileNameWithoutExtension(PathFile) + ".json";
            if (!File.Exists(Destination))
                return null;
            else
            {
                var ret = FilesHelper.ReadFileDef(Destination);
                var retcols = ret.Select(x => new ColumnDTO(x.Name, x.OriginalPosition,
                (EnumActionToDoWithUncastedDTO)Enum.Parse(typeof(EnumActionToDoWithUncasted), x.ActionToDoWithUncasted.ToString()),
                (EnumAlowedDataTypeDTO)Enum.Parse(typeof(EnumAlowedDataType), x.DataTypeSelected.ToString()))).ToList();
                return retcols;
            }

        }
        public AnalizedFile GetAnalizedFile(string PathFile, DataFileConfigurationDTO DataFileConfigurationDTO)
        {
            return new AnalizedFile(null,null) ;
        }

        public NativeFileDTO GetJsonSerializedFileFromFile(string PathFile, DataFileConfigurationDTO DataFileConfigurationDTO, Hexagon.Model.Layout Layout)
        {
            try
            {
                object FileType = new object();
                object FileProperties = new object();
                NativeFile NativeFile = new NativeFile();
                var Destination = Path.GetDirectoryName(PathFile) + @"\Data" + Path.GetFileNameWithoutExtension(PathFile) + ".json";
                if (!File.Exists(Destination))
                {
                    var clazz = Type.GetType("Hexagon.Services.ConvertSourceFileToJsonStrategy.Convert" + DataFileConfigurationDTO.FileType + "ToJsonStrategy");
                    var Strategy = (IConvertSourceFileToJsonStrategy)Activator.CreateInstance(clazz);
                    var FileData = new DataFileConfiguration() { FileType = DataFileConfigurationDTO.FileType, FileProperties = DataFileConfigurationDTO.FileProperties };

                    var resultado = Strategy.DoFromFile(PathFile, Destination, FileData);
                }
                else
                {
                    using (StreamReader StreamReader = new StreamReader(Destination))
                    {
                        NativeFile.Content  = Newtonsoft.Json.JsonConvert.DeserializeObject<Line[]>(StreamReader.ReadToEnd()).ToList();
                        StreamReader.Close();

                    }
                    string FileDestinationDef = Path.GetDirectoryName(PathFile) + @"\Def" + Path.GetFileNameWithoutExtension(PathFile) + ".json";

                    using (StreamReader streamReader = new StreamReader(FileDestinationDef))
                    {
                        NativeFile.Columns = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Column>>(streamReader.ReadToEnd());
                        streamReader.Close();
                    }
                    string FileDestinationPrue = Path.GetDirectoryName(PathFile) + @"\Pru" + Path.GetFileNameWithoutExtension(PathFile) + ".json";
                    Layout layout = new Layout(true, new System.Drawing.PointF(50f, 50f), new System.Drawing.PointF(0, 0),1000);


                }

                    return new NativeFileDTO
                    {
                        Content = NativeFile.Content.Select(x => new LineDTO(x.Number, x.Fieds )).Take(10).ToList(),
                        Columns = NativeFile.Columns.Select(x => new ColumnDTO(x.Name, x.OriginalPosition,
    (EnumActionToDoWithUncastedDTO)Enum.Parse(typeof(EnumActionToDoWithUncasted), x.ActionToDoWithUncasted.ToString()),
    (EnumAlowedDataTypeDTO)Enum.Parse(typeof(EnumAlowedDataType), x.DataTypeSelected.ToString()))).ToList()
                    };
                

            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public NativeJsonFile GetJsonSerializedFile(string Base64File, DataFileConfigurationDTO DataFileConfigurationDTO)
        {
            try
            {
                object FileType = new object();
                object FileProperties = new object();
                NativeJsonFile NativeJsonFile = new NativeJsonFile();
            var clazz = Type.GetType("Hexagon.Services.ConvertSourceFileToJsonStrategy.Convert" + DataFileConfigurationDTO.FileType + "ToJsonStrategy");
                var Strategy = (IConvertSourceFileToJsonStrategy)Activator.CreateInstance(clazz);
                var FileData = new DataFileConfiguration() { FileType = DataFileConfigurationDTO.FileType, FileProperties = DataFileConfigurationDTO.FileProperties };
                return Strategy.Do(Base64File, FileData);
            }
            catch (Exception Exception) 
            {
                throw Exception;
            }
        }
        public string GenerateImge(LayoutDto Layout, string PathFile)
        {
            HexagonGrid HexagonGrid = new HexagonGrid();
            var layout = new Layout(Layout.Flat, Layout.Size, Layout.Origin, Layout.HexPerLine,
                Layout.MaxPictureSizeX, Layout.MaxPictureSizeY, Layout.FillPolygon);
            if (Layout.MapDefinition!=null)
                layout.MapDefinition = new MapDefinition {
                ColumnForMapGroup = Layout.MapDefinition.ColumnForMapGroup, ColumnNameForX = Layout.MapDefinition.ColumnNameForX,
                ColumnNameForY = Layout.MapDefinition.ColumnNameForY, ColumnsNameForFuntion = Layout.MapDefinition.ColumnsNameForFuntion,
                FunctionName = Layout.MapDefinition.FunctionName,
                 ActionToDoWithUncasted = ( EnumActionToDoWithUncasted)Enum.Parse(typeof(EnumAlowedDataType), Layout.MapDefinition.ActionToDoWithUncasted.ToString())
            };
            HexagonGrid.Layout = layout;
            var hexs = MapHelper.HexMapGeoJSon(PathFile,  ref HexagonGrid);


            using (StreamWriter StreamWriter = new StreamWriter(Path.Combine(Path.GetDirectoryName (PathFile), Path.GetFileNameWithoutExtension(PathFile) + ".jsonHex")))
            {
                //var hexText = JsonConvert.SerializeObject(hexs.Select(hex => new object[] { hex.BorderColor, hex.BorderType, hex.Color, hex.Opacity, hex.RGBColor, hex.Q, hex.R, hex.S, hex.Value, hex.Values }));
                foreach (var hex in hexs)
                {
                    
                    StreamWriter.WriteLine( hex.ListValues);
                    
                    //StreamWriter.Write(JsonConvert.SerializeObject( hexs.Select(hex => new object[] { hex.BorderColor, hex.BorderType, hex.Color, hex.Opacity, hex.RGBColor, hex.Q, hex.R, hex.S, hex.Value, hex.Values })));
                    StreamWriter.Flush();
                }

                StreamWriter.Close();
            }
            return CreadorMapaService.CreadorMapa(ref HexagonGrid, PathFile );
            
        }
        public List<ProyectDataDTO> GetProyects(string Path, string Name )
        {
            var ret = FilesHelper.ReadProject(Path, Name);

            return ret;
        }
    }
}
