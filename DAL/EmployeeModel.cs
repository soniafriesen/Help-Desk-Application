//Project: Case Study2
//Purpose: Employee Model Class
//Coder: Sonia Friesen, 0813682
//Date: Due Dec.11 2019
using System;
using System.Collections.Generic;
using System.Text;
using HelpdeskDAL;
using System.Linq;
using System.Reflection;

namespace HelpdeskDAL
{
    public class EmployeeModel
    {
        IRepository<Employees> repository;
        public EmployeeModel()
        {
            repository = new HelpdeskRepository<Employees>();
        }
        public Employees GetByLastname(string lastname)
        {
            return repository.GetByExpression(emp => emp.LastName == lastname).FirstOrDefault();
        }
        public Employees GetByEmail(string email)
        {
            return repository.GetByExpression(emp => emp.Email == email).FirstOrDefault();
        }       
        public Employees GetById(int id)
        {
            return repository.GetByExpression(emp => emp.Id == id).FirstOrDefault();

        }
        public List<Employees> GetAll()
        {
            return repository.GetAll();
        }
        public int Add(Employees newEmployee)
        {
            return repository.Add(newEmployee).Id;

        }
        public UpdatedStatus Update(Employees updatedEmployee)
        {
            return repository.Update(updatedEmployee);
        }
        public int Delete(int id)
        {
            return repository.Delete(id);
        }
    }
}
