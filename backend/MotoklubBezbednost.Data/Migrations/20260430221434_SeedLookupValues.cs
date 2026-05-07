using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoklubBezbednost.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedLookupValues : Migration
    {
        // Fixed timestamp captures when the seed was authored. The DbModelTimestampInterceptor
        // only stamps rows that flow through SaveChanges, so raw migration SQL must set it explicitly.
        private const string SeedTimestamp = "2026-04-30T00:00:00.000";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // INSERT OR IGNORE keeps this migration safe to re-run (or re-apply after partial failure).
            migrationBuilder.Sql($@"
INSERT OR IGNORE INTO Levels (Id, Name, Note, CreationTimestamp) VALUES
    (1, 'osnovni', NULL, '{SeedTimestamp}'),
    (2, 'napredni', NULL, '{SeedTimestamp}'),
    (3, 'offroad',  NULL, '{SeedTimestamp}');");

            migrationBuilder.Sql($@"
INSERT OR IGNORE INTO MemberTypes (Id, Prefix, TypeName, Color, PaidMembership, CreationTimestamp) VALUES
    (1, 1, 'Uprava',                '#FF0288D1', 1, '{SeedTimestamp}'),
    (2, 2, 'Aktiv',                 '#FF4CAF50', 1, '{SeedTimestamp}'),
    (3, 3, 'Prijatelji kluba/Deca', '#FFFBC02D', 0, '{SeedTimestamp}'),
    (4, 4, 'Počasni članovi',       '#FF9C27B0', 0, '{SeedTimestamp}');");

            migrationBuilder.Sql($@"
INSERT OR IGNORE INTO PaymentTypes (Id, Type, Description, CreationTimestamp) VALUES
    (1, 'Gotovina', NULL, '{SeedTimestamp}'),
    (2, 'Banka',    NULL, '{SeedTimestamp}');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM PaymentTypes WHERE Id IN (1, 2);");
            migrationBuilder.Sql("DELETE FROM MemberTypes WHERE Id IN (1, 2, 3, 4);");
            migrationBuilder.Sql("DELETE FROM Levels WHERE Id IN (1, 2, 3);");
        }
    }
}
