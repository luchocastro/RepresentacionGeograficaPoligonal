using AutoMapper;
using Hexagon.Model.Models;
using Hexagon.Model.Repository;
using Hexagon.Services.Interfaces;
using Hexagon.Shared.DTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexagon.Services 
{
    public class FileLocation : IFileLocation
    {
        private IDataRepository<ProyectDataDTO, ProyectData> IDataRepository;

        public FileLocation(IConfiguration Configuration, IMapper Mapper,
    IDataRepository<ProyectDataDTO, ProyectData> IDataRepository)
        {
            this.IDataRepository = IDataRepository;
        }

        public string Folder( AnalizedFileDTO AnalizedFile, string Type)
        {
            return AnalizedFile.PathFiles; 
        }

        public List<ProyectData> ProyectData(string UserName)
        {
            throw new NotImplementedException();
        }
    }
}
