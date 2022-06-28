using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAGE.Data.Migrations
{
    public partial class SavedRecipes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "savedRecipes",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "SavedRecipe",
                columns: table => new
                {
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    recipeId = table.Column<int>(type: "int", nullable: false),
                    isFavorite = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedRecipe", x => new { x.userId, x.recipeId });
                    table.ForeignKey(
                        name: "FK_SavedRecipe_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedRecipe_Recipe_recipeId",
                        column: x => x.recipeId,
                        principalTable: "Recipe",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SavedRecipe_recipeId",
                table: "SavedRecipe",
                column: "recipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavedRecipe");

            
            migrationBuilder.AddColumn<string>(
                name: "savedRecipes",
                table: "AspNetUsers",
                type: "varchar(max)",
                unicode: false,
                nullable: true);
        }
    }
}
