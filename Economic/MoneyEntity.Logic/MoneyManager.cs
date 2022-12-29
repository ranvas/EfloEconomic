using DataAccess.Abstractions;
using GoogleSheet.Abstractions;
using GoogleSheet.Core;
using Integrators.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using MoneyEntity.Dto;
using MoneyEntity.Logic.Commands;
using MoneyEntity.Logic.Entities;
using MoneyEntity.Logic.GoogleSheets;
using MoneyEntity.Logic.Primitives;
using SqlLiteDataAccess;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TgBot.DataSphere;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MoneyEntity.Logic
{
    public class MoneyManager
    {
        DataAccessConfiguration _dbConfig;
        MyDataService GetDb() => new MyDataService(_dbConfig);
        protected MemoryCacheService _memoryCacheService;
        protected virtual TimeSpan DataLoadGoogleSheetCacheTime { get; set; } = new TimeSpan(0, 1, 0);
        protected virtual TimeSpan ProfileCacheTime { get; set; } = new TimeSpan(0, 1, 0);
        GoogleSheetDataManager _googleManager { get; set; }

        public MoneyManager(MemoryCache memoryCache, string dbConnectionString)
        {
            _memoryCacheService = new(memoryCache);
            _dbConfig = new(dbConnectionString);
            _googleManager = new();
        }

        public async Task<ProfileResponse> GetStatus(MoneyCommandRequest command)
        {
            var profile = await GetProfile(command.TgId, command.Username);
            var result = new ProfileResponse(profile);
            if (profile == null)
                return result.ReturnError($"Персонаж для {command.Username} не найден");

            result.IsSuccess = true;
            return result;
        }

        public async Task<MoneyCommandResponse> StartCycle(MoneyCommandRequest request)
        {
            var profile = await CheckAdmin(request);
            if (profile == null)
                return new MoneyEmptyResponse().ReturnError("Ошибка проверки прав");

            return new MoneyEmptyResponse { IsSuccess = true };
        }

        public async Task<GetRoleResponse> GetRole(MoneyCommandRequest request)
        {
            var profile = await CheckAdmin(request);
            if (profile == null)
                return new GetRoleResponse().ReturnError("user");

            return new GetRoleResponse { IsSuccess = true };
        }

        public async Task<MoneyEmptyResponse> StopCycle(MoneyCommandRequest request)
        {
            var profile = await CheckAdmin(request);
            if (profile == null)
                return new MoneyEmptyResponse().ReturnError("Ошибка проверки прав");

            return new MoneyEmptyResponse { IsSuccess = true };
        }

        public async Task<MoneyEmptyResponse> PayCycle(MoneyCommandRequest request)
        {
            var profile = await CheckAdmin(request);
            if (profile == null)
                return new MoneyEmptyResponse().ReturnError("Ошибка проверки прав");

            return new MoneyEmptyResponse { IsSuccess = true };
        }

        public async Task<MoneyEmptyResponse> DoBid(MoneyCommandRequest request)
        {
            var profile = await GetProfile(request.TgId, request.Username);
            
            
            await CreateNewTransfer(profile, null, 1, CurrencyCodes.Credit);
            var db = GetDb();
            var random = new Random();
            if(random.Next(1,3) == 1)
            {
                var bid = new Bid { AccountId = profile?.Account?.Id ?? 0, MineId = 145, Value = 1 };
                await db.AddItem(bid);
            }
            else
            {
                var bid = new Bid { AccountId = profile?.Account?.Id ?? 0, MineId = 245, Value = 1 };
                await db.AddItem(bid);
            }
            return new MoneyEmptyResponse { IsSuccess = true };
        }

        public async Task<MineResponse> GetMines(MoneyCommandRequest command)
        {
            var result = new MineResponse();
            var db = GetDb();
            var profile = await GetProfile(command.TgId, command.Username);
            if (profile == null)
                return result.ReturnError($"Персонаж для {command.Username} не найден");
            if (!profile.CanSeeMines)
                return result.ReturnError($"У {profile.ProfileSheet?.CharacterName} нет доступа для просмотра шахт");
            var mines = (await _googleManager.MineSheet.GetAllItems()).Select(m => new Mine(m));
            result.Mines = mines.ToList();
            foreach (var mine in result.Mines)
            {
                mine.Bids = db.GetList<Bid>(m=>m.MineId == mine.Id).ToList();
            }
            
            result.IsSuccess = true;
            return result;
        }

        public async Task<TransferResponse> SendCredits(MoneyCommandRequest command)
        {
            return await Send(command, CurrencyCodes.Credit);
        }

        public async Task<TransferResponse> SendOres(MoneyCommandRequest command)
        {
            return await Send(command, CurrencyCodes.Ore);
        }

        public async Task<TransferResponse> SendMetalls(MoneyCommandRequest command)
        {
            return await Send(command, CurrencyCodes.Metall);
        }

        private async Task<TransferResponse> Send(MoneyCommandRequest command, CurrencyCodes code)
        {
            var profileFrom = await GetProfile(command.TgId, command.Username);
            var result = new TransferResponse();
            if (profileFrom == null || profileFrom.Account == null)
                return result.ReturnError($"Персонаж для {command.Username} не найден");
            var parsedParams = MoneyHelper.ParseTransferParam(command.Params);
            if (parsedParams == null)
                return result.ReturnError("Ошибка передачи параметров");
            if (parsedParams.Value < 0)
                return result.ReturnError("Нельзя указать к переводу отрицательное значение или 0");
            if (string.IsNullOrEmpty(parsedParams.WalletCode))
                return result.ReturnError("Ошибка передачи получателя");
            var profileTo = await GetProfileByWalletCode(parsedParams.WalletCode);
            if (profileTo == null || profileTo.Account == null)
                return result.ReturnError("Не удалось найти получателя");
            switch (code)
            {
                case CurrencyCodes.Credit:
                    if (profileFrom.CurrentCredits - parsedParams.Value < 0)
                        return result.ReturnError("Недостаточно кредитов");
                    break;
                case CurrencyCodes.Ore:
                    if (profileFrom.CurrentOres - parsedParams.Value < 0)
                        return result.ReturnError("Недостаточно руды");
                    break;
                case CurrencyCodes.Metall:
                    if (profileFrom.CurrentMetalls - parsedParams.Value < 0)
                        return result.ReturnError("Недостаточно металлов");
                    break;
                default:
                    throw new Exception("Внутренняя ошибка, код: ECC");
            }

            //прошли все проверки
            result.IsSuccess = true;
            return await CreateNewTransfer(profileFrom, profileTo, parsedParams.Value, code) ?
                result : result.ReturnError("ошибка при отправке");
        }

        private async Task<bool> CreateNewTransfer(Profile? from, Profile? to, decimal value, CurrencyCodes currencyCode)
        {
            var db = GetDb();
            var key = "profiles";
            if (from != null)
            {
                var newTransferFrom = new Transfer
                {
                    AccountFromId = from.Account?.Id,
                    Comment = $"перевод в пользу {to?.Account?.Id.ToString() ?? "неизвестный"}",
                    CurrencyValue = -value,
                    CurrencyCode = currencyCode.ToString()
                };
                await db.AddItem(newTransferFrom);
                from.AllTransfers.Add(newTransferFrom);
                _memoryCacheService.UpdateOrCreateCacheAsync($"{key}_{from.TgId}", from, true, ProfileCacheTime);
            }
            if (to != null)
            {
                var newTransferTo = new Transfer
                {
                    AccountFromId = to.Account!.Id,
                    Comment = $"перевод от {from?.Account?.Id.ToString() ?? "неизвестного"}",
                    CurrencyValue = value,
                    CurrencyCode = currencyCode.ToString()
                };
                await db.AddItem(newTransferTo);
                to.AllTransfers.Add(newTransferTo);
                _memoryCacheService.UpdateOrCreateCacheAsync($"{key}_{to.TgId}", to, true, ProfileCacheTime);
            }
            return true;
        }

        private async Task<Profile?> GetProfileByWalletCode(string walletCode)
        {
            var db = GetDb();
            var account = await db.GetAsync<Account>(a => a.WalletCode == walletCode);
            if (account == null)
                return null;
            return await GetProfile(account.TgId, account.TgName);
        }

        private async Task<Profile?> GetProfile(long tgId, string? tgName)
        {
            var key = MoneyHelper.GetProfileKey(tgId);
            if (_memoryCacheService.TryGetValue(key, out Profile? result))
            {
                return result;
            }
            return await CreateProfile(tgId, tgName);
        }

        private async Task<Profile?> CreateProfile(long tgId, string? tgName)
        {
            var init = await InitProfile(tgName);
            if (init == null)
                return null;
            init.TgId = tgId;
            var fill = await FillProfile(init);
            _memoryCacheService.UpdateOrCreateCacheAsync(MoneyHelper.GetProfileKey(tgId), fill, true, ProfileCacheTime);
            return fill;
        }

        private async Task<Profile> FillProfile(Profile profile)
        {
            var db = GetDb();
            profile.Account = await db.GetOrAddAccount(profile.TgId, profile.ProfileSheet?.TgName, profile.ProfileSheet?.WalletCode);
            profile.AllTransfers = (await db.GetListAsync<Transfer>(t => t.AccountFromId == profile.Account.Id)).ToList();
            return profile;
        }

        private async Task<Profile?> InitProfile(string? tgName)
        {
            var profiles = await _memoryCacheService.GetOrCreateCacheList(_googleManager.ProfileSheet.GetAllItems, false, DataLoadGoogleSheetCacheTime);
            var profile = profiles.FirstOrDefault(p => p.TgName?.ToLower() == tgName);
            if (profile == null || string.IsNullOrEmpty(profile.WalletCode))
                return null;
            return new Profile
            {
                ProfileSheet = profile
            };
        }

        private async Task<Profile?> CheckAdmin(MoneyCommandRequest request)
        {
            var profile = await GetProfile(request.TgId, request.Username);
            if (!profile?.IsAdmin ?? false)
                return null;
            return profile;
        }
    }
}
