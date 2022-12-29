using MoneyEntity.Dto;
using MoneyEntity.Logic.GoogleSheets;
using MoneyEntity.Logic.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntity.Logic.Entities
{
    public class Mine
    {
        public Mine(MineSheet mineSheeet) 
        { 
            MineSheet = mineSheeet; 
        }
        public MineSheet MineSheet { get; set; }

        public List<Bid> Bids { get; set; } = new();

        public int Capacity => Bids.Sum(b => b.Value);

        public int Id => int.TryParse(MineSheet.Id, out var mineId) ? mineId : 0;

        public CurrencyCodes CurrencyCode => MineSheet.Resourse switch
        {
            "руда" => CurrencyCodes.Ore,
            "металл" => CurrencyCodes.Metall,
            _ => CurrencyCodes.Unknown
        };

        public MineActivity MineActivity => MineSheet.Active switch
        {
            "READY" => MineActivity.READY,
            "ACTIVE" => MineActivity.ACTIVE,
            "HIDE" => MineActivity.HIDE,
            _ => MineActivity.UNKNOWN
        };

        public int MaxCapacity => int.TryParse(MineSheet.MaxCapacity, out var maxCapacity) ? maxCapacity : 0;
    }
}
