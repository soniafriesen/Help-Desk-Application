//Project: Case Study1
//Purpose: stores the id an dtimer for both employeid and departmentid so its no longer hardcoded in the employee/deparmtnet class
//Coder: Sonia Friesen, 0813682
//Date: Due oct 23rd.

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace HelpdeskDAL
{
    public class HelpdeskEntity
    {     
        public int Id { get; set; }
        [Timestamp]
        public byte[] Timer { get; set; }
    }
}
