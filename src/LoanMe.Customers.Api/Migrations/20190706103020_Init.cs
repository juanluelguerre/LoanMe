using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LoanMe.Catalog.Api.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 100, nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomersAccount",
                columns: table => new
                {
                    CustomerId = table.Column<int>(nullable: false),
                    BankAccount = table.Column<string>(maxLength: 50, nullable: false),
                    MarkAsDefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomersAccount", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_CustomersAccount_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Active", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, true, "Juan Luis", "Guerrero Minero" },
                    { 2, true, "Francisco", "Ruiz Vázquez" },
                    { 3, true, "Eva", "Perez Moreno" },
                    { 4, true, "Maria", "Serrano Sanchez" }
                });

            migrationBuilder.InsertData(
                table: "CustomersAccount",
                columns: new[] { "CustomerId", "BankAccount", "MarkAsDefault" },
                values: new object[] { 1, "ES12-1234-1234-1234567890", true });

            migrationBuilder.InsertData(
                table: "CustomersAccount",
                columns: new[] { "CustomerId", "BankAccount", "MarkAsDefault" },
                values: new object[] { 2, "ES13-4321-4321-0987654321", true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomersAccount");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
