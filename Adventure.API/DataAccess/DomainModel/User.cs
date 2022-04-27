using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
 
namespace Adventure.API.DataAccess.DomainModel
{
    public class User
    {
        [Key]
        public string Id { get; } = Guid.NewGuid().ToString();

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public ICollection<UserQuestionRoutes> UserQuestionRoute { get; set; }

    }
}
