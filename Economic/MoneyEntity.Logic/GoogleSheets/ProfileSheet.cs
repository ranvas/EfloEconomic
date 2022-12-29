using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntity.Logic.GoogleSheets
{
    public class ProfileSheet
    {
        [Display(Name = "имя персонажа")]
        public string? CharacterName { get; set; }
        [Display(Name = "Код счета")]
        public string? WalletCode { get; set; }
        [Display(Name = "TgName")]
        public string? TgName { get; set; }
        [Display(Name = "Стартовый баланс кредитов")]
        public string? StartCredits { get; set; }
        [Display(Name = "Стартовый баланс руды")]
        public string? StartOre { get; set; }
        [Display(Name = "Стартовый баланс металла")]
        public string? StartMetall { get; set; }
        [Display(Name = "Видит шахты")]
        public string? CanSeeMines { get; set; }
        [Display(Name = "Админ")]
        public string? IsAdminString { get; set; }
    }
}
