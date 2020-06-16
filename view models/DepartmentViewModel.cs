//Project: Case Study2
//Purpose: has one getall function wich gets the list of all departments 
//Coder: Sonia Friesen, 0813682
//Date: Due Dec 11 2019

using System;
using HelpdeskDAL;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace HelpdeskViewModels
{
    public class DepartmentViewModel
    {
        private DepartmentModel _model;
        public int Id { get; set; }
        public string Name { get; set; }

        public DepartmentViewModel()
        {
            _model = new DepartmentModel();
        }

        public List<DepartmentViewModel> GetAll()
        {
            List<DepartmentViewModel> allVms = new List<DepartmentViewModel>();
            try
            {
                List<Departments> allDepartments = _model.GetAll();
                foreach (Departments depart in allDepartments)
                {
                    DepartmentViewModel dVm = new DepartmentViewModel();
                    dVm.Id = depart.Id;
                    dVm.Name = depart.DepartmentName;
                    allVms.Add(dVm);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return allVms;
        }
    }
}
