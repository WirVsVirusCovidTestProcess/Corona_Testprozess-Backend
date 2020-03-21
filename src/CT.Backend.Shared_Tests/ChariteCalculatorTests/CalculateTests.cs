using System.Collections.Generic;
using CT.Backend.Shared.ScoreCalculators;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Backend.Shered_Tests.ChariteCalculatorTests
{
    [TestClass]
    public class CalculateTests
    {
        [TestMethod]
        public void QuestionAAnswer1_Results2()
        {
            var answers = new List<Dictionary<string, string>>()
            {
                new Dictionary<string, string>()
                { 
                    {"A", "1"}
                }
            };

            var target = new ChariteCalculator();
            var actual = target.Calculate(answers);
            actual.Should().Be(2);
        }

        [TestMethod]
        public void QuestionAAnswer0_Results0()
        {
            var answers = new List<Dictionary<string, string>>()
            {
                new Dictionary<string, string>()
                {
                    {"A", "0"}
                }
            };

            var target = new ChariteCalculator();
            var actual = target.Calculate(answers);
            actual.Should().Be(0);
        }

        [TestMethod]
        public void EmptyAnswers_Results0()
        {
            var answers = new List<Dictionary<string, string>>()
            {
            };

            var target = new ChariteCalculator();
            var actual = target.Calculate(answers);
            actual.Should().Be(0);
        }
    }
}
