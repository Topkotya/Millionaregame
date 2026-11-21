using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WpfApp2.Helpers;
using WpfApp2.Models.Hints;

namespace WpfApp2
{
    public class GameplayViewModel : INotifyPropertyChanged, IDisposable
    {
        private const int WinningRound = 15;
        private static readonly Brush SelectedBrush = new SolidColorBrush(Color.FromRgb(250, 176, 5));

        private GameplayModel mod = new GameplayModel();
        private AnimationHelper animator = new AnimationHelper();
        private AudienceBarAnimation audienceBarAnimation;
        private readonly AudioService _audio = new();
        private readonly int fireproofQuestion;
        private bool isAnswering;
        private string selectedAnswer = "";

        public FiftyFiftyHintView FiftyFiftyHint { get; }
        public ErrorRightHintView ErrorRightHint { get; }
        public AudienceHelpHintView AudienceHelpHint { get; }
        public FriendCallHintView FriendCallHint { get; }
        public Action<EGameState> OnStateChanged;

        public int Winnings { get; private set; }
        public string LastCorrectAnswer { get; private set; } = "";
        public bool TookMoney { get; private set; }
        public string FireproofText { get; }

        public GameplayViewModel(int fireproofQuestion)
        {
            this.fireproofQuestion = Math.Clamp(fireproofQuestion, 1, WinningRound - 1);
            FireproofText = "Несгораемая сумма: " + PrizeLadder.Format(PrizeLadder.Sums[this.fireproofQuestion - 1]);

            PrintRound = "1";
            InitializeRewards();
            InitializeAnswers();
            InirialieAudience();
            FieldAnswers();

            SubmitCommand = new RelayCommand(OnSubmit, _ => !isAnswering);
            ConfirmCommand = new RelayCommand(_ => ConfirmSelected(), _ => ConfirmVisibility == "Visible" && !isAnswering);
            CancelCommand = new RelayCommand(_ => ClearSelection());
            TakeMoneyCommand = new RelayCommand(_ => TakeMoney(), _ => CanTakeMoney);
            CloseAudienceVoteCommand = new RelayCommand(CloseAudienceWindow, _ => true);
            CloseFriendCallCommand = new RelayCommand(CloseFriendCall, _ => true);            

            FiftyFiftyHint = new FiftyFiftyHintView(OnFiftyFifty);
            ErrorRightHint = new ErrorRightHintView(OnErrorRight);
            AudienceHelpHint = new AudienceHelpHintView(OnAudienceHelp);
            FriendCallHint = new FriendCallHintView(OnFriendCallHelp);

            audienceBarAnimation = new AudienceBarAnimation(AudienceVotes);
            FiftyFiftyHint.FiftyFiftyActive = false;
        }

        private bool IsInteractionLocked => isAnswering || ConfirmVisibility == "Visible";

        public bool CanTakeMoney => !isAnswering
            && !ErrorRightHint.ErrorRightActive
            && ConfirmVisibility != "Visible"
            && int.Parse(mod.CurrentRound) > 1;

        public void StartGame()
        {
            StartRound(1);
        }

