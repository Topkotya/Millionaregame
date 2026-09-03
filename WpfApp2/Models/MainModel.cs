namespace WpfApp2;

public class MainModel
{
    private string rightAnswer;
    private int currentRound = 0;
    public string CurrentRound => (currentRound + 1).ToString();
    public bool GetAnswer(string answer)
    {
        if (answer == rightAnswer)
        {
            currentRound++;
            return true;
        }
        return false;
    }
    private QuestionModel[] questions = new QuestionModel[]
    {
        new("Сколько ног у жирафа?",
        [
            new("3", false),
            new("4", true),
            new("2", false),
            new("5", false)
        ]),
        new("Сколько ног у жирафаddddddddСколько ног у жирафаddddddddСколько ног у жирафаddddddddСколько ног у жирафаddddddddСколько ног у жирафаddddddddСколько ног у жирафаddddddddСколько ног у жирафаddddddddСколько ног у жирафаddddddddСколько ног у жирафаddddddddСколько ног у жирафаddddddddСколько ног у жирафаdddddddd?",
        [
            new("3", false),
            new("4", true),
            new("2", false),
            new("5", false)
        ]),

        new("Какую планету называют Красной планетой?",
        [
            new("Венеру", false),
            new("Юпитер", false),
            new("Марс", true),
            new("Меркурий", false)
        ]),

        new("Какой океан является самым большим на Земле?",
        [
            new("Атлантический", false),
            new("Индийский", false),
            new("Тихий", true),
            new("Северный Ледовитый", false)
        ]),

        new("Какой металл обозначается химическим символом Fe?",
        [
            new("Медь", false),
            new("Серебро", false),
            new("Железо", true),
            new("Золото", false)
        ]),

        new("Какой газ преобладает в атмосфере Земли?",
        [
            new("Кислород", false),
            new("Углекислый газ", false),
            new("Азот", true),
            new("Водород", false)
        ]),

        new("В каком году человек впервые высадился на Луне?",
        [
            new("1959", false),
            new("1965", false),
            new("1969", true),
            new("1975", false)
        ]),

        new("Как называется самая глубокая океаническая впадина на Земле?",
        [
            new("Пуэрто-Риканский жёлоб", false),
            new("Марианская впадина", true),
            new("Кермадекский жёлоб", false),
            new("Яванский жёлоб", false)
        ]),

        new("Какой элемент имеет атомный номер 79?",
        [
            new("Серебро", false),
            new("Платина", false),
            new("Золото", true),
            new("Ртуть", false)
        ]),

        new("Как называется наука, изучающая землетрясения?",
        [
            new("Метеорология", false),
            new("Сейсмология", true),
            new("Геодезия", false),
            new("Вулканология", false)
        ]),

        new("Как называется гипотетическая граница вокруг чёрной дыры, за которой ничто не может покинуть её?",
        [
            new("Сингулярность", false),
            new("Аккреционный диск", false),
            new("Горизонт событий", true),
            new("Гравитационная линза", false)
        ])
    };

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

public struct OptionModel(string str, bool isRight)
{
    public string Str { get; } = str;
    public bool IsRight { get; } = isRight;
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