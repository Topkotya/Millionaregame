using System.IO;
using System.Linq;
using System.Text.Json;

namespace WpfApp2;

public class MainModel
{
    private string rightAnswer;
    private int currentRound = 0;
    private OptionModel[] Options;
    private QuestionModel[] questions;
    public string CurrentRound => (currentRound + 1).ToString();
    public MainModel()
    {
        string path = Path.Combine(AppContext.BaseDirectory,"Assets","questionsss.json");                
        var Questions = JsonSerializer.Deserialize<Questions>(File.ReadAllText(path), new JsonSerializerOptions() { IncludeFields = true })
                    ?? throw new InvalidOperationException("Не удалось десериализовать questions.json");
        questions = Questions.GetQuestions();
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
    public int[] GetIncorrectAnswersIDs()
    {
        int[] tempNotRightAnswers = Options.Select((x, index) => new { Value = x, Index = index })
                      .Where(pair => pair.Value.IsRight == false)
                      .Select(pair => pair.Index)
                      .ToArray();        
       
        for (int i = tempNotRightAnswers.Length - 1; i > 0; i--)
        {
            int j = random.Next(2);
            (tempNotRightAnswers[i], tempNotRightAnswers[j]) = (tempNotRightAnswers[j], tempNotRightAnswers[i]);
        }        

        return [tempNotRightAnswers[0], tempNotRightAnswers[1]];
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