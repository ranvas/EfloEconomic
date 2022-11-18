using DataAccess.Abstractions;
using DataAccess.EFCore;
using DataAccess.EFCore.Extensions;
using DataAccess.SqlLite;
using Microsoft.EntityFrameworkCore;
using MoneyEntity.Dto;
using SqlLiteDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SqlLiteDataAccess
{
    public class MyDataService : SqlLiteDataManager<MyContext>
    {
        public MyDataService(IDataAccessConfiguration configuration)
            : base(configuration)
        {
        }

        protected override Func<DbDataContext> DataContextCreator
            => () => new MyContext(this.Configuration.ConnectionString);

        public async Task<Account> GetOrAddAccount(long tgId, string? tgName, string? walletCode)
        {
            var account = await GetAsync<Account>(a => a.TgId == tgId && a.TgName == tgName && a.WalletCode == walletCode);
            if (account == null)
            {
                account = new Account
                {
                    TgId = tgId,
                    WalletCode = walletCode,
                    TgName = tgName
                };
                await AddItem(account);
            }
            return account;
        }

        public async Task AddItem<T>(T item) where T : class
        {
            using var db = CreateDataContext();
            await db.AddAsync(item);
            await db.SaveChangesAsync();
        }
    }
}
