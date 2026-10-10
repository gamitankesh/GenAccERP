using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenAccERP.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AppUser__1788CCAC5E6604F9", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_AppUser_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AppUser",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_AppUser_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AppUser",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "ItemCategory",
                columns: table => new
                {
                    CategoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ItemCate__19093A2BC386A1FF", x => x.CategoryID);
                    table.ForeignKey(
                        name: "FK_ItemCategory_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AppUser",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_ItemCategory_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AppUser",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseMaster",
                columns: table => new
                {
                    PurchaseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    PurchaseDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    SupplierName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Purchase__6B0A6BDE4DA25346", x => x.PurchaseID);
                    table.ForeignKey(
                        name: "FK_PurchaseMaster_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AppUser",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_PurchaseMaster_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AppUser",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "SalesMaster",
                columns: table => new
                {
                    SaleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    SaleDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    CustomerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SalesMas__1EE3C41F44E42E30", x => x.SaleID);
                    table.ForeignKey(
                        name: "FK_SalesMaster_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AppUser",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_SalesMaster_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AppUser",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "ItemMaster",
                columns: table => new
                {
                    ItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    CategoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ItemDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalesPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentStock = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ItemMast__727E83EB4F7BA50B", x => x.ItemID);
                    table.ForeignKey(
                        name: "FK_ItemMaster_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AppUser",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_ItemMaster_ItemCategory",
                        column: x => x.CategoryID,
                        principalTable: "ItemCategory",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemMaster_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AppUser",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseDetail",
                columns: table => new
                {
                    PurchaseDetailID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    PurchaseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(29,2)", nullable: true, computedColumnSql: "([Quantity]*[UnitPrice])", stored: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Purchase__88C328D5F4554020", x => x.PurchaseDetailID);
                    table.ForeignKey(
                        name: "FK_PurchaseDetail_ItemMaster",
                        column: x => x.ItemID,
                        principalTable: "ItemMaster",
                        principalColumn: "ItemID");
                    table.ForeignKey(
                        name: "FK_PurchaseDetail_PurchaseMaster",
                        column: x => x.PurchaseID,
                        principalTable: "PurchaseMaster",
                        principalColumn: "PurchaseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesDetail",
                columns: table => new
                {
                    SalesDetailID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    SaleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(29,2)", nullable: true, computedColumnSql: "([Quantity]*[UnitPrice])", stored: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SalesDet__391C57922F9A8F9D", x => x.SalesDetailID);
                    table.ForeignKey(
                        name: "FK_SalesDetail_ItemMaster",
                        column: x => x.ItemID,
                        principalTable: "ItemMaster",
                        principalColumn: "ItemID");
                    table.ForeignKey(
                        name: "FK_SalesDetail_SalesMaster",
                        column: x => x.SaleID,
                        principalTable: "SalesMaster",
                        principalColumn: "SaleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_CreatedBy",
                table: "AppUser",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_ModifiedBy",
                table: "AppUser",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "UQ__AppUser__536C85E47860BA76",
                table: "AppUser",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__AppUser__A9D1053404901E15",
                table: "AppUser",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemCategory_CreatedBy",
                table: "ItemCategory",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCategory_ModifiedBy",
                table: "ItemCategory",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ItemMaster_CategoryID",
                table: "ItemMaster",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemMaster_CreatedBy",
                table: "ItemMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ItemMaster_ModifiedBy",
                table: "ItemMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "UQ__ItemMast__727D9FE581E61244",
                table: "ItemMaster",
                column: "ItemNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDetail_ItemID",
                table: "PurchaseDetail",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDetail_PurchaseID",
                table: "PurchaseDetail",
                column: "PurchaseID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseMaster_CreatedBy",
                table: "PurchaseMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseMaster_ModifiedBy",
                table: "PurchaseMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SalesDetail_ItemID",
                table: "SalesDetail",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesDetail_SaleID",
                table: "SalesDetail",
                column: "SaleID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesMaster_CreatedBy",
                table: "SalesMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SalesMaster_ModifiedBy",
                table: "SalesMaster",
                column: "ModifiedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseDetail");

            migrationBuilder.DropTable(
                name: "SalesDetail");

            migrationBuilder.DropTable(
                name: "PurchaseMaster");

            migrationBuilder.DropTable(
                name: "ItemMaster");

            migrationBuilder.DropTable(
                name: "SalesMaster");

            migrationBuilder.DropTable(
                name: "ItemCategory");

            migrationBuilder.DropTable(
                name: "AppUser");
        }
    }
}
