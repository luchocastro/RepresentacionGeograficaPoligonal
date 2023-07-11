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
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Serialization;
using Hexagon.AsyncIO;
using System.ComponentModel;
using System.Text;
using Hexagon.Model.Helper;
using Point = Hexagon.Model.Point;

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
        private readonly IDataRepository<UserDTO, User> DataUser;
        private IDataRepository<AnalizedFileDTO, AnalizedFile> AnalizedFileDataManager;
        private IDataRepository<NativeFileDTO, NativeFile> NativeFileDataManager;
        private readonly IDataRepository<HexFileDTO, HexFile> FileRepository;
        private IDataRepository<LayoutDto, Layout> LayoutDataManager;
        private IDataRepository<MapDefinitionDTO, MapDefinition> MapDefinitionDataManager;
        private IDataRepository<HexagonDetailsDTO, HexagonDetails> HexagonDetailsManager;
        private IDataRepository<FunctionDTO, Function> FunctionManager;
        private IDataRepository<CalculatedHexagonDTO, CalculatedHexagon> CalculatedHexagonManager;
        private IDataRepository<ColumnDTO, Column> ColumnManager;
        private IDataRepository<DataFileConfigurationDTO, DataFileConfiguration> DataFileConfigurationManager;
        private IDataRepository<PaletteClassDTO, PaletteClass> PaletteClassManager;
        private readonly User User = null;
        private readonly FileDataManagerOptions FileDataManagerOptions;
        public FileService(IConfiguration Configuration, IMapper Mapper,
            IDataRepository<ProyectDataDTO, ProyectData> IDataRepository,
            IDataRepository<HexFileDTO, HexFile> FileRepository,
            IDataRepository<AnalizedFileDTO, AnalizedFile> AnalizedFileDataManager,
            IDataRepository<UserDTO, User> DataUser,
            IFileDataManagerOptions IFileDataManagerOptions,
            IDataRepository<NativeFileDTO, NativeFile> NativeFileDataManager,
            IDataRepository<LayoutDto, Layout> LayoutDataManager,
            IDataRepository<MapDefinitionDTO, MapDefinition> MapDefinitionDataManager,
            IDataRepository<HexagonDetailsDTO, HexagonDetails> HexagonDetailsDataManager,
            IDataRepository<FunctionDTO, Function> FunctionManager,
            IDataRepository<DataFileConfigurationDTO, DataFileConfiguration> DataFileConfigurationManager,

            IDataRepository<CalculatedHexagonDTO, CalculatedHexagon> CalculatedHexagonManager,
        IDataRepository<ColumnDTO, Column> ColumnManager,
        IDataRepository<PaletteClassDTO, PaletteClass> PaletteClassManager

            )
        {
            this.DataUser = DataUser;
            this.IDataRepository = IDataRepository;
            this.AnalizedFileDataManager = AnalizedFileDataManager;
            this.FileRepository = FileRepository;
            _Configuration = Configuration;
            FileDataManagerOptions = IFileDataManagerOptions.Get();
            _Mapper = Mapper;
            this.NativeFileDataManager = NativeFileDataManager;
            this.LayoutDataManager = LayoutDataManager;
            this.HexagonDetailsManager = HexagonDetailsDataManager;
            this.MapDefinitionDataManager = MapDefinitionDataManager;
            this.FunctionManager = FunctionManager;
            this.DataFileConfigurationManager = DataFileConfigurationManager;
            this.ColumnManager = ColumnManager;
            this.CalculatedHexagonManager = CalculatedHexagonManager;
            this.PaletteClassManager = PaletteClassManager;

        }
        public NativeJsonFileDTO ConvertFileBase64(string Base64File, DataFileConfigurationDTO FileData)
        {
            NativeJsonFile NativeJsonFile = new NativeJsonFile();

            NativeJsonFile = GetJsonSerializedFile(Base64File, FileData);
            //return new NativeJsonFileDTO { Content = NativeJsonFile.Content, Columns = NativeJsonFile.Columns };

            var columns = NativeJsonFile.Columns.Select(x => new ColumnDTO(x.Name, x.OriginalPosition,
                (EnumActionToDoWithUncastedDTO)Enum.Parse(typeof(EnumActionToDoWithUncasted), x.ActionToDoWithUncasted.ToString()),
                (EnumAlowedDataTypeDTO)Enum.Parse(typeof(EnumAlowedDataType), x.DataTypeSelected.ToString())));


            return new NativeJsonFileDTO { Content = NativeJsonFile.Content, Columns = columns.ToList() };

        }
        public NativeFileDTO ConvertFile(string PathFile, DataFileConfigurationDTO FileData, LayoutDto Layout)
        {
            NativeFileDTO NativeFile = new NativeFileDTO();





            return NativeFile;

        }
        public async Task<NativeFileDTO> ConvertFileAsync(DataFileConfigurationDTO FileData, string HexFileID)
        {
            NativeFileDTO NativeFileDto = new NativeFileDTO();
            var File = FileRepository.Get(HexFileID);


            NativeFileDto.FileName = FileData.FileType + "_" + File.FileName;
            NativeFileDto.ParentID = File.ID;
            var NativeFile = NativeFileDataManager.Get(NativeFileDto);
            //var PathFile = Path.Combine(ProyectDataDTO.Location.ProyectFolder, NicData, ProyectDataDTO.Location.FileFolder, ProyectDataDTO.AnalizedFiles.FirstOrDefault(x => x.NicName == NicData).FileName);
            if (NativeFile == null)
            {
                NativeFile = new NativeFile();
                NativeFile.ParentID = HexFileID;
                NativeFile.FileName = FileData.FileType + "_" + File.FileName;
                NativeFile.DataFileConfiguration = _Mapper.Map<DataFileConfiguration>(FileData);
                var Natid = NativeFileDataManager.GenerateFullID(NativeFile);
                //NativeFile.Path = Path.Combine(NativeFileDataManager.ParentDirectory(), Natid, NativeFile.FileName +);

                NativeFile = GetJsonSerializedFileFromFileAsync(File, FileData, NativeFile).Result;

                NativeFileDataManager.Add(NativeFile);
            }


            //return new NativeJsonFileDTO { Content = NativeJsonFile.Content, Columns = NativeJsonFile.Columns };




            return _Mapper.Map<NativeFileDTO>(NativeFile);

        }

        public NativeFileDTO ConvertFile(DataFileConfigurationDTO FileData, string HexFileID)
        {
            NativeFileDTO NativeFileDto = new NativeFileDTO();
            var File = FileRepository.Get(HexFileID);


            NativeFileDto.FileName = FileData.FileType + "_" + File.FileName + ".HexJson";
            NativeFileDto.ParentID = File.ID;
            var NativeFile = NativeFileDataManager.Get(NativeFileDto);
            //var PathFile = Path.Combine(ProyectDataDTO.Location.ProyectFolder, NicData, ProyectDataDTO.Location.FileFolder, ProyectDataDTO.AnalizedFiles.FirstOrDefault(x => x.NicName == NicData).FileName);
            if (NativeFile == null)
            {
                NativeFile = GetJsonSerializedFileFromFile(File, FileData);
                NativeFile.ParentID = HexFileID;
                NativeFile.FileName = FileData.FileType + "_" + File.FileName + ".HexJson";
                NativeFile.DataFileConfiguration = _Mapper.Map<DataFileConfiguration>(FileData);
                var Natid = NativeFileDataManager.GenerateFullID(NativeFile);
                NativeFile.PathFile = Path.Combine(Path.GetDirectoryName(Path.Combine(FileRepository.ParentDirectory(), Natid)), NativeFile.FileName);
                NativeFileDto = NativeFileDataManager.Add(NativeFile);
                NativeFileDto = _Mapper.Map<NativeFileDTO>(NativeFile);

            }
            else
            {
                NativeFileDto = _Mapper.Map<NativeFileDTO>(NativeFile);
            }

            //return new NativeJsonFileDTO { Content = NativeJsonFile.Content, Columns = NativeJsonFile.Columns };



            return new NativeFileDTO()
            {
                FileName = NativeFileDto.FileName,
                Columns = NativeFileDto.Columns,
                ParentID = NativeFileDto.ParentID,
                PathFile = NativeFileDto.PathFile,
                DataFileConfigurationDTO = NativeFileDto.DataFileConfigurationDTO,
                ID = NativeFileDto.ID
            };

        }
        public List<DataFileConfigurationDTO> GetDataFileConfiguration(string Path)
        {
            return DataFileConfigurationManager.GetColectionFromParent("").ToList();

        }

        public NativeFileDTO GetFileColumsFromFile(DataFileConfigurationDTO FileData, string HexFileID, int FirstNRows)
        {



            NativeFileDTO NativeFileDto = new NativeFileDTO();
            var File = FileRepository.Get(HexFileID);

            NativeFileDto.FileName = "FirstNRows_" + FileData.FileType + "_" + File.FileName + ".HexJson";
            NativeFileDto.ParentID = File.ID;
            var NativeFile = NativeFileDataManager.Get(NativeFileDto);
            //var PathFile = Path.Combine(ProyectDataDTO.Location.ProyectFolder, NicData, ProyectDataDTO.Location.FileFolder, ProyectDataDTO.AnalizedFiles.FirstOrDefault(x => x.NicName == NicData).FileName);
            if (NativeFile == null)
            {
                NativeFile = GetJsonSerializedFileFromFile(File, FileData);
                NativeFile.ParentID = HexFileID;
                NativeFile.FileName = FileData.FileType + "_" + File.FileName + ".HexJson";
                NativeFile.DataFileConfiguration = _Mapper.Map<DataFileConfiguration>(FileData);
                var Natid = NativeFileDataManager.GenerateFullID(NativeFile);
                NativeFileDto = NativeFileDataManager.Add(NativeFile);

            }

            else
            {
                NativeFileDto = _Mapper.Map<NativeFileDTO>(NativeFile);
            }
            return NativeFileDto;
        }
        public AnalizedFileDTO GetAnalizedFile(string PathFile, DataFileConfigurationDTO DataFileConfigurationDTO)
        {
            return new AnalizedFileDTO();
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
                var FileData = _Mapper.Map<DataFileConfiguration>(DataFileConfigurationDTO);
                return Strategy.DoFromFile(Path.Combine(HexFile.Path), FileData);



            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
    //    public async Task<Column> GenerateCoorPair(string HexID, string ColumnForX, String ColumnForY)
    //    {
    //        var columns = ColumnManager.GetColectionFromParent(HexID).Where(x => x.Name.ToUpper() == ColumnForX.ToUpper() || x.Name.ToUpper() == ColumnForY.ToUpper()).ToList();
    //        if (columns.Count() != 2)
    //            return default;
    //        var ColumnX = columns.Where(x => x.Name == ColumnForX).FirstOrDefault();
    //        var ColumnY = columns.Where(x => x.Name == ColumnForY).FirstOrDefault();
    //        var ColumnXY = new Column();
    //        ColumnXY.ID = "";
    //        ColumnXY.Name = "ColumnXY" + Guid.NewGuid(); ;
    //        ColumnXY.ParentID = HexID;
    //        ColumnXY.ID = ColumnManager.GenerateFullID(ColumnXY);
    //        ColumnXY.PathFields = Path.Combine(Path.GetDirectoryName(columns[0].PathFields), ColumnXY.Name + FileDataManagerOptions.DefaultExtension));
    //        var t = Task.Run(() =>
    //        {
    //            bool merge = true;
    //            while (merge)
    //            {
    //                var x = "";
    //                var y = "";
    //                using (StreamReader StreamReaderX = new StreamReader(columns[0].PathFields))
    //                {
    //                    x = StreamReaderX.ReadLine();
    //                }
    //                using (StreamReader StreamReaderY = new StreamReader(columns[1].PathFields))
    //                {
    //                    y = StreamReaderY.ReadLine();
    //                }

    //            }
    //        });
    //    }
    //}
            public async Task<NativeFile> GetJsonSerializedFileFromFileAsync(HexFileDTO HexFile, DataFileConfigurationDTO DataFileConfigurationDTO, NativeFile NativeFile)
        {
            try
            {
                object FileType = new object();
                object FileProperties = new object();
                var clazz = Type.GetType("Hexagon.Services.ConvertSourceFileToJsonStrategy.Convert" + DataFileConfigurationDTO.FileType + "ToJsonStrategy");
                var Strategy = (IConvertSourceFileToJsonStrategy)Activator.CreateInstance(clazz);
                Strategy.ColumnRepository = this.ColumnManager;
                var FileData = _Mapper.Map<DataFileConfiguration>(DataFileConfigurationDTO);
                var NativeFileRet = new NativeFile();
                var t = Task.Run(
                   () => { NativeFile  = Strategy.DoFromFileAsync(Path.Combine(HexFile.PathFile), FileData, -1, NativeFile).Result; });
             return NativeFile ;
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
        public async Task<string> GenerateLayout(PointDTO Scale, string NativeFileID)
        {

            string path = NativeFileID;
            var AsyncIO = new AsyncIO<Line>(path);
            AsyncIO.ReadArrayElement += Native_ReadLine;

            using (StreamReader StreamReader = new StreamReader(""))
            {
                return "";
            }

        }

        private void Native_ReadLine(object sender,
            AsyncIO.AsyncIO<Line>.EventReadArray e)
        {
        
        }

        public string PrepareMap(string [] ColumnForXY, string HexID)
        {
             
                var ColumnXY = new Column();
                ColumnXY.ID = "";
                ColumnXY.Name = "ColumnXY";
                ColumnXY.ParentID = HexID;


             
                

                var NativeFileDTO = NativeFileDataManager.GetColectionFromParent(HexID).FirstOrDefault();
                if (ColumnForXY.Count() == 2)
                {
                    var columns = ColumnManager.GetColectionFromParent(HexID);

                    var ColumnX = columns.Where(x => x.Name == ColumnForXY[0]).FirstOrDefault();
                    var ColumnY = columns.Where(x => x.Name == ColumnForXY[1]).FirstOrDefault();

                var FieldType = new FieldType();
                FieldType.FieldTypeName = typeof(float).FullName;
                FieldType.OwnIndexInData = false;
                ColumnX.FieldType = _Mapper.Map<FieldTypeDTO>( FieldType) ;
                ColumnY.FieldType = _Mapper.Map<FieldTypeDTO>(FieldType);
                ColumnXY.NumberOfRows = ColumnX.NumberOfRows;
                var FieldTypeXY = new FieldType();
                FieldTypeXY.FieldTypeName = typeof(Model.Point).FullName;
                FieldTypeXY.OwnIndexInData = true;
                FieldTypeXY.Add(typeof(float).FullName, 0);
                FieldTypeXY.Add(typeof(float).FullName, 1);
                ColumnXY.FieldType = FieldType;
                    ColumnXY.ID = ColumnManager.GenerateFullID(ColumnXY);

                    ColumnXY.PathFields = Path.Combine(Path.GetDirectoryName(ColumnX.PathFields), ColumnXY.Name + FileDataManagerOptions.DefaultExtension);
                    ColumnManager.Add(ColumnXY);
                Task.Run(() => GenerateXY(_Mapper.Map<Column>(ColumnX), _Mapper.Map<Column>(ColumnY), ColumnXY, ColumnManager, "."));
                    

                
            }

            return "";
        }
        public void GenerateXY(Column ColumnX, Column ColumnY, Column ColumnXY, IDataRepository<ColumnDTO, Column> dataRepository, string decimalSepar = ".", bool DeleteIfExist = true)
        {

            bool merge = true;
            bool Exist = false;
            if (File.Exists(ColumnXY.PathFields))
                if (DeleteIfExist)
                    File.Delete(ColumnXY.PathFields);
                else
                    return;
            
            var ColumnPointDistance = new Column { ParentID = ColumnXY.ParentID, Name = "MAE", NumberOfRows = ColumnY.NumberOfRows, ActionToDoWithUncasted = EnumActionToDoWithUncasted.DeleteData };
            ColumnPointDistance.ID = dataRepository.GenerateFullID(ColumnPointDistance);
            ColumnPointDistance.Path = Path.Combine(dataRepository.ClassLocation(ColumnPointDistance), ColumnPointDistance.Name + ".Hex.Json");
            ColumnPointDistance.PathFields = Path.Combine(dataRepository.ClassLocation(ColumnPointDistance), typeof(Field).Name, ColumnPointDistance.Name + ".Hex.Json");
            var ColXYDistinc = new Column { ParentID = ColumnXY.ParentID, Name = "XYDistinct" };
            ColXYDistinc.ID = dataRepository.GenerateFullID(ColXYDistinc);
            ColXYDistinc.Path = Path.Combine(dataRepository.ClassLocation(ColXYDistinc), ColXYDistinc.Name + ".Hex.Json");
            ColXYDistinc.PathFields = Path.Combine(dataRepository.ClassLocation(ColXYDistinc), typeof(Field).Name, ColXYDistinc.Name + ".Hex.Json");
            var ColumnPoints = new Column { ParentID = ColumnXY.ParentID, Name = "Points", ActionToDoWithUncasted = EnumActionToDoWithUncasted.DeleteData };
            ColumnPoints.ID = dataRepository.GenerateFullID(ColumnPoints);
            ColumnPoints.Path = Path.Combine(dataRepository.ClassLocation(ColumnPoints), ColumnPoints.Name + ".Hex.Json"); 
            ColumnPoints.PathFields = Path.Combine(dataRepository.ClassLocation(ColumnPoints), typeof(Field).Name, ColumnPoints.Name + ".Hex.Json");
 

            if (File.Exists(ColXYDistinc.PathFields))
                File.Delete(ColXYDistinc.PathFields);

            if (File.Exists(ColumnPointDistance.PathFields))
                File.Delete(ColumnPointDistance.PathFields);
            if (File.Exists(ColumnPoints.PathFields))
                File.Delete(ColumnPoints.PathFields);
            
            using StreamReader StreamReaderX = new StreamReader(ColumnX.PathFields);
            using StreamReader StreamReaderY = new StreamReader(ColumnY.PathFields);
            


            ColumnPoints.RWDataColumn.DeleteFile = true;
            ;
            var PointsXY = new Points(ColumnPoints); 
            var ColumnsOfPoint = new List<Column>();
            
            var sortedPoit = new List<Point>() ;
            try
            {




                var MinX = float.MaxValue;
                var MaxX = float.MinValue;

                var MinY = float.MaxValue;
                var MaxY = float.MinValue;


                while (merge)
                {
                    var x = "";
                    var y = "";

                    var fieldX = ColumnX.RWDataColumn.GetNext;
                    

                    var fieldY = ColumnY.RWDataColumn.GetNext;

                    
                    if (fieldX == null)
                        merge = false;

                    
                    if (fieldY == null)
                        merge = false;


                    if (merge && fieldY.Index == fieldX.Index)
                    {
                        //fieldX.Value = fieldX.Value.ToString().Replace(decimalSepar == "." ? "," : ".", "").Replace(",", ".");
                        var xNum = (float) fieldX.Value;
                        //fieldY.Value = fieldY.Value.ToString().Replace(decimalSepar == "." ? "," : ".", "").Replace(",", ".");
                        var yNum = (float)fieldY.Value;  

                        MinX = MinX > xNum ? xNum : MinX;
                        MaxX = MaxX < xNum ? xNum : MaxX;
                        MinY = MinY > yNum ? yNum : MinY;
                        MaxY = MaxY < yNum ? yNum : MaxY;


                        var colectionXY = new List<string>();
                        var point = new Model.Point(xNum, yNum);
                        point.Row = fieldY.Index;

                        //var fieldXY = ColumnXY.Field ( fieldY.Index, point) ;
                        
                        ColumnXY.RWDataColumn.Add(point.ObjectToString());
                        sortedPoit.Add(point);
                    }

                }
                ColumnXY.RWDataColumn.Writer.Flush();
                ColumnY.RWDataColumn.CloseReader();
                ColumnX.RWDataColumn.CloseReader();
                ColumnXY.RWDataColumn.CloseWriter();
                ColXYDistinc.FieldType = ColumnXY.FieldType;
                sortedPoit.Sort(Point.TComparer ()) ;
                foreach (var item in sortedPoit)
                {

                

                    ColXYDistinc.RWDataColumn.Add(item.ObjectToString()); 
                }
                ColXYDistinc.RWDataColumn.CloseWriter();
                PointsXY.Add(ColXYDistinc);
                ColumnY.MinValue = MinY.ToString(CultureInfo.InvariantCulture);
                ColumnX.MaxValue = MaxX.ToString(CultureInfo.InvariantCulture);
                ColumnY.MaxValue = MaxY.ToString(CultureInfo.InvariantCulture);
                
                //using StreamReader StreamReaderXY = new StreamReader(ColumnXY.PathFields);

                var ColumnXMinValue = float.Parse(ColumnX.MinValue.ToString(), CultureInfo.InvariantCulture); ;
                var ColumnXMaxValue = float.Parse(ColumnX.MaxValue.ToString(), CultureInfo.InvariantCulture);
                var ColumnYMinValue = float.Parse(ColumnY.MinValue.ToString(), CultureInfo.InvariantCulture);
                var ColumnYMaxValue = float.Parse(ColumnY.MaxValue.ToString(), CultureInfo.InvariantCulture);



                merge = true;
                var SUMMAE = 0f;
                //try
                //{
                //    foreach (var item in PointsXY)
                //    {


                //        var Column = new Column
                //        {
                //            ParentID = ColumnXY.ParentID,
                //            Name = item,
                //            NumberOfRows = q,
                //            ActionToDoWithUncasted = EnumActionToDoWithUncasted.DeleteData,
                //            FieldType = new FieldType()
                //            {
                //                OwnIndexInData = true,
                //                FieldTypeName = typeof(KeyValuePair<float, List<float>>).FullName
                //            }
                //        };

                //        Column.ID = dataRepository.GenerateFullID(Column);
                //        Column.Path = Path.Combine(dataRepository.ClassLocation(Column), Column.Name + ".Hex.Json");
                //        Column.PathFields = Path.Combine(dataRepository.ClassLocation(Column), typeof(Field).Name, Column.Name + ".Hex.Json");
                //        ColumnsOfPoint.Add(Column);
                //        if (File.Exists(Column.PathFields))
                //            File.Delete(Column.PathFields);
                //        using (StreamWriter StreamWriterPoints = new StreamWriter(Column.PathFields, true))
                //        {



                //            for (int i = 0; i < q; i++)
                //            {
                //                string ToWrite = PointsXY.PointListsvalue(item, i);
                //                StreamWriterPoints.WriteLine(ToWrite);

                //            }
                //            StreamWriterPoints.Close();

                //        }

                //    }
                //}
                //finally { ColumnXY.RWDataColumn.CloseReader(); }
                ////var clusters = Hexagon.Cluster.DBSCAN.Get(points, eps, minPts);
                
            }

            finally
            {
                StreamReaderX.Dispose();
                StreamReaderY.Dispose();
                
            }
            dataRepository.Add(ColumnY);
            dataRepository.Add(ColumnX);
            dataRepository.Add(ColumnXY);
            dataRepository.Add(ColXYDistinc);
            foreach (var item in ColumnsOfPoint)
            {
                dataRepository.Add(item);
            }

            //GenerateLayout(ColumnXY.ParentID);

            ;
            //GetClusters(points, eps, minPts)
        }
        public string GenerateLayout(LayoutDto Layout, string HexID)
        {
            return "";
        }
        public void GenerateLayout(string HexID)
        {
            var NativeFile = NativeFileDataManager.GetColectionFromParent(HexID).FirstOrDefault();
            
            var Columns = ColumnManager.GetColectionFromParent(HexID).Select(x => _Mapper.Map<Column>(x));


            var QColumns = NativeFile.Columns.Count();
            var HexDetailList = new List<HexagonDetail>();
            var HexagonDetails = new HexagonDetails();
            
            long NumLine = 0;
            var layout = new Layout();


            var ColumnXY = Columns.FirstOrDefault(x=>x.Name == "ColumnXY");
            var colMAE = Columns.FirstOrDefault(x => x.Name == "MAE");
            var ColX = Columns.FirstOrDefault(x => x.Name == "x");
            var ColY = Columns.FirstOrDefault(x => x.Name == "y");
            var ColumnXYDist = Columns.FirstOrDefault(x => x.Name == "XWithYFamily");

            var ImageDifinition = new ImageDefinition (ColX, ColY, colMAE, ColumnXYDist);

            //var ImageDifinition = new ImageDefinition(PoliygonList );
            layout = ImageDifinition.Layout;
            layout.MaxPictureSizeX = ImageDifinition.TransformedWidth;
            layout.MaxPictureSizeY = ImageDifinition.TransformedHeigth;
            layout.HexPerLine = (int)ImageDifinition.TransformedWidth;
            layout.Origin = new System.Drawing.PointF(0, 0);
            layout.Size = new System.Drawing.PointF(ImageDifinition.HexagonSize, ImageDifinition.HexagonSize);
            ;
            var ret = new List<Hex>();
            var ColumnHex = new Column();
            ColumnHex.ID = "";
            ColumnHex.Name =  "Hexagons" ;
            ColumnHex.ParentID = ColX.ParentID;
            ColumnHex.NumberOfRows = ColX.NumberOfRows;
            ColumnHex.FieldType = new FieldType { FieldTypeName = typeof(HexagonsRow).Name };
            ColumnHex.FieldType.OwnIndexInData = true;
            ColumnHex.ID = ColumnManager.GenerateFullID(ColumnHex);

            ColumnHex.PathFields = Path.Combine(Path.GetDirectoryName(ColX.PathFields), ColumnHex.Name + this.FileDataManagerOptions.DefaultExtension);
            var ColumnHexDist = new Column();
            ColumnHexDist.ID = "";
            ColumnHexDist.Name = 
                "HExagonsDist" ;
            ColumnHexDist.ParentID = ColX.ParentID;
            ColumnHexDist.FieldType = new FieldType { FieldTypeName = typeof(HexagonDetail).Name };

            ColumnHexDist.ID = ColumnManager.GenerateFullID(ColumnHexDist);

            ColumnHexDist.PathFields = Path.Combine(Path.GetDirectoryName(ColX.PathFields), ColumnHexDist.Name + this.FileDataManagerOptions.DefaultExtension);
            


            //using StreamReader StreamReaderXY = new StreamReader(ColumnXY.PathFields);

            //using StreamWriter StreamWriter = new StreamWriter(ColumnHex.PathFields, true);
            //using StreamReader StreamReaderXYDist = new StreamReader(ColumnXYDist.PathFields);

            //using StreamWriter StreamWriterDist = new StreamWriter(ColumnHexDist.PathFields, true);

            var merge = true;
            var SUMMAE = 0f;
            Points Points = new Points(null).RestoreFromFile(Path.GetDirectoryName(ColX.PathFields));
            var HExs = new Dictionary<float[], List<long>> ();
            var HexInColumn = new List<Hex>();


            foreach (var item in Points.GetPointRowFromXEtYs())
            {


                var ActualRead = "";


                


                var linea = new List<Hex>();
                var PointsHexCornes = new List<PointF>();



                var HexAnterior = new Hex();
                var HexInLine = new List<Hex>();
                PointsHexCornes = new List<PointF>();
                var PointsInHEx = new List<Point>();
                
                foreach (var DetPoints in item.Value)
                {
                    var Point = new Point (DetPoints.Item2.Row, DetPoints.Item2.Col);
                    Point.Row = item.Key;
                    var XOriginal = Point.X;
                    PointsInHEx.Add(Point);
                    var YOriginal = Point.Y;
                    var X = (XOriginal - ImageDifinition.OriginalMinX) * ImageDifinition.ProportationToScale;
                    var Y = (ImageDifinition.OriginalMaxY - YOriginal) * ImageDifinition.ProportationToScale;
                    var hexPosition1 = HexagonFunction.PixelToHexagonFlat(
                                            new Model.Point(X, Y));
                    hexPosition1.Rows.Add(item.Key);
                    var Corners = new PointF(HexagonFunction.HexagonToPixel(layout, hexPosition1).X, HexagonFunction.HexagonToPixel(layout, hexPosition1).Y);
                    
                    PointsHexCornes.Add(new PointF(HexagonFunction.HexagonToPixel(layout, hexPosition1).X, HexagonFunction.HexagonToPixel(layout, hexPosition1).Y));

                   
                    


                    HexInLine.Add(hexPosition1);

                    if (hexPosition1 != HexAnterior)
                    {
                        linea = HexagonFunction.HexagonLinedraw(hexPosition1, HexAnterior);
                        linea.Remove(hexPosition1);

                        HexInLine.AddRange(linea);

                    }
                    HexAnterior = hexPosition1;
                }
                if (layout.FillPolygon)
                   Helpers.MapHelper.PaintHexInsidePolygon(PointsHexCornes, HexInLine, layout);
                for (int i = 0; i < HexInLine.Count; i++)
                {

                    HexInLine[i].Rows.Add(item.Key);
                    var Pos = new float[]{  HexInLine[i].Q,  HexInLine[i].R, HexInLine[i].S };
                    if(!HExs.ContainsKey(Pos))
                    {
                        HExs.Add(Pos, new List<long>());
                    }
                    HExs[Pos].AddRange(HexInLine[i].Rows);
                } ;
                var hex = new HexagonsRow { Hexagons = HexInLine,  Row = item.Key  };
                var field = ColumnHex.Field(item.Key, hex);
                ColumnHex.RWDataColumn.Add  (hex.ObjectToString());

                NumLine++;
            }

            
            ColumnManager.Add (ColumnHex);

            NumLine = 0;
            
                foreach (var item in HExs  )
            {

                var det = new HexagonDetail();
                det.HexagonPositionForValues = new HexagonPosition { Q = item.Key[0], R = item.Key[1], S = item.Key[2] };
                det.IndexLines = item.Value.Distinct ().ToList();




                ColumnHexDist.RWDataColumn.Add(det.ObjectToString());
                NumLine++;

            }
            ColumnHexDist.NumberOfRows = NumLine;
            ColumnManager.Add(ColumnHexDist);
            //else
            //{
            //    var colX = Map.ColumnForX;
            //    var colY = Map.ColumnForY;
            //    layout.PaintLines = false;
            //    var EventPoint = new EventPoint() { PositionInMeters = new PointF((float)Convert.ToDouble(Line.Fieds[colX.OriginalPosition].ToString(CultureInfo.InvariantCulture)), (float)Convert.ToDouble(Line.Fieds[colY.OriginalPosition].ToString(CultureInfo.InvariantCulture))) };
            //    var X = (EventPoint.PositionInMeters.X - ImageDifinition.OriginalMinX) * ImageDifinition.ProportationToScale;
            //    var Y = (ImageDifinition.OriginalMaxY - EventPoint.PositionInMeters.Y) * ImageDifinition.ProportationToScale;
            //    var hexPosition1 = HexagonFunction.PixelToHexagon(
            //                             new Model.Point(X, Y));

            //    HexInLine.Add(hexPosition1);

            //}


        }

        public CalculatedHexagonDTO DoCalc(string FunctionID, List<string> Columns = null)
        {
            var Function = FunctionManager.Get(FunctionID);
            var columns = ColumnManager.GetColectionFromParent(Function.ParentID).Select(x => _Mapper.Map<Column>(x));
            var HexaCol = columns.FirstOrDefault(x => x.Name == "HExagonsDist");
            var CalculatedHexagon = new CalculatedHexagonDTO();
            var ListaColumns = new List<Column>();
            if (Columns != null && Columns.Count > 0)
            {
                foreach (var OrderCol in Columns)
                {
                    ListaColumns.Add(columns.FirstOrDefault(x => x.Name ==  OrderCol ));
                }
            }
            var ColCalculatedHexagon = new Column();
            ColCalculatedHexagon.ID = "";
            ColCalculatedHexagon.Name =
                "CalculatedHexagon";
            ColCalculatedHexagon.ParentID = HexaCol.ParentID;


            ColCalculatedHexagon.ID = ColumnManager.GenerateFullID(ColCalculatedHexagon);

            ColCalculatedHexagon.PathFields = Path.Combine(Path.GetDirectoryName(HexaCol.PathFields), ColCalculatedHexagon.Name + this.FileDataManagerOptions.DefaultExtension);


            using StreamReader StreamReader = new StreamReader(HexaCol.PathFields);

            using StreamWriter StreamWriter  = new StreamWriter(ColCalculatedHexagon.PathFields, true);
            var merge = true;
            var numRow = 0;
            while (merge)
            {
                var ActualRead = "";
                ActualRead = StreamReader.ReadLine();

                if (ActualRead == null)
                    break;

               var fieldX = new Field ().FromString(ActualRead);
                var Data = System.Text.Json.JsonSerializer.Deserialize<string[]>(fieldX.Value.ToString());

                var DataForFunction = new Dictionary<string, object[]>();
                var i = 0;
                var value = 0f;
                var Hexa = System.Text.Json.JsonSerializer.Deserialize<string[]>(Data[0]);
                var Points = System.Text.Json.JsonSerializer.Deserialize<string[]>(Data[1]);

                var Rows = System.Text.Json.JsonSerializer.Deserialize<string[]>(Data[2]);
                if (Columns != null && Columns.Count > 0)
                {
                    
                    foreach (var item in ListaColumns)
                    {
                        var ListVal = new List<object>();
                        using StreamReader StreamReaderVallues = new StreamReader(item.PathFields);
                        bool mergeCol = true;
                        while (mergeCol)
                        {
                            var ColVAL = "";
                            ColVAL = StreamReader.ReadLine();
                            if (ColVAL == null)
                                break;
                            i++;
                            var fieldcol = (Field)new Field().FromString(ColVAL);
                            if (Rows.Contains( fieldcol.Index.ToString()))
                            { ListVal.Add(fieldcol.Value); }
                            if (ListVal.Count() == Rows.Count())
                                break; 
                        }
                        DataForFunction.Add(i.ToString(), ListVal.ToArray());
                        i++;

                    }
                    
                    value = CalcStrategy.DoCalc.Do(new Object[] { DataForFunction }, Function.Path, Function.FullClassName, Function.FunctionName);
                 }
                else
                {
                    value = 0f;
                }
                var ToAdd = System.Text.Json.JsonSerializer.Serialize ( new HexaDetailWithValue { Value = value, HexagonPosition = new HexagonPosition { Q = Utils.FloatFromString(Hexa[0]), R = Utils.FloatFromString(Hexa[1]), S = Utils.FloatFromString(Hexa[2]) } }) ;
                var field = new Field();
                field.Index = fieldX.Index;
                field.Value = ToAdd;
                StreamWriter.WriteLine(field.ToString());
                numRow++;
                
            //CalculatedHexagon.HexaDetailWithValue.AddRange(lista.HexagonPositionForValues.Select(x => new HexaDetailWithValueDTO { HexagonPosition = x, Value = value }));
        }
            ColCalculatedHexagon.NumberOfRows = numRow;
            ColumnManager.Add(ColCalculatedHexagon);
            StreamWriter.Close();
            if (Columns != null && Columns.Count > 0)
            {
                CalculatedHexagon.ColumnNamesForFunction = Columns;
                CalculatedHexagon.Name = String.Join("_", CalculatedHexagon.ColumnNamesForFunction);
            }
            else
            {
                CalculatedHexagon.Name = "WithoutCol";
            }
            CalculatedHexagon.ParentID = Function.ID;
            //CalculatedHexagon.LayoutID = Layout.ID;
            CalculatedHexagonManager.Add(_Mapper.Map<CalculatedHexagon>(CalculatedHexagon));
            

            return CalculatedHexagon;

        }
        public List<ProyectDataDTO> GetProyects(string UserID)
        {
            //string Path = @"C:\Users\Usuario\AppData\Hexagon.Api\User\PruebaJSON\ProyectData\Desde0\AnalizedFile\Alguno\HexFile\SensoresCada10.csv\NativeFile\DelimitedFile_4dd45987-a392-4938-b07d-51c63c1271c3.FILE.HexJson.Hex.Json";
            //var json = new AsyncIO<Line>(Path);
            //json.ReadArrayElement += Native_ReadLine;
            //var b = new Line();
            // json.ReadJsonArrayAsync("Content");
            var UserDTO = DataUser.Get(UserID);


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
        public HexFileDTO PutFileAsync(string User, string Project, string NicName, string FileName, string OriginalName)
        {


            return PutFile(User, Project, NicName, FileName, OriginalName);
        }

        public async Task<HexFileDTO> PutFileAsync(string User, string Project, string NicName, IFormFile IFormFile)
        {



            var file = IFormFile;

            HexFileDTO fileDTO = new HexFileDTO();
            if (file.Length > 0)
            {
                var FileName = Guid.NewGuid().ToString() + ".FILE";
                var OriginalName = file.FileName;
                var Nic = NicName;// Path.GetFileNameWithoutExtension(file.FileName);

                fileDTO = PutFile(User, Project, NicName, FileName, OriginalName);

                if (!Directory.Exists(Path.GetDirectoryName(fileDTO.PathFile)))
                    Directory.CreateDirectory(Path.GetDirectoryName(fileDTO.PathFile));

                using var source = IFormFile.OpenReadStream();
                using var Stream = File.OpenWrite(fileDTO.PathFile);
                await Task.Run(()=> source.CopyToAsync(Stream)).ConfigureAwait(false);
                

            }
            return fileDTO; ;
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

                var T = Task.Run(() =>
                {

                    using (Stream source = file.OpenReadStream())
                    {
                        if (!Directory.Exists(Path.GetDirectoryName(fileDTO.PathFile)))
                            Directory.CreateDirectory(Path.GetDirectoryName(fileDTO.PathFile));
                        using (Stream destination = File.Create(fileDTO.PathFile))
                        {
                            source.CopyToAsync(destination);
                        }
                    }
                }).ConfigureAwait(false);
            }
            return fileDTO;
        }
        public HexFileDTO PutFile(string User, string Project, string NicName, string FileName, string OriginalName)
        {


            var Proyect = GetProyect(User, Project);
            var AnalizedFileDTO = new AnalizedFileDTO() { Name = NicName, ParentID = Proyect.ID };
            var AnalizedFile = AnalizedFileDataManager.Get(AnalizedFileDTO);
            //AnalizedFileDTO = AnalizedFileDataManager.FindByCondition (NicName );
            if (AnalizedFile == null)
            {

                AnalizedFileDTO = AnalizedFileDataManager.Add(this._Mapper.Map<AnalizedFile>(AnalizedFileDTO));
            }
            else
                AnalizedFileDTO = this._Mapper.Map<AnalizedFileDTO>(AnalizedFile);
            var fileDTO = new HexFileDTO();

            fileDTO = new HexFileDTO();
            fileDTO.Name = OriginalName;
            fileDTO.ParentID = AnalizedFileDTO.ID;

            var File = FileRepository.Get(fileDTO);

            if (File == null)
            {
                fileDTO.FileName = FileName;

                fileDTO.TypeFile = Enum.GetName(typeof(EnumFileType), EnumFileType.Original);

                fileDTO.ID = FileRepository.GenerateFullID(_Mapper.Map<HexFile>(fileDTO));

                fileDTO.PathFile = (Path.Combine(Path.GetDirectoryName(Path.Combine(FileRepository.ParentDirectory(), fileDTO.ID + FileDataManagerOptions.DefaultExtension)), FileName));

                fileDTO = FileRepository.Add(_Mapper.Map<HexFile>(fileDTO));
            }
            else
            {
                fileDTO = _Mapper.Map<HexFileDTO>(File);
               
            }

            return fileDTO;
        }

            public ProyectDataDTO GetProyect(string User, string Name)
        {
            var UserDT = DataUser.Get(User);
            var ID = IDataRepository.GetID(Name, UserDT.ID, typeof(ProyectData));
            var ProyectDataDTO = IDataRepository.Get(ID);

            if (ProyectDataDTO == null)
            {

                var Location = new Location(Name, ServicesConstants.MapsDirectory, ServicesConstants.FilesDirectory, ServicesConstants.NativesDirectory);

                ProyectDataDTO = IDataRepository.Add(new ProyectData { ID = Name, Name = Name, Location = Location, ParentID = UserDT.ID });
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

        public FunctionDTO SetFunction(string HexagonDetailstID, FunctionDTO Function)
        {
            try
            {

                var FunctionExist = FunctionManager.GetColectionFromParent(HexagonDetailstID).Where(x => x == Function).FirstOrDefault();
                if (FunctionExist != null)
                    Function = FunctionExist;
                else
                {
                    Function.ParentID = HexagonDetailstID;
                    Function = FunctionManager.Add(_Mapper.Map<Function>(Function));


                }
            }
            catch (Exception)
            {
                Function.ParentID = HexagonDetailstID;
                Function = FunctionManager.Add(_Mapper.Map<Function>(Function));

            }

            return Function;

            ;
        }

        public string GenerateImge(string PaletteClassID, string CalculatedHexagonID, float Size = 0)
        {

            var paletteClass = PaletteClassManager.Get(PaletteClassID);

            var CalculatedHexagon = CalculatedHexagonManager.Get(CalculatedHexagonID);
            var Max = CalculatedHexagon.HexaDetailWithValue.Select(x => x.Value).Max();
            var Min = CalculatedHexagon.HexaDetailWithValue.Select(x => x.Value).Min();
            var Layout = _Mapper.Map<Layout>(LayoutDataManager.Get(CalculatedHexagon.LayoutID));

            var dif = Max - Min;
            var NumClass = paletteClass.MemberNumber;
            var Graf = new string("<svg xmlns='http://www.w3.org/2000/svg' version='1.1'  height='" + Layout.MaxPictureSizeY.ToString() + "' width='" + Layout.MaxPictureSizeX.ToString() + "' >");
            var Rango = dif / NumClass;
            foreach (var item in CalculatedHexagon.HexaDetailWithValue)
            {
                var Hex = new Hex(item.HexagonPosition.Q, item.HexagonPosition.R, item.HexagonPosition.S);
                var Points = Hex.Corners;

                Graf += "<polygon points= '";
                foreach (var point in Points)
                {
                    Graf += point.X.ToString(CultureInfo.InvariantCulture) + "," + point.Y.ToString(CultureInfo.InvariantCulture);
                    Graf += " ";

                }
                Graf += Points[0].X.ToString(CultureInfo.InvariantCulture) + "," + Points[0].Y.ToString(CultureInfo.InvariantCulture);
                var index = ((int)(Math.Floor(item.Value - Min) / Rango)) + 1;
                var Color = paletteClass.RGBS.GetValueOrDefault((NumClass - index) + 1);
                var col = ColorTranslator.ToHtml(Color);

                Graf += "' style = 'fill:" + col + ";stroke:purple;stroke-width:1'/> ";
            }
            Graf += "</svg>";

            var file = Path.Combine(CalculatedHexagonManager.ParentDirectory(), CalculatedHexagonID) + paletteClass.Palette + "_" + paletteClass.EnumPaletteClass.ToString() + "_" + paletteClass.MemberNumber.ToString() + ".SVG";
            using (StreamWriter StreamWriter = new StreamWriter(file))
            {
                StreamWriter.Write(Graf);
            }
            return "";
        }
        public List<PaletteClass> GetPaletteClasses(string Name, string Enumj, int Q)
        {
            var Bewer = Path.Combine(FileRepository.ParentDirectory(), "Bewer.txt");


            using (StreamReader file = File.OpenText(Bewer))
            {
                var filetexy = (file.ReadToEnd());
                var split = filetexy.Split("\n");
                var Paletas = new List<PaletteClass>();
                var paleta = new PaletteClass();

                paleta.Palette = "";

                paleta.MemberNumber = -1;


                foreach (var item in split)
                {
                    var col = item.Split(",");

                    var color = Color.FromArgb(Convert.ToInt32(col[0]), Convert.ToInt32(col[1]), Convert.ToInt32(col[2]));
                    paleta.RGBS.Add(Convert.ToInt32(col[8]), color);


                    if (Convert.ToInt32(col[8]) == Convert.ToInt32(col[5]))
                    {
                        paleta.EnumPaletteClass = (EnumPaletteClass)Enum.Parse(typeof(EnumPaletteClass), col[3].ToString(), true); ;
                        paleta.ParentID = Path.Combine(paleta.GetType().Name, col[3], col[5], "Brewer");
                        paleta.MemberNumber = Convert.ToInt32(col[5]);
                        paleta.Palette = col[4];

                        PaletteClassManager.Add(paleta);

                        paleta = new PaletteClass();



                    }





                }
            }
            return null;

        }

    }
}
