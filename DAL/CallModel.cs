//Project: Case Study2
//Purpose: Call Model Class
//Coder: Sonia Friesen, 0813682
//Date: Due Dec.11 2019
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelpdeskDAL
{
    public class CallModel
    {
        IRepository<Calls> repository;

        public CallModel()
        {
            repository = new HelpdeskRepository<Calls>();
        }
       public Calls GetById(int id)
        {
            return repository.GetByExpression(c => c.Id == id).FirstOrDefault();

        }
        public Calls GetByNotes(string notes)
        {
            return repository.GetByExpression(c => c.Notes == notes).FirstOrDefault();
        }
        public List<Calls> GetAll()
        {
            return repository.GetAll();
        }

        public int Add(Calls newCall)
        {
            return repository.Add(newCall).Id;

        }
        public UpdatedStatus Update(Calls updatedCall)
        {
            return repository.Update(updatedCall);
        }
        public int Delete(int id)
        {
            return repository.Delete(id);
        }
    }
}
