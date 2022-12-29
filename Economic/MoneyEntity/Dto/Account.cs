using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntity.Dto
{
    public class Account : MoneyEntityBase
    {
        public long TgId { get; set; }
        public string? TgName { get; set; }
        public string? WalletCode { get; set; }
        public List<Transfer>? TransfersFrom { get; set; }
        public List<Transfer>? TransfersTo { get; set; }
        public List<Bid>? Bids { get; set; }
    }
}
