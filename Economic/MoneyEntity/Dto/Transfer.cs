using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntity.Dto
{
    public class Transfer : MoneyEntityBase
    {
        public long AccountFromId { get; set; }
        public Account? AccountFrom { get; set; }
        public long AccountToId { get; set; }
        public Account? AccountTo { get; set; }
        public string? CurrencyCode { get; set; }
        public string? TransferTime { get; set; }
        public decimal CurrencyValue { get; set; }
    }
}
