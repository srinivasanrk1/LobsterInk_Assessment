using Adventure.API.DataAccess.DomainModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using Newtonsoft.Json;
using Adventure.API.System;

namespace Adventure.DataAccessLayer.DBContexts
{
    public class AdventureContext : DbContext
    {
        public AdventureContext(DbContextOptions<AdventureContext> options)
           : base(options)
        {
        }
        public DbSet<Adventures> Adventures { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<QuestionRoute> QuestionRoutes { get; set; }

        public DbSet<Questions> Questions { get; set; }

        public DbSet<Responses> Responses { get; set; }

        public DbSet<UserQuestionRoutes> UserQuestionRoutes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // seed the database with dummy data


            modelBuilder.Entity<User>().HasKey(e => new { e.Id });

            modelBuilder.Entity<Adventures>().HasKey(e => new { e.Id });
            //one-to-many relationship 


            //specify no autogenerate the Id Column
            modelBuilder.Entity<Questions>().HasKey(b => new { b.Id });
           
            //specify no autogenerate the Id Column
            modelBuilder.Entity<Responses>().HasKey(b => new { b.Id });

            modelBuilder.Entity<QuestionRoute>().HasKey(b => new { b.Id });

            modelBuilder.Entity<QuestionRoute>()
                     .HasOne(x => x.Questions)
                     .WithMany(t => t.QuestionRoute)
                     .HasForeignKey(m => m.QuestionId);

            modelBuilder.Entity<QuestionRoute>()
                       .HasOne(x => x.PreviousQuestionRoute)
                       .WithMany()
                       .HasForeignKey(m => m.PreviousQuestionRouteId);

            modelBuilder.Entity<QuestionRoute>()
                     .HasOne(x => x.Responses)
                     .WithMany(t => t.QuestionRoute)
                     .HasForeignKey(m => m.ResponseId);

            modelBuilder.Entity<QuestionRoute>().Property(m => m.Order).IsRequired(false);

            modelBuilder.Entity<QuestionRoute>()
                    .HasOne(x => x.Adventure)
                    .WithMany(t => t.Questions)
                    .HasForeignKey(m => m.AdventureId);
            //one-to-many relationship 

            modelBuilder.Entity<UserQuestionRoutes>().HasKey(b => new { b.Id });

            modelBuilder.Entity<UserQuestionRoutes>()
                       .HasOne(x => x.QuestionRoute)
                       .WithMany(t => t.UserQuestionRoute)
                       .HasForeignKey(m => m.QuestionRouteId);

            modelBuilder.Entity<UserQuestionRoutes>()
                       .HasOne(x => x.User)
                       .WithMany(t => t.UserQuestionRoute)
                       .HasForeignKey(m => m.UserId);


            Console.WriteLine("Seeding data to User table");


            //var jsonString = File.ReadAllText(@"sample.json");
            //var jsonData = JsonConvert.DeserializeObject<Root>(jsonString);





            modelBuilder.Entity<User>().HasData(
                 new User
                 {
                     FirstName = "David",
                     LastName = "Mike",
                     Email = "David.Mike@Gmail.com"
                 },
                new User
                {
                    FirstName = "Steve",
                    LastName = "Warner",
                    Email = "stevewarner@outlook.com"
                },
                 new User
                 {
                     FirstName = "Gotham",
                     LastName = "City",
                     Email = "GothamCity@batman.com"
                 }
                );



            base.OnModelCreating(modelBuilder);
        }
    }
}
