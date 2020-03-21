using CT.Backend.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CT.Backend.Shared.ScoreCalculators
{
    public class ChariteCalculator : ICalculator
    {
        public string SourceId { get => "covapp.charite"; }
        public IEnumerable<Question> Questions { get => new List<Question>() { 
            new Question()
            {
                Description = "Wie alt sind Sie?",
                Id = "A",
                PossibleAnswers = new List<Answer>()
                {
                    new Answer()
                    {
                        Value = "0",
                        Description = "unter 40",
                        Score = 0
                    },
                    new Answer()
                    {
                        Value = "1",
                        Description = "unter 50",
                        Score = 2
                    }
                }
            }
        } ; set => throw new NotImplementedException(); }

        public int Calculate(IEnumerable<IDictionary<string, string>> Answers)
        {
            return Answers
                .Select(a => 
                    Questions.First(q => 
                        q.Id == a.First().Key)
                    .PossibleAnswers.First(p => 
                        p.Value == a.First().Value)
                    .Score)
                .Sum();
        }
    }
}
