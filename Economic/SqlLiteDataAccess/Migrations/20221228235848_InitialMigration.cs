using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqlLiteDataAccess.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transfers_accounts_account_from_id",
                table: "transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_transfers_accounts_account_to_id",
                table: "transfers");

            migrationBuilder.DropIndex(
                name: "IX_transfers_account_to_id",
                table: "transfers");

            migrationBuilder.DropColumn(
                name: "account_to_id",
                table: "transfers");

            migrationBuilder.AlterColumn<long>(
                name: "account_from_id",
                table: "transfers",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<long>(
                name: "AccountId",
                table: "transfers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "comment",
                table: "transfers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "bids",
                columns: table => new
                {
                    bid_id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    value = table.Column<int>(type: "INTEGER", nullable: false),
                    AccountId = table.Column<long>(type: "INTEGER", nullable: false),
                    mine_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Table = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bids", x => x.bid_id);
                    table.ForeignKey(
                        name: "FK_bids_accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_transfers_AccountId",
                table: "transfers",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_bids_AccountId",
                table: "bids",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_transfers_accounts_account_from_id",
                table: "transfers",
                column: "account_from_id",
                principalTable: "accounts",
                principalColumn: "account_id");

            migrationBuilder.AddForeignKey(
                name: "FK_transfers_accounts_AccountId",
                table: "transfers",
                column: "AccountId",
                principalTable: "accounts",
                principalColumn: "account_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transfers_accounts_account_from_id",
                table: "transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_transfers_accounts_AccountId",
                table: "transfers");

            migrationBuilder.DropTable(
                name: "bids");

            migrationBuilder.DropIndex(
                name: "IX_transfers_AccountId",
                table: "transfers");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "transfers");

            migrationBuilder.DropColumn(
                name: "comment",
                table: "transfers");

            migrationBuilder.AlterColumn<long>(
                name: "account_from_id",
                table: "transfers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "account_to_id",
                table: "transfers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_transfers_account_to_id",
                table: "transfers",
                column: "account_to_id");

            migrationBuilder.AddForeignKey(
                name: "FK_transfers_accounts_account_from_id",
                table: "transfers",
                column: "account_from_id",
                principalTable: "accounts",
                principalColumn: "account_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transfers_accounts_account_to_id",
                table: "transfers",
                column: "account_to_id",
                principalTable: "accounts",
                principalColumn: "account_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
