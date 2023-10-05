using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB_153501_BYCHKO.API.Migrations
{
    /// <inheritdoc />
    public partial class Added_MIMETYPE_Removed_CategotyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_airplanes_engineTypes_CategoryId",
                table: "airplanes");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "airplanes",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "MIMEType",
                table: "airplanes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_airplanes_engineTypes_CategoryId",
                table: "airplanes",
                column: "CategoryId",
                principalTable: "engineTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_airplanes_engineTypes_CategoryId",
                table: "airplanes");

            migrationBuilder.DropColumn(
                name: "MIMEType",
                table: "airplanes");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "airplanes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_airplanes_engineTypes_CategoryId",
                table: "airplanes",
                column: "CategoryId",
                principalTable: "engineTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
