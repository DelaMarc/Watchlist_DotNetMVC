using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Watchlist.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigrationFilmUserAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Films",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Films", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FilmUsers",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FilmID = table.Column<int>(type: "int", nullable: false),
                    Watched = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<int>(type: "int", nullable: false),
                    AccountUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmUsers", x => new { x.UserId, x.FilmID });
                    table.ForeignKey(
                        name: "FK_FilmUsers_AspNetUsers_AccountUserId",
                        column: x => x.AccountUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmUsers_Films_FilmID",
                        column: x => x.FilmID,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilmUsers_AccountUserId",
                table: "FilmUsers",
                column: "AccountUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmUsers_FilmID",
                table: "FilmUsers",
                column: "FilmID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilmUsers");

            migrationBuilder.DropTable(
                name: "Films");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");
        }
    }
}
