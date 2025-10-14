using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealtorTool.Data.Context.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotosTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoId",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoId",
                table: "Clients",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "HasLoggia",
                table: "Apartments",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "HasBalcony",
                table: "Apartments",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    EntityType = table.Column<int>(type: "integer", nullable: false),
                    EntityId = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsMain = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FileData = table.Column<byte[]>(type: "bytea", maxLength: 20971520, nullable: false),
                    RealtyId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Realties_RealtyId",
                        column: x => x.RealtyId,
                        principalTable: "Realties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PhotoId",
                table: "Employees",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_PhotoId",
                table: "Clients",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_CreatedDate",
                table: "Photos",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_EntityId",
                table: "Photos",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_EntityType",
                table: "Photos",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_IsMain",
                table: "Photos",
                column: "IsMain");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_RealtyId",
                table: "Photos",
                column: "RealtyId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_SortOrder",
                table: "Photos",
                column: "SortOrder");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Photos_PhotoId",
                table: "Clients",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Photos_PhotoId",
                table: "Employees",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Photos_PhotoId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Photos_PhotoId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PhotoId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Clients_PhotoId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Clients");

            migrationBuilder.AlterColumn<bool>(
                name: "HasLoggia",
                table: "Apartments",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "HasBalcony",
                table: "Apartments",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");
        }
    }
}
