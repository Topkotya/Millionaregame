using System.IO;
using System.Linq;
using System.Text.Json;

namespace WpfApp2;

public class MainModel
{
    private string rightAnswer;
    private int currentRound = 0;
    private QuestionModel[] questions;
    public string CurrentRound => (currentRound + 1).ToString();
    public MainModel()
    {
        string path = Path.Combine(AppContext.BaseDirectory,"Assets","questionsss.json");                
        var Questions = JsonSerializer.Deserialize<Questions>(File.ReadAllText(path), new JsonSerializerOptions() { IncludeFields = true })
                    ?? throw new InvalidOperationException("Не удалось десериализовать questions.json");
        questions = Questions.GetQuestions();
    }
    public bool GetAnswer(string answer)
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
    public string[] GetAnswers()
    {
        var options = questions[currentRound].options.ToArray();
        for (int i = options.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (options[i], options[j]) = (options[j], options[i]);
        }
        rightAnswer = options.First(x => x.IsRight).Str;
        return options.Select(x => x.Str).ToArray();
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