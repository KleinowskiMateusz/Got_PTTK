using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KsiazeczkaPTTK.DAL.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GotPttk",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Level = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GotPttk", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MountainGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MountainGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "MountainRanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    GroupId = table.Column<int>(type: "integer", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Login = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Password = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    Email = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserRoleName = table.Column<string>(type: "character varying(40)", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "TouristsBooks",
                columns: table => new
                {
                    OwnerId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Disability = table.Column<bool>(type: "boolean", nullable: false),
                    Points = table.Column<int>(type: "integer", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "GotPttkOwnerships",
                columns: table => new
                {
                    Owner = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    GotPttkId = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateOfAward = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "TerrainPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Lat = table.Column<double>(type: "double precision", nullable: false),
                    Lng = table.Column<double>(type: "double precision", nullable: false),
                    Mnpm = table.Column<double>(type: "double precision", nullable: false),
                    TouristsBookOwner = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerrainPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TerrainPoints_TouristsBooks_TouristsBookOwner",
                        column: x => x.TouristsBookOwner,
                        principalTable: "TouristsBooks",
                        principalColumn: "OwnerId");
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    TouristsBookId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "Confirmations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    TerrainPointId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsAdministration = table.Column<bool>(type: "boolean", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "Segments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Points = table.Column<int>(type: "integer", nullable: false),
                    PointsBack = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    FromId = table.Column<int>(type: "integer", nullable: false),
                    TargetId = table.Column<int>(type: "integer", nullable: false),
                    MountainRangeId = table.Column<int>(type: "integer", nullable: false),
                    TouristsBookOwner = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "SegmentCloses",
                columns: table => new
                {
                    SegmentId = table.Column<int>(type: "integer", nullable: false),
                    ClosedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OpenedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Cause = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "SegmentTravels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    TripId = table.Column<int>(type: "integer", nullable: false),
                    SegmentId = table.Column<int>(type: "integer", nullable: false),
                    IsBack = table.Column<bool>(type: "boolean", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "SegmentConfirmations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConfirmationId = table.Column<int>(type: "integer", nullable: false),
                    SegmentTravelId = table.Column<int>(type: "integer", nullable: false)
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
                });

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
