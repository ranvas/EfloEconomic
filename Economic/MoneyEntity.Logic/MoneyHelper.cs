using MoneyEntity.Logic.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntity.Logic
{
    public static class MoneyHelper
    {

        public static T ReturnError<T>(this T item, string errorText) where T: MoneyCommandResponse
        {
            item.IsSuccess = false;
            item.ErrorText = errorText;
            return item;
        }

        public static TransferToParam? ParseTransferParam(string? param)
        {
            if (param == null) return null;
            var list = param.Split(' ').ToList();
            if (list.Count != 2) return null;
            var walletTo = list[0];
            var amount = list[1].Replace(',', '.');
            if (!decimal.TryParse(amount, out decimal value)) return null;
            return new TransferToParam
            {
                Value = value,
                WalletCode = walletTo
            };
        }

        public static string GetProfileKey(long tgId)
        {
            return $"profile_{tgId}";
        }

    }
}
