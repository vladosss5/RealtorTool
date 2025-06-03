using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealtorTool.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRealtorDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorizationData_Persons_Id",
                table: "AuthorizationData");

            migrationBuilder.AddColumn<string>(
                name: "EMail",
                table: "Persons",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Persons",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RealtorDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    WorkExperience = table.Column<string>(type: "text", nullable: true),
                    Education = table.Column<string>(type: "text", nullable: true),
                    Qualification = table.Column<string>(type: "text", nullable: true),
                    Services = table.Column<string>(type: "text", nullable: true),
                    AboutMe = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealtorDetails", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_AuthorizationData_Id",
                table: "Persons",
                column: "Id",
                principalTable: "AuthorizationData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_RealtorDetails_Id",
                table: "Persons",
                column: "Id",
                principalTable: "RealtorDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_AuthorizationData_Id",
                table: "Persons");

            migrationBuilder.DropForeignKey(
                name: "FK_Persons_RealtorDetails_Id",
                table: "Persons");

            migrationBuilder.DropTable(
                name: "RealtorDetails");

            migrationBuilder.DropColumn(
                name: "EMail",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Persons");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizationData_Persons_Id",
                table: "AuthorizationData",
                column: "Id",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
