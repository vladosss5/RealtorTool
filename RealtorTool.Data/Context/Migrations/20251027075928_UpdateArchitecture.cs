using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealtorTool.Data.Context.Migrations
{
    /// <inheritdoc />
    public partial class UpdateArchitecture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientRequests_Realties_RealtyId",
                table: "ClientRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_DealParticipant_ClientRequests_ClientRequestId",
                table: "DealParticipant");

            migrationBuilder.DropForeignKey(
                name: "FK_DealParticipant_Deals_DealId",
                table: "DealParticipant");

            migrationBuilder.DropIndex(
                name: "IX_ClientRequests_RealtyId",
                table: "ClientRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DealParticipant",
                table: "DealParticipant");

            migrationBuilder.DropColumn(
                name: "RealtyId",
                table: "Realties");

            migrationBuilder.DropColumn(
                name: "RealtyId",
                table: "ClientRequests");

            migrationBuilder.RenameTable(
                name: "DealParticipant",
                newName: "DealParticipants");

            migrationBuilder.RenameIndex(
                name: "IX_DealParticipant_Role",
                table: "DealParticipants",
                newName: "IX_DealParticipants_Role");

            migrationBuilder.RenameIndex(
                name: "IX_DealParticipant_DealId_ClientRequestId",
                table: "DealParticipants",
                newName: "IX_DealParticipants_DealId_ClientRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_DealParticipant_ClientRequestId",
                table: "DealParticipants",
                newName: "IX_DealParticipants_ClientRequestId");

            migrationBuilder.AlterColumn<string>(
                name: "HeatingTypeId",
                table: "PrivateHouses",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ConstructionMaterialId",
                table: "PrivateHouses",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DealTypeId",
                table: "Deals",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "RenovationTypeId",
                table: "Apartments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "BathroomTypeId",
                table: "Apartments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DealParticipants",
                table: "DealParticipants",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DealParticipants_ClientRequests_ClientRequestId",
                table: "DealParticipants",
                column: "ClientRequestId",
                principalTable: "ClientRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DealParticipants_Deals_DealId",
                table: "DealParticipants",
                column: "DealId",
                principalTable: "Deals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealParticipants_ClientRequests_ClientRequestId",
                table: "DealParticipants");

            migrationBuilder.DropForeignKey(
                name: "FK_DealParticipants_Deals_DealId",
                table: "DealParticipants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DealParticipants",
                table: "DealParticipants");

            migrationBuilder.RenameTable(
                name: "DealParticipants",
                newName: "DealParticipant");

            migrationBuilder.RenameIndex(
                name: "IX_DealParticipants_Role",
                table: "DealParticipant",
                newName: "IX_DealParticipant_Role");

            migrationBuilder.RenameIndex(
                name: "IX_DealParticipants_DealId_ClientRequestId",
                table: "DealParticipant",
                newName: "IX_DealParticipant_DealId_ClientRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_DealParticipants_ClientRequestId",
                table: "DealParticipant",
                newName: "IX_DealParticipant_ClientRequestId");

            migrationBuilder.AddColumn<string>(
                name: "RealtyId",
                table: "Realties",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HeatingTypeId",
                table: "PrivateHouses",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConstructionMaterialId",
                table: "PrivateHouses",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DealTypeId",
                table: "Deals",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RealtyId",
                table: "ClientRequests",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RenovationTypeId",
                table: "Apartments",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BathroomTypeId",
                table: "Apartments",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DealParticipant",
                table: "DealParticipant",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRequests_RealtyId",
                table: "ClientRequests",
                column: "RealtyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientRequests_Realties_RealtyId",
                table: "ClientRequests",
                column: "RealtyId",
                principalTable: "Realties",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DealParticipant_ClientRequests_ClientRequestId",
                table: "DealParticipant",
                column: "ClientRequestId",
                principalTable: "ClientRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DealParticipant_Deals_DealId",
                table: "DealParticipant",
                column: "DealId",
                principalTable: "Deals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
