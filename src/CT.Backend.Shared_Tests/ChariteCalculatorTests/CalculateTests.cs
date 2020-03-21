using System;
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
                    {"A", "2"}
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

        [TestMethod]
        public void DateValue3DaysBeforeToday_Results3()
        {
            var answers = new List<Dictionary<string, string>>()
            {
                new Dictionary<string, string>()
                {
                    {"A", DateTime.Now.AddDays(-3).ToString("dd.MM.yyy")}
                }
            };

            var target = new ChariteCalculator();
            var actual = target.Calculate(answers);
            actual.Should().Be(3);
        }

        [TestMethod]
        public void DateValueMultipleChoiceMixed_Results5()
        {
            var answers = new List<Dictionary<string, string>>()
            {
                new Dictionary<string, string>()
                {
                    {"A", "2"}
                },
                new Dictionary<string, string>()
                {
                    {"B",  DateTime.Now.AddDays(-3).ToString("dd.MM.yyy")}
                }
            };

            var target = new ChariteCalculator();
            var actual = target.Calculate(answers);
            actual.Should().Be(5);
        }

        [TestMethod]
        public void MultipleQuestions_Results5()
        {
            var answers = new List<Dictionary<string, string>>()
            {
                new Dictionary<string, string>()
                {
                    {"A", "0"}
                },
                new Dictionary<string, string>()
                {
                    {"B", "1"}
                },
                new Dictionary<string, string>()
                {
                    {"C", "2"}
                },
                new Dictionary<string, string>()
                {
                    {"D", "1"}
                },
                new Dictionary<string, string>()
                {
                    {"E", "1"}
                }
            };

            var target = new ChariteCalculator();
            var actual = target.Calculate(answers);
            actual.Should().Be(5);
        }
    }
}
