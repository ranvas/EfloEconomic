using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntity
{
    public class MoneyEntityBase
    {
        public MoneyEntityBase() { }
        public MoneyEntityBase(long id, string? table)
        {
            Id = id;
            Table = table;
        }

        public long Id { get; set; }
        public string? Table { get; set; }
        public override string ToString()
        {
            return $"{Table}_{Id}";
        }
    }
}
