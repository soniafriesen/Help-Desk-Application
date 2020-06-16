//Project: Case Study2
//Purpose: Problem Model Class
//Coder: Sonia Friesen, 0813682
//Date: Due Dec.11 2019
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HelpdeskDAL
{
    public class ProblemModel
    {
        IRepository<Problems> probRepo;        

        public ProblemModel()
        {
            probRepo = new HelpdeskRepository<Problems>();            
        }

        public Problems GetByDescription(string description)
        {
            return probRepo.GetByExpression(prob => prob.Description == description).FirstOrDefault();
        }

        public List<Problems> GetAll()
        {
            return probRepo.GetAll();
        }
    }
}
