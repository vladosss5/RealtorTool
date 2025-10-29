using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealtorTool.Data.Context.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePotentialMatchesView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sqlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Context", "Migrations", "Scripts", "CreatePotentialMatchesView.sql");
            var sqlScript = File.ReadAllText(sqlPath);
            migrationBuilder.Sql(sqlScript);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sqlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Context", "Migrations", "Scripts", "DropPotentialMatchesView.sql");
            var sqlScript = File.ReadAllText(sqlPath);
            migrationBuilder.Sql(sqlScript);
        }
    }
}
