using MoneyEntity.Logic.Entities;
using MoneyEntity.Logic.GoogleSheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntity.Logic.Commands
{
    public class MineResponse : MoneyCommandResponse
    {
        public List<Mine> Mines { get; set; } = new();

        public override string ToBotString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Список шахт:");
            foreach (var sector in Mines.Where(m => m.MineActivity != Primitives.MineActivity.HIDE).GroupBy(m => m.MineSheet.Sector))
            {
                sb.AppendLine($" {sector.Key}: ");
                foreach (var planet in sector.GroupBy(m => m.MineSheet.Planet))
                {
                    sb.AppendLine($"  {planet.Key}: ");
                    foreach (var mine in planet)
                    {
                        sb.AppendLine($"    {mine.MineSheet.Id} {mine.MineSheet.Resourse} {mine.MineSheet.Active} {mine.Capacity}/{mine.MaxCapacity}");
                    }
                }
            }
            return sb.ToString();
        }
    }
}
