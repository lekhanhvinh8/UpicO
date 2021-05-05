using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Upico.Core.Repositories;

namespace Upico.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;
        private DbSet<TEntity> _entities;
        public Repository(DbContext context)
        {
            this.Context = context;
            this._entities = context.Set<TEntity>();
        }
        public void Add(TEntity entity)
        {
            this._entities.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            this._entities.AddRange(entities);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return this._entities.Where(predicate);
        }

        public TEntity Get(int id)
        {
            return this._entities.Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return this._entities.ToList();
        }
        public void Load(Expression<Func<TEntity, bool>> predicate)
        {
            this._entities.Where(predicate).Load();
        }
        public void Remove(TEntity entity)
        {
            this._entities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            this._entities.RemoveRange(entities);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return this._entities.SingleOrDefault(predicate);
        }
    }
}
