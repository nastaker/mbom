using Microsoft.Practices.Unity;
using System.Data.Entity;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Web.Mvc;
using Repository;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace MBOM.Controllers
{
    public class BaseController<T> : Controller where T : class
    {
        protected BaseDbContext Repository { get; set; }
        protected DbSet<T> _db;

        public BaseController(BaseDbContext _repository)
        {
            Repository = _repository;
            _db = Repository.Set<T>();
        }

        public virtual T Add(T entity)
        {
            return _db.Add(entity);
        }

        public virtual IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            return _db.AddRange(entities);
        }

        public virtual T Delete(int id)
        {
            var entity = Get(id);
            return _db.Remove(entity);
        }

        public virtual T Delete(T entity)
        {
            return _db.Remove(entity);
        }

        public virtual IEnumerable<T> Delete(Expression<Func<T, bool>> where)
        {
            var entities = GetList(where);
            return _db.RemoveRange(entities);
        }


        public void Modify(T model, params string[] properties)
        {
            DbEntityEntry entry = Repository.Entry(model);
            _db.Attach(model);
            if (properties.Length > 0)
            {
                foreach (string property in properties)
                {
                    entry.Property(property).IsModified = true;
                }
            }
            else
            {
                entry.State = EntityState.Modified;
            }
        }

        public void Modify(List<T> models, params string[] properties)
        {
            foreach (var model in models)
            {
                DbEntityEntry entry = Repository.Entry(model);
                _db.Attach(model);
                if (properties.Length > 0)
                {
                    foreach (string property in properties)
                    {
                        entry.Property(property).IsModified = true;
                    }
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public virtual T Edit(T entity)
        {
            Repository.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public virtual List<T> Edit(List<T> entities)
        {
            entities.ForEach(entity => {
                Repository.Entry(entity).State = EntityState.Modified;
            });
            return entities;
        }

        public virtual List<T> GetAll()
        {
            return _db.ToList();
        }

        public virtual T Get(int id)
        {
            return _db.Find(id);
        }

        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return _db.SingleOrDefault(where);
        }

        public virtual List<T> GetList(Expression<Func<T, bool>> where)
        {
            return _db.Where(where).ToList();
        }

        public virtual IQueryable<T> GetQueryable(Expression<Func<T, bool>> where)
        {
            return _db.Where(where);
        }

        public virtual IQueryable<T> GetQueryable()
        {
            return _db.AsQueryable();
        }

        public virtual List<T> SqlQuery(string sql, params object[] parameters)
        {
            return _db.SqlQuery(sql, parameters).ToList();
        }

        public virtual int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return Repository.Database.ExecuteSqlCommand(sql, parameters);
        }

        public virtual int Count()
        {
            return _db.Count();
        }

        public bool SaveChanges()
        {
            return Repository.SaveChanges() > 0;
        }
    }
}