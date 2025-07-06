using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TaskManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "TM");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "TM",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                },
                comment: "Таблиця користувачiв");

            migrationBuilder.CreateTable(
                name: "TaskList",
                schema: "TM",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DateTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WhoCreated = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskList", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TaskList_Users_WhoCreated",
                        column: x => x.WhoCreated,
                        principalSchema: "TM",
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Таблиця задач");

            migrationBuilder.CreateTable(
                name: "TaskListUsers",
                schema: "TM",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ID_Task = table.Column<int>(type: "integer", nullable: false),
                    ID_User = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskListUsers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TaskListUser_Task",
                        column: x => x.ID_Task,
                        principalSchema: "TM",
                        principalTable: "TaskList",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskListUser_User",
                        column: x => x.ID_User,
                        principalSchema: "TM",
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Таблиця зв'язку користувачiв та задач");

            migrationBuilder.CreateIndex(
                name: "IX_TaskList_WhoCreated",
                schema: "TM",
                table: "TaskList",
                column: "WhoCreated");

            migrationBuilder.CreateIndex(
                name: "IX_TaskListUsers_ID_Task",
                schema: "TM",
                table: "TaskListUsers",
                column: "ID_Task");

            migrationBuilder.CreateIndex(
                name: "IX_TaskListUsers_ID_User",
                schema: "TM",
                table: "TaskListUsers",
                column: "ID_User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskListUsers",
                schema: "TM");

            migrationBuilder.DropTable(
                name: "TaskList",
                schema: "TM");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "TM");
        }
    }
}
