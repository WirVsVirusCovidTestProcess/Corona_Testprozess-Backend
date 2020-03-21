using System;
using System.Collections.Generic;
using System.Text;

namespace CT.Backend.Shared.Models
{
    public class Question
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public IEnumerable<Answer> PossibleAnswers { get; set; }
    }
}
