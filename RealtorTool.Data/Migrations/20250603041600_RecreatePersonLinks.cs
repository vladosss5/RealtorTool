using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealtorTool.Data.Migrations
{
    /// <inheritdoc />
    public partial class RecreatePersonLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_AuthorizationData_Id",
                table: "Persons");

            migrationBuilder.DropForeignKey(
                name: "FK_Persons_RealtorDetails_Id",
                table: "Persons");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizationData_Persons_Id",
                table: "AuthorizationData",
                column: "Id",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RealtorDetails_Persons_Id",
                table: "RealtorDetails",
                column: "Id",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorizationData_Persons_Id",
                table: "AuthorizationData");

            migrationBuilder.DropForeignKey(
                name: "FK_RealtorDetails_Persons_Id",
                table: "RealtorDetails");

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
    }
}
