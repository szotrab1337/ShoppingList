using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdbieranieRest.Models
{
    public class Temperature
    {
        public int ID_Temp { get; set; }
        public string Value { get; set; }
        public DateTime DateAdd { get; set; }
        public string CreatedBy { get; set; }

    }
}
