using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntity.Logic.Commands
{
    public class MoneyEmptyResponse : MoneyCommandResponse
    {
        public override string ToBotString()
        {
            if (this.IsSuccess)
                return "команда выполнена успешно";
            else
                return this.ErrorText ?? "ошибка выполнения команды";
        }
    }
}
