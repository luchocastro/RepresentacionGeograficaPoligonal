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
using AutoMapper;

namespace Hexagon.Services
{
    public class FileService : IFileService
    {
        public struct HexagonGridForFile
        {
            public string HexagonsHexagonMap { get; set; }
            public string FullClassName { get; set; }
            public string Layout { get; set; }
        }
        private IConfiguration _Configuration;
        private IMapper _Mapper;
        public FileService(IConfiguration Configuration,IMapper Mapper)
        {
            
            _Configuration = Configuration;
            _Mapper = Mapper;
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
        public NativeFileDTO ConvertFile(string PathFile, DataFileConfigurationDTO FileData, LayoutDto Layout )
        {
            NativeFileDTO NativeFile = new NativeFileDTO();

             NativeFile = GetJsonSerializedFileFromFile(PathFile, FileData, new Model.Layout(Layout.Flat, Layout.Size, Layout.Origin, 1000));

            //return new NativeJsonFileDTO { Content = NativeJsonFile.Content, Columns = NativeJsonFile.Columns };




            return NativeFile;

        } 
        public NativeFileDTO ConvertFile (  DataFileConfigurationDTO FileData, LayoutDto Layout, ProyectDataDTO ProyectDataDTO, string NicData)
        {
            NativeFileDTO NativeFile = new NativeFileDTO();

            var PathFile = Path.Combine(ProyectDataDTO.Location.ProyectFolder, NicData, ProyectDataDTO.Location.FileFolder, ProyectDataDTO.AnalizedFiles.FirstOrDefault(x => x.NicName == NicData).FileName);
            var Name = Path.GetFileNameWithoutExtension(PathFile) + ".json";
            var PathFileDestino = Path.Combine(ProyectDataDTO.Location.ProyectFolder, NicData, ProyectDataDTO.Location.NativeFileFolder, Name );
            NativeFile = GetJsonSerializedFileFromFile(PathFile, FileData, new Model.Layout(Layout.Flat, Layout.Size,Layout.Origin,1000 ),PathFileDestino);

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
        public AnalizedFileDTO GetAnalizedFile(string PathFile, DataFileConfigurationDTO DataFileConfigurationDTO)
        {
            return new AnalizedFileDTO( ) ;
        }

        public AnalizedFileDTO ConvertFileToHexList(ProyectDataDTO ProyectDataDTO, AnalizedFileDTO AnalizedFileDTO, LayoutDto LayoutDto)
        {

            var Layout = _Mapper.Map<Layout>(LayoutDto);
             var HexagonGrid = new HexagonGrid() ;
            HexagonGrid.Layout = Layout;
            string NativeToUse = Path.Combine(ProyectDataDTO.Location.ProyectFolder, AnalizedFileDTO.NicName, ProyectDataDTO.Location.NativeFileFolder, Path.GetFileNameWithoutExtension(AnalizedFileDTO.FileName) + ".json"); // ProyectData.AnalizedFiles.Where(x => x.NicName == DataName).FirstOrDefault ().FileName;
            if (NativeToUse == "" || ! File.Exists(NativeToUse))
                throw new FileNotFoundException("El archivo no encontrado " +NativeToUse );
            var ProyectData = _Mapper.Map<ProyectData>(ProyectDataDTO); 
            MapHelper.HexMap(ProyectData, AnalizedFileDTO.FileName , ref HexagonGrid);

            var ToFile = new HexagonGridForFile
            {
                HexagonsHexagonMap = JsonConvert.SerializeObject(HexagonGrid.HexagonMap.Select(x => x.ListValues)),
                FullClassName = JsonConvert.SerializeObject(HexagonGrid.Function),
                Layout = JsonConvert.SerializeObject(HexagonGrid.Layout)
            };

            FilesHelper.SaveFile<HexagonGridForFile>(Path.Combine(Path.GetFullPath(NativeToUse), "HexGrid" + Path.GetFileNameWithoutExtension(NativeToUse) + ".Json"),ToFile);
            return AnalizedFileDTO;
        }
        public NativeFileDTO GetJsonSerializedFileFromFile(string PathFile, DataFileConfigurationDTO DataFileConfigurationDTO, Hexagon.Model.Layout Layout, string Destination ="")
        {
            try
            {
                object FileType = new object();
                object FileProperties = new object();
                NativeFile NativeFile = new NativeFile();
                if(Destination=="")
                 Destination = Path.GetDirectoryName(PathFile) + @"\Data" + Path.GetFileNameWithoutExtension(PathFile) + ".json";
                if (!File.Exists(Destination))
                {
                    var clazz = Type.GetType("Hexagon.Services.ConvertSourceFileToJsonStrategy.Convert" + DataFileConfigurationDTO.FileType + "ToJsonStrategy");
                    var Strategy = (IConvertSourceFileToJsonStrategy)Activator.CreateInstance(clazz);
                    var FileData = new DataFileConfiguration() { FileType = DataFileConfigurationDTO.FileType, FileProperties = DataFileConfigurationDTO.FileProperties };

                    var resultado = Strategy.DoFromFile(PathFile, Destination, FileData);
                    NativeFile = resultado;
                }
                else
                {
                    using (StreamReader StreamReader = new StreamReader(Destination))
                    {
                        NativeFile.Content  =  JsonConvert.DeserializeObject<Line[]>(StreamReader.ReadToEnd()).ToList();
                        StreamReader.Close();

                    }
                    string FileDestinationDef = Path.GetDirectoryName(PathFile) + @"\Def" + Path.GetFileNameWithoutExtension(PathFile) + ".json";

                    using (StreamReader streamReader = new StreamReader(FileDestinationDef))
                    {
                        NativeFile.Columns = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Column>>(streamReader.ReadToEnd());
                        streamReader.Close();
                    }
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
        public string GenerateImge(LayoutDto Layout, ProyectDataDTO ProyectDataDTO, AnalizedFileDTO AnalizedFileDTO)
        {
            HexagonGrid HexagonGrid = new HexagonGrid();
            var layout = new Layout(Layout.Flat, Layout.Size, Layout.Origin, Layout.HexPerLine,
                Layout.MaxPictureSizeX, Layout.MaxPictureSizeY, Layout.FillPolygon);
            if (Layout.MapDefinition != null)
                layout.MapDefinition = new MapDefinition
                {
                    ColumnForMapGroup = Layout.MapDefinition.ColumnForMapGroup,
                    ColumnNameForX = Layout.MapDefinition.ColumnNameForX,
                    ColumnNameForY = Layout.MapDefinition.ColumnNameForY,
                    ColumnsNameForFuntion = Layout.MapDefinition.ColumnsNameForFuntion,
                    FunctionName = Layout.MapDefinition.FunctionName,
                    ActionToDoWithUncasted = (EnumActionToDoWithUncasted)Enum.Parse(typeof(EnumAlowedDataType), Layout.MapDefinition.ActionToDoWithUncasted.ToString())
                };
            HexagonGrid.Layout = layout;
            ProyectData ProyectData = new ProyectData() { Location = new Location(ProyectDataDTO.Location.ProyectFolder, ProyectDataDTO.Location.MapsFolder, ProyectDataDTO.Location.FileFolder, ProyectDataDTO.Location.NativeFileFolder), Name = ProyectDataDTO.Name, AnalizedFiles = ProyectDataDTO.AnalizedFiles.Select(x => new AnalizedFile() { FileName = x.FileName, NicName = x.NicName, OriginalFileName = x.OriginalFileName }).ToList() };
            MapHelper.HexMap(ProyectData, AnalizedFileDTO.FileName, ref HexagonGrid);
            var hexs = HexagonGrid.HexagonMap;
            string DestineFile = Path.Combine(ProyectData.Location.ProyectFolder, ProyectData.Location.MapsFolder) + AnalizedFileDTO.NicName + Guid.NewGuid();
            using (StreamWriter StreamWriter = new StreamWriter( DestineFile + ".jsonHex"))
            {
                //var hexText = JsonConvert.SerializeObject(hexs.Select(hex => new object[] { hex.BorderColor, hex.BorderType, hex.Color, hex.Opacity, hex.RGBColor, hex.Q, hex.R, hex.S, hex.Value, hex.Values }));
                foreach (var hex in hexs)
                {

                    StreamWriter.WriteLine(hex.ListValues);

                    //StreamWriter.Write(JsonConvert.SerializeObject( hexs.Select(hex => new object[] { hex.BorderColor, hex.BorderType, hex.Color, hex.Opacity, hex.RGBColor, hex.Q, hex.R, hex.S, hex.Value, hex.Values })));
                    StreamWriter.Flush();
                }

                StreamWriter.Close();
            }
            return CreadorMapaService.CreadorMapa(ref HexagonGrid, DestineFile);

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
        public List<ProyectDataDTO> GetProyects(string Path, string Name, AnalizedFileDTO AnalizedFileDTO =null )
        {
            var ret = FilesHelper.ReadProject(Path, Name, AnalizedFileDTO);

            return ret;
        }
        public AnalizedFileDTO GetFilesinProjects(string Path, string Name, AnalizedFileDTO DataName)
        {

            var Project = FilesHelper.ReadProject(Path, Name, DataName).Where(x=>x.Name==Name).FirstOrDefault();
            var files = Project.AnalizedFiles.Where(x=>x.NicName==DataName.NicName).First();
            
            return files;
        }
    }
}
