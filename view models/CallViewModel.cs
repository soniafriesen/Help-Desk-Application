//Project: Case Study2
//Purpose: Call viewmodel Class
//Coder: Sonia Friesen, 0813682
//Date: Due Dec.11 2019
using System;
using System.Collections.Generic;
using System.Text;
using HelpdeskDAL;
using System.Reflection;

namespace HelpdeskDAL
{
    public class CallViewModel
    {
        private CallModel _model;

        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int ProblemId { get; set; }
        public string EmployeeName { get; set; }
        public string ProblemDescription { get; set; }
        public string TechName { get; set; }
        public int TechId { get; set; }
        public DateTime DateOpened { get; set; }
        public DateTime? DateClosed { get; set; }
        public bool OpenStatus { get; set; }
        public string Notes { get; set; }
        public string Timer { get; set; }

        //constructor
        public CallViewModel()
        {
            _model = new CallModel();
        }

        //viewmodel functions

        public void GetById()
        {
            try
            {
                Calls call = _model.GetById(Id);
                this.Id = call.Id;
                this.EmployeeId = call.EmployeeId;
                this.ProblemId = call.ProblemId;
                this.TechId = call.TechId;               
                this.DateOpened = call.DateOpened;
                this.DateClosed = call.DateClosed;
                this.OpenStatus = call.OpenStatus;
                this.Notes = call.Notes;              
                
                Timer = Convert.ToBase64String(call.Timer);
            }
            catch (NullReferenceException nex)
            {
                throw nex;
            }
            catch (Exception ex)
            {
                this.Notes = "not Found";
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }
        public void Add()
        {
            Id = -1;
            try
            {
                Calls call = new Calls();
                call.EmployeeId = EmployeeId;         
                call.ProblemId = ProblemId;
                call.TechId = TechId;                
                call.DateOpened = DateOpened;
                call.DateClosed = DateClosed ;
                call.OpenStatus = OpenStatus;
                call.Notes = Notes;

                Id = _model.Add(call);
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
                Calls call = new Calls();
                call.Id = Id;
                call.EmployeeId = EmployeeId;
                call.ProblemId = ProblemId;
                call.TechId = TechId;
                call.DateOpened = DateOpened;
                call.DateClosed = DateClosed;
                call.OpenStatus = OpenStatus;
                call.Notes = Notes;
                call.Timer = Convert.FromBase64String(Timer);
                osStatus = _model.Update(call);
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
            int callsDeleted = -1;
            try
            {
                callsDeleted = _model.Delete(Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return callsDeleted;
        }
        public void GetByNotes()
        {
            try
            {
                Calls call = _model.GetByNotes(Notes);
                this.Id = call.Id;
                this.EmployeeId = call.EmployeeId;
                this.EmployeeName = call.Employee.LastName;
                this.ProblemId = call.ProblemId;
                this.ProblemDescription = call.Problem.Description;
                this.TechId = call.TechId;
                this.TechName = call.Tech.FirstName + " " + call.Tech.LastName;
                this.DateOpened = call.DateOpened;
                this.DateClosed = call.DateClosed;
                this.OpenStatus = call.OpenStatus;
                this.Notes = call.Notes;
                Timer = Convert.ToBase64String(call.Timer);
            }
            catch (NullReferenceException nex)
            {
                Notes = "not found";
            }
            catch (Exception ex)
            {
                Notes = "not Found";
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }
        public List<CallViewModel> GetAll()
        {
            List<CallViewModel> allVms = new List<CallViewModel>();
            try
            {
                List<Calls> allCalls = _model.GetAll();
                foreach (Calls call in allCalls)
                {
                    CallViewModel callVm = new CallViewModel();
                    callVm.EmployeeId = call.EmployeeId;
                    callVm.EmployeeName = call.Employee.FirstName + " " + call.Employee.LastName;
                    callVm.ProblemId = call.ProblemId;
                    callVm.ProblemDescription = call.Problem.Description;
                    callVm.TechId = call.TechId;
                    callVm.TechName = call.Tech.FirstName + " " + call.Tech.LastName;
                    callVm.DateOpened = call.DateOpened;
                    callVm.DateClosed = call.DateClosed;
                    callVm.OpenStatus = call.OpenStatus;
                    callVm.Notes = call.Notes;
                    callVm.Timer = Convert.ToBase64String(call.Timer);
                    allVms.Add(callVm);
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
