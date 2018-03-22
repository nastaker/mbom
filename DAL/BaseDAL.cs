using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace DAL
{
    public class DbContextFactory
    {
        /// <summary>
        /// 创建EF上下文对象,已存在就直接取,不存在就创建,保证线程内是唯一。
        /// </summary>
        public static BaseDbContext Create()
        {
            BaseDbContext dbContext = HttpContext.Current.Items["DbContext"] as BaseDbContext;
            if (dbContext == null)
            {
                dbContext = new BaseDbContext();
                HttpContext.Current.Items["DbContext"] = dbContext;
            }
            return dbContext;
        }
    }

    public class BaseDAL<TEntity> where TEntity : class
    {
        protected BaseDbContext _context = DbContextFactory.Create();
        protected DbSet<TEntity> _db;

        public BaseDAL()
        {
            _db = _context.Set<TEntity>();
        }

        public virtual TEntity Add(TEntity entity)
        {
            return _db.Add(entity);
        }

        public virtual IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            return _db.AddRange(entities);
        }

        public virtual TEntity Delete(int id)
        {
            var entity = Get(id);
            return _db.Remove(entity);
        }

        public virtual TEntity Delete(TEntity entity)
        {
            return _db.Remove(entity);
        }

        public virtual IEnumerable<TEntity> Delete(Expression<Func<TEntity, bool>> where)
        {
            var entities = GetList(where);
            return _db.RemoveRange(entities);
        }


        public void Modify(TEntity model, params string[] properties)
        {
            DbEntityEntry entry = _context.Entry(model);
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

        public void Modify(List<TEntity> models, params string[] properties)
        {
            foreach (var model in models)
            {
                DbEntityEntry entry = _context.Entry(model);
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

        public virtual TEntity Edit(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public virtual List<TEntity> Edit(List<TEntity> entities)
        {
            entities.ForEach(entity => {
                _context.Entry(entity).State = EntityState.Modified;
            });
            return entities;
        }

        public virtual List<TEntity> GetAll()
        {
            return _db.ToList();
        }

        public virtual TEntity Get(int id)
        {
            return _db.Find(id);
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> where)
        {
            return _db.SingleOrDefault(where);
        }

        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> where)
        {
            return _db.Where(where).ToList();
        }

        public virtual IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> where)
        {
            return _db.Where(where);
        }

        public virtual IQueryable<TEntity> GetQueryable()
        {
            return _db.AsQueryable();
        }

        public virtual List<TEntity> SqlQuery(string sql, params object[] parameters)
        {
            return _db.SqlQuery(sql, parameters).ToList();
        }

        public virtual int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return _context.Database.ExecuteSqlCommand(sql, parameters);
        }

        public virtual int Count()
        {
            return _db.Count();
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