        public void StartRound(int round)
        {
            if (round == 1)
            {
                _audio.PlayMusic("100-1000 Questions.mp3");
            }
            else if (round == 6)
            {
                _audio.PlayMusic("2000-100000 Questons.mp3");
            }
            else if (round == 12)
            {
                _audio.PlayMusic("2000-100000 Questons.mp3");
            }
            else if (round == 14)
            {
                _audio.PlayMusic("Let's Play for One Million!.mp3");
            }
            else if (round == 15)
            {
                _audio.PlayMusic("Last Night on Super Millionaire (Bonus Track).mp3");
            }
        }

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
        public string ConfirmVisibility
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(ConfirmVisibility));
            }
        } = "Hidden";
        public string ConfirmText
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(ConfirmText));
            }
        } = "";
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
                Answers[i].IsEnabled = true;
                Answers[i].Text = answers[i];
            }
            ResetVisibility();
            OnPropertyChanged(nameof(QuestionText));
        }

        private async void OnErrorRight(object parameter)
        {
            if (!ErrorRightHint.IsEnabled || ErrorRightHint.ErrorRightActive || IsInteractionLocked)
                return;

            ErrorRightHint.ErrorRightActive = true;
            OnPropertyChanged(nameof(CanTakeMoney));
            await animator.PlayHintAnimation(b => ErrorRightHint.ErrorRightBackground = b);
            ErrorRightHint.IsEnabled = false;
            ErrorRightHint.Opacity = "0.5";
        }

        private async void OnFiftyFifty(object parameter)
        {            
            if (!FiftyFiftyHint.IsEnabled || FiftyFiftyHint.FiftyFiftyActive || IsInteractionLocked || ErrorRightHint.ErrorRightActive)
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

        private void InitializeRewards()
        {
            List<RewardView> tempRewards = PrizeLadder.Sums.Select(reward => new RewardView { Reward = reward.ToString("N0")}).ToList();
            tempRewards[0].State = ERewardState.Current;
            tempRewards[fireproofQuestion - 1].IsFireproof = true;
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

        private async void OnFriendCallHelp(object parameter)
        {
            if (!FriendCallHint.IsEnabled || FriendCallHint.FriendCallHelpActive || IsInteractionLocked || ErrorRightHint.ErrorRightActive)
                return;

            FriendCallHint.FriendCallHelpActive = true;
            FriendCallHint.IsEnabled = false;            
            _audio.PlayEffect("Ring.wav");
            await animator.PlayHintAnimation(b => FriendCallHint.FriendCallBackground = b);
            FriendCallHint.Opacity = "0.5";
            FriendCallVisibility = "Visible";
            TextLineItems.Clear();
            await FriendCallAnimation.PlayAnimation(c => { Application.Current.Dispatcher.Invoke(() => { TextLineItems.Add(c); }); }, mod.GetReplics());
            await Task.Delay(5000);
            FriendCallVisibility = "Hidden";
        }

        private void CloseFriendCall(object parametr)
        {
            FriendCallVisibility = "Hidden";
        }

        private async void OnAudienceHelp(object parameter)
        {
            if (!AudienceHelpHint.IsEnabled || AudienceHelpHint.AudienceHelpActive || IsInteractionLocked || ErrorRightHint.ErrorRightActive)
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

        private void OnSubmit(object parameter)
        {
            if (isAnswering || string.IsNullOrEmpty(parameter?.ToString()))
                return;

            string answer = parameter.ToString()!;
            if (ConfirmVisibility == "Visible")
            {
                if (answer == selectedAnswer)
                {
                    ConfirmSelected();
                    return;
                }
                ClearSelection();
            }
            SelectAnswer(answer);
        }

        private void SelectAnswer(string answer)
        {
            selectedAnswer = answer;
            var item = Answers.First(a => a.Text == answer);
            item.Background = SelectedBrush;
            ConfirmText = $"Вы выбрали «{item.Letter}: {answer}».";
            ConfirmVisibility = "Visible";
            _audio.PlayEffect("Select.wav");
            OnPropertyChanged(nameof(CanTakeMoney));
        }

        private void ClearSelection()
        {
            if (selectedAnswer.Length > 0)
            {
                var item = Answers.FirstOrDefault(a => a.Text == selectedAnswer);
                if (item != null)
                    item.Background = null;
            }
            selectedAnswer = "";
            ConfirmVisibility = "Hidden";
            OnPropertyChanged(nameof(CanTakeMoney));
        }

        private void ConfirmSelected()
        {
            if (ConfirmVisibility != "Visible" || selectedAnswer.Length == 0)
                return;

            _ = ConfirmAnswer(selectedAnswer);
        }

        private async Task ConfirmAnswer(string answer)
        {
            isAnswering = true;
            ConfirmVisibility = "Hidden";
            OnPropertyChanged(nameof(CanTakeMoney));
            _audio.PlayEffect("Confirm.wav");
            await Task.Delay(400);

            string round = mod.CurrentRound;
            bool isCorrect = mod.CheckAnswer(answer);
            bool hasSecondChance = !isCorrect && ErrorRightHint.ErrorRightActive;
            var clicked = Answers.First(a => a.Text == answer);

            await animator.PlayButtonAnimation(b => clicked.Background = b, isCorrect);

            if (isCorrect)
            {
                int answeredRound = int.Parse(round);
                _audio.PlayEffect("Correct.wav");
                Winnings = PrizeLadder.Sums[answeredRound - 1];
                ErrorRightHint.ErrorRightActive = false;
                Rewards[answeredRound - 1].State = ERewardState.Completed;

                if (answeredRound == WinningRound)
                {
                    foreach (var reward in Rewards)
                    {
                        reward.State = ERewardState.Completed;
                    }
                    await Task.Delay(1500);
                    OnStateChanged?.Invoke(EGameState.Win);
                    return;
                }

                Rewards[answeredRound].State = ERewardState.Current;
                PrintRound = mod.CurrentRound;
                await Task.Delay(900);

                ClearAnswers();
                FieldAnswers();
                StartRound(int.Parse(mod.CurrentRound));
                isAnswering = false;
                OnPropertyChanged(nameof(CanTakeMoney));
                return;
            }

            _audio.PlayEffect("Wrong.wav");

            if (hasSecondChance)
            {
                ErrorRightHint.ErrorRightActive = false;
                clicked.IsEnabled = false;
                selectedAnswer = "";
                isAnswering = false;
                OnPropertyChanged(nameof(CanTakeMoney));
                return;
            }

            LastCorrectAnswer = mod.RightAnswer;
            int answeredWrongRound = int.Parse(round);
            Winnings = answeredWrongRound > fireproofQuestion ? PrizeLadder.Sums[fireproofQuestion - 1] : 0;
            var rightAnswer = Answers.FirstOrDefault(a => a.Text == mod.RightAnswer);
            if (rightAnswer != null)
                rightAnswer.Background = new SolidColorBrush(Color.FromRgb(18, 200, 0));

            await Task.Delay(1800);
            OnStateChanged?.Invoke(EGameState.Lose);
        }

        private void ClearAnswers()
        {
            foreach (var item in Answers)
            {
                item.Background = null;
                item.IsEnabled = true;
            }
            selectedAnswer = "";
            ConfirmVisibility = "Hidden";
        }

        private void TakeMoney()
        {
            if (!CanTakeMoney)
                return;

            TookMoney = true;
            Winnings = PrizeLadder.Sums[int.Parse(mod.CurrentRound) - 2];
            OnStateChanged?.Invoke(EGameState.Win);
        }

        public ObservableCollection<RewardView> Rewards { get; set; } = new ObservableCollection<RewardView>();
        public ICommand SubmitCommand { get; }
        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand TakeMoneyCommand { get; }
        public ICommand CloseAudienceVoteCommand { get; }
        public ICommand CloseFriendCallCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<AnswerItem> Answers { get; set; } = new ObservableCollection<AnswerItem>();
        public ObservableCollection<AudienceVoteItem> AudienceVotes { get; set; } = new ObservableCollection<AudienceVoteItem>();
        public ObservableCollection<ItemTemplate> TextLineItems { get; set; } = new ObservableCollection<ItemTemplate>();
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public void Dispose()
        {
            _audio.StopAll();
        }
    }
}
