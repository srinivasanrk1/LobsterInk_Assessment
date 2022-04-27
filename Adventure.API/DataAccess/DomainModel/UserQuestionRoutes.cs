using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure.API.DataAccess.DomainModel
{
    public class UserQuestionRoutes
    {
        [Key]
        public string Id { get; } = Guid.NewGuid().ToString();
        public string QuestionRouteId { get; set; }
        [ForeignKey("QuestionRouteId")]
        public  QuestionRoute QuestionRoute { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public  User User { get; set; }

    }
}
