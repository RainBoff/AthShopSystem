﻿using asp_mvc_std_v2.Data;
using asp_mvc_std_v2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace asp_mvc_std_v2
{
    public interface IRepositoryService<T> where T : IEntity<int>
    {
        IQueryable<T> GetAllRecords();
        T GetSingle(int id);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        ServiceResult Add(T entity);
        ServiceResult Delete(T entity);
        ServiceResult Edit(T entity);
        ServiceResult Save();
    }
    public class RepositoryService<T> : IRepositoryService<T> where T : class, IEntity<int>
    {
        protected DbContext _context;
        protected DbSet<T> _set;
        public RepositoryService(ApplicationDbContext context)
        {
            _context = context;
            _set = _context.Set<T>();
        }
        public virtual ServiceResult Add(T entity)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                _set.Add(entity);
                result = Save();
            }
            catch (Exception e)
            {
                result.Result = ServiceResultStatus.Error;
                result.Messages.Add(e.Message);
            }
            return result;
        }
        public virtual ServiceResult Delete(T entity)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                _set.Remove(entity);
                result = Save();
            }
            catch (Exception e)
            {
                result.Result = ServiceResultStatus.Error;
                result.Messages.Add(e.Message);
            }
            return result;
        }
        public virtual ServiceResult Edit(T entity)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                result = Save();
            }
            catch (Exception e)
            {
                result.Result = ServiceResultStatus.Error;
                result.Messages.Add(e.Message);
            }
            return result;
        }
        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _set.Where(predicate);
            return query;
        }
        public IQueryable<T> GetAllRecords()
        {
            return _set;
        }
        public virtual T GetSingle(int id)
        {
            var result = _set.FirstOrDefault(r => r.Id == id);
            return result; ;
        }
        public virtual ServiceResult Save()
        {
            ServiceResult result = new ServiceResult();
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                result.Result = ServiceResultStatus.Error;
                result.Messages.Add(e.Message);
            }
            return result;
        }
    }
}