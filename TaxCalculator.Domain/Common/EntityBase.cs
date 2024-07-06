using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Domain.Common
{
    public abstract record EntityBase
    {
        public long Id { get; set; }
    }
}
