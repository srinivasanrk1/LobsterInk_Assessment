using System.Collections.Generic;

namespace Adventure.API.System
{
    public class AdventureStepResult
    {
        public string currentquestionRouteId { get; set; }
        public string currentQuestionText { get; set; }
        public List<NextQuestions> nextQuestions { get; set; } = new List<NextQuestions>();
    }
    public class NextQuestions
    {
        public string questionRouteId;
        public string responseId;
        public string questionText;
        public string responseText;
    }
}



