using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Text.Json;

namespace WpfApp2;

public class GameplayModel
{
    private string rightAnswer = "";
    private int currentRound = 0;
    private OptionModel[] Options = Array.Empty<OptionModel>();
    private QuestionModel[] questions;
    private readonly Random random = new Random();

    public string CurrentRound => (currentRound + 1).ToString();
    public string RightAnswer => rightAnswer;

    public GameplayModel()
    {        
        string path = Path.Combine(AppContext.BaseDirectory,"Assets","questionsss.json");        
        var Questions = JsonSerializer.Deserialize<Questions>(File.ReadAllText(path), new JsonSerializerOptions() { IncludeFields = true })
                   ?? throw new InvalidOperationException("Не удалось десериализовать questions.json");

        questions = Questions.GetQuestions();
    }

    public List<ItemTemplate> GetReplics()
    {
        var question = questions[currentRound];
        List<ItemTemplate> tempLines = new List<ItemTemplate>();
        tempLines.Add(new ItemTemplate() { Text = "звонок: " + ".  .  .  .  .  .  .  .  .  .", TextColor = "#FF5733", TypeSpeed = 8 });

        // replics[0] - вопрос игрока, [1] - верный ответ, [2] - неверный ответ, [3] - абонент не отвечает
        tempLines.Add(CloneLine(question.replics[0], question.question));

        int roll = random.Next(100);
        if (roll < 5)
        {
            tempLines.Add(CloneLine(question.replics[3], null));
        }
        else if (roll < 20)
        {
            var wrongAnswers = Options.Where(x => !x.IsRight).ToArray();
            string wrongAnswer = wrongAnswers[random.Next(wrongAnswers.Length)].Str;
            tempLines.Add(CloneLine(question.replics[2], wrongAnswer));
        }
        else
        {
            tempLines.Add(CloneLine(question.replics[1], rightAnswer));
        }

        return tempLines;        
    }

    private static ItemTemplate CloneLine(ItemTemplate source, string? value)
    {
        string text = value == null ? source.Text : source.Text.Replace("{0}", value);
        return new ItemTemplate() { Text = text, TypeSpeed = source.TypeSpeed, TextColor = source.TextColor };
    }

    public bool CheckAnswer(string answer)
    {
        if (answer == rightAnswer)
        {
            currentRound++;
            return true;
        }
        return false;
    }

    public string GetQuestion()
    {
        return questions[currentRound].question;
    }

    public int[] GetIncorrectAnswersIDs(int maxCount = 2)
    {
        int[] tempNotRightAnswers = Options.Select((x, index) => new { Value = x, Index = index })
                      .Where(pair => pair.Value.IsRight == false)
                      .Select(pair => pair.Index)
                      .ToArray();

        if (maxCount == 3)
        {
            return [tempNotRightAnswers[0], tempNotRightAnswers[1], tempNotRightAnswers[2]];
        }

        for (int i = tempNotRightAnswers.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (tempNotRightAnswers[i], tempNotRightAnswers[j]) = (tempNotRightAnswers[j], tempNotRightAnswers[i]);
        }        
        return [tempNotRightAnswers[0], tempNotRightAnswers[1]];               
    }

    public int[] PercentAnswers()
    {
        int[] wrongIDAnswers = GetIncorrectAnswersIDs(3);
        int rightIDAnswer = Options.Select((x, index) => new { Value = x, Index = index })
              .Where(pair => pair.Value.IsRight == true)
              .Select(pair => pair.Index).First();

        int[] answersPercentage = new int[4];
        switch (currentRound)
        {
            case < 4:
                for (int i = 0; i < answersPercentage.Length - 1; i++)
                {
                    answersPercentage[wrongIDAnswers[i]] = random.Next(0, 5);
                }
                answersPercentage[rightIDAnswer] = random.Next(80, 89);
                return answersPercentage;

            case < 8:
                for (int i = 0; i < answersPercentage.Length - 1; i++)
                {
                    answersPercentage[wrongIDAnswers[i]] = random.Next(5, 15);
                }
                answersPercentage[rightIDAnswer] = random.Next(50, 59);
                return answersPercentage;

            case < 12:
                for (int i = 0; i < answersPercentage.Length - 1; i++)
                {
                    answersPercentage[wrongIDAnswers[i]] = random.Next(10, 20);
                }
                answersPercentage[rightIDAnswer] = random.Next(35, 44);
                return answersPercentage;

            default:
                for (int i = 0; i < answersPercentage.Length - 1; i++)
                {
                    answersPercentage[wrongIDAnswers[i]] = random.Next(15, 25);
                }
                answersPercentage[rightIDAnswer] = random.Next(20, 29);
                return answersPercentage;
        }
    }

    public string[] GetAnswers()
    {
        Options = questions[currentRound].options.ToArray();
        for (int i = Options.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (Options[i], Options[j]) = (Options[j], Options[i]);
        }
        rightAnswer = Options.First(x => x.IsRight).Str;        
        return Options.Select(x => x.Str).ToArray();        
    }
}

public class OptionModel
{
    public string Str { get; init; }
    public bool IsRight { get; init; }
}
public class QuestionModel
{
    public readonly string question;    
    public readonly OptionModel[] options;
    public readonly ItemTemplate[] replics;
    public QuestionModel(string question, OptionModel[] options, ItemTemplate[] replics)
    {
        this.question = question;
        this.options = options;
        this.replics = replics;
    }
}
