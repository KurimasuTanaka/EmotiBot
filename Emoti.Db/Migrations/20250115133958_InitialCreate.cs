using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace codespaces_blank.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Emoticons",
                columns: table => new
                {
                    Emoticon = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emoticons", x => x.Emoticon);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Tag = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Tag);
                });

            migrationBuilder.CreateTable(
                name: "EmoticonModelTagModel",
                columns: table => new
                {
                    EmoticonsEmoticon = table.Column<string>(type: "TEXT", nullable: false),
                    TagsTag = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmoticonModelTagModel", x => new { x.EmoticonsEmoticon, x.TagsTag });
                    table.ForeignKey(
                        name: "FK_EmoticonModelTagModel_Emoticons_EmoticonsEmoticon",
                        column: x => x.EmoticonsEmoticon,
                        principalTable: "Emoticons",
                        principalColumn: "Emoticon",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmoticonModelTagModel_Tags_TagsTag",
                        column: x => x.TagsTag,
                        principalTable: "Tags",
                        principalColumn: "Tag",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmoticonModelTagModel_TagsTag",
                table: "EmoticonModelTagModel",
                column: "TagsTag");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmoticonModelTagModel");

            migrationBuilder.DropTable(
                name: "Emoticons");

            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
