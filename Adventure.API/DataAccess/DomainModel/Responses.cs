using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure.API.DataAccess.DomainModel
{
    public class Responses
    {
        [Key]
        public string Id { get; } = Guid.NewGuid().ToString();       
        public string Text { get; set; }
        public  ICollection<QuestionRoute> QuestionRoute { get; set; }


    }
}
