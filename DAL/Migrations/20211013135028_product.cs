using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "adressDelivery",
                table: "AspNetUsers",
                newName: "AdressDelivery");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Platform = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalRating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "78aac644-d8c2-41c7-81cb-ff94d5ec2dd3", "Admin", "Administrator" },
                    { 2, "54280541-b409-42e9-84a5-4af164baab86", "User", "User" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "DateCreated", "Name", "Platform", "TotalRating" },
                values: new object[,]
                {
                    { 1, "03.09.2020", "Ultrakill", 0, 100 },
                    { 2, "24.04.2015", "Bloodborne", 2, 84 },
                    { 3, "18.11.2011", "Minecraft", 1, 95 },
                    { 4, "29.11.2006", "Garrys Mod", 0, 98 },
                    { 5, "20.03.2020", "Animal Crossing", 3, 75 },
                    { 6, "19.11.1998", "HalfLife", 0, 80 },
                    { 7, "22.03.2019", "Sekiro: Shadows Die Twice", 2, 76 },
                    { 8, "25.09.2015", "Until Dawn", 2, 75 },
                    { 9, "18.04.2011", "Portal 2", 0, 99 },
                    { 10, "11.11.2011", "Skyrim", 0, 90 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_DateCreated",
                table: "Products",
                column: "DateCreated");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Platform",
                table: "Products",
                column: "Platform");

            migrationBuilder.CreateIndex(
                name: "IX_Products_TotalRating",
                table: "Products",
                column: "TotalRating");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.RenameColumn(
                name: "AdressDelivery",
                table: "AspNetUsers",
                newName: "adressDelivery");
        }
    }
}
