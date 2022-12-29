using DataAccess.SqlLite;
using Microsoft.EntityFrameworkCore;
using SqlLiteDataAccess.Model;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SqlLiteDataAccess
{
    public class MyContext : SqlLiteDataContext
    {
        public MyContext() : base("Data Source=Money.db;", opt =>
        {
            opt.CommandTimeout(300);  // 5 minutes
        })
        {
        }

        public MyContext(string connectionString) : base(connectionString, opt =>
        {
            opt.CommandTimeout(300);  // 5 minutes
        })
        {
        }

        public MyContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountMap());
            modelBuilder.ApplyConfiguration(new TransferMap());
            modelBuilder.ApplyConfiguration(new BidMap());
        }
    }
}