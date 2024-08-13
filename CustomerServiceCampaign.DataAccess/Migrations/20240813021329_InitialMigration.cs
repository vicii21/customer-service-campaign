using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerServiceCampaign.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "city",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    city_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_city", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "color",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    color_name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_color", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "log_entry",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    actor_id = table.Column<int>(type: "int", nullable: false),
                    actor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    use_case_name = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    use_case_data = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_entry", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "service",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    service_name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "state",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    state_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_state", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "address",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    zip = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    city_id = table.Column<int>(type: "int", nullable: false),
                    state_id = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_address", x => x.id);
                    table.ForeignKey(
                        name: "FK_address_city_city_id",
                        column: x => x.city_id,
                        principalTable: "city",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_address_state_state_id",
                        column: x => x.state_id,
                        principalTable: "state",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "person",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    social_security_number = table.Column<string>(type: "char(11)", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    age = table.Column<int>(type: "int", nullable: false),
                    spouse_id = table.Column<int>(type: "int", nullable: true),
                    house_address_id = table.Column<int>(type: "int", nullable: false),
                    office_address_id = table.Column<int>(type: "int", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person", x => x.id);
                    table.ForeignKey(
                        name: "FK_person_address_house_address_id",
                        column: x => x.house_address_id,
                        principalTable: "address",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_person_address_office_address_id",
                        column: x => x.office_address_id,
                        principalTable: "address",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_person_person_spouse_id",
                        column: x => x.spouse_id,
                        principalTable: "person",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "agent",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    salary = table.Column<long>(type: "bigint", nullable: true),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    person_id = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agent", x => x.id);
                    table.ForeignKey(
                        name: "FK_agent_person_person_id",
                        column: x => x.person_id,
                        principalTable: "person",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "credentials",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    person_id = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credentials", x => x.id);
                    table.ForeignKey(
                        name: "FK_credentials_person_person_id",
                        column: x => x.person_id,
                        principalTable: "person",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_discount_id = table.Column<int>(type: "int", nullable: false),
                    person_id = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer", x => x.id);
                    table.ForeignKey(
                        name: "FK_customer_person_person_id",
                        column: x => x.person_id,
                        principalTable: "person",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "person_color",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    ColorId = table.Column<int>(type: "int", nullable: false),
                    ID = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person_color", x => new { x.PersonId, x.ColorId });
                    table.ForeignKey(
                        name: "FK_person_color_color_ColorId",
                        column: x => x.ColorId,
                        principalTable: "color",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_person_color_person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "person",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "person_use_case",
                columns: table => new
                {
                    person_id = table.Column<int>(type: "int", nullable: false),
                    use_case_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person_use_case", x => new { x.person_id, x.use_case_id });
                    table.ForeignKey(
                        name: "FK_person_use_case_person_person_id",
                        column: x => x.person_id,
                        principalTable: "person",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customer_discount",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountValue = table.Column<int>(type: "int", nullable: false, defaultValue: 15),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    DiscountExpires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AgentId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_discount", x => x.id);
                    table.ForeignKey(
                        name: "FK_customer_discount_agent_AgentId",
                        column: x => x.AgentId,
                        principalTable: "agent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_customer_discount_customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "order",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_discount_id = table.Column<int>(type: "int", nullable: false),
                    service_id = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_customer_discount_customer_discount_id",
                        column: x => x.customer_discount_id,
                        principalTable: "customer_discount",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_service_service_id",
                        column: x => x.service_id,
                        principalTable: "service",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_address_city_id",
                table: "address",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "IX_address_state_id",
                table: "address",
                column: "state_id");

            migrationBuilder.CreateIndex(
                name: "IX_agent_person_id",
                table: "agent",
                column: "person_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_color_color_name",
                table: "color",
                column: "color_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_credentials_email",
                table: "credentials",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_credentials_person_id",
                table: "credentials",
                column: "person_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_customer_person_id",
                table: "customer",
                column: "person_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_customer_discount_AgentId",
                table: "customer_discount",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_customer_discount_CustomerId",
                table: "customer_discount",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_order_customer_discount_id",
                table: "order",
                column: "customer_discount_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_service_id",
                table: "order",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "IX_person_house_address_id",
                table: "person",
                column: "house_address_id");

            migrationBuilder.CreateIndex(
                name: "IX_person_office_address_id",
                table: "person",
                column: "office_address_id");

            migrationBuilder.CreateIndex(
                name: "IX_person_social_security_number",
                table: "person",
                column: "social_security_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_person_spouse_id",
                table: "person",
                column: "spouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_person_color_ColorId",
                table: "person_color",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_service_service_name",
                table: "service",
                column: "service_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "credentials");

            migrationBuilder.DropTable(
                name: "log_entry");

            migrationBuilder.DropTable(
                name: "order");

            migrationBuilder.DropTable(
                name: "person_color");

            migrationBuilder.DropTable(
                name: "person_use_case");

            migrationBuilder.DropTable(
                name: "customer_discount");

            migrationBuilder.DropTable(
                name: "service");

            migrationBuilder.DropTable(
                name: "color");

            migrationBuilder.DropTable(
                name: "agent");

            migrationBuilder.DropTable(
                name: "customer");

            migrationBuilder.DropTable(
                name: "person");

            migrationBuilder.DropTable(
                name: "address");

            migrationBuilder.DropTable(
                name: "city");

            migrationBuilder.DropTable(
                name: "state");
        }
    }
}
