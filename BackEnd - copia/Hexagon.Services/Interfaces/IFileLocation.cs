using Hexagon.Model.Models;
using Hexagon.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hexagon.Services.Interfaces
{
    public interface IFileLocation
    {

        public List<ProyectData> ProyectData(string UserName);
        public  string Folder ( AnalizedFileDTO AnalizedFile, string Type);


    }
}
