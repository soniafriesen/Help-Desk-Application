//Project: Case Study2
//Purpose: Uses a template class to accept data from both employee and department 
//Coder: Sonia Friesen, 0813682
//Date: Due Dec 11 2019.

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace HelpdeskDAL
{
    public class HelpdeskRepository<T> : IRepository<T> where T : HelpdeskEntity
    {
        private helpDeskContext _db = null;
        public HelpdeskRepository(helpDeskContext content = null)
        {
            _db = content != null ? content : new helpDeskContext();
        }
        public List<T> GetAll()
        {
            return _db.Set<T>().ToList();
        }
        public List<T> GetByExpression(Expression<Func<T, bool>> match)
        {
            return _db.Set<T>().Where(match).ToList();
        }
        public T Add(T entity)
        {
            _db.Set<T>().Add(entity);
            _db.SaveChanges();
            return entity;
        }
        public UpdatedStatus Update(T updatedEntity)
        {
            UpdatedStatus operationStatus = UpdatedStatus.Failed;

            try
            {
                helpDeskContext _db = new helpDeskContext();
                T currentEntity = _db.Set<T>().FirstOrDefault(ent => ent.Id == updatedEntity.Id);
                _db.Entry(currentEntity).OriginalValues["Timer"] = updatedEntity.Timer;
                _db.Entry(currentEntity).CurrentValues.SetValues(updatedEntity);

                if (_db.SaveChanges() == 1) //shoudl throw if stale;
                    operationStatus = UpdatedStatus.Ok;
            }
            catch (DbUpdateConcurrencyException dbx)
            {
                operationStatus = UpdatedStatus.Stale;
                Console.WriteLine("Problem in " + MethodBase.GetCurrentMethod().Name + dbx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + MethodBase.GetCurrentMethod().Name + ex.Message);
            }
            return operationStatus;
        }
        public int Delete(int id)
        {
            T currentEntity = GetByExpression(ent => ent.Id == id).FirstOrDefault();
            _db.Set<T>().Remove(currentEntity);
            return _db.SaveChanges();
        }
    }
}
