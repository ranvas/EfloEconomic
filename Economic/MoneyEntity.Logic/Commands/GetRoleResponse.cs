using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntity.Logic.Commands
{
    public class GetRoleResponse : MoneyCommandResponse
    {
        public override string ToBotString()
        {
            if (this.IsSuccess)
                return "admin";
            else
                return "user";
        }
    }
}
