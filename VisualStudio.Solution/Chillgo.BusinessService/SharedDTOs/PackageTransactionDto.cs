using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chillgo.BusinessService.SharedDTOs
{
    public class PackageTransactionDto
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid PackageId { get; set; }
        public int ChillCoinApplied { get; set; }
        public string PayMethod { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = null!;
    }
}
