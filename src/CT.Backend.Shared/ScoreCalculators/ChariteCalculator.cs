using CT.Backend.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CT.Backend.Shared.ScoreCalculators
{
    public class ChariteCalculator : ICalculator
    {
        public string SourceId { get => "covapp.charite"; }

        public IEnumerable<Question> Questions
        {
            get => new QuestionGenerator().GetQuestions();
            set => throw new NotImplementedException();
        }

        /// <summary>
        /// Calculate a risk store.
        /// </summary>
        /// <param name="Answers">The answers from the user.</param>
        /// <returns></returns>
        public int Calculate(IEnumerable<IDictionary<string, string>> Answers)
        {
            var sum = 0;
            foreach (var answer in Answers)
            {
                var theQuestion = Questions.FirstOrDefault(q => q.Id == answer.First().Key);
                if (theQuestion == null)
                {
                    sum = 0;
                }
                else
                {
                    DateTime dateValue;
                    if (DateTime.TryParse(answer.First().Value, out dateValue))
                    {
                        sum += DateTime.Now.Subtract(dateValue).Days;
                    }
                    else
                    {
                       var fullAnswer = theQuestion.PossibleAnswers.FirstOrDefault(p => p.Value == answer.First().Value);
                        if(fullAnswer == null)
                        {   
                            sum = 0;
                        }
                        else
                        {
                            sum += fullAnswer.Score;
                        }
                    }
                }

            }
            return sum;
        }
    }
}
