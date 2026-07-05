using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CinemaTickets.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Img = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cinemas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Img = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cinemas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Halls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TotalSeats = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CinemaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Halls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Halls_Cinemas_CinemaId",
                        column: x => x.CinemaId,
                        principalTable: "Cinemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MainImg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvailableSeats = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CinemaId = table.Column<int>(type: "int", nullable: false),
                    HallId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movies_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Movies_Cinemas_CinemaId",
                        column: x => x.CinemaId,
                        principalTable: "Cinemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Movies_Halls_HallId",
                        column: x => x.HallId,
                        principalTable: "Halls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "MovieActors",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    ActorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieActors", x => new { x.MovieId, x.ActorId });
                    table.ForeignKey(
                        name: "FK_MovieActors_Actors_ActorId",
                        column: x => x.ActorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieActors_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieSubImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Img = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieSubImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieSubImages_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Actors",
                columns: new[] { "Id", "Bio", "Img", "Name" },
                values: new object[,]
                {
                    { 1, "Academy Award-winning actor.", null, "Tom Hanks" },
                    { 2, "Hollywood A-list actress.", null, "Scarlett Johansson" },
                    { 3, "Oscar-winning actor.", null, "Leonardo DiCaprio" },
                    { 4, "Hunger Games star.", null, "Jennifer Lawrence" },
                    { 5, "Iron Man himself.", null, "Robert Downey Jr." }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Action" },
                    { 2, "Drama" },
                    { 3, "Comedy" },
                    { 4, "Horror" },
                    { 5, "Sci-Fi" }
                });

            migrationBuilder.InsertData(
                table: "Cinemas",
                columns: new[] { "Id", "Img", "Location", "Name" },
                values: new object[,]
                {
                    { 1, null, "Downtown", "Grand Cinema" },
                    { 2, null, "Westside", "Star Cinema" },
                    { 3, null, "North Mall", "Galaxy Cinema" }
                });

            migrationBuilder.InsertData(
                table: "Halls",
                columns: new[] { "Id", "CinemaId", "Description", "Name", "TotalSeats" },
                values: new object[,]
                {
                    { 1, 1, "Main IMAX Screen", "Screen 1", 200 },
                    { 2, 1, "Standard Hall", "Screen 2", 150 },
                    { 3, 2, "Dolby Atmos", "Hall A", 180 },
                    { 4, 2, "Premium Seating", "VIP Room", 60 },
                    { 5, 3, "4DX Experience", "Hall 1", 220 }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "AvailableSeats", "CategoryId", "CinemaId", "CreatedAt", "DateTime", "Description", "HallId", "MainImg", "Name", "Price", "Status" },
                values: new object[,]
                {
                    { 1, 145, 5, 1, new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 6, 3, 0, 0, 0, 0, DateTimeKind.Utc), "An epic sci-fi adventure across the cosmos where humanity battles an alien empire.", 1, null, "Galactic Warriors", 12.99m, 1 },
                    { 2, 180, 2, 2, new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), "A heartwarming romance between two strangers in the City of Light.", 3, null, "Love in Paris", 9.99m, 0 },
                    { 3, 0, 4, 3, new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 5, 22, 0, 0, 0, 0, DateTimeKind.Utc), "A chilling horror mystery set in an isolated mansion.", 5, null, "The Dark Secret", 11.50m, 2 },
                    { 4, 55, 3, 1, new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Utc), "A hilarious comedy following three mismatched roommates in New York City.", 2, null, "Laughing Out Loud", 8.99m, 1 }
                });

            migrationBuilder.InsertData(
                table: "MovieActors",
                columns: new[] { "ActorId", "MovieId" },
                values: new object[,]
                {
                    { 3, 1 },
                    { 5, 1 },
                    { 1, 2 },
                    { 4, 2 },
                    { 2, 3 },
                    { 1, 4 },
                    { 4, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Halls_CinemaId",
                table: "Halls",
                column: "CinemaId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieActors_ActorId",
                table: "MovieActors",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_CategoryId",
                table: "Movies",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_CinemaId",
                table: "Movies",
                column: "CinemaId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_HallId",
                table: "Movies",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieSubImages_MovieId",
                table: "MovieSubImages",
                column: "MovieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieActors");

            migrationBuilder.DropTable(
                name: "MovieSubImages");

            migrationBuilder.DropTable(
                name: "Actors");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Halls");

            migrationBuilder.DropTable(
                name: "Cinemas");
        }
    }
}
