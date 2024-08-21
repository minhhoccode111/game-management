using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameManagementMvc.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameCompany_Company_CompanyId",
                table: "GameCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_GameGenre_Genre_GenreId",
                table: "GameGenre");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameGenre",
                table: "GameGenre");

            migrationBuilder.DropIndex(
                name: "IX_GameGenre_GenreId",
                table: "GameGenre");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Genre",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Game",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Company",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameGenre",
                table: "GameGenre",
                columns: new[] { "GenreId", "GameId" });

            migrationBuilder.CreateIndex(
                name: "IX_GameGenre_GameId",
                table: "GameGenre",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameCompany_Company_CompanyId",
                table: "GameCompany",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameGenre_Genre_GenreId",
                table: "GameGenre",
                column: "GenreId",
                principalTable: "Genre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameCompany_Company_CompanyId",
                table: "GameCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_GameGenre_Genre_GenreId",
                table: "GameGenre");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameGenre",
                table: "GameGenre");

            migrationBuilder.DropIndex(
                name: "IX_GameGenre_GameId",
                table: "GameGenre");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Genre");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Company");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameGenre",
                table: "GameGenre",
                columns: new[] { "GameId", "GenreId" });

            migrationBuilder.CreateIndex(
                name: "IX_GameGenre_GenreId",
                table: "GameGenre",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameCompany_Company_CompanyId",
                table: "GameCompany",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameGenre_Genre_GenreId",
                table: "GameGenre",
                column: "GenreId",
                principalTable: "Genre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
