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
        public Brush Answer1Background
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Answer1Background));
            }
        }
        public Brush Answer2Background
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Answer2Background));
            }
        }

        public Brush Answer3Background
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Answer3Background));
            }
        }

        public Brush Answer4Background
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Answer4Background));
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

        public string Answer1
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Answer1));
            }
        }

        public string Answer2
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Answer2));
            }
        }

        public string Answer3
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Answer3));
            }
        }

        public string Answer4
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Answer4));
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
        }

        public ICommand SubmitCommand { get; }

        private void FieldAnswers()
        {
            var answers = mod.GetAnswers();
            Answer1 = answers[0];
            Answer2 = answers[1];
            Answer3 = answers[2];
            Answer4 = answers[3];
            OnPropertyChanged(nameof(QuestionText));
        }

        public MainViewModel()
        {
            PrintRound = "1";
            InitializeRewards();
            FieldAnswers();
            SubmitCommand = new RelayCommand(OnSubmit, CanSubmit);
        }
        private async Task SetAnswerColor(string answer, bool isCorrect)
        {
            if (answer == Answer1) await animator.PlayButtonAnimation(b => Answer1Background = b, isCorrect);
            else if (answer == Answer2) await animator.PlayButtonAnimation(b => Answer2Background = b, isCorrect);
            else if (answer == Answer3) await animator.PlayButtonAnimation(b => Answer3Background = b, isCorrect);
            else if (answer == Answer4) await animator.PlayButtonAnimation(b => Answer4Background = b, isCorrect);
        }
        private async void OnSubmit(object parameter)
        {
            if (isAnswering) return;
            isAnswering = true;
            string answer = parameter.ToString()!;
            bool isCorrect = mod.GetAnswer(answer);

            // Подсвечиваем выбранный ответ
            await SetAnswerColor(answer, isCorrect);

            if (isCorrect)
            {
                if (int.TryParse(mod.CurrentRound, out int result) && result == WinningRound) //переменная result это текущий раунд
                {
                    WinScreenState = true;
                    return;
                }
                int tempResult = result - 2;
                PrintRound = mod.CurrentRound; // перешли на следующий раунд                
                Rewards[result - 1].State = ERewardState.current;                
                while(tempResult >= 0)
                {
                    Rewards[tempResult--].State = ERewardState.completed;                    
                }                

                // Возвращаем кнопкам исходный градиент
                Answer1Background = null;
                Answer2Background = null;
                Answer3Background = null;
                Answer4Background = null;
                FieldAnswers();
            }
            else
            {
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
        public ObservableCollection<RewardView> Rewards { get; set; } = new ObservableCollection<RewardView>();
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}