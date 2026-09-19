using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows;
using System.Windows;
using System.Windows.Input;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WpfApp2
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private MainModel mod = new MainModel();
        private AnimationHelper animator = new AnimationHelper();
        private const int WinningRound = 16;
        private bool isAnswering;
        public bool FiftyFiftyAvailable
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(FiftyFiftyAvailable));
            }
        }
        public bool FiftyFiftyActive
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(FiftyFiftyActive));
            }
        }
        public ICommand FiftyFiftyCommand { get; }
        public bool ErrorRightAvailable
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(ErrorRightAvailable));
            }
        }

        public bool ErrorRightActive
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(ErrorRightActive));
            }
        }
        public ICommand ErrorRightCommand { get; }
        public bool WinScreenState
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(WinScreenState));
            }
        }
        public bool LoseScreenState
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(LoseScreenState));
            }
        }
        public Brush FiftyFiftyBackground
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(FiftyFiftyBackground));
            }
        }

        public Brush ErrorRightBackground
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(ErrorRightBackground));
            }
        }
        public string PrintRound
        {
            get => field;
            set
            {
                field = value + " Вопрос";
                OnPropertyChanged(nameof(PrintRound));
            }
        }
        public string QuestionText
        {
            get => mod.GetQuestion();
            set
            {
                field = value;
                OnPropertyChanged(nameof(QuestionText));
            }
        } = "Visible";

        public ICommand SubmitCommand { get; }

        private void FieldAnswers()
        {
            var answers = mod.GetAnswers();
            for (int i = 0; i < answers.Length; i++)
            {
                Answers[i].Text = answers[i];
            }
            OnPropertyChanged(nameof(QuestionText));
        }
        private async void OnErrorRight(object parameter)
        {
            if (!ErrorRightAvailable || ErrorRightActive)
                return;

            ErrorRightActive = true;
            await animator.PlayHintAnimation(b => ErrorRightBackground = b);
        }
        private async void OnFiftyFifty(object parameter)
        {
            if (!FiftyFiftyAvailable || FiftyFiftyActive)
                return;

            int[] array = mod.GetIncorrectAnswersIDs();
            for (int i = 0; i < array.Length; i++)
            {
                switch (array[i])
                {
                    case 0:
                        Answers[0].Visability = "Hidden";
                        break;

                    case 1:
                        Answers[1].Visability = "Hidden";
                        break;

                    case 2:
                        Answers[2].Visability = "Hidden";
                        break;

                    case 3:
                        Answers[3].Visability = "Hidden";
                        break;
                }
            }

            FiftyFiftyActive = true;
            FiftyFiftyAvailable = false;
            await animator.PlayHintAnimation(b => FiftyFiftyBackground = b);
        }

        public MainViewModel()
        {
            PrintRound = "1";
            InitializeRewards();
            InitializeAnswers();
            FieldAnswers();

            SubmitCommand = new RelayCommand(OnSubmit, CanSubmit);
            ErrorRightCommand = new RelayCommand(OnErrorRight);
            FiftyFiftyCommand = new RelayCommand(OnFiftyFifty);

            ErrorRightAvailable = true;
            FiftyFiftyAvailable = true;
            FiftyFiftyActive = false;
        }
        private void ResetVisibility()
        {
            foreach (var item in Answers) 
            {
                item.Visability = "Visible";
            }
        }
        private async Task SetAnswerColor(string answer, bool isCorrect)
        {
            var answerr = Answers.First(a => a.Text == answer);
            await animator.PlayButtonAnimation(b => answerr.Background = b, isCorrect);            
        }
        private async void OnSubmit(object parameter)
        {
            if (isAnswering) return;
            isAnswering = true;
            string answer = parameter.ToString()!;
            bool isCorrect = mod.CheckAnswer(answer);
            // Подсвечиваем выбранный ответ
            await SetAnswerColor(answer, isCorrect);

            if (isCorrect)
            {
                // Правильный ответ не тратит право на ошибку
                ErrorRightActive = false;
                if (int.TryParse(mod.CurrentRound, out int result) && result == WinningRound)
                {
                    WinScreenState = true;
                    return;
                }
                int tempResult = result - 2;
                PrintRound = mod.CurrentRound;
                Rewards[result - 1].State = ERewardState.Current;
                while (tempResult >= 0)
                {
                    Rewards[tempResult--].State = ERewardState.Completed;
                }

                foreach (var item in Answers)
                {
                    item.Background = null;
                }
                FieldAnswers();
                ResetVisibility();
            }
            else
            {
                if (ErrorRightActive)
                {
                    ErrorRightActive = false;
                    ErrorRightAvailable = false;
                    foreach (var item in Answers)
                    {
                        item.Background = null;
                    }
                    isAnswering = false;
                    return;
                }
                // Обычная неправильная попытка
                LoseScreenState = true;
                return;
            }
            isAnswering = false;
        }
        private bool CanSubmit(object parameter)
        {
            return !isAnswering;
        }
        private void InitializeRewards()
        {            
            List<RewardView> tempRewards = Enumerable.Range(0,15).Select(x => new RewardView() { Reward = (Math.Pow(2, x) * 300).ToString() }).ToList();
            Rewards = new ObservableCollection<RewardView>(tempRewards);
        }
        private void InitializeAnswers()
        {
            List<AnswerItem> tempAnswers = Enumerable.Range(0, 4).Select(x => new AnswerItem() { Text = "text", Letter = "letter", Visability = "Visability"}).ToList();
            tempAnswers[0].Letter = "A"; tempAnswers[1].Letter = "B"; tempAnswers[2].Letter = "C"; tempAnswers[3].Letter = "D";
            Answers = new ObservableCollection<AnswerItem>(tempAnswers);
        }
        public ObservableCollection<RewardView> Rewards { get; set; } = new ObservableCollection<RewardView>();
        public ObservableCollection<AnswerItem> Answers { get; set; } = new ObservableCollection<AnswerItem>();
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}