using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitalMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    passwordhash = table.Column<string>(type: "text", nullable: false),
                    refreshtoken = table.Column<string>(type: "text", nullable: true),
                    refreshtokenexpirytime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<Guid>(type: "uuid", nullable: false),
                    help = table.Column<int>(type: "integer", nullable: false),
                    seconds = table.Column<decimal>(type: "numeric(8,3)", precision: 8, scale: 3, nullable: false),
                    difficulty = table.Column<string>(type: "text", nullable: false),
                    rows = table.Column<int>(type: "integer", nullable: false),
                    cols = table.Column<int>(type: "integer", nullable: false),
                    mines = table.Column<int>(type: "integer", nullable: false),
                    n3bv = table.Column<int>(type: "integer", nullable: false),
                    clicks = table.Column<int>(type: "integer", nullable: false),
                    efficiency = table.Column<int>(type: "integer", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_games", x => x.id);
                    table.ForeignKey(
                        name: "FK_games_users_userid",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_games_userid",
                table: "games",
                column: "userid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "games");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
