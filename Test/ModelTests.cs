//Project: Case Study2
//Purpose:model tests
//Coder: Sonia Friesen, 0813682
//Date: Due Dec 11, 2019.

using System;
using Xunit;
using HelpdeskDAL;
using System.Linq;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace HelpdeskTests
{
    public class ModelTests
    {
        private readonly ITestOutputHelper output;
        
        public ModelTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Employee_GetByEmailTest()
        {
            EmployeeModel model = new EmployeeModel();
            Employees employee = model.GetByEmail("bs@abc.com");
            Assert.True(employee.Id > 0);
        }

        [Fact]
        public void Employee_Add()
        {
            EmployeeModel model = new EmployeeModel();
            Employees newEmployee = new Employees();
            newEmployee.FirstName = "Test";
            newEmployee.LastName = "Employee";
            newEmployee.PhoneNo = "(555)555-1234";
            newEmployee.Title = "Mrs.";
            newEmployee.DepartmentId = 100;
            newEmployee.Email = "SF@EmployeeEmail.com";

            model.Add(newEmployee);

            Assert.True(newEmployee.Id > 0);
        }
        [Fact]
        public void Employee_DeleteTest()
        {
            EmployeeModel model = new EmployeeModel();            
            Assert.True(model.Delete(15) == 1);
        }
        [Fact]
        public void Employee_GetAllTest()
        {
            EmployeeModel model = new EmployeeModel();
            List<Employees> allEmployees = model.GetAll();
            Assert.True(allEmployees.Count() > 0);
        }
        [Fact]
        public void Employee_GetByIdTest()
        {
            EmployeeModel model = new EmployeeModel();
            var emp = model.GetById(1);
            Assert.True(emp.Timer != null);
        }
        [Fact]
        public void Employee_UpdateTest()
        {
            EmployeeModel model = new EmployeeModel();     
            Employees selectedEmployee = model.GetByEmail("SF@EmployeeEmail.com");

            if (selectedEmployee != null)
            {
                string oldEmail = selectedEmployee.Email;
                string newEmail = oldEmail == "TestEmail.com" ? "SF@EmployeeEmail.com" : "TestEmail.com";
                selectedEmployee.Email = newEmail;               
            }            
            Assert.True(model.Update(selectedEmployee)== UpdatedStatus.Ok);
        }
        [Fact]
        public void Employee_ConcurrencyTest()
        {
            EmployeeModel model1 = new EmployeeModel();
            EmployeeModel model2 = new EmployeeModel();
            Employees emp1 = model1.GetByEmail("SF@EmployeeEmail.com");
            Employees emp2 = model2.GetByEmail("SF@EmployeeEmail.com");

            if (emp1 != null)
            {
                string oldPhoneNo = emp1.PhoneNo;
                string newPhoneNo = oldPhoneNo == "(555)555-1234" ? "555-555-5555" : "(555)555-1234";
                emp1.PhoneNo = newPhoneNo;
                if (model1.Update(emp1) == UpdatedStatus.Ok)
                {
                    //need to change the phone # to something else
                    emp2.PhoneNo = "666-666-6666";
                    Assert.True(model2.Update(emp2) == UpdatedStatus.Stale);
                }
                else
                    Assert.True(false);
            }

        }
        [Fact]
        public void Employee_LoadPicsTest()
        {
            DALUtil util = new DALUtil();
            Assert.True(util.AddEmployeePicsToDb());
        }
        [Fact]
        public void Call_ComprehensiceTest()
        {
            CallModel cmodel = new CallModel();
            EmployeeModel emodel = new EmployeeModel();
            ProblemModel pmodel = new ProblemModel();
            Calls call = new Calls();
            call.DateOpened = DateTime.Now;
            call.DateClosed = null;
            call.OpenStatus = true;
            call.EmployeeId = emodel.GetByLastname("Friesen").Id;
            call.TechId = emodel.GetByLastname("Burner").Id;
            call.ProblemId = pmodel.GetByDescription("Hard Drive Failure").Id;
            call.Notes = "Sonia's drive is shot, Burner to fix it";
            int newCallId = cmodel.Add(call);
            output.WriteLine("New Call Generated - Id = " + newCallId);
            call = cmodel.GetById(newCallId);
            byte[] oldtimer = call.Timer;
            output.WriteLine("New Call Retrieved");
            call.Notes += "\n Ordered new drive!";

            if(cmodel.Update(call) == UpdatedStatus.Ok)
            {
                output.WriteLine("Call was updated " + call.Notes);
            }
            else
            {
                output.WriteLine("Call was not updated!");
            }
            call.Timer = oldtimer;
            call.Notes = "doesn't matter data is stale now";
            if(cmodel.Update(call) == UpdatedStatus.Stale)
            {
                output.WriteLine("Call was not updated due to stale data");
            }
            cmodel = new CallModel();
            call = cmodel.GetById(newCallId);

            if(cmodel.Delete(newCallId) == 1)
            {
                output.WriteLine("Call was deleted!");
            }
            else
            {
                output.WriteLine("Call was not deleted");
            }
            Assert.Null(cmodel.GetById(newCallId));
        }        
    }
}
