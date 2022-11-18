using Integrators.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntity.Logic.Commands
{
    public class MoneyCommandRequest
    {
        public long TgId { get; set; }
        public string? Username { get; set; }
        public string? Params { get; set; }
    }
}
