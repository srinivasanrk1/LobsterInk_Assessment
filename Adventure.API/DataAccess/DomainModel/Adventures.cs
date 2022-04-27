using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Adventure.API.DataAccess.DomainModel
{
    public class Adventures
    {
        [Key]
        public string Id { get; } = Guid.NewGuid().ToString();

        public string Text { get; set; }
        public ICollection<QuestionRoute> Questions { get; set; }

    }
}
