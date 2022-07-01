using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreamingDB.Migrations
{
    public partial class PlaysFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Plays",
                table: "Artists",
                type: "INTEGER",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Plays",
                table: "Albums",
                type: "INTEGER",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Album_Id",
                table: "Albums",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Album_Name",
                table: "Albums",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Album_Id",
                table: "Albums");

            migrationBuilder.DropIndex(
                name: "IX_Album_Name",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "Plays",
                table: "Albums");

            migrationBuilder.AlterColumn<int>(
                name: "Plays",
                table: "Artists",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true,
                oldDefaultValue: 0);
        }
    }
}
