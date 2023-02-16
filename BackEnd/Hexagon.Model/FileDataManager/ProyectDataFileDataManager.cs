using AutoMapper;
using Hexagon.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Hexagon.Model.Models;
using Hexagon.Shared.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Hexagon.Model.Repository;
using System.Threading;

namespace Hexagon.Model.FileDataManager
{
    public class ProyectDataFileDataManager<G, T> : FileDataManager<G, T>, IForFile<T> where T : ProyectData where G : ProyectDataDTO
    {
        private string DefProyectDataName = "";
        public ProyectDataFileDataManager(IMapper Mapper,IConfiguration IConfiguration, 
            IFileDataManagerOptions IFileDataManagerOptions) : base(Mapper, IConfiguration, IFileDataManagerOptions  )
        {
            try
            {
                DefProyectDataName = this.Configuration.GetSection("DefaultProyectName").Value;
            }
            catch { DefProyectDataName = "HexaProjectData"; }

        }
        public override G Update(T dbEntity, T entity)
        {
            dbEntity.Location = entity.Location;
            dbEntity.Name = entity.Name;
            var AnalizedFiles = new List<AnalizedFile>();
            var FileToWrite = base.GetPath(entity);
            if (!Directory.Exists(Path.GetDirectoryName(FileToWrite)))
                Directory.CreateDirectory(Path.GetDirectoryName(FileToWrite));

            

//            dbEntity.AnalizedFiles = PreparedFolders (entity);
            SaveChanges(dbEntity);
            return IMapper.Map<G>(dbEntity);
        }
        public override G Get(string Id)
        {
            G ProyectData;
            try
            {

                ProyectData = base.Get(Path.Combine(Id  )); ;
            }
            catch (Exception ex)
            {
                if ((ex.GetType()) == typeof(FileNotFoundException))
                {
                    return null;
                }
                else
                    throw ex;

            }

            return ProyectData;

        }
        public override G Add(T entity)
        {
            entity.ID = base.GenerateFullID(entity);
            var FileToWrite = base.GetPath(entity);
            var location = entity.Location;
            
            location.ProyectFolder = Path.Combine(this.ParentDirectory(),entity.ID) ;
            entity.Location = location;
            ////var FileToWrite = Path.Combine(this.ParentDirectory(), entity.ID + base.de);
            //if (!Directory.Exists(Path.GetDirectoryName(FileToWrite)))
            //    Directory.CreateDirectory(Path.GetDirectoryName(FileToWrite));
            // var ToWrite = JsonConvert.SerializeObject(entity);
           // Write(FileToWrite, ToWrite);
            return base.Add (entity);
        }
        public override IEnumerable<G> GetAllDTO()
        {
            var ret = new List<ProyectDataDTO>();
            var User = Thread.CurrentPrincipal;
            foreach (var dir in Directory.GetDirectories(base.ParentDirectory()))
            {

                string nameFileProject = Path.Combine(dir, Path.GetDirectoryName(dir) +  base.DefaultExtension);

                if (File.Exists(nameFileProject))
                {
                    using (StreamReader file = File.OpenText(nameFileProject))
                    {
                        var def1 = JsonConvert.DeserializeObject<ProyectDataDTO>(file.ReadToEnd());
                        ret.Add(def1);
                    }
                }

            }
            return (IEnumerable<G>)ret ;
        }

    }
}
