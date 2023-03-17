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
        private readonly User User = null;
        public FileService(IConfiguration Configuration,IMapper Mapper, 
            IDataRepository<ProyectDataDTO, ProyectData> IDataRepository,
            IDataRepository<HexFileDTO, HexFile> FileRepository,
            IDataRepository<AnalizedFileDTO, AnalizedFile> AnalizedFileDataManager,
            IDataRepository<UserDTO, User>  DataUser,
            IFileDataManagerOptions IFileDataManagerOptions ,
            IDataRepository<NativeFileDTO, NativeFile> NativeFileDataManager
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
            }
            else
            {
                NativeFileDto = _Mapper.Map<NativeFileDTO>(NativeFile);
            }
                        
            //return new NativeJsonFileDTO { Content = NativeJsonFile.Content, Columns = NativeJsonFile.Columns };




            return new NativeFileDTO ()  {FileName=NativeFileDto.FileName, Columns=NativeFileDto.Columns, 
                ParentID = NativeFileDto.ParentID, PathFile= NativeFileDto.PathFile, 
                DataFileConfigurationDTO= NativeFileDto.DataFileConfigurationDTO, Content=NativeFileDto.Content.GetRange(0,100),
                ID = NativeFileDto.ID};

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
                FullClassName = JsonConvert.SerializeObject(HexagonGrid.Layout.MapDefinition.Function),
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
                    var FileData = new DataFileConfiguration() { FileType = DataFileConfigurationDTO.FileType, FileProperties = DataFileConfigurationDTO.FileProperties };
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
        public string GenerateImge(LayoutDto Layout, string NativeFileID )
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
