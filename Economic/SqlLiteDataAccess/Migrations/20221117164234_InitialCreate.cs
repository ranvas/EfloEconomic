using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqlLiteDataAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    account_id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    telegram_id = table.Column<long>(type: "INTEGER", nullable: false),
                    telegram_name = table.Column<string>(type: "TEXT", nullable: true),
                    wallet_code = table.Column<string>(type: "TEXT", nullable: true),
                    Table = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.account_id);
                });

            migrationBuilder.CreateTable(
                name: "transfers",
                columns: table => new
                {
                    transfer_id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    account_from_id = table.Column<long>(type: "INTEGER", nullable: false),
                    account_to_id = table.Column<long>(type: "INTEGER", nullable: false),
                    transfer_code = table.Column<string>(type: "TEXT", nullable: true),
                    transfer_time = table.Column<string>(type: "TEXT", nullable: true),
                    currency_value = table.Column<decimal>(type: "TEXT", nullable: false),
                    Table = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transfers", x => x.transfer_id);
                    table.ForeignKey(
                        name: "FK_transfers_accounts_account_from_id",
                        column: x => x.account_from_id,
                        principalTable: "accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transfers_accounts_account_to_id",
                        column: x => x.account_to_id,
                        principalTable: "accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_transfers_account_from_id",
                table: "transfers",
                column: "account_from_id");

            migrationBuilder.CreateIndex(
                name: "IX_transfers_account_to_id",
                table: "transfers",
                column: "account_to_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transfers");

            migrationBuilder.DropTable(
                name: "accounts");
        }
    }
}
