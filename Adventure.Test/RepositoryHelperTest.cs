using Adventure.DataAccessLayer.DBContexts;
using System;
using Microsoft.EntityFrameworkCore;

namespace Adventure.Test
{
    public static class RepositoryHelperTest
    {
        public static DbContextOptions<AdventureContext> DbContextOptionsInMemory()
        {
            var options = new DbContextOptionsBuilder<AdventureContext>()
                                .UseInMemoryDatabase($"AdventureInMenmory{Guid.NewGuid()}")
                                .Options;
            return options;
        }

        public static void CreateInMemoryDBData(DbContextOptions<AdventureContext>  dbContextOptions)
        {
            using var context = new AdventureContext(dbContextOptions);

            context.Users.Add(
                new API.DataAccess.DomainModel.User
                { Email = "test@mail.com", FirstName = "test", LastName = "user" });
            context.Users.Add(
                new API.DataAccess.DomainModel.User
                { Email = "test1@mail.com", FirstName = "test10", LastName = "user10" });

            context.Adventures.Add(new API.DataAccess.DomainModel.Adventures
            { Text = "Choclate Helper" });

            context.SaveChanges();
        }
    }
}
