using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageSharingPlatform.Domain.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "user",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "image_category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    category_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_image_category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "image_request",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    complete_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    RequesterUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ArtistId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_image_request", x => x.Id);
                    table.ForeignKey(
                        name: "FK_image_request_user_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_image_request_user_RequesterUserId",
                        column: x => x.RequesterUserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "shared_image",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArtistId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shared_image", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shared_image_image_category_ImageCategoryId",
                        column: x => x.ImageCategoryId,
                        principalTable: "image_category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_shared_image_user_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "user",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "review",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_review", x => x.Id);
                    table.ForeignKey(
                        name: "FK_review_shared_image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "shared_image",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_review_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_image_request_ArtistId",
                table: "image_request",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_image_request_RequesterUserId",
                table: "image_request",
                column: "RequesterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_review_ImageId",
                table: "review",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_review_UserId",
                table: "review",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_shared_image_ArtistId",
                table: "shared_image",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_shared_image_ImageCategoryId",
                table: "shared_image",
                column: "ImageCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "image_request");

            migrationBuilder.DropTable(
                name: "review");

            migrationBuilder.DropTable(
                name: "shared_image");

            migrationBuilder.DropTable(
                name: "image_category");

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "user",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
