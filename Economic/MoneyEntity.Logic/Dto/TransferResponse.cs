using MoneyEntity.Logic.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntity.Logic.Dto
{
    public class TransferResponse : MoneyCommandResponse
    {
        //public TransferResponse ReturnError(string errorText)
        //{
        //    IsSuccess = false;
        //    ErrorText = errorText;
        //    return this;
        //}

        public override string ToBotString()
        {
            return "Успешный перевод";
        }
    }
}
