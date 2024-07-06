using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTdd
{
    public record VehicleDetailsDto
    {
 
        public string VehicleType { get; set; }
        public DateTime[] PassedDate { get; set; }
    }
  
}
