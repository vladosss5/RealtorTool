using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealtorTool.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTagsForClients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePhotoPath",
                table: "Persons",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClientTags",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ClientId = table.Column<string>(type: "text", nullable: false),
                    clientTag_client = table.Column<string>(type: "text", nullable: true),
                    TagId = table.Column<string>(type: "text", nullable: false),
                    clientTag_tag = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientTags_DictionaryValues_clientTag_tag",
                        column: x => x.clientTag_tag,
                        principalTable: "DictionaryValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientTags_Persons_clientTag_client",
                        column: x => x.clientTag_client,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientTags_clientTag_client",
                table: "ClientTags",
                column: "clientTag_client");

            migrationBuilder.CreateIndex(
                name: "IX_ClientTags_clientTag_tag",
                table: "ClientTags",
                column: "clientTag_tag");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientTags");

            migrationBuilder.DropColumn(
                name: "ProfilePhotoPath",
                table: "Persons");
        }
    }
}
