using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MoneyEntity.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlLiteDataAccess.Model
{
    internal class TransferMap : IEntityTypeConfiguration<Transfer>
    {
        public void Configure(EntityTypeBuilder<Transfer> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("transfers");

            builder.Property(x => x.Id).HasColumnName("transfer_id");
            builder.Property(x => x.CurrencyValue).HasColumnName("currency_value");
            builder.Property(x => x.TransferTime).HasColumnName("transfer_time");
            builder.Property(x => x.CurrencyCode).HasColumnName("transfer_code");
            builder.Property(x=>x.AccountFromId).HasColumnName("account_from_id");
            builder.Property(x => x.AccountToId).HasColumnName("account_to_id");
        }
    }
}
