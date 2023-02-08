using AutoMapper;
using Hexagon.Model.Models;
using Hexagon.Shared.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Model.FileDataManager
{
    public class ProyectDataFileDataManager : FileDataManager<ProyectDataDTO, ProyectData>, IForFile<ProyectData>
    {
        public ProyectDataFileDataManager(IMapper Mapper) : base(Mapper)
        {

        }
        public override ProyectData GetFromFile(string Path)
        {
            return (ProyectData)this.Read(Path);
        }

        public override string SetToFile(ProyectData ProyectData)
        {
            this.Write(this.GetPath(ProyectData), JsonConvert.SerializeObject(ProyectData));
            return this.GetPath(ProyectData);
        }
    }
}
