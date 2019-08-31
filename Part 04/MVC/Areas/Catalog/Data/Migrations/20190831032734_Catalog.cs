using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC.Areas.Catalog.Data.Migrations
{
    public partial class Catalog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Fruits" },
                    { 2, "Legumes" },
                    { 3, "Vegetables" },
                    { 4, "Grains / Beans" },
                    { 5, "Dairy products" },
                    { 6, "Meat & eggs" }
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "CategoryId", "Code", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "001", "Oranges", 5.90m },
                    { 28, 6, "028", "Shrimp", 8.90m },
                    { 27, 6, "027", "Beef", 6.90m },
                    { 26, 6, "026", "Sliced ham", 7.90m },
                    { 25, 5, "025", "Cream cheese", 6.90m },
                    { 24, 5, "024", "Butter", 5.90m },
                    { 23, 5, "023", "Yogurt", 6.90m },
                    { 22, 5, "022", "Cheese", 8.90m },
                    { 21, 5, "021", "Milk", 4.90m },
                    { 20, 4, "020", "Peas", 5.90m },
                    { 19, 4, "019", "Corn", 3.90m },
                    { 18, 4, "018", "Oat", 3.90m },
                    { 17, 4, "017", "Beans", 6.90m },
                    { 16, 4, "016", "Rice", 6.90m },
                    { 15, 3, "015", "Aspargus", 8.90m },
                    { 14, 3, "014", "Onion", 5.90m },
                    { 13, 3, "013", "Cauliflower", 4.90m },
                    { 12, 3, "012", "Broccoli", 5.90m },
                    { 11, 3, "011", "Lettuce", 7.90m },
                    { 10, 2, "010", "Eggplants", 5.90m },
                    { 9, 2, "009", "Tomatoes", 4.90m },
                    { 8, 2, "008", "Potatoes", 5.90m },
                    { 7, 2, "007", "Yellow peppers", 8.90m },
                    { 6, 2, "006", "Carrots", 5.90m },
                    { 5, 1, "005", "Grapes", 5.90m },
                    { 4, 1, "004", "Strawberries", 7.90m },
                    { 3, 1, "003", "Bananas", 6.90m },
                    { 2, 1, "002", "Lemons", 5.90m },
                    { 29, 6, "029", "Barbecue", 6.90m },
                    { 30, 6, "030", "Eggs", 4.90m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
