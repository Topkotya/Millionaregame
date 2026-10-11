using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Text.Json;

namespace WpfApp2;

public class GameplayModel
{
    private string rightAnswer;
    private int currentRound = 0;
    private OptionModel[] Options;
    private QuestionModel[] questions;
    public string CurrentRound => (currentRound + 1).ToString();
    public GameplayModel()
    {
        string path = Path.Combine(AppContext.BaseDirectory,"Assets","questionsss.json");                
        var Questions = JsonSerializer.Deserialize<Questions>(File.ReadAllText(path), new JsonSerializerOptions() { IncludeFields = true })
                    ?? throw new InvalidOperationException("Не удалось десериализовать questions.json");
        questions = Questions.GetQuestions();
    }
    public List<ItemTemplate> GetReplics()
    {
        List<ItemTemplate> tempLines = new List<ItemTemplate>();
        tempLines.Add(new ItemTemplate() { Text = "звонок: " + ". . . . . . . . . .", TextColor = new SolidColorBrush(Color.FromRgb(255, 255, 255)), TypeSpeed = 8 });
        tempLines.Add(new ItemTemplate() { Text = "Я: " + "Привет помоги с вопросом", TextColor = new SolidColorBrush(Color.FromRgb(0, 212, 9)), TypeSpeed = 8 });
        tempLines.Add(new ItemTemplate() { Text = "Друг: " + "Да лады без а ответ Б", TextColor = new SolidColorBrush(Color.FromRgb(245, 52, 52)), TypeSpeed = 8 });
        return tempLines;
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
    private Random random = new Random();
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
            int j = random.Next(2);
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
                    answersPercentage[wrongIDAnswers[i]] = random.Next(0, 20);
                }
                answersPercentage[rightIDAnswer] = random.Next(60, 80);
                return answersPercentage;
            case < 8:
                for (int i = 0; i < answersPercentage.Length - 1; i++)
                {
                    answersPercentage[wrongIDAnswers[i]] = random.Next(20, 40);
                }
                answersPercentage[rightIDAnswer] = random.Next(30, 60);
                return answersPercentage;
            case < 12:
                for (int i = 0; i < answersPercentage.Length - 1; i++)
                {
                    answersPercentage[wrongIDAnswers[i]] = random.Next(30, 35);
                }
                answersPercentage[rightIDAnswer] = random.Next(40, 50);
                return answersPercentage;
            default:
                for (int i = 0; i < answersPercentage.Length - 1; i++)
                {
                    answersPercentage[wrongIDAnswers[i]] = random.Next(30, 35);
                }
                answersPercentage[rightIDAnswer] = random.Next(30, 35);
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
    public QuestionModel(string question, OptionModel[] options)
    {
        this.question = question;
        this.options = options;
    }
}