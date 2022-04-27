﻿// <auto-generated />
using System;
using Adventure.DataAccessLayer.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Adventure.API.Migrations
{
    [DbContext(typeof(AdventureContext))]
    partial class AdventureContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Adventure.API.DataAccess.DomainModel.Adventures", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Adventures");
                });

            modelBuilder.Entity("Adventure.API.DataAccess.DomainModel.QuestionRoute", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AdventureId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("Order")
                        .HasColumnType("int");

                    b.Property<string>("PreviousQuestionRouteId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("QuestionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("QuestionRouteId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("QuestionsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ResponseId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("AdventureId");

                    b.HasIndex("PreviousQuestionRouteId");

                    b.HasIndex("QuestionId");

                    b.HasIndex("QuestionRouteId");

                    b.HasIndex("QuestionsId");

                    b.HasIndex("ResponseId");

                    b.ToTable("QuestionRoutes");
                });

            modelBuilder.Entity("Adventure.API.DataAccess.DomainModel.Questions", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("Adventure.API.DataAccess.DomainModel.Responses", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("QuestionsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("QuestionsId");

                    b.ToTable("Responses");
                });

            modelBuilder.Entity("Adventure.API.DataAccess.DomainModel.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = "2d5fc533-a3d4-43a8-8db4-5c220b83a917",
                            Email = "David.Mike@Gmail.com",
                            FirstName = "David",
                            LastName = "Mike"
                        },
                        new
                        {
                            Id = "eaf5b47f-cf4c-488f-b3dd-56874c4251f8",
                            Email = "stevewarner@outlook.com",
                            FirstName = "Steve",
                            LastName = "Warner"
                        },
                        new
                        {
                            Id = "55b84e75-6cb9-4812-9056-e6c1059a0378",
                            Email = "GothamCity@batman.com",
                            FirstName = "Gotham",
                            LastName = "City"
                        });
                });

            modelBuilder.Entity("Adventure.API.DataAccess.DomainModel.UserQuestionRoutes", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("QuestionRouteId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("QuestionRouteId");

                    b.HasIndex("UserId");

                    b.ToTable("UserQuestionRoutes");
                });

            modelBuilder.Entity("Adventure.API.DataAccess.DomainModel.QuestionRoute", b =>
                {
                    b.HasOne("Adventure.API.DataAccess.DomainModel.Adventures", "Adventure")
                        .WithMany("Questions")
                        .HasForeignKey("AdventureId");

                    b.HasOne("Adventure.API.DataAccess.DomainModel.QuestionRoute", "PreviousQuestionRoute")
                        .WithMany()
                        .HasForeignKey("PreviousQuestionRouteId");

                    b.HasOne("Adventure.API.DataAccess.DomainModel.Questions", "Questions")
                        .WithMany("QuestionRoute")
                        .HasForeignKey("QuestionId");

                    b.HasOne("Adventure.API.DataAccess.DomainModel.QuestionRoute", null)
                        .WithMany("Children")
                        .HasForeignKey("QuestionRouteId");

                    b.HasOne("Adventure.API.DataAccess.DomainModel.Questions", null)
                        .WithMany("NextQuestionRoute")
                        .HasForeignKey("QuestionsId");

                    b.HasOne("Adventure.API.DataAccess.DomainModel.Responses", "Responses")
                        .WithMany("QuestionRoute")
                        .HasForeignKey("ResponseId");
                });

            modelBuilder.Entity("Adventure.API.DataAccess.DomainModel.Responses", b =>
                {
                    b.HasOne("Adventure.API.DataAccess.DomainModel.Questions", null)
                        .WithMany("Responses")
                        .HasForeignKey("QuestionsId");
                });

            modelBuilder.Entity("Adventure.API.DataAccess.DomainModel.UserQuestionRoutes", b =>
                {
                    b.HasOne("Adventure.API.DataAccess.DomainModel.QuestionRoute", "QuestionRoute")
                        .WithMany("UserQuestionRoute")
                        .HasForeignKey("QuestionRouteId");

                    b.HasOne("Adventure.API.DataAccess.DomainModel.User", "User")
                        .WithMany("UserQuestionRoute")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}