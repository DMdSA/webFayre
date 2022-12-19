using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebFayre.Migrations
{
    /// <inheritdoc />
    public partial class v03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IdPatrocinador",
                schema: "webfayre",
                table: "stand_patrocinador",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(45)");

            migrationBuilder.AlterColumn<int>(
                name: "PatrocinadorId",
                schema: "webfayre",
                table: "patrocinador_feira",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(45)");

            migrationBuilder.AlterColumn<string>(
                name: "telefone",
                schema: "webfayre",
                table: "patrocinador",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "descricao",
                schema: "webfayre",
                table: "patrocinador",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "id_patrocinador",
                schema: "webfayre",
                table: "patrocinador",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(45)",
                oldMaxLength: 45)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "telemovel",
                schema: "webfayre",
                table: "funcionario",
                type: "nvarchar(17)",
                maxLength: 17,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<int>(
                name: "id_funcao",
                schema: "webfayre",
                table: "funcao",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IdPatrocinador",
                schema: "webfayre",
                table: "stand_patrocinador",
                type: "nvarchar(45)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "PatrocinadorId",
                schema: "webfayre",
                table: "patrocinador_feira",
                type: "nvarchar(45)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "telefone",
                schema: "webfayre",
                table: "patrocinador",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "descricao",
                schema: "webfayre",
                table: "patrocinador",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "id_patrocinador",
                schema: "webfayre",
                table: "patrocinador",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "telemovel",
                schema: "webfayre",
                table: "funcionario",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(17)",
                oldMaxLength: 17);

            migrationBuilder.AlterColumn<int>(
                name: "id_funcao",
                schema: "webfayre",
                table: "funcao",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}
