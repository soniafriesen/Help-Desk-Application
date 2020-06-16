//Project: Case Study2
//Purpose: has 6 function to get information on employees
//Coder: Sonia Friesen, 0813682
//Date: Due Dec 11 2019.

using System;
using System.Collections.Generic;
using System.Text;
using HelpdeskDAL;
using System.Reflection;


namespace HelpdeskViewModels
{
    public class EmployeeViewModel
    {
        private EmployeeModel _model;

        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phoneno { get; set; }
        public string Timer { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int Id { get; set; }
        public bool IsTech { get; set; }
        public string StaffPicture64 { get; set; }

        //constructor
        public EmployeeViewModel()
        {
            _model = new EmployeeModel();
        }

        public void GetByLastname()
        {
            try
            {
                Employees emp = _model.GetByLastname(Lastname);
                this.Title = emp.Title;
                this.Firstname = emp.FirstName;
                this.Lastname = emp.LastName;
                this.Phoneno = emp.PhoneNo;
                this.Email = emp.Email;
                this.Id = emp.Id;
                this.DepartmentId = emp.DepartmentId;
                this.IsTech = emp.IsTech ?? false;
                if (emp.StaffPicture != null)
                {
                    this.StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                }
                Timer = Convert.ToBase64String(emp.Timer);
            }
            catch (NullReferenceException nex)
            {
                Lastname = "not found";
            }
            catch (Exception ex)
            {
                Lastname = "not Found";
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }
        //find Employee using Email property       
        public void GetByEmail()
        {
            try
            {
                Employees emp = _model.GetByEmail(Email);
                this.Title = emp.Title;
                this.Firstname = emp.FirstName;
                this.Lastname = emp.LastName;
                this.Phoneno = emp.PhoneNo;
                this.Email = emp.Email;
                this.Id = emp.Id;
                this.DepartmentId = emp.DepartmentId;
                this.IsTech = emp.IsTech ?? false;
                if (emp.StaffPicture != null)
                {
                    this.StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                }
                Timer = Convert.ToBase64String(emp.Timer);
            }
            catch (NullReferenceException nex)
            {
                Lastname = "not found";
            }
            catch (Exception ex)
            {
                Lastname = "not Found";
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }
        public void GetById()
        {
            try
            {
                Employees emp = _model.GetById(Id);
                this.Title = emp.Title;
                this.Firstname = emp.FirstName;
                this.Lastname = emp.LastName;
                this.Phoneno = emp.PhoneNo;
                this.Email = emp.Email;
                this.Id = emp.Id;
                this.DepartmentId = emp.DepartmentId;
                this.IsTech = emp.IsTech ?? false;
                if (emp.StaffPicture != null)
                {
                    this.StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                }
                Timer = Convert.ToBase64String(emp.Timer);
            }
            catch (NullReferenceException nex)
            {
                Lastname = "not found";
            }
            catch (Exception ex)
            {
                Lastname = "not Found";
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }
        public List<EmployeeViewModel> GetAll()
        {
            List<EmployeeViewModel> allVms = new List<EmployeeViewModel>();
            try
            {
                List<Employees> allEmployees = _model.GetAll();
                foreach (Employees emp in allEmployees)
                {
                    EmployeeViewModel empVm = new EmployeeViewModel();
                    empVm.Title = emp.Title;
                    empVm.Firstname = emp.FirstName;
                    empVm.Lastname = emp.LastName;
                    empVm.Phoneno = emp.PhoneNo;
                    empVm.Email = emp.Email;
                    empVm.Id = emp.Id;
                    empVm.DepartmentId = emp.DepartmentId;
                    empVm.DepartmentName = emp.Department.DepartmentName;
                    empVm.Timer = Convert.ToBase64String(emp.Timer);
                    empVm.IsTech = emp.IsTech ?? false;
                    if (emp.StaffPicture != null)
                    {
                        empVm.StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                    }
                    allVms.Add(empVm);
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
        public void Add()
        {
            Id = -1;
            try
            {
                Employees emp = new Employees();
                emp.Title = Title;
                emp.FirstName = Firstname;
                emp.LastName = Lastname;
                emp.PhoneNo = Phoneno;
                emp.Email = Email;
                emp.DepartmentId = DepartmentId;
                emp.IsTech = IsTech;
                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                }
                Id = _model.Add(emp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }
        public int Update()
        {
            UpdatedStatus osStatus = UpdatedStatus.Failed;
            try
            {
                Employees emp = new Employees();
                emp.Title = Title;
                emp.FirstName = Firstname;
                emp.LastName = Lastname;
                emp.PhoneNo = Phoneno;
                emp.Email = Email;
                emp.Id = Id;
                emp.DepartmentId = DepartmentId;
                emp.IsTech = IsTech;
                if (StaffPicture64 != null)
                {
                    emp.StaffPicture = Convert.FromBase64String(StaffPicture64);
                }
                emp.Timer = Convert.FromBase64String(Timer);
                osStatus = _model.Update(emp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return Convert.ToInt16(osStatus);
        }
        public int Delete()
        {
            int empolyeesDeleted = -1;
            try
            {
                empolyeesDeleted = _model.Delete(Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return empolyeesDeleted;
        }
    }
}
