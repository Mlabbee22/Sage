using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAGE.Data.Migrations
{
    public partial class IdentityUserMerge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PremiumMember",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "allergens",
                table: "AspNetUsers",
                type: "varchar(max)",
                unicode: false,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "savedRecipes",
                table: "AspNetUsers",
                type: "varchar(max)",
                unicode: false,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Following",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FollowingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FollowingNavigationId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Following", x => new { x.UserId, x.FollowingId });
                    table.ForeignKey(
                        name: "FK_Following_AspNetUsers_FollowingNavigationId",
                        column: x => x.FollowingNavigationId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Following_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ingredient",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    department = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    allergens = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ingredient", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Recipe",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    steps = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    tags = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ingredients = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    rating = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    author = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipe", x => x.id);
                    table.ForeignKey(
                        name: "FK_Recipe_AspNetUsers_author",
                        column: x => x.author,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SAGEUserSAGEUser",
                columns: table => new
                {
                    FollowingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAGEUserSAGEUser", x => new { x.FollowingId, x.UserId });
                    table.ForeignKey(
                        name: "FK_SAGEUserSAGEUser_AspNetUsers_FollowingId",
                        column: x => x.FollowingId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SAGEUserSAGEUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "groceryList",
                columns: table => new
                {
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ingredientId = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    notes = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groceryList", x => new { x.userId, x.ingredientId });
                    table.ForeignKey(
                        name: "FK_groceryList_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_groceryList_ingredient_ingredientId",
                        column: x => x.ingredientId,
                        principalTable: "ingredient",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "review",
                columns: table => new
                {
                    userID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    recipeID = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    review = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_review", x => new { x.userID, x.recipeID });
                    table.ForeignKey(
                        name: "FK_review_AspNetUsers_userID",
                        column: x => x.userID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_review_Recipe_recipeID",
                        column: x => x.recipeID,
                        principalTable: "Recipe",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Following_FollowingNavigationId",
                table: "Following",
                column: "FollowingNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_groceryList_ingredientId",
                table: "groceryList",
                column: "ingredientId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_author",
                table: "Recipe",
                column: "author");

            migrationBuilder.CreateIndex(
                name: "IX_review_recipeID",
                table: "review",
                column: "recipeID");

            migrationBuilder.CreateIndex(
                name: "IX_SAGEUserSAGEUser_UserId",
                table: "SAGEUserSAGEUser",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Following");

            migrationBuilder.DropTable(
                name: "groceryList");

            migrationBuilder.DropTable(
                name: "review");

            migrationBuilder.DropTable(
                name: "SAGEUserSAGEUser");

            migrationBuilder.DropTable(
                name: "ingredient");

            migrationBuilder.DropTable(
                name: "Recipe");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PremiumMember",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "allergens",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "savedRecipes",
                table: "AspNetUsers");
        }
    }
}
