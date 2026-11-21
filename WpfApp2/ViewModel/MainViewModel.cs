using System.ComponentModel;
namespace WpfApp2
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public GameplayViewModel? gameplayViewModel { get; private set; }
        public WinScreenViewModel winScreenViewModel { get; set; }
        public LoseScreenViewModel loseScreenViewModel { get; set; }
        public IntroScreenViewModel introScreenViewModel { get; set; }
        public FireproofViewModel fireproofViewModel { get; set; }
        public EGameState GameResult
        {
            get => field;
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(GameResult));
                }
            }
        } = EGameState.Intro;
        private void ChangeState(EGameState eGameState)
        {
            if (eGameState == EGameState.Win && gameplayViewModel != null)
            {
                winScreenViewModel.Update(gameplayViewModel.Winnings, gameplayViewModel.TookMoney);
            }
            else if (eGameState == EGameState.Lose && gameplayViewModel != null)
            {
                loseScreenViewModel.Update(gameplayViewModel.LastCorrectAnswer, gameplayViewModel.Winnings);
            }

            GameResult = eGameState;

            if (eGameState == EGameState.Win || eGameState == EGameState.Lose)
            {
                gameplayViewModel?.Dispose();
            }
        }
        private void StartGame(int fireproofQuestion)
        {
            gameplayViewModel?.OnStateChanged -= ChangeState;
            gameplayViewModel?.Dispose();
            gameplayViewModel = new GameplayViewModel(fireproofQuestion);
            gameplayViewModel.OnStateChanged += ChangeState;
            GameResult = EGameState.Playing;
            gameplayViewModel.StartGame();
        }
        private void RestartGame()
        {
            gameplayViewModel?.OnStateChanged -= ChangeState;
            gameplayViewModel?.Dispose();
            gameplayViewModel = null;
            fireproofViewModel = new FireproofViewModel();
            fireproofViewModel.OnFireproofChosen += StartGame;
            GameResult = EGameState.ChooseFireproof;
        }
        public MainViewModel()
        {
            introScreenViewModel = new IntroScreenViewModel();
            winScreenViewModel = new WinScreenViewModel();
            loseScreenViewModel = new LoseScreenViewModel();
            fireproofViewModel = new FireproofViewModel();
            introScreenViewModel.OnIntroEndend += _ => GameResult = EGameState.ChooseFireproof;
            fireproofViewModel.OnFireproofChosen += StartGame;
            winScreenViewModel.OnRestart += RestartGame;
            loseScreenViewModel.OnRestart += RestartGame;
        }     
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
