using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrsWebApi2.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(maxLength: 20, nullable: false),
                    Password = table.Column<string>(maxLength: 20, nullable: false),
                    FirstName = table.Column<string>(maxLength: 20, nullable: false),
                    LastName = table.Column<string>(maxLength: 20, nullable: false),
                    Phone = table.Column<string>(maxLength: 12, nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    Reviewer = table.Column<bool>(nullable: false),
                    Admin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(maxLength: 10, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Address = table.Column<string>(maxLength: 255, nullable: false),
                    City = table.Column<string>(maxLength: 50, nullable: false),
                    State = table.Column<string>(maxLength: 2, nullable: false),
                    Zip = table.Column<int>(nullable: false),
                    Phone = table.Column<string>(maxLength: 12, nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: false),
                    Justification = table.Column<string>(maxLength: 255, nullable: false),
                    DateNeeded = table.Column<DateTime>(nullable: false),
                    DeliveryMode = table.Column<string>(maxLength: 25, nullable: false),
                    Status = table.Column<string>(maxLength: 20, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    SubmittedDate = table.Column<DateTime>(nullable: false),
                    ReasonForRejection = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorId = table.Column<int>(nullable: false),
                    PartNumber = table.Column<string>(maxLength: 30, nullable: false),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    Unit = table.Column<string>(maxLength: 100, nullable: true),
                    PhotoPath = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LineItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LineItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LineItems_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LineItems_ProductId",
                table: "LineItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_LineItems_RequestId_ProductId",
                table: "LineItems",
                columns: new[] { "RequestId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_VendorId",
                table: "Products",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_PartNumber_VendorId",
                table: "Products",
                columns: new[] { "PartNumber", "VendorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_UserId",
                table: "Requests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_Code",
                table: "Vendors",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LineItems");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
