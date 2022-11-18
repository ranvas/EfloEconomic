using Integrators.Abstractions;
using Integrators.Dispatcher;
using Microsoft.Extensions.Hosting;
using MoneyEntity.Logic.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using TgBot.DataSphere;

namespace MoneyEntity.Logic
{
    public class MoneyBot : BotServiceBase, IHostedService
    {
        MoneyDispatcher _dispatcher;
        public Dictionary<string, string> _commands = new();
        public MoneyBot(string token, MoneyDispatcher dispatcher)
            : base(token)
        {
            _dispatcher = dispatcher;
            AddCommand("/get_status", "Информация о профайле", nameof(MoneyManager.GetStatus));
            AddCommand("/send_credits"
                , "Отправить кредиты на кошелек другого пользователя. Формат: '/send_credits X Y', где X-код кошелька получателя, Y - количество кредитов для отправки'"
                , nameof(MoneyManager.SendCredits));
            AddCommand("/send_ore"
                , "Отправить руду на кошелек другого пользователя. Формат: '/send_ores X Y', где X-код кошелька получателя, Y - количество руды для отправки'"
                , nameof(MoneyManager.SendOres));
            AddCommand("/send_metalls"
                , "Отправить металлы на кошелек другого пользователя. Формат: '/send_metalls X Y', где X-код кошелька получателя, Y - количество металлов для отправки'"
                , nameof(MoneyManager.SendMetalls));
            AddCommand("/get_mines"
                , "Список шахт"
                , nameof(MoneyManager.GetMines));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            StartReceiving();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected override async Task HandleCommand(string command, string? param, Update executionContext)
        {
            var tgId = GetChatId(executionContext);
            string? response;
            if (_commands.ContainsKey(command))
            {
                var request = new MoneyCommandRequest
                {
                    TgId = tgId,
                    Username = GetUserName(executionContext),
                    Params = param ?? string.Empty
                };
                try
                {
                    response = await _dispatcher.DispatchSpecified(command, request);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                    response = "Ошибка";
                }
            }
            else
            {
                response = GetHelp();
            }
            await SendTextMessage(chatId: tgId, text: response ?? "Пустой ответ");
        }

        private void AddCommand(string command, string description, string methodName)
        {
            _commands.Add(command, description);
            _dispatcher.RegisterService<MoneyManager>(command, typeof(MoneyCommandRequest), methodName);
        }

        public string GetHelp()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Список доступных комманд:");
            foreach (var item in _commands)
            {
                sb.AppendLine($"<b>{item.Key}</b> : {item.Value}");
            }
            return sb.ToString();
        }

        private string GetUserName(Update executionContext)
        {
            var userName = executionContext.Message?.Chat?.Username ?? string.Empty;
            if (userName.StartsWith("@"))
            {
                userName = userName.Substring(1);
            }
            return userName.ToLower();
        }

        private long GetChatId(Update executionContext)
        {
            return executionContext.Message?.Chat?.Id ?? 0;
        }

        private string GetChatIdString(Update executionContext)
        {
            return (executionContext.Message?.Chat?.Id ?? 0).ToString();
        }
    }
}

