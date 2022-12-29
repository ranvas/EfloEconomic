using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntity.Dto
{
    public class Bid : MoneyEntityBase
    {
        public int Value { get; set; }
        public long AccountId { get; set; }
        public Account? Account { get; set; }
        public int MineId { get; set; }
    }
}
