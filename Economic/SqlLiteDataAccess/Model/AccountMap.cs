using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyEntity.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlLiteDataAccess.Model
{
    internal class AccountMap : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("accounts");

            builder.Property(x => x.Id).HasColumnName("account_id");
            builder.Property(x => x.WalletCode).HasColumnName("wallet_code");
            builder.Property(x => x.TgName).HasColumnName("telegram_name");
            builder.Property(x => x.TgId).HasColumnName("telegram_id");
            builder.HasMany(x => x.TransfersFrom).WithOne(t => t.AccountFrom).HasForeignKey(t => t.AccountFromId);
        }
    }
}
