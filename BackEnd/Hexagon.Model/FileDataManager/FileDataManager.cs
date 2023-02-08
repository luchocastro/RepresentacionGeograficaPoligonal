using Hexagon.Model.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Hexagon.Model;
using Hexagon.Model.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Hexagon.Model.FileDataManager
{
    public class contextFactoryFile  
    { }
    public class FileDataManager<G, TEntity>  : IForFile<TEntity>, IDataRepository<G, TEntity> where TEntity : IModelPersistible
    {
        private readonly IMapper Mapper;
        private readonly IConfiguration IConfiguration;
        private readonly string User; 
        
        public FileDataManager(IMapper Mapper )
        {    User = IConfiguration.GetConnectionString("File") ;
            this.Mapper = Mapper;
             
        }
        private string Head { get; set; }
        public bool  Open(string Head)
        {
            if (Head != null && Head != "")
                this.Head = Head;
            return true;
                
        }
        public string ParentDirectory()
        {

            return Path.Combine(Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)).FullName, AppDomain.CurrentDomain.FriendlyName , Head );
        }
        public virtual G Add(TEntity entity)
        {
            var FileToWrite = Path.Combine(this.ParentDirectory(), entity.ID + ".json");
            if (!Directory.Exists(Path.GetDirectoryName  (FileToWrite)))
                Directory.CreateDirectory(Path.GetDirectoryName(FileToWrite));
            var ToWrite = JsonConvert.SerializeObject (entity);
            Write(FileToWrite, ToWrite);
            return Mapper.Map<G>(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<G> FindByConditionDTO(Expression<Func<TEntity, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public virtual G Get(string id)

        {   
            var PathToRead = Path.Combine(this.ParentDirectory(), id + ".json");
            if (!File.Exists(PathToRead))
                throw new FileNotFoundException(PathToRead);
            
                return Mapper.Map<G>( Read (PathToRead));
            

        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<G> GetAllDTO()
        {
            throw new NotImplementedException();
        }
        public string GetPath(TEntity Entity)
        { 
            return  Path.Combine(ParentDirectory(), Entity.ID + ".json");

        }
        public void SaveChanges(TEntity entity)
        {
            var ToSave = JsonConvert.SerializeObject(entity);

            Write(GetPath(entity), ToSave);
        }

        public virtual G Update(TEntity dbEntity, TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task UpdateAsync(TEntity dbEntity, TEntity entity)
        {
            throw new NotImplementedException();
        }
        public void Write(string Path, string ToSave)
        {
            using (StreamWriter StreamWriter = new StreamWriter(Path))
            {
                StreamWriter.Write(ToSave);
                StreamWriter.Close();

            }
        }

        public TEntity Read(string Path)
        {

            TEntity ret  ;
            using (StreamReader file = File.OpenText(Path))
            {
                ret =JsonConvert.DeserializeObject<TEntity>(file.ReadToEnd());
                
            }
            return ret;
        }

        public virtual TEntity GetFromFile(string Path)
        {
            throw new NotImplementedException();
        }

        public virtual string  SetToFile(TEntity TEntity)
        {
            throw new NotImplementedException();
        }
    }
}
