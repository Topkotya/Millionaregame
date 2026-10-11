using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using WpfApp2.Helpers;
using WpfApp2.Models.Hints;
//сделать виртуальных друзей, 65/30/5, в 65 случаях из 100 подсказывает правильно, а в остальных 30 не уверенно отвечат
//"может то а может это или хз", в 5 случаях из 100 не дозвонились до друга
//про: учитывать сложность вопроса для ответа друга
namespace WpfApp2
{
    public class GameplayViewModel : INotifyPropertyChanged
    {
        private GameplayModel mod = new GameplayModel();
        private AnimationHelper animator = new AnimationHelper();
        private AudienceBarAnimation audienceBarAnimation;
        public FiftyFiftyHintView FiftyFiftyHint { get; }
        public ErrorRightHintView ErrorRightHint { get; }
        public AudienceHelpHintView AudienceHelpHint { get; }
        public FriendCallHintView FriendCallHint { get; }
        private const int WinningRound = 16;
        private bool isAnswering;
        public Action<EGameState> OnStateChanged;
        public string AudienceVoteVisibility
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(AudienceVoteVisibility));
            }
        }
        public string FriendCallVisibility
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(FriendCallVisibility));
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
            if (!ErrorRightHint.IsEnabled || ErrorRightHint.ErrorRightActive)
                return;

            ErrorRightHint.ErrorRightActive = true;
            await animator.PlayHintAnimation(b => ErrorRightHint.ErrorRightBackground = b);
            ErrorRightHint.IsEnabled = false;
            ErrorRightHint.Opacity = "0.5";
        }
        private async void OnFiftyFifty(object parameter)
        {            
            if (!FiftyFiftyHint.IsEnabled || FiftyFiftyHint.FiftyFiftyActive)
                return;

            int[] array = mod.GetIncorrectAnswersIDs(2);
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

            FiftyFiftyHint.FiftyFiftyActive = true;
            FiftyFiftyHint.IsEnabled = false;
            await animator.PlayHintAnimation(b => FiftyFiftyHint.FiftyFiftyBackground = b);
            FiftyFiftyHint.Opacity = "0.5";            
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
        public GameplayViewModel()
        {
            PrintRound = "1";
            InitializeRewards();
            InitializeAnswers();
            InirialieAudience();
            FieldAnswers();

            SubmitCommand = new RelayCommand(OnSubmit, CanSubmit);
            CloseAudienceVoteCommand = new RelayCommand(CloseAudienceWindow, _ => true);
            CloseFriendCallCommand = new RelayCommand(CloseFriendCall, _ => true);            

            FiftyFiftyHint = new FiftyFiftyHintView(OnFiftyFifty);
            ErrorRightHint = new ErrorRightHintView(OnErrorRight);
            AudienceHelpHint = new AudienceHelpHintView(OnAudienceHelp);
            FriendCallHint = new FriendCallHintView(OnFriendCallHelp);

            audienceBarAnimation = new AudienceBarAnimation(AudienceVotes);
            FiftyFiftyHint.FiftyFiftyActive = false;
        }
        private async void OnFriendCallHelp(object parameter)
        {
            if (!FriendCallHint.IsEnabled || FriendCallHint.FriendCallHelpActive)
                return;

            FriendCallHint.FriendCallHelpActive = true;
            FriendCallHint.IsEnabled = false;            
            await animator.PlayHintAnimation(b => FriendCallHint.FriendCallBackground = b);
            FriendCallHint.Opacity = "0.5";
            FriendCallVisibility = "Visible";
            await FriendCallAnimation.PlayAnimation(c => TextLineItems = new ObservableCollection<ItemTemplate>(c), mod.GetReplics());
        }
        private void CloseFriendCall(object parametr)
        {
            FriendCallVisibility = "Hidden";
        }
        private async void OnAudienceHelp(object parameter)
        {
            if (!AudienceHelpHint.IsEnabled || AudienceHelpHint.AudienceHelpActive)
                return;            
            AudienceHelpHint.AudienceHelpActive = true;
            AudienceHelpHint.IsEnabled = false;
            
            // Анимация кнопки и полупрозрачность
            await animator.PlayHintAnimation(b => AudienceHelpHint.AudienceHelpBackground = b);
            AudienceHelpHint.Opacity = "0.5";

            // Рассчитываем проценты, запускаем анимацию колонок и открываем модальное окно
            int[] percentages = mod.PercentAnswers();
            AudienceVoteVisibility = "Visible";
            await audienceBarAnimation.PlayAnimation(percentages);            
        }
        private void CloseAudienceWindow(object parametr)
        {
            AudienceVoteVisibility = "Hidden";
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
                AudienceVoteVisibility = "Visible";
                // любой ответ тратит право на ошибку
                ErrorRightHint.ErrorRightActive = false;
                if (int.TryParse(mod.CurrentRound, out int result) && result == WinningRound)
                {
                    OnStateChanged(EGameState.Win);
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
                if (ErrorRightHint.ErrorRightActive)
                {
                    ErrorRightHint.ErrorRightActive = false;
                    ErrorRightHint.IsEnabled = false;
                    var wrongAsnwer = Answers.Where(a => a.Text == answer).First();
                    wrongAsnwer.IsEnabled = false;
                    isAnswering = false;
                    return;
                }
                // Обычная неправильная попытка
                OnStateChanged(EGameState.Lose);
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
            List<RewardView> tempRewards = Enumerable.Range(0, 15).Select(x => new RewardView() { Reward = (Math.Pow(2, x) * 300).ToString() }).ToList();
            tempRewards[0].State = ERewardState.Current;
            Rewards = new ObservableCollection<RewardView>(tempRewards);
        }
        private void InitializeAnswers()
        {
            List<AnswerItem> tempAnswers = Enumerable.Range(0, 4).Select(x => new AnswerItem() { Text = "text", Letter = "letter", Visability = "Visability" }).ToList();
            tempAnswers[0].Letter = "A"; tempAnswers[1].Letter = "B"; tempAnswers[2].Letter = "C"; tempAnswers[3].Letter = "D";
            Answers = new ObservableCollection<AnswerItem>(tempAnswers);
        }
        private void InirialieAudience()
        {
            List<AudienceVoteItem> tempAudienceBars = Enumerable.Range(1, 4).Select(x => new AudienceVoteItem() { Letter = "letter", Percent = 0, BarHeight = 100,
                BarColor = new SolidColorBrush(Color.FromRgb(100,10,1)) }).ToList();
            tempAudienceBars[0].Letter = "A"; tempAudienceBars[1].Letter = "B"; tempAudienceBars[2].Letter = "C"; tempAudienceBars[3].Letter = "D";
            AudienceVotes = new ObservableCollection<AudienceVoteItem>(tempAudienceBars);
        }
        public ObservableCollection<RewardView> Rewards { get; set; } = new ObservableCollection<RewardView>();
        public ICommand SubmitCommand { get; }
        public ICommand CloseAudienceVoteCommand { get; }
        public ICommand CloseFriendCallCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<AnswerItem> Answers { get; set; } = new ObservableCollection<AnswerItem>();
        public ObservableCollection<AudienceVoteItem> AudienceVotes { get; set; } = new ObservableCollection<AudienceVoteItem>();
        public ObservableCollection<ItemTemplate> TextLineItems { get; set; } = new ObservableCollection<ItemTemplate>();
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}