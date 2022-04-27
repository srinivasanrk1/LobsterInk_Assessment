using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure.API.DataAccess.DomainModel
{
    public class QuestionRoute
    {
        [Key]
        public string Id { get; } = Guid.NewGuid().ToString();
        public string QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public Questions Questions { get; set; }
        public string ResponseId { get; set; }
        [ForeignKey("ResponseId")]
        public Responses Responses { get; set; }
        public string PreviousQuestionRouteId { get; set; }
        [ForeignKey("PreviousQuestionRouteId")]
        public QuestionRoute PreviousQuestionRoute { get; set; }
        public string AdventureId { get; set; }

        [ForeignKey("AdventureId")]
        public Adventures Adventure { get; set; }

        public int? Order { get; set; }
        public ICollection<UserQuestionRoutes> UserQuestionRoute { get; set; }
        public ICollection<QuestionRoute> Children { get; set; }


    }
}
