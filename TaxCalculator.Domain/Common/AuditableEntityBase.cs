using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Domain.Common
{
    public abstract record AuditableEntityBase : EntityBase
    {
        public DateTimeOffset CreatedAt { get; set; }

        public long CreatedBy { get; set; }

        public DateTimeOffset LastUpdatedAt { get; set; }

        public long LastUpdatedBy { get; set; }
    }
}
