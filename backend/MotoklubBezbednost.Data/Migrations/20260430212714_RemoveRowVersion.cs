using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoklubBezbednost.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRowVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "TrainingSessions");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "PaymentTypes");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Motorcycles");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "MemberTypes");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "MembershipPayments");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Comments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "TrainingSessions",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Trainings",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Tags",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "PaymentTypes",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Motorcycles",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "MemberTypes",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "MembershipPayments",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Members",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Levels",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Equipment",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Comments",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
