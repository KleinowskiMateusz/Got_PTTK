using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KsiazeczkaPTTK.DAL.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GotPttk",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Level = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GotPttk", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MountainGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MountainGroups", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Name = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Name);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MountainRanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MountainRanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MountainRanges_MountainGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "MountainGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Login = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserRoleName = table.Column<string>(type: "varchar(40)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Login);
                    table.ForeignKey(
                        name: "FK_Users_UserRoles_UserRoleName",
                        column: x => x.UserRoleName,
                        principalTable: "UserRoles",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TouristsBooks",
                columns: table => new
                {
                    OwnerId = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Disability = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TouristsBooks", x => x.OwnerId);
                    table.ForeignKey(
                        name: "FK_TouristsBooks_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Login",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GotPttkOwnerships",
                columns: table => new
                {
                    Owner = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GotPttkId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DateOfAward = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GotPttkOwnerships", x => new { x.Owner, x.GotPttkId });
                    table.ForeignKey(
                        name: "FK_GotPttkOwnerships_GotPttk_GotPttkId",
                        column: x => x.GotPttkId,
                        principalTable: "GotPttk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GotPttkOwnerships_TouristsBooks_Owner",
                        column: x => x.Owner,
                        principalTable: "TouristsBooks",
                        principalColumn: "OwnerId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TerrainPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Lat = table.Column<double>(type: "double", nullable: false),
                    Lng = table.Column<double>(type: "double", nullable: false),
                    Mnpm = table.Column<double>(type: "double", nullable: false),
                    TouristsBookOwner = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerrainPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TerrainPoints_TouristsBooks_TouristsBookOwner",
                        column: x => x.TouristsBookOwner,
                        principalTable: "TouristsBooks",
                        principalColumn: "OwnerId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TouristsBookId = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trips_TouristsBooks_TouristsBookId",
                        column: x => x.TouristsBookId,
                        principalTable: "TouristsBooks",
                        principalColumn: "OwnerId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Confirmations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TerrainPointId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsAdministration = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Confirmations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Confirmations_TerrainPoints_TerrainPointId",
                        column: x => x.TerrainPointId,
                        principalTable: "TerrainPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Segments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Points = table.Column<int>(type: "int", nullable: false),
                    PointsBack = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FromId = table.Column<int>(type: "int", nullable: false),
                    TargetId = table.Column<int>(type: "int", nullable: false),
                    MountainRangeId = table.Column<int>(type: "int", nullable: false),
                    TouristsBookOwner = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Segments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Segments_MountainRanges_MountainRangeId",
                        column: x => x.MountainRangeId,
                        principalTable: "MountainRanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Segments_TerrainPoints_FromId",
                        column: x => x.FromId,
                        principalTable: "TerrainPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Segments_TerrainPoints_TargetId",
                        column: x => x.TargetId,
                        principalTable: "TerrainPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Segments_TouristsBooks_TouristsBookOwner",
                        column: x => x.TouristsBookOwner,
                        principalTable: "TouristsBooks",
                        principalColumn: "OwnerId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SegmentCloses",
                columns: table => new
                {
                    SegmentId = table.Column<int>(type: "int", nullable: false),
                    ClosedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    OpenedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Cause = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SegmentCloses", x => new { x.SegmentId, x.ClosedDate });
                    table.ForeignKey(
                        name: "FK_SegmentCloses_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SegmentTravels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Order = table.Column<int>(type: "int", nullable: false),
                    TripId = table.Column<int>(type: "int", nullable: false),
                    SegmentId = table.Column<int>(type: "int", nullable: false),
                    IsBack = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SegmentTravels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SegmentTravels_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SegmentTravels_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SegmentConfirmations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConfirmationId = table.Column<int>(type: "int", nullable: false),
                    SegmentTravelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SegmentConfirmations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SegmentConfirmations_Confirmations_ConfirmationId",
                        column: x => x.ConfirmationId,
                        principalTable: "Confirmations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SegmentConfirmations_SegmentTravels_SegmentTravelId",
                        column: x => x.SegmentTravelId,
                        principalTable: "SegmentTravels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Confirmations_TerrainPointId",
                table: "Confirmations",
                column: "TerrainPointId");

            migrationBuilder.CreateIndex(
                name: "IX_GotPttk_Level",
                table: "GotPttk",
                column: "Level",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GotPttkOwnerships_GotPttkId",
                table: "GotPttkOwnerships",
                column: "GotPttkId");

            migrationBuilder.CreateIndex(
                name: "IX_MountainGroups_Name",
                table: "MountainGroups",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MountainRanges_GroupId",
                table: "MountainRanges",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MountainRanges_Name",
                table: "MountainRanges",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SegmentConfirmations_ConfirmationId",
                table: "SegmentConfirmations",
                column: "ConfirmationId");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentConfirmations_SegmentTravelId",
                table: "SegmentConfirmations",
                column: "SegmentTravelId");

            migrationBuilder.CreateIndex(
                name: "IX_Segments_FromId",
                table: "Segments",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Segments_MountainRangeId",
                table: "Segments",
                column: "MountainRangeId");

            migrationBuilder.CreateIndex(
                name: "IX_Segments_TargetId",
                table: "Segments",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_Segments_TouristsBookOwner",
                table: "Segments",
                column: "TouristsBookOwner");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentTravels_SegmentId",
                table: "SegmentTravels",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentTravels_TripId",
                table: "SegmentTravels",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_TerrainPoints_Name",
                table: "TerrainPoints",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TerrainPoints_TouristsBookOwner",
                table: "TerrainPoints",
                column: "TouristsBookOwner");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_TouristsBookId",
                table: "Trips",
                column: "TouristsBookId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserRoleName",
                table: "Users",
                column: "UserRoleName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GotPttkOwnerships");

            migrationBuilder.DropTable(
                name: "SegmentCloses");

            migrationBuilder.DropTable(
                name: "SegmentConfirmations");

            migrationBuilder.DropTable(
                name: "GotPttk");

            migrationBuilder.DropTable(
                name: "Confirmations");

            migrationBuilder.DropTable(
                name: "SegmentTravels");

            migrationBuilder.DropTable(
                name: "Segments");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "MountainRanges");

            migrationBuilder.DropTable(
                name: "TerrainPoints");

            migrationBuilder.DropTable(
                name: "MountainGroups");

            migrationBuilder.DropTable(
                name: "TouristsBooks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserRoles");
        }
    }
}
