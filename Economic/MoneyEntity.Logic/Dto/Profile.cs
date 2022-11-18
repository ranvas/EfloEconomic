using Google.Apis.Sheets.v4.Data;
using MoneyEntity.Dto;
using MoneyEntity.Logic.Commands;
using MoneyEntity.Logic.GoogleSheets;
using MoneyEntity.Logic.Primitives;
using System.Text;

namespace MoneyEntity.Logic
{
    public class Profile : MoneyCommandResponse
    {
        public Profile()
        {
            AllTransfers = new();
        }
        public long TgId { get; set; }
        public string? TgIdString => TgId.ToString();
        public ProfileSheet? ProfileSheet { get; set; }
        public Account? Account { get; set; }
        public List<Transfer> AllTransfers { get; set; }

        public IEnumerable<Transfer> Credits
        {
            get
            {
                var code = CurrencyCodes.Credit.ToString();
                return AllTransfers.Where(t => t.CurrencyCode == code);
            }
        }

        public IEnumerable<Transfer> Ores
        {
            get
            {
                var code = CurrencyCodes.Ore.ToString();
                return AllTransfers.Where(t => t.CurrencyCode == code);
            }
        }

        public IEnumerable<Transfer> Metalls
        {
            get
            {
                var code = CurrencyCodes.Metall.ToString();
                return AllTransfers.Where(t => t.CurrencyCode == code);
            }
        }

        public decimal StartCredits
        {
            get
            {
                if (decimal.TryParse(ProfileSheet?.StartCredits, out decimal result))
                {
                    return result;
                }
                return 0;
            }
        }

        public decimal StartOres
        {
            get
            {
                if (decimal.TryParse(ProfileSheet?.StartOre, out decimal result))
                {
                    return result;
                }
                return 0;
            }
        }

        public decimal StartMetalls
        {
            get
            {
                if (decimal.TryParse(ProfileSheet?.StartMetall, out decimal result))
                {
                    return result;
                }
                return 0;
            }
        }

        public decimal CurrentMetalls
        {
            get
            {
                var sum = Metalls.Sum(t => t.CurrencyValue);
                return StartMetalls + sum;
            }
        }

        public decimal CurrentOres
        {
            get
            {
                var sum = Ores.Sum(t => t.CurrencyValue);
                return StartOres + sum;
            }
        }

        public decimal CurrentCredits
        {
            get
            {
                var sum = Credits.Sum(t => t.CurrencyValue);
                return StartCredits + sum;
            }
        }

        public bool CanSeeMines
        {
            get
            {
                if (bool.TryParse(ProfileSheet?.CanSeeMines, out bool result))
                {
                    return result;
                }
                return false;
            }
        }

        public override string ToBotString()
        {
            if (ProfileSheet == null)
                return "Ошибка инициализации персонажа, код ошибки: GD";
            var sb = new StringBuilder();
            sb.AppendLine($"<b>Профайл для @{ProfileSheet.TgName}:</b>");
            sb.AppendLine($"Имя персонажа: {ProfileSheet.CharacterName}");
            sb.AppendLine($"Код кошелька: {ProfileSheet.WalletCode}");
            sb.AppendLine($"ID персонажа: {Account?.Id ?? 0}(для отладки, убрать)");
            sb.AppendLine($"Количество кредитов: {CurrentCredits}");
            sb.AppendLine($"Количество руды: {CurrentOres}");
            sb.AppendLine($"Количество металла: {CurrentMetalls}");
            sb.AppendLine($"Видит шахты: {CanSeeMines}");
            return sb.ToString();
        }
    }
}