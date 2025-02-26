using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solution.Database.Migrations
{
    /// <inheritdoc />
    public partial class cools : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var query = @$"
        insert into
            [Cool]
            ([Name])
        values
            ('Water'),
            ('Air'),
            ('Oil')
    ";

            migrationBuilder.Sql(query);
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var query = $"truncate table [Cool]";

            migrationBuilder.Sql(query);
        }
    }
}
