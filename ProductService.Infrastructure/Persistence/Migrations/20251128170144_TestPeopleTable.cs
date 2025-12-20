using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TestPeopleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                table: "Countries");

            migrationBuilder.EnsureSchema(
                name: "userstore");

            migrationBuilder.RenameTable(
                name: "Countries",
                newName: "People",
                newSchema: "userstore");

            migrationBuilder.AddPrimaryKey(
                name: "PK_People",
                schema: "userstore",
                table: "People",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_People",
                schema: "userstore",
                table: "People");

            migrationBuilder.RenameTable(
                name: "People",
                schema: "userstore",
                newName: "Countries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                table: "Countries",
                column: "Id");
        }
    }
}
