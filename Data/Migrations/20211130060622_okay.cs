using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAGE.Data.Migrations
{
    public partial class okay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                   name: "account",
                   columns: table => new
                   {
                       Id = table.Column<int>(type: "int", nullable: false)
                           .Annotation("SqlServer:Identity", "1, 1"),
                       allergens = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                       Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                       FirstName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                       LastName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                       PremiumMember = table.Column<bool>(type: "bit", nullable: false),
                       savedRecipes = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                   },
                   constraints: table =>
                   {
                       table.PrimaryKey("PK_account", x => x.Id);
                   });

            migrationBuilder.CreateTable(
                name: "ingredient",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    allergens = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    department = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ingredient", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "following",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false),
                    followingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__followin__FD46854414F01331", x => new { x.userId, x.followingId });
                    table.ForeignKey(
                        name: "FK__following__follo__05D8E0BE",
                        column: x => x.followingId,
                        principalTable: "account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__following__userI__04E4BC85",
                        column: x => x.userId,
                        principalTable: "account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Recipe",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    author = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ingredients = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    rating = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    steps = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    tags = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipe", x => x.id);
                    table.ForeignKey(
                        name: "FK__Recipe__author__6FE99F9F",
                        column: x => x.author,
                        principalTable: "account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "groceryList",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false),
                    ingredientId = table.Column<int>(type: "int", nullable: false),
                    notes = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__groceryL__A9EF26ADCF29B53B", x => new { x.userId, x.ingredientId });
                    table.ForeignKey(
                        name: "FK__groceryLi__ingre__7E37BEF6",
                        column: x => x.ingredientId,
                        principalTable: "ingredient",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__groceryLi__userI__7D439ABD",
                        column: x => x.userId,
                        principalTable: "account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "review",
                columns: table => new
                {
                    userID = table.Column<int>(type: "int", nullable: false),
                    recipeID = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    review = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__review__E78B52395B118F05", x => new { x.userID, x.recipeID });
                    table.ForeignKey(
                        name: "FK__review__recipeID__7A672E12",
                        column: x => x.recipeID,
                        principalTable: "Recipe",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__review__userID__797309D9",
                        column: x => x.userID,
                        principalTable: "account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_following_followingId",
                table: "following",
                column: "followingId");

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
        
    }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    allergens = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    FirstName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    LastName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    PremiumMember = table.Column<bool>(type: "bit", nullable: false),
                    savedRecipes = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ingredient",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    allergens = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    department = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ingredient", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "following",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false),
                    followingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__followin__FD46854414F01331", x => new { x.userId, x.followingId });
                    table.ForeignKey(
                        name: "FK__following__follo__05D8E0BE",
                        column: x => x.followingId,
                        principalTable: "account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__following__userI__04E4BC85",
                        column: x => x.userId,
                        principalTable: "account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Recipe",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    author = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ingredients = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    rating = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    steps = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    tags = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipe", x => x.id);
                    table.ForeignKey(
                        name: "FK__Recipe__author__6FE99F9F",
                        column: x => x.author,
                        principalTable: "account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "groceryList",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false),
                    ingredientId = table.Column<int>(type: "int", nullable: false),
                    notes = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__groceryL__A9EF26ADCF29B53B", x => new { x.userId, x.ingredientId });
                    table.ForeignKey(
                        name: "FK__groceryLi__ingre__7E37BEF6",
                        column: x => x.ingredientId,
                        principalTable: "ingredient",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__groceryLi__userI__7D439ABD",
                        column: x => x.userId,
                        principalTable: "account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "review",
                columns: table => new
                {
                    userID = table.Column<int>(type: "int", nullable: false),
                    recipeID = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    review = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__review__E78B52395B118F05", x => new { x.userID, x.recipeID });
                    table.ForeignKey(
                        name: "FK__review__recipeID__7A672E12",
                        column: x => x.recipeID,
                        principalTable: "Recipe",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__review__userID__797309D9",
                        column: x => x.userID,
                        principalTable: "account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_following_followingId",
                table: "following",
                column: "followingId");

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
        }
    }
}
