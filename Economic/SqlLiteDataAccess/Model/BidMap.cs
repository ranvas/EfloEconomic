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
    public class BidMap : IEntityTypeConfiguration<Bid>
    {
        public void Configure(EntityTypeBuilder<Bid> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("bids");

            builder.Property(x => x.Id).HasColumnName("bid_id");
            builder.Property(x => x.MineId).HasColumnName("mine_id");
            builder.Property(x => x.Value).HasColumnName("value");
            builder.HasOne(x => x.Account).WithMany(a=>a.Bids).HasForeignKey(b => b.AccountId);
        }
    }
}
