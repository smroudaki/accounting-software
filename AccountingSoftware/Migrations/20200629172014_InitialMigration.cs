using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountingSoftware.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 128, nullable: false),
                    LastName = table.Column<string>(maxLength: 128, nullable: false),
                    NationalCode = table.Column<string>(maxLength: 128, nullable: true),
                    Address = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedDate = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    IsDelete = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodeGroup",
                columns: table => new
                {
                    CodeGroupGuid = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    Key = table.Column<string>(maxLength: 128, nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    IsDelete = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeGroup", x => x.CodeGroupGuid);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountGuid = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    UserGuid = table.Column<string>(nullable: false),
                    BankName = table.Column<string>(maxLength: 128, nullable: true),
                    AccountName = table.Column<string>(maxLength: 128, nullable: true),
                    AccountNumber = table.Column<string>(maxLength: 128, nullable: false),
                    CardNumber = table.Column<string>(maxLength: 128, nullable: true),
                    Credit = table.Column<long>(nullable: false, defaultValueSql: "((0))"),
                    CreationDate = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedDate = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    IsDelete = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountGuid);
                    table.ForeignKey(
                        name: "FK_Account_User",
                        column: x => x.UserGuid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Code",
                columns: table => new
                {
                    CodeGuid = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    CodeGroupGuid = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(maxLength: 128, nullable: false),
                    DisplayValue = table.Column<string>(maxLength: 128, nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    IsDelete = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Code", x => x.CodeGuid);
                    table.ForeignKey(
                        name: "FK_Code_CodeGroup",
                        column: x => x.CodeGroupGuid,
                        principalTable: "CodeGroup",
                        principalColumn: "CodeGroupGuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    TransactionGuid = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    AccountGuid = table.Column<Guid>(nullable: true),
                    TypeCodeGuid = table.Column<Guid>(nullable: false),
                    StateCodeGuid = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 512, nullable: true),
                    Cost = table.Column<long>(nullable: false),
                    Credit = table.Column<long>(nullable: false, defaultValueSql: "((0))"),
                    AccountSide = table.Column<string>(maxLength: 128, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ReceiptDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    IsCheckTransaction = table.Column<bool>(nullable: false, defaultValueSql: "((0))"),
                    IsDelete = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionGuid);
                    table.ForeignKey(
                        name: "FK_Transaction_Account",
                        column: x => x.AccountGuid,
                        principalTable: "Account",
                        principalColumn: "AccountGuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StateTransaction_StateCode",
                        column: x => x.StateCodeGuid,
                        principalTable: "Code",
                        principalColumn: "CodeGuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TypeTransaction_TypeCode",
                        column: x => x.TypeCodeGuid,
                        principalTable: "Code",
                        principalColumn: "CodeGuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CheckTransaction",
                columns: table => new
                {
                    CheckTransactionGuid = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    TransactionGuid = table.Column<Guid>(nullable: false),
                    Serial = table.Column<string>(maxLength: 128, nullable: true),
                    IssueDate = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckTransaction", x => x.CheckTransactionGuid);
                    table.ForeignKey(
                        name: "FK_CheckTransaction_Transaction",
                        column: x => x.TransactionGuid,
                        principalTable: "Transaction",
                        principalColumn: "TransactionGuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NationalCode", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "a0c060fb-eb1f-4132-a14f-afc2ecccd728", 0, null, "7201a16a-333c-474d-950b-5f8fb0ac92e2", null, false, "علی", "تهرانچی", true, null, null, null, "1", "AQAAAAEAACcQAAAAEGduCXP6wkO8iRG5duhbeZ1thUWVoNsyiCZoNXuOc7HpfUDfAtB0BxVUMWColWDdxw==", null, false, "52NWE56246TSEHNP6BTSLIYRCO24AXPH", false, "1" });

            migrationBuilder.InsertData(
                table: "CodeGroup",
                columns: new[] { "CodeGroupGuid", "Key" },
                values: new object[] { new Guid("b7a903f3-2c65-49ef-997b-03966578a4c0"), "TransactionState" });

            migrationBuilder.InsertData(
                table: "CodeGroup",
                columns: new[] { "CodeGroupGuid", "Key" },
                values: new object[] { new Guid("8d9375da-33d4-41f0-b589-d91c96af8eb5"), "TransactionType" });

            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "AccountGuid", "AccountName", "AccountNumber", "BankName", "CardNumber", "UserGuid" },
                values: new object[] { new Guid("a8a1632c-5a7f-4bd7-a9ac-d10a6375ade0"), null, "1", null, null, "a0c060fb-eb1f-4132-a14f-afc2ecccd728" });

            migrationBuilder.InsertData(
                table: "Code",
                columns: new[] { "CodeGuid", "CodeGroupGuid", "DisplayValue", "Value" },
                values: new object[,]
                {
                    { new Guid("b508bd08-5534-4d26-9ed4-c36575c8d90a"), new Guid("b7a903f3-2c65-49ef-997b-03966578a4c0"), "وصول شده", "Passed" },
                    { new Guid("3d905312-ae57-498c-a733-f5cbaf114940"), new Guid("b7a903f3-2c65-49ef-997b-03966578a4c0"), "وصول نشده", "NotPassed" },
                    { new Guid("fe92b8f8-f206-4273-8ca8-f1ecf70a8197"), new Guid("b7a903f3-2c65-49ef-997b-03966578a4c0"), "در انتظار وصول", "Waiting" },
                    { new Guid("d1605abf-8a46-4f2e-8e22-194298b9fd33"), new Guid("8d9375da-33d4-41f0-b589-d91c96af8eb5"), "دریافتی", "Creditor" },
                    { new Guid("749d242b-55f7-4361-bf1b-42c139411c49"), new Guid("8d9375da-33d4-41f0-b589-d91c96af8eb5"), "پرداختی", "Debtor" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_UserGuid",
                table: "Account",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CheckTransaction_TransactionGuid",
                table: "CheckTransaction",
                column: "TransactionGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Code_CodeGroupGuid",
                table: "Code",
                column: "CodeGroupGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_AccountGuid",
                table: "Transaction",
                column: "AccountGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_StateCodeGuid",
                table: "Transaction",
                column: "StateCodeGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_TypeCodeGuid",
                table: "Transaction",
                column: "TypeCodeGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CheckTransaction");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Code");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CodeGroup");
        }
    }
}
