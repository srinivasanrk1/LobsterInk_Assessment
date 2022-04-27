using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure.API.DataAccess.DomainModel
{
    public class Questions
    {
        [Key]
        public string Id { get; } = Guid.NewGuid().ToString();     

        public string Text { get; set; }      

        public  ICollection<Responses> Responses { get; set; }
        public  ICollection<QuestionRoute> QuestionRoute { get; set; }
        public  ICollection<QuestionRoute> NextQuestionRoute { get; set; }


    }
}
