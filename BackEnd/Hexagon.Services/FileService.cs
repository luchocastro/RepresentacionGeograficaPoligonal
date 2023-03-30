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
using Hexagon.Model.Repository;
using Microsoft.AspNetCore.Http;
using Hexagon.Model.FileDataManager;
using System.Drawing;

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
        private readonly IDataRepository<ProyectDataDTO, ProyectData> IDataRepository;
        private readonly IFileDataManagerOptions FileDataManagerOptions;
        private readonly IDataRepository<UserDTO, User> DataUser;
        private IDataRepository<AnalizedFileDTO, AnalizedFile> AnalizedFileDataManager;
        private IDataRepository<NativeFileDTO , NativeFile> NativeFileDataManager;

        private readonly IDataRepository<HexFileDTO , HexFile> FileRepository;
        private IDataRepository<LayoutDto, Layout> LayoutDataManager;
        private IDataRepository<MapDefinitionDTO, MapDefinition> MapDefinitionDataManager;
        private IDataRepository<HexagonDetailsDTO, HexagonDetails> HexagonDetailsManager;
        private IDataRepository<FunctionDTO, Function> FunctionManager;
        private IDataRepository<DataFileConfigurationDTO, DataFileConfiguration> DataFileConfigurationManager;
        private readonly User User = null;
        public FileService(IConfiguration Configuration,IMapper Mapper, 
            IDataRepository<ProyectDataDTO, ProyectData> IDataRepository,
            IDataRepository<HexFileDTO, HexFile> FileRepository,
            IDataRepository<AnalizedFileDTO, AnalizedFile> AnalizedFileDataManager,
            IDataRepository<UserDTO, User>  DataUser,
            IFileDataManagerOptions IFileDataManagerOptions ,
            IDataRepository<NativeFileDTO, NativeFile> NativeFileDataManager,
            IDataRepository<LayoutDto, Layout> LayoutDataManager,
            IDataRepository<MapDefinitionDTO, MapDefinition> MapDefinitionDataManager,
            IDataRepository<HexagonDetailsDTO, HexagonDetails> HexagonDetailsDataManager,
            IDataRepository<FunctionDTO, Function> FunctionManager,
            IDataRepository<DataFileConfigurationDTO, DataFileConfiguration> DataFileConfigurationManager
            )
        {
            this.DataUser = DataUser;
            this.IDataRepository = IDataRepository;
            this.AnalizedFileDataManager = AnalizedFileDataManager;
            this.FileRepository  = FileRepository;
            _Configuration = Configuration;
            FileDataManagerOptions = IFileDataManagerOptions;
            _Mapper = Mapper;
            this.NativeFileDataManager = NativeFileDataManager;
            this.LayoutDataManager = LayoutDataManager;
            this.HexagonDetailsManager = HexagonDetailsDataManager;
            this.MapDefinitionDataManager = MapDefinitionDataManager;
            this.FunctionManager = FunctionManager;
            this.DataFileConfigurationManager = DataFileConfigurationManager;

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

         



            return NativeFile;

        } 
        public NativeFileDTO ConvertFile (  DataFileConfigurationDTO FileData,  string  HexFileID )
        {
            NativeFileDTO NativeFileDto = new NativeFileDTO();
            var File = FileRepository.Get(HexFileID);
            
            
            NativeFileDto.FileName = FileData.FileType +"_"+  File.FileName + ".HexJson";
            NativeFileDto.ParentID = File.ID;
            var NativeFile = NativeFileDataManager.Get(NativeFileDto);
            //var PathFile = Path.Combine(ProyectDataDTO.Location.ProyectFolder, NicData, ProyectDataDTO.Location.FileFolder, ProyectDataDTO.AnalizedFiles.FirstOrDefault(x => x.NicName == NicData).FileName);
            if (NativeFile == null)
            {
                NativeFile = GetJsonSerializedFileFromFile(File, FileData);
                NativeFile.ParentID = HexFileID;
                NativeFile.FileName = FileData.FileType + "_" +  File.FileName  + ".HexJson";
                NativeFile.DataFileConfiguration = _Mapper.Map<DataFileConfiguration>(FileData);
                var Natid = NativeFileDataManager.GenerateFullID(NativeFile);
                NativeFile.PathFile = Path.Combine(Path.GetDirectoryName(Path.Combine(FileRepository.ParentDirectory(), Natid)), NativeFile.FileName);
                NativeFileDto = NativeFileDataManager.Add(NativeFile);
                foreach (var item in NativeFile.Columns)
                {
                    var NativeCol = new NativeFile();
                    NativeCol.Columns = NativeFile.Columns.Where(x => x.Name == item.Name).ToList();
                    NativeCol.ParentID = HexFileID;
                    NativeCol.FileName = "Column_" + item.OriginalPosition.ToString() + "_"+NativeFile.FileName + ".HexJson";
                    NativeCol.Content = NativeFile.Content.Select(x => new Line { Fieds = new string[] { x.Fieds[item.OriginalPosition] }, Number = x.Number }).ToList();
                    NativeCol.DataFileConfiguration = _Mapper.Map<DataFileConfiguration>(FileData);
                    Natid = NativeFileDataManager.GenerateFullID(NativeCol);
                    NativeCol.PathFile = Path.Combine(Path.GetDirectoryName(Path.Combine(FileRepository.ParentDirectory(), Natid)), NativeFile.FileName);
                    NativeFileDto = NativeFileDataManager.Add(NativeCol);
                }
            }
            else
            { 
                NativeFileDto = _Mapper.Map<NativeFileDTO>(NativeFile);
            }

            //return new NativeJsonFileDTO { Content = NativeJsonFile.Content, Columns = NativeJsonFile.Columns };



            var max = NativeFile.Content.Count() > 100 ? 100 : NativeFile.Content.Count();
            return new NativeFileDTO ()  {FileName=NativeFileDto.FileName, Columns=NativeFileDto.Columns, 
                ParentID = NativeFileDto.ParentID, PathFile= NativeFileDto.PathFile, 
                DataFileConfigurationDTO= NativeFileDto.DataFileConfigurationDTO, Content=NativeFileDto.Content.GetRange(0, max),
                ID = NativeFileDto.ID};

        }
        public List<DataFileConfigurationDTO> GetDataFileConfiguration(string Path)
        {
            return DataFileConfigurationManager.GetColectionFromParent("").ToList();
            
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

        public LayoutDto  ConvertFileToHexList(NativeFileDTO NativeToUse, LayoutDto LayoutDto)
        {

            var Layout = _Mapper.Map<Layout>(LayoutDto);
            var HexagonDetails = new HexagonDetails();
             var HexagonGrid = new HexagonGrid() ;
            HexagonGrid.Layout = Layout;
            MapHelper.HexMap(NativeToUse, ref HexagonGrid, ref HexagonDetails);

            var ToFile = new HexagonGridForFile
            {

                HexagonsHexagonMap = JsonConvert.SerializeObject(HexagonGrid.HexagonMap.Select(x => x.ListValues)),
                //FullClassName = JsonConvert.SerializeObject(HexagonGrid.Layout.MapDefinition.Function),
                Layout = JsonConvert.SerializeObject(HexagonGrid.Layout)
            }; 

            return _Mapper.Map<LayoutDto>(HexagonGrid.Layout);
        }
        public NativeFile GetJsonSerializedFileFromFile(HexFileDTO HexFile, DataFileConfigurationDTO DataFileConfigurationDTO)
        {
            try
            {
                object FileType = new object();
                object FileProperties = new object();
                NativeFile NativeFile = new NativeFile();
                
                    var clazz = Type.GetType("Hexagon.Services.ConvertSourceFileToJsonStrategy.Convert" + DataFileConfigurationDTO.FileType + "ToJsonStrategy");
                    var Strategy = (IConvertSourceFileToJsonStrategy)Activator.CreateInstance(clazz);
                var FileData = _Mapper.Map<DataFileConfiguration> (DataFileConfigurationDTO);
                    return Strategy.DoFromFile(Path.Combine(HexFile.Path ), FileData);
                
                

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
        public string GenerateImge(LayoutDto Layout, string NativeFileID)
        {
            NativeFileDTO NativeFile = NativeFileDataManager.Get(NativeFileID);
            var layout = _Mapper.Map<Layout>(Layout);
            var QColumns = NativeFile.Columns.Count();
            var HexDetailList = new List<HexagonDetail>();
            var HexagonDetails = new HexagonDetails();
            HexagonDetails.Columns = NativeFile.Columns.Select(x=> _Mapper.Map<Column>(x)).ToList(); 
            var Map = layout.MapDefinition;
            long NumLine = 0;

            if (layout.MapDefinition.ColumnForMapGroup != "")
            {
                var col = NativeFile.Columns.Where(x => x.Name == Map.ColumnForMapGroup).FirstOrDefault();

                var pols = NativeFile.Content.Select(x => x.Fieds[col.OriginalPosition]);
                var PoliygonList = pols.Select(X => X.Split(",").Select(y => new Model.Point((float)Convert.ToDouble(y.Split(":") [0]), (float)Convert.ToDouble(y.Split(":")[1]))).ToList()).SelectMany(x => x.ToArray()).ToList();
                var ImageDifinition = new ImageDefinition(PoliygonList, layout);


                layout.Size = new System.Drawing.PointF(ImageDifinition.HexagonSize, ImageDifinition.HexagonSize);
                layout.Origin = new PointF(ImageDifinition.TransformedWidth / 2f, ImageDifinition.TransformedHeigth / 2f);
                layout.MaxPictureSizeX = ImageDifinition.TransformedWidth;
                layout.MaxPictureSizeY = ImageDifinition.TransformedHeigth; 
                ;
                var ret = new List<Hex>();

                
                foreach (var Line in NativeFile.Content)
                {

                    var HexagonDetail = new HexagonDetail();
                    HexagonDetail.IndexLines.Add((long)Line.Number);
                    if (QColumns != Line.Fieds.Count())
                    {
                        NumLine++;
                        continue;
                    }
                    Hex HexAnterior = new Hex();

                    var linea = new List<Hex>();
                    var PointsHexCornes = new List<PointF>();
                    var item = Line.Fieds[col.OriginalPosition].Split(",") ;
                    ret = new List<Hex>();
                     for (int i = 0; i < item.Count(); i++)
                    {

                        var XOriginal = float.Parse(item[i].Split(":")[0]);

                        var YOriginal = float.Parse(item[i].Split(":")[1]);
                        var EventPoint = new EventPoint() { PositionInMeters = new PointF(XOriginal,YOriginal )   };
                        var X = Layout.Size.X * 2 + (EventPoint.PositionInMeters.X - ImageDifinition.OriginalMinX) * ImageDifinition.ProportationToScale;
                        var Y = MathF.Sqrt(3) * Layout.Size.Y + (ImageDifinition.OriginalMaxY - EventPoint.PositionInMeters.Y) * ImageDifinition.ProportationToScale;
                         var hexPosition1 = HexagonFunction.PixelToHexagon(layout,
                                                 new Model.Point(X, Y));
                        var Existe = false;
                        //if (ret.Contains(hexPosition1))
                        //{
                        //    Existe = true;
                        //    hexPosition1 = ret[ret.IndexOf(hexPosition1)];

                        //    hexPosition1.Values.Add(item);
                        //    ret[ret.IndexOf(hexPosition1)] = hexPosition1 ;

                        //}
                        //else
                        {
                            hexPosition1.Values = new List<EventPoint>();

                            //hexPosition1.Values.Add(Line );
                            ret.Add(hexPosition1);
                        }
                        PointsHexCornes.Add(new PointF(HexagonFunction.HexagonToPixel(layout, hexPosition1).X, HexagonFunction.HexagonToPixel(layout, hexPosition1).Y));
                        if (i != 0 && hexPosition1 != HexAnterior)
                        {
                            linea = HexagonFunction.HexagonLinedraw(hexPosition1, HexAnterior);
                            linea.Remove(hexPosition1);
                            ret.AddRange(linea);

                        }
                        HexAnterior = HexagonFunction.PixelToHexagon(layout,
                                                 new Model.Point(X, Y));
                    }
                    if (Layout.FillPolygon)
                       Helpers.MapHelper.PaintHexInsidePolygon(PointsHexCornes, ref ret, layout);
                    var Grupo = ret.GroupBy(x => x.ToString());
                    var RetGroup = new List<Hex>();
                    foreach (var grupo in Grupo)
                    {
                        var hex = grupo.First();
                        if (hex.Values == null)
                            hex.Values = new List<EventPoint>();
                        var hexs = grupo.Select(X => X).ToList();
                        for (int i = 1; i < hexs.Count(); i++)
                        {
                            if (hexs[i].Values != null)
                                hex.Values.AddRange(hexs[i].Values);

                        }
                        RetGroup.Add(hex);
                    }
                    HexagonDetail.HexagonPositionForValues = RetGroup.Select(x => new HexagonPosition { Q = x.Q, R = x.R, S = x.S }).ToList();
NumLine++;          HexDetailList.Add(HexagonDetail);

                }
                HexagonDetails.List = HexDetailList;
            }
            else
            {
                var colX = NativeFile.Columns.Where(x => x.Name == Map.ColumnNameForX).FirstOrDefault();
                var colY = NativeFile.Columns.Where(x => x.Name == Map.ColumnNameForY).FirstOrDefault();
                layout.PaintLines = false;
                var PoliygonList = NativeFile.Content.Where(x => x.Fieds.Count() == QColumns).Select(y => new Model.Point((float)Convert.ToDouble(y.Fieds[colX.OriginalPosition].ToString().Replace(".", ",")), (float)Convert.ToDouble(y.Fieds[colY.OriginalPosition].ToString().Replace(".", ",")))).ToList();
                var ImageDifinition = new ImageDefinition(PoliygonList, layout);
                layout.Size = new System.Drawing.PointF(ImageDifinition.HexagonSize, ImageDifinition.HexagonSize);
                layout.Origin = new PointF(ImageDifinition.TransformedWidth / 2f, ImageDifinition.TransformedHeigth / 2f);
                layout.MaxPictureSizeX = ImageDifinition.TransformedWidth;
                layout.MaxPictureSizeY = ImageDifinition.TransformedHeigth;
                //layout = new Layout(Layout.Flat, new System.Drawing.PointF(ImageDifinition.HexagonSize, ImageDifinition.HexagonSize), new PointF(ImageDifinition.TransformedWidth / 2f, ImageDifinition.TransformedHeigth / 2f), Layout.HexPerLine, ImageDifinition.TransformedWidth, ImageDifinition.TransformedHeigth, Layout.FillPolygon);
                var QLines = NativeFile.Content.Count();
                foreach (var Line in NativeFile.Content)
                {
                    var HexagonDetail = new HexagonDetail();
                    HexagonDetail.IndexLines.Add(NumLine);

                    if (QColumns != Line.Fieds.Count())
                    {
                        NumLine++;
                        continue;
                    }
                    var EventPoint = new EventPoint() { PositionInMeters = new PointF((float)Convert.ToDouble(Line.Fieds[colX.OriginalPosition].ToString().Replace(".", ",")), (float)Convert.ToDouble(Line.Fieds[colY.OriginalPosition].ToString().Replace(".", ",")))  };
                    var X = Layout.Size.X * 2 + (EventPoint.PositionInMeters.X - ImageDifinition.OriginalMinX) * ImageDifinition.ProportationToScale;
                    var Y = MathF.Sqrt(3) * Layout.Size.Y + (ImageDifinition.OriginalMaxY - EventPoint.PositionInMeters.Y) * ImageDifinition.ProportationToScale;
                    var hexPosition1 = HexagonFunction.PixelToHexagon(layout,
                                             new Model.Point(X, Y));
                     
                    var ListLine = new List<Line>();
                    var HexagonPosition = new HexagonPosition { Q = hexPosition1.Q, R = hexPosition1.R, S = hexPosition1.S };
                    var IdexHexDetail = HexDetailList.FindLastIndex(x => x.HexagonPositionForValues.Exists(y => y.Q == hexPosition1.Q && y.R == hexPosition1.R && y.S == hexPosition1.S));
                    if (IdexHexDetail==-1)
                    {//if (hexPosition1.Values == null)
                            
                        HexagonDetail = new HexagonDetail();
                        HexagonDetail.HexagonPositionForValues.Add(HexagonPosition);
                        HexagonDetail.IndexLines.Add(NumLine) ;
                        HexDetailList.Add(HexagonDetail);
                    }
                    else
                    {


                        HexagonDetail = new HexagonDetail();
                        HexagonDetail.HexagonPositionForValues.Add(HexagonPosition);
                        HexagonDetail.IndexLines.Add((long)Line.Number);
                        HexDetailList.Add(HexagonDetail);

                    }
                    NumLine++;
                }
                HexagonDetails.List = HexDetailList;


            }
            layout.ParentID = NativeFile.ID;
            layout.ID = layout.Name;
            var lay = LayoutDataManager.Add(layout);
            HexagonDetails.ParentID = lay.ID;
            HexagonDetails.Name = DateTime.Now.ToString("ddMMyyyyhhmmss");
            HexagonDetailsManager.Add(HexagonDetails);
            return "";
        }
            public string GenerateImge4(LayoutDto Layout, string NativeFileID )
        {
            HexagonGrid HexagonGrid = new HexagonGrid();
            NativeFileDTO NativeFile = NativeFileDataManager.Get(NativeFileID);
           var layout = _Mapper.Map<Layout>(Layout);
            HexagonGrid.Layout = layout;
            var HexagonDetails = new HexagonDetails();
            MapHelper.HexMap(NativeFile ,ref HexagonGrid, ref HexagonDetails);
            var hexs = HexagonGrid.HexagonMap;
            string DestineFile = Path.Combine(Path.GetDirectoryName(NativeFile.PathFile), Enum.GetName(typeof(EnumFileType), EnumFileType.HexaDetails), NativeFile.FileName);

            if (!Directory.Exists(Path.GetDirectoryName(DestineFile)))
                Directory.CreateDirectory(Path.GetDirectoryName(DestineFile));

            using (StreamWriter StreamWriter = new StreamWriter(DestineFile + ".Hex.json"))
            {

                StreamWriter.Write(JsonConvert.SerializeObject(HexagonDetails));
            }
            DestineFile = Path.Combine(Path.GetDirectoryName(NativeFile.PathFile), Enum.GetName(typeof(EnumFileType), EnumFileType.MapFile), NativeFile.FileName);
            
            if (!Directory.Exists(Path.GetDirectoryName(DestineFile)))
                Directory.CreateDirectory(Path.GetDirectoryName(DestineFile));

            using (StreamWriter StreamWriter = new StreamWriter( DestineFile + ".Hex.json"))
            {

                foreach (var hex in hexs)
                {
 
                    var HexToFile = new
                    {
                        Q = hex.Q,
                        R = hex.R,
                        S = hex.S,
                        RGBColor = hex.RGBColor,
                        Value = hex.Value,
                        BorderColor = hex.BorderColor,
                        BorderType = hex.BorderType,
                        Points= HexagonFunction.GetPoints(hex,layout) ,
                        ListValues=hex.ListValues
                    };

                    StreamWriter.WriteLine(JsonConvert.SerializeObject(HexToFile));

                    //StreamWriter.Write(JsonConvert.SerializeObject( hexs.Select(hex => new object[] { hex.BorderColor, hex.BorderType, hex.Color, hex.Opacity, hex.RGBColor, hex.Q, hex.R, hex.S, hex.Value, hex.Values })));
                    StreamWriter.Flush();
                }

                StreamWriter.Close();
            }
           return DestineFile + ".Hex.json";
        }
        public string GenerateImge2(LayoutDto Layout, string PathFile)
        {
            HexagonGrid HexagonGrid = new HexagonGrid();
 
            HexagonGrid.Layout = _Mapper.Map <Layout>( Layout);
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
        public List<ProyectDataDTO> GetProyects(string User )
        {
            ;
            var  UserDTO = DataUser.Get( User);
            return IDataRepository.GetColectionFromParent(UserDTO.ID).ToList();

        }
        public List<AnalizedFileDTO> GetAnalizedFiles(string ProyectDataID)
        {
            

            return AnalizedFileDataManager.GetColectionFromParent(ProyectDataID).ToList();

        }
        public List<HexFileDTO> GetHexFiles(string AnalizedFileID)
        {
             
            return FileRepository.GetColectionFromParent(AnalizedFileID).ToList();

        }
        public HexFileDTO PutFile(string User, string Project, string NicName, IFormFile IFormFile)
        {
            ;
            var file = IFormFile;
            HexFileDTO fileDTO = new HexFileDTO();
            if (file.Length > 0)
            {
                var FileName = Guid.NewGuid().ToString() + ".FILE";
                var OriginalName = file.FileName;
                var Nic = NicName;// Path.GetFileNameWithoutExtension(file.FileName);

                fileDTO = PutFile(User, Project, NicName, FileName, OriginalName);
            
            using (var stream = new FileStream(fileDTO.Path, FileMode.Create))
            {
                 file.CopyTo  (stream);
            }
            }
            return fileDTO;
        }
        public HexFileDTO PutFile(string User, string Project, string NicName, string FileName, string OriginalName)
            {

            
            var Proyect = GetProyect(User, Project);
            var AnalizedFileId=AnalizedFileDataManager.GetID(NicName, Proyect.ID, typeof(AnalizedFile));
            AnalizedFileDTO AnalizedFileDTO = AnalizedFileDataManager.Get(AnalizedFileId);
            //AnalizedFileDTO = AnalizedFileDataManager.FindByCondition (NicName );
            if (AnalizedFileDTO == null)
            {
                AnalizedFileDTO = new AnalizedFileDTO();
                //AnalizedFileDTO.ID = Path.Combine(Proyect.Name, NicName, typeof(AnalizedFile).Name);
                AnalizedFileDTO.ParentID = Proyect.ID;
                AnalizedFileDTO.NicName = NicName;
                AnalizedFileDTO = AnalizedFileDataManager.Add(this._Mapper.Map<AnalizedFile>(AnalizedFileDTO));
            }

            var fileDTO = new HexFileDTO();
            
            fileDTO = new HexFileDTO();
            fileDTO.OriginalFileName =  OriginalName;
            fileDTO.FileName = FileName;
            fileDTO.ParentID = AnalizedFileDTO.ID;
            fileDTO.TypeFile = Enum.GetName(typeof(EnumFileType), EnumFileType.Original);
            var File = FileRepository.Get (fileDTO);

            if (File == null)
            {
                var id = FileRepository.GenerateFullID(_Mapper.Map<HexFile>(fileDTO));
                fileDTO.Path = Path.Combine( Path.GetDirectoryName( Path.Combine(FileRepository.ParentDirectory() , id)), FileName);
                 fileDTO = FileRepository.Add(_Mapper.Map<HexFile> (fileDTO));
            }


            return fileDTO;
        }
        public  ProyectDataDTO GetProyect(string  User, string Name )
        {
            var UserDT = DataUser.Get(User);
            var ID = IDataRepository.GetID(Name, UserDT.ID, typeof(ProyectData));
            var ProyectDataDTO = IDataRepository.Get( ID);

            if (ProyectDataDTO == null)
            {
                
                var Location = new Location(Name, ServicesConstants.MapsDirectory, ServicesConstants.FilesDirectory, ServicesConstants.NativesDirectory);

                ProyectDataDTO = IDataRepository.Add(new ProyectData { ID=Name , Name = Name, Location = Location, ParentID=UserDT.ID });
;
                //FileRepository.Add()
            } 


            return ProyectDataDTO;
        }
        public AnalizedFileDTO GetFilesinProjects(string Path, string Name, AnalizedFileDTO DataName)
        {

            //var Project = FilesHelper.ReadProject(Path, Name, DataName).Where(x=>x.Name==Name).FirstOrDefault();
            //var files = Project.AnalizedFiles.Where(x=>x.NicName==DataName.NicName).First();
            
            return null;
        }

        public AnalizedFileDTO ConvertFileToHexList(ProyectDataDTO ProyectDataDTO, AnalizedFileDTO AnalizedFileDTO, LayoutDto LayoutDto)
        {
            throw new NotImplementedException();
        }
    }
}
