using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal class Math_Game
    {


        public enum QuestionLevel
        {
            Easy = 1,
            Medium,
            Hard,
            Mixed
        }

        public enum OperationType
        {
            Addition = 1,
            Subtraction,
            Multiplication,
            Division,
            Mixed
        }

        public class Question
        {
            public int Number1 { get; set; }
            public int Number2 { get; set; }
            public OperationType OpType { get; set; }
            public QuestionLevel Level { get; set; }
            public int CorrectAnswer { get; set; }
            public int PlayerAnswer { get; set; }
            public bool IsCorrect => PlayerAnswer == CorrectAnswer;
        }

        public class Quiz
        {
            public List<Question> Questions { get; set; } = new List<Question>();
            public int NumberOfQuestions { get; set; }
            public OperationType OpType { get; set; }
            public QuestionLevel Level { get; set; }
            public int CorrectAnswers { get; set; }
            public int IncorrectAnswers { get; set; }
            public bool IsPass => CorrectAnswers >= IncorrectAnswers;
        }

        public class MathGame
        {
            private readonly Random _rand = new Random();

            private int GenerateRandomNumber(int from, int to) => _rand.Next(from, to);

            private int PromptForInteger(string message, int minValue, int maxValue)
            {
                int result;
                do
                {
                    Console.WriteLine(message);
                } while (!int.TryParse(Console.ReadLine(), out result) || result < minValue || result > maxValue);
                return result;
            }

            private QuestionLevel PromptForQuestionLevel() =>
                (QuestionLevel)PromptForInteger("Enter Question Level: [1] Easy, [2] Medium, [3] Hard, [4] Mixed", 1, 4);

            private OperationType PromptForOperationType() =>
                (OperationType)PromptForInteger("Enter Operation Type: [1] Addition, [2] Subtraction, [3] Multiplication, [4] Division, [5] Mixed", 1, 5);

            private void SetConsoleColor(ConsoleColor color)
            {
                Console.ForegroundColor = color;
                Console.ResetColor();
            }

            private Question GenerateQuestion(OperationType opType, QuestionLevel level)
            {
                if (level == QuestionLevel.Mixed) level = (QuestionLevel)GenerateRandomNumber(1, 3);
                if (opType == OperationType.Mixed) opType = (OperationType)GenerateRandomNumber(1, 4);

                int number1 = level switch
                {
                    QuestionLevel.Easy => GenerateRandomNumber(1, 10),
                    QuestionLevel.Medium => GenerateRandomNumber(11, 30),
                    QuestionLevel.Hard => GenerateRandomNumber(31, 100),
                    _ => 1
                };

                int number2 = level switch
                {
                    QuestionLevel.Easy => GenerateRandomNumber(1, 10),
                    QuestionLevel.Medium => GenerateRandomNumber(11, 30),
                    QuestionLevel.Hard => GenerateRandomNumber(31, 100),
                    _ => 1
                };

                int correctAnswer = opType switch
                {
                    OperationType.Addition => number1 + number2,
                    OperationType.Subtraction => number1 - number2,
                    OperationType.Multiplication => number1 * number2,
                    OperationType.Division => number1 / number2,
                    _ => 0
                };

                return new Question
                {
                    Number1 = number1,
                    Number2 = number2,
                    OpType = opType,
                    Level = level,
                    CorrectAnswer = correctAnswer
                };
            }

            private void GenerateQuizQuestions(Quiz quiz)
            {
                for (int i = 0; i < quiz.NumberOfQuestions; i++)
                {
                    quiz.Questions.Add(GenerateQuestion(quiz.OpType, quiz.Level));
                }
            }

            private void DisplayQuestion(Question question, int index, int total)
            {
                Console.WriteLine($"\nQuestion {index + 1}/{total}");
                Console.WriteLine($" {question.Number1}\n   {GetOperationSymbol(question.OpType)}\n {question.Number2}\n-----------------");
            }

            private string GetOperationSymbol(OperationType opType) =>
                opType switch
                {
                    OperationType.Addition => "+",
                    OperationType.Subtraction => "-",
                    OperationType.Multiplication => "*",
                    OperationType.Division => "/",
                    _ => "?"
                };

            private void DisplayQuizResults(Quiz quiz)
            {
                Console.WriteLine("\n------------------");
                Console.WriteLine($"Final: {(quiz.IsPass ? "Pass :-)" : "Fail :-(")}");
                Console.WriteLine("------------------");
                Console.WriteLine($"Number Of Questions   : {quiz.NumberOfQuestions}");
                Console.WriteLine($"Questions Level       : {quiz.Level}");
                Console.WriteLine($"Operation Type        : {quiz.OpType}");
                Console.WriteLine($"Number Of Correct Answers : {quiz.CorrectAnswers}");
                Console.WriteLine($"Number Of Incorrect Answers: {quiz.IncorrectAnswers}");
                Console.WriteLine("------------------");
            }

            private void ProcessQuizAnswers(Quiz quiz)
            {
                foreach (var question in quiz.Questions)
                {
                    DisplayQuestion(question, quiz.Questions.IndexOf(question), quiz.NumberOfQuestions);
                    int playerAnswer = int.Parse(Console.ReadLine());
                    question.PlayerAnswer = playerAnswer;

                    if (question.IsCorrect)
                    {
                        quiz.CorrectAnswers++;
                        Console.WriteLine("Correct :-)");
                        SetConsoleColor(ConsoleColor.Green);
                    }
                    else
                    {
                        quiz.IncorrectAnswers++;
                        Console.WriteLine($"Incorrect :-( Correct Answer: {question.CorrectAnswer}");
                        SetConsoleColor(ConsoleColor.Red);
                    }
                }
            }

            private void ResetConsole()
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.ResetColor();
            }

            public void StartGame()
            {
                string playAgain;
                do
                {
                    ResetConsole();
                    Quiz quiz = new Quiz
                    {
                        NumberOfQuestions = PromptForInteger("How many questions do you want to answer?", 1, int.MaxValue),
                        Level = PromptForQuestionLevel(),
                        OpType = PromptForOperationType()
                    };

                    GenerateQuizQuestions(quiz);
                    ProcessQuizAnswers(quiz);
                    DisplayQuizResults(quiz);

                    Console.WriteLine("\nDo you want to play again? (Y/N)");
                    playAgain = Console.ReadLine();
                } while (playAgain.Equals("Y", StringComparison.OrdinalIgnoreCase));
            }

       
        }

       static void Main()
        {
            MathGame Game = new MathGame();
            Game.StartGame();   
        }
    }

}

