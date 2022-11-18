using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntity.Logic.GoogleSheets
{
    public class MineSheet
    {
        [Display(Name = "id шахты")]
        public string? Id { get; set; }
        [Display(Name = "планета шахты")]
        public string? Planet { get; set; }
        [Display(Name = "сектор шахты")]
        public string? Sector { get; set; }
        [Display(Name = "ресурс шахты")]
        public string? Resourse { get; set; }
        [Display(Name = "выработка")]
        public string? Value { get; set; }
        [Display(Name = "активная")]
        public string? Active { get; set; }
    }
}
