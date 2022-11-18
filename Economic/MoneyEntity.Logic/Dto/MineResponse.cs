using MoneyEntity.Logic.Commands;
using MoneyEntity.Logic.GoogleSheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntity.Logic.Dto
{
    public class MineResponse : MoneyCommandResponse
    {
        public List<MineSheet> Mines { get; set; } = new();

        public override string ToBotString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Список шахт:");
            foreach (var sector in Mines.GroupBy(m => m.Sector))
            {
                sb.AppendLine($" {sector.Key}: ");
                foreach (var planet in sector.GroupBy(m => m.Planet))
                {
                    sb.AppendLine($"  {planet.Key}: ");
                    foreach (var mine in planet)
                    {
                        sb.AppendLine($"    {mine.Id} {mine.Resourse} {mine.Active} {mine.Value}");
                    }
                }
            }
            return sb.ToString();
        }
    }
}
