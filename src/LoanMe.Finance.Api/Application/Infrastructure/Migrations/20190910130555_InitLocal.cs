using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LoanMe.Finance.Api.Application.Infrastructure.Migrations
{
    public partial class InitLocal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "catalog_brand_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    AccountNumber_Entity = table.Column<string>(maxLength: 4, nullable: true),
                    AccountNumber_Office = table.Column<string>(maxLength: 4, nullable: true),
                    AccountNumber_Control = table.Column<string>(maxLength: 2, nullable: true),
                    AccountNumber_Number = table.Column<string>(maxLength: 10, nullable: true),
                    Balance = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    AccountNumber_Entity = table.Column<string>(maxLength: 4, nullable: true),
                    AccountNumber_Office = table.Column<string>(maxLength: 4, nullable: true),
                    AccountNumber_Control = table.Column<string>(maxLength: 2, nullable: true),
                    AccountNumber_Number = table.Column<string>(maxLength: 10, nullable: true),
                    Amount = table.Column<decimal>(nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    Currency = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CreditCards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    ExpiredDate = table.Column<DateTime>(nullable: false),
                    CardType = table.Column<int>(nullable: false),
                    Limit = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditCards_Accounts_Id",
                        column: x => x.Id,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Accounts_Id",
                        column: x => x.Id,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditCards");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropSequence(
                name: "catalog_brand_hilo");
        }
    }
}
