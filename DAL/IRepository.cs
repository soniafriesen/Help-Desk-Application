//Project: Case Study2
//Purpose: Interface
//Coder: Sonia Friesen, 0813682
//Date: Due Dec 11 2019.

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace HelpdeskDAL
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        List<T> GetByExpression(Expression<Func<T, bool>> match);
        T Add(T entity);
        UpdatedStatus Update(T entity);
        int Delete(int i);
    }
}
