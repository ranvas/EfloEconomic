using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntity
{
    public class MoneyEntityBaseException : Exception
    {
        public MoneyEntityBaseException()
            : base() { }
        public MoneyEntityBaseException(string? message)
            : base(message) { }
        public MoneyEntityBaseException(string? message, Exception? inner)
            : base(message, inner) { }
    }
}
