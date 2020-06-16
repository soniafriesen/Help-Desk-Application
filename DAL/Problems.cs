//Project: Case Study2
//Purpose: Problem Class
//Coder: Sonia Friesen, 0813682
//Date: Due Dec.11 2019
using System;
using System.Collections.Generic;

namespace HelpdeskDAL
{
    public partial class Problems : HelpdeskEntity
    {
        public Problems()
        {
            Calls = new HashSet<Calls>();
        }

        public string Description { get; set; }

        public virtual ICollection<Calls> Calls { get; set; }
    }
}
