using CT.Backend.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CT.Backend.Shared.ScoreCalculators
{
    public interface ICalculator
    {
        string SourceId { get; }
        IEnumerable<Question> Questions { get; set; }
        int Calculate(IEnumerable<IDictionary<string, string>> Answers);
    }
}
