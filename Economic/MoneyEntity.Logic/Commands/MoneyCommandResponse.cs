using Integrators.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntity.Logic.Commands
{
    public abstract class MoneyCommandResponse 
    {
        public bool IsSuccess { get; set; }
        public string? ErrorText { get; set; }
        public abstract string ToBotString();
    }
}
