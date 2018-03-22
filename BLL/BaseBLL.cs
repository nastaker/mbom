using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BLL
{
    public class BaseBLL<TEntity> where TEntity : class
    {
        protected BaseDAL<TEntity> dal = new BaseDAL<TEntity>();

        public virtual TEntity Add(TEntity entity)
        {
            return dal.Add(entity);
        }
        public virtual IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            return dal.AddRange(entities);
        }
        public virtual TEntity Delete(int id)
        {
            return dal.Delete(id);
        }
        public virtual TEntity Delete(TEntity entity)
        {
            return dal.Delete(entity);
        }
        public virtual IEnumerable<TEntity> Delete(Expression<Func<TEntity, bool>> where)
        {
            return dal.Delete(where);
        }
        public virtual TEntity Edit(TEntity entity)
        {
            return dal.Edit(entity);
        }
        public virtual List<TEntity> Edit(List<TEntity> entities)
        {
            return dal.Edit(entities);
        }
        public void Modify(TEntity model, params string[] properties)
        {
            dal.Modify(model, properties);
        }
        public void Modify(List<TEntity> models, params string[] properties)
        {
            dal.Modify(models, properties);
        }
        public virtual TEntity Get(int id)
        {
            return dal.Get(id);
        }
        public virtual TEntity Get(Expression<Func<TEntity, bool>> where)
        {
            return dal.Get(where);
        }
        public virtual List<TEntity> GetAll()
        {
            return dal.GetAll();
        }
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> where)
        {
            return dal.GetList(where);
        }
        public virtual IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> where)
        {
            return dal.GetQueryable(where);
        }
        public virtual IQueryable<TEntity> GetQueryable()
        {
            return dal.GetQueryable();
        }

        public virtual List<TEntity> SqlQuery(string sql, params object[] parameters)
        {
            return dal.SqlQuery(sql, parameters);
        }

        public virtual int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return dal.ExecuteSqlCommand(sql, parameters);
        }

        public virtual int Count()
        {
            return dal.Count();
        }
        public virtual bool SaveChanges()
        {
            return dal.SaveChanges();
        }
    }
}
