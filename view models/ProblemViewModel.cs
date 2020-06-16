//Project: Case Study2
//Purpose: Problem Viewmodel Class
//Coder: Sonia Friesen, 0813682
//Date: Due Dec.11 2019
using System;
using System.Collections.Generic;
using System.Text;
using HelpdeskDAL;
using System.Reflection;

namespace HelpdeskViewModels
{
    public class ProblemViewModel
    {
        private ProblemModel _model;

        public int Id { get; set; }
        public string Description { get; set; }
        public string Timer { get; set; }

        public ProblemViewModel()
        {
            _model = new ProblemModel();
        }

        public void GetByDescription()
        {
            try
            {
                Problems prob = _model.GetByDescription(Description);
                this.Id = prob.Id;
                this.Description = prob.Description;
                Timer = Convert.ToBase64String(prob.Timer);
            }
            catch(NullReferenceException nex)
            {
                throw nex;
            }
            catch (Exception ex)
            {
                Description = "not Found";
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        public List<ProblemViewModel> GetAll()
        {
            List<ProblemViewModel> allVms = new List<ProblemViewModel>();
            try
            {
                List<Problems> allProblems = _model.GetAll();
                foreach (Problems prob in allProblems)
                {
                    ProblemViewModel probVm = new ProblemViewModel();
                    probVm.Id = prob.Id;
                    probVm.Description = prob.Description;
                    probVm.Timer = Convert.ToBase64String(prob.Timer);                    
                    allVms.Add(probVm);
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
