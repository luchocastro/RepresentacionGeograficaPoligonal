using Hexagon.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hexagon.Model.Repository
{
    
        public interface IDataRepository<G, TEntity> where TEntity : IModelPersistible //G DTO to return, TEntity Entity to query
        {
            IEnumerable<TEntity> GetAll();
            IEnumerable<G> GetAllDTO();
            G Get(string id);
            G Add(TEntity entity);
            G Update(TEntity dbEntity, TEntity entity);
            Task UpdateAsync(TEntity dbEntity, TEntity entity);
            void Delete(TEntity entity);
            IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression);
            IQueryable<G> FindByConditionDTO(Expression<Func<TEntity, bool>> expression);
            void SaveChanges(TEntity entity);
        bool Open(string Conection);
         
    }
    
}

