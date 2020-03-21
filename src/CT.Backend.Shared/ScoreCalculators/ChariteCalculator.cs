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
            var sum = 0;
            foreach (var answer in Answers)
            {
                var theQuestion = Questions.FirstOrDefault(q => q.Id == answer.First().Key);
                if(theQuestion == null)
                {
                    sum = 0;
                }
                else
                {
                    sum += theQuestion.PossibleAnswers.First(p =>
                    p.Value == answer.First().Value).Score;
                }
                
            }
            return sum;
        }
    }
}
