using AutoMapper;
using Hexagon.Model.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hexagon.Model.Repository
{

    public interface IDataRepository<G, TEntity> where TEntity : Base //G DTO to return, TEntity Entity to query
    {
        IEnumerable<TEntity> GetAll();
        IEnumerable<G> GetAllDTO();
        G Get(string id);
        TEntity Get(G EntityPersisted);
        string GetID(string ParametersForMak, string PararentID, Type EntityType);
        string ParentDirectory();
        G Add(TEntity entity);
        Task<G> AddAsync(TEntity entity);
        Task<TEntity > GetAsync(G EntityPersisted); 
        string ClassLocation(G DTOEntity);
        G Update(TEntity dbEntity, TEntity entity);
        Task UpdateAsync(TEntity dbEntity, TEntity entity);
        void Delete(TEntity entity);
        IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression);
        IQueryable<G> FindByConditionDTO(Expression<Func<TEntity, bool>> expression);
        void SaveChanges(TEntity entity);
        IEnumerable<Base> GetChild(TEntity TEntity, Type ChildType);
        IEnumerable<G> GetColectionFromParent(string ParentID);
        bool Open(string Conection);
        public IConfiguration Configuration { get; }
        public IMapper IMapper { get; }
        public string GenerateFullID(TEntity TEntity);
    }
    
}

