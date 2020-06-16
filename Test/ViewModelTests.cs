//Project: Case Study2
//Purpose: viewModel Tests
//Coder: Sonia Friesen, 0813682
//Date: Due Dec 11 2019

using System;
using Xunit;
using HelpdeskViewModels;
using System.Linq;
using Xunit.Abstractions;
using HelpdeskDAL;

namespace HelpdeskTests
{
   public class ViewModelTests
    {
        private readonly ITestOutputHelper output;

        public ViewModelTests(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
        public void Employee_GetbyEmailTest()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            vm.Email = "ng@abc.com"; // look for existing student
            vm.GetByEmail();
            Assert.True(vm.Id > 0);
        }
        [Fact]
        public void Employee_AddTest()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            vm.Title = "Mrs";
            vm.Firstname = "Sonia";
            vm.Lastname = "Friesen";
            vm.Phoneno = "123-345-222";
            vm.Email = "testEmployee@helpdesk.com";
            vm.DepartmentId = 200;
            vm.Add();
            Assert.True(vm.Id > 0);
        }
        [Fact]
        public void Employee_GetById()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            vm.GetById();
            Assert.True(vm.Timer != null);
        }
        [Fact]
        public void Employee_GetAllTest()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            var emp = vm.GetAll();
            Assert.True(emp.Count() > 0);
        }
        [Fact]
        public void Employee_UpdateTest()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            vm.Email = "testEmployee@helpdesk.com";
            vm.GetByEmail();
            string oldPhone = vm.Phoneno;
            string newPhone = oldPhone == "123-345-4444" ? "123-345-222" : "123-345-4444";
            vm.Phoneno = newPhone;
            Assert.True(vm.Update() == 1);
        }
        [Fact]
        public void Employee_DeleteTest()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            vm.Email = "testEmployee@helpdesk.com";
            vm.GetByEmail();

            Assert.True(vm.Delete() == 1);
        }

        [Fact]
        public void Employee_ConcurrencyTest()
        {
            EmployeeViewModel vm1 = new EmployeeViewModel();
            EmployeeViewModel vm2 = new EmployeeViewModel();
            vm1.Lastname = "Pincher";
            vm2.Lastname = "Pincher";
            vm1.GetByEmail();
            vm2.GetByEmail();
            vm1.Email = (vm1.Email.IndexOf(".com") > 0) ? "pp@abc.com" : "bn@someschool.ca";
            if (vm1.Update() == 1)
            {
                vm2.Email = "someting@diferent.ca";
                Assert.True(vm2.Update() == -2);
            }
            else
                Assert.True(false);
        }
        [Fact]
        public void Call_ComprehensiveVMTest()
        {
            CallViewModel cvm = new CallViewModel();
            EmployeeViewModel evm = new EmployeeViewModel();
            ProblemViewModel pvm = new ProblemViewModel();
            cvm.DateOpened = DateTime.Now;
            cvm.DateClosed = null;
            cvm.OpenStatus = true;
            evm.Email = "sfriesen@fanshawe.com";
            evm.GetByEmail();
            cvm.EmployeeId = evm.Id;
            evm.Lastname = "sfriesen@fanshawe.com";
            evm.GetByEmail();
            cvm.EmployeeName = evm.Lastname;
            cvm.TechId = evm.Id;
            pvm.Description = "Memory Upgrade";
            pvm.GetByDescription();
            cvm.ProblemId = pvm.Id;
            cvm.Notes = "Sonia has bad RAM, Burner to fix it";
            cvm.Add();
            output.WriteLine("New Call Generated - Id = " + cvm.Id);
            int id = cvm.Id;
            cvm.GetById();
            cvm.Notes += "\n Ordered new RAM!";
            if(cvm.Update() == 1)
            {
                output.WriteLine("Call was updated " + cvm.Notes);
            }
            else
            {
                output.WriteLine("Call was not updated!");
            }
            cvm.Notes = "Another change to comments that should not work";
            if(cvm.Update() == -2)
            {
                output.WriteLine("Call was not updated data was stale");
            }
            cvm = new CallViewModel();// need to reset because of concureency error
            cvm.Id = id;
            cvm.GetById();

            if(cvm.Delete() == 1)
            {
                output.WriteLine("Call was deleted");
            }
            else
            {
                output.WriteLine("Call was not deleted");
            }
            Exception ex = Assert.Throws<NullReferenceException>(() => cvm.GetById()); // should throw expected exception
            Assert.Equal("Object reference not set to an instance of an object.", ex.Message);
        }
    }
}
