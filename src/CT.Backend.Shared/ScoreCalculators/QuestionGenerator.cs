using System;
using System.Collections.Generic;
using System.Text;
using CT.Backend.Shared.Models;

namespace CT.Backend.Shared.ScoreCalculators
{
    class QuestionGenerator
    {
        internal List<Question> GetQuestions()
        {
            return new List<Question>()
            {
                new Question()
                {
                    Description = "Wie alt sind Sie?",
                    Id = "A",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Jünger als 40", Score = 0 },
                        new Answer() { Value = "1", Description = "40-50", Score = 0 },
                        new Answer() { Value = "2", Description = "51-60", Score = 0 },
                        new Answer() { Value = "3", Description = "61-70", Score = 0 },
                        new Answer() { Value = "4", Description = "71-80", Score = 0 },
                        new Answer() { Value = "5", Description = "Über 80", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Wie ist Ihre aktuelle Wohnsituation?",
                    Id = "B",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Alleine wohnend", Score = 0 },
                        new Answer() { Value = "1", Description = "Zusammen mit Familie, in einer Wohngemeinschaft oder betreuten Gemeinschaftseinrichtung", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Sind Sie in einem der folgenden Bereiche tätig?",
                    Id = "C",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Im medizinischen Bereich", Score = 0 },
                        new Answer() { Value = "1", Description = "In einer Gemeinschaftseinrichtung (Schule, Kita, Universität, Heim etc.)", Score = 0 },
                        new Answer() { Value = "2", Description = "Nein, in keinem der genannten Bereiche", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Rauchen Sie?",
                    Id = "D",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Ja", Score = 0 },
                        new Answer() { Value = "1", Description = "Nein", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Sind Sie in den letzten 4 Wochen verreist? (innerhalb Deutschlands oder ins Ausland)",
                    Id = "E",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Ja", Score = 0 },
                        new Answer() { Value = "1", Description = "Nein", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Haben Sie sich im Landkreis Heinsberg (Nordrhein-Westfalen) aufgehalten?",
                    Id = "F",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Ja", Score = 1 },
                        new Answer() { Value = "1", Description = "Nein", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "An welchem Tag sind Sie von Ihrer Reise zurückgekehrt?",
                    Id = "R1",
                    PossibleAnswers = new List<Answer>()
                    {
                    }
                },
                new Question()
                {
                    Description = "Waren Sie in den letzten 4 Wochen außerhalb Deutschlands?",
                    Id = "G",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Ja", Score = 0 },
                        new Answer() { Value = "1", Description = "Nein", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Waren Sie in einem der folgenden Länder?",
                    Id = "H",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Italien", Score = 1 },
                        new Answer() { Value = "1", Description = "Iran", Score = 1 },
                        new Answer() { Value = "2", Description = "China", Score = 0 },
                        new Answer() { Value = "3", Description = "Südkorea", Score = 0 },
                        new Answer() { Value = "4", Description = "Frankreich", Score = 0 },
                        new Answer() { Value = "5", Description = "Österreich", Score = 0 },
                        new Answer() { Value = "6", Description = "Spanien", Score = 0 },
                        new Answer() { Value = "7", Description = "USA", Score = 0 },
                        new Answer() { Value = "8", Description = "Ich war in keinem der genannten Länder", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Italien: Waren Sie in einer der folgenden Regionen?",
                    Id = "J",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Südtirol (entspricht Provinz Bozen) in der Region Trentino-Südtirol", Score = 1 },
                        new Answer() { Value = "1", Description = "Region Emilia-Romagna", Score = 1 },
                        new Answer() { Value = "2", Description = "Region Lombardei", Score = 1 },
                        new Answer() { Value = "3", Description = "Stadt Vo in der Provinz Padua in der Region Venetien", Score = 1 },
                        new Answer() { Value = "5", Description = "Ich war in keiner der genannten Regionen", Score = 0 }
                    },
                },
                new Question()
                {
                    Description = "Iran: Waren Sie in einer der folgenden Regionen?",
                    Id = "K",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Provinz Ghom", Score = 1 },
                        new Answer() { Value = "1", Description = "Teheran", Score = 1 },
                        new Answer() { Value = "5", Description = "Ich war in keiner der genannten Regionen", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "China: Waren Sie in einer der folgenden Regionen?",
                    Id = "K",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Provinz Hubei (inkl. Stadt Wuhan)", Score = 1 },
                        new Answer() { Value = "5", Description = "Ich war in keiner der genannten Regionen", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Südkorea: Waren Sie in einer der folgenden Regionen?",
                    Id = "L",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Provinz Gyeongsangbuk-do (Nord-Gyeongsang)", Score = 1 },
                        new Answer() { Value = "5", Description = "Ich war in keiner der genannten Regionen", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Frankreich: Waren Sie in einer der folgenden Regionen?",
                    Id = "M",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Region Grand Est (diese Region enthält Elsass, Lothringen und Champagne-Ardenne)", Score = 1 },
                        new Answer() { Value = "5", Description = "Ich war in keiner der genannten Regionen", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Österreich: Waren Sie in einer der folgenden Regionen?",
                    Id = "N",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Bundesland Tirol", Score = 1 },
                        new Answer() { Value = "5", Description = "Ich war in keiner der genannten Regionen", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Spanien: Waren Sie in einer der folgenden Regionen?",
                    Id = "O",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Madrid", Score = 1 },
                        new Answer() { Value = "5", Description = "Ich war in keiner der genannten Regionen", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "USA: Waren Sie in einer der folgenden Regionen?",
                    Id = "P",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Bundesstaat New York", Score = 1 },
                        new Answer() { Value = "0", Description = "Bundesstaat Washington", Score = 1 },
                        new Answer() { Value = "0", Description = "Bundesstaat Kalifornien", Score = 1 },
                        new Answer() { Value = "5", Description = "Ich war in keiner der genannten Regionen", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "An welchem Tag sind Sie von Ihrer Reise zurückgekehrt?",
                    Id = "Q",
                    PossibleAnswers = new List<Answer>()
                    {
                    }
                },
                new Question()
                {
                    Description = "Hatten Sie engen Kontakt zu einem bestätigtem Fall?",
                    Id = "R",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Ja", Score = 1 },
                        new Answer() { Value = "1", Description = "Nein", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "An welchem Tag war der letzte Kontakt?",
                    Id = "S",
                    PossibleAnswers = new List<Answer>()
                    {
                    }
                },
                new Question()
                {
                    Description = "Hatten Sie Fieber (über 38 °C) in den letzten 24 Stunden?",
                    Id = "T",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Ja", Score = 1 },
                        new Answer() { Value = "1", Description = "Nein", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Hatten Sie Fieber (über 38 °C) in den letzten 4 Tagen?",
                    Id = "T",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Ja", Score = 1 },
                        new Answer() { Value = "1", Description = "Nein", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Wie hoch war die höchste Temperatur ca.?",
                    Id = "U",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Weniger als 38 °C", Score = 0 },
                        new Answer() { Value = "1", Description = "38 °C", Score = 0 },
                        new Answer() { Value = "2", Description = "39 °C", Score = 0 },
                        new Answer() { Value = "3", Description = "40 °C", Score = 0 },
                        new Answer() { Value = "4", Description = "41 °C", Score = 0 },
                        new Answer() { Value = "4", Description = "42 °C", Score = 0 },
                        new Answer() { Value = "4", Description = "Über 42 °C", Score = 0 },
                        new Answer() { Value = "5", Description = "Ich weiß es nicht", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Haben Sie Schüttelfrost?",
                    Id = "V",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Ja", Score = 1 },
                        new Answer() { Value = "1", Description = "Nein", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Fühlen Sie sich schlapp oder abgeschlagen?",
                    Id = "W",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Ja", Score = 1 },
                        new Answer() { Value = "1", Description = "Nein", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Haben Sie Gliederschmerzen?",
                    Id = "X",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Ja", Score = 1 },
                        new Answer() { Value = "1", Description = "Nein", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Haben Sie anhaltenden Husten?",
                    Id = "Y",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Ja", Score = 1 },
                        new Answer() { Value = "1", Description = "Nein", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Haben Sie Schnupfen?",
                    Id = "Z",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Ja", Score = 1 },
                        new Answer() { Value = "1", Description = "Nein", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Haben Sie Durchfall?",
                    Id = "AA",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Ja", Score = 1 },
                        new Answer() { Value = "1", Description = "Nein", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Haben Sie Halsschmerzen?",
                    Id = "AB",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Ja", Score = 0 },
                        new Answer() { Value = "1", Description = "Nein", Score = 1 }
                    }
                },
                new Question()
                {
                    Description = "Haben Sie Kopfschmerzen?",
                    Id = "AC",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Ja", Score = 1 },
                        new Answer() { Value = "1", Description = "Nein", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Sind Sie schneller außer Atem als sonst?",
                    Id = "AD",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Ja", Score = 1 },
                        new Answer() { Value = "1", Description = "Nein", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Seit wann haben Sie die Symptome?",
                    Id = "AE",
                    PossibleAnswers = new List<Answer>()
                    {
                    }
                },
                new Question()
                {
                    Description = "Wurde bei Ihnen eine chronische Lungenerkrankung diagnostiziert?",
                    Id = "AF",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Ja", Score = 0 },
                        new Answer() { Value = "1", Description = "Nein", Score = 0 },
                        new Answer() { Value = "2", Description = "Ich weiß es nicht", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Haben Sie Diabetes?",
                    Id = "AG",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Ja", Score = 0 },
                        new Answer() { Value = "1", Description = "Nein", Score = 0 },
                        new Answer() { Value = "2", Description = "Ich weiß es nicht", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Wurde bei Ihnen eine Herzerkrankung diagnostiziert?",
                    Id = "AH",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Ja", Score = 0 },
                        new Answer() { Value = "1", Description = "Nein", Score = 0 },
                        new Answer() { Value = "2", Description = "Ich weiß es nicht", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Wurde bei Ihnen Adipositas (Fettsucht) diagnostiziert?",
                    Id = "AI",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Ja", Score = 0 },
                        new Answer() { Value = "1", Description = "Nein", Score = 0 },
                        new Answer() { Value = "2", Description = "Ich weiß es nicht", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Sind Sie schwanger?",
                    Id = "AJ",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Ja", Score = 0 },
                        new Answer() { Value = "1", Description = "Nein", Score = 0 },
                        new Answer() { Value = "2", Description = "Ich weiß es nicht", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Nehmen Sie aktuell Cortison ein (in Tablettenform)?",
                    Id = "AK",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Ja", Score = 0 },
                        new Answer() { Value = "1", Description = "Nein", Score = 0 },
                        new Answer() { Value = "2", Description = "Ich weiß es nicht", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Nehmen Sie aktuell Immunsuppressiva?",
                    Id = "AL",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Ja", Score = 0 },
                        new Answer() { Value = "1", Description = "Nein", Score = 0 },
                        new Answer() { Value = "2", Description = "Ich weiß es nicht", Score = 0 }
                    }
                },
                new Question()
                {
                    Description = "Haben Sie sich im Zeitraum von Oktober 2019 bis heute gegen Grippe impfen lassen?",
                    Id = "AM",
                    PossibleAnswers = new List<Answer>()
                    {
                        new Answer() { Value = "0", Description = "Ja", Score = 0 },
                        new Answer() { Value = "1", Description = "Nein", Score = 0 }
                    }
                },
            };
        }
    }
}
