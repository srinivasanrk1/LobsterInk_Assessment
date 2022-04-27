using Microsoft.EntityFrameworkCore.Migrations;

namespace Adventure.API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Adventures",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adventures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Responses",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    QuestionsId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Responses_Questions_QuestionsId",
                        column: x => x.QuestionsId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionRoutes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    QuestionId = table.Column<string>(nullable: true),
                    ResponseId = table.Column<string>(nullable: true),
                    PreviousQuestionRouteId = table.Column<string>(nullable: true),
                    AdventureId = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: true),
                    QuestionRouteId = table.Column<string>(nullable: true),
                    QuestionsId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionRoutes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionRoutes_Adventures_AdventureId",
                        column: x => x.AdventureId,
                        principalTable: "Adventures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionRoutes_QuestionRoutes_PreviousQuestionRouteId",
                        column: x => x.PreviousQuestionRouteId,
                        principalTable: "QuestionRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionRoutes_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionRoutes_QuestionRoutes_QuestionRouteId",
                        column: x => x.QuestionRouteId,
                        principalTable: "QuestionRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionRoutes_Questions_QuestionsId",
                        column: x => x.QuestionsId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionRoutes_Responses_ResponseId",
                        column: x => x.ResponseId,
                        principalTable: "Responses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserQuestionRoutes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    QuestionRouteId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuestionRoutes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserQuestionRoutes_QuestionRoutes_QuestionRouteId",
                        column: x => x.QuestionRouteId,
                        principalTable: "QuestionRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserQuestionRoutes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName" },
                values: new object[] { "2d5fc533-a3d4-43a8-8db4-5c220b83a917", "David.Mike@Gmail.com", "David", "Mike" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName" },
                values: new object[] { "eaf5b47f-cf4c-488f-b3dd-56874c4251f8", "stevewarner@outlook.com", "Steve", "Warner" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName" },
                values: new object[] { "55b84e75-6cb9-4812-9056-e6c1059a0378", "GothamCity@batman.com", "Gotham", "City" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionRoutes_AdventureId",
                table: "QuestionRoutes",
                column: "AdventureId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionRoutes_PreviousQuestionRouteId",
                table: "QuestionRoutes",
                column: "PreviousQuestionRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionRoutes_QuestionId",
                table: "QuestionRoutes",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionRoutes_QuestionRouteId",
                table: "QuestionRoutes",
                column: "QuestionRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionRoutes_QuestionsId",
                table: "QuestionRoutes",
                column: "QuestionsId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionRoutes_ResponseId",
                table: "QuestionRoutes",
                column: "ResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_Responses_QuestionsId",
                table: "Responses",
                column: "QuestionsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestionRoutes_QuestionRouteId",
                table: "UserQuestionRoutes",
                column: "QuestionRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestionRoutes_UserId",
                table: "UserQuestionRoutes",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserQuestionRoutes");

            migrationBuilder.DropTable(
                name: "QuestionRoutes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Adventures");

            migrationBuilder.DropTable(
                name: "Responses");

            migrationBuilder.DropTable(
                name: "Questions");
        }
    }
}
