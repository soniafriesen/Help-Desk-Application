//Project: Case Study2
//Purpose: has one getall function wich gets the list of all departments using IRepository class
//Coder: Sonia Friesen, 0813682
//Date: Due Dec 11 2019.
using System;
using HelpdeskDAL;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HelpdeskDAL
{
    public class DepartmentModel
    {
        IRepository<Departments> repository;

        public DepartmentModel()
        {
            repository = new HelpdeskRepository<Departments>();
        }

        public List<Departments> GetAll()
        {
            return repository.GetAll();
        }
    }
}
