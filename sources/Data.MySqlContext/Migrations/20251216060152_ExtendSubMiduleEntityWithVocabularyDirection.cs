using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.MySqlContext.Migrations
{
    /// <inheritdoc />
    public partial class ExtendSubMiduleEntityWithVocabularyDirection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VocabularyDirection",
                table: "SubModuleTable",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VocabularyDirection",
                table: "SubModuleTable");
        }
    }
}
