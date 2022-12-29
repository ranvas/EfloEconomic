using Google.Apis.Sheets.v4.Data;
using MoneyEntity.Dto;
using MoneyEntity.Logic.Entities;
using MoneyEntity.Logic.GoogleSheets;
using MoneyEntity.Logic.Primitives;
using System.Text;

namespace MoneyEntity.Logic.Commands
{
    public class ProfileResponse : MoneyCommandResponse
    {
        public ProfileResponse(Profile? profile = null) { Profile = profile; }
        public Profile? Profile { get; set; }

        public override string ToBotString()
        {
            if (Profile?.ProfileSheet == null)
                return "Ошибка инициализации персонажа";
            var sb = new StringBuilder();
            sb.AppendLine($"<b>Профайл для @{Profile.ProfileSheet.TgName}:</b>");
            sb.AppendLine($"Имя персонажа: {Profile.ProfileSheet.CharacterName}");
            sb.AppendLine($"Код кошелька: {Profile.ProfileSheet.WalletCode}");
            sb.AppendLine($"ID персонажа: {Profile.Account?.Id ?? 0}(для отладки, убрать)");
            sb.AppendLine($"Количество кредитов: {Profile.CurrentCredits}");
            sb.AppendLine($"Количество руды: {Profile.CurrentOres}");
            sb.AppendLine($"Количество металла: {Profile.CurrentMetalls}");
            sb.AppendLine($"Видит шахты: {Profile.CanSeeMines}");
            return sb.ToString();
        }
    }
}