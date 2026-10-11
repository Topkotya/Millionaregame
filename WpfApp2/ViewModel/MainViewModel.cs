using System.ComponentModel;
namespace WpfApp2
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public GameplayViewModel gameplayViewModel { get; set; }
        public WinScreenViewModel winScreenViewModel { get; set; }
        public LoseScreenViewModel loseScreenViewModel { get; set; }        
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
        } = EGameState.Playing;     
        private void ChangeState(EGameState eGameState)
        {
            GameResult = eGameState;
        }
        private void RestartGame()
        {            
            gameplayViewModel?.OnStateChanged -= ChangeState;
            gameplayViewModel = new GameplayViewModel();
            gameplayViewModel?.OnStateChanged += ChangeState;
            GameResult = EGameState.Playing;
        }
        public MainViewModel()
        {
            gameplayViewModel = new GameplayViewModel();
            winScreenViewModel = new WinScreenViewModel();
            loseScreenViewModel = new LoseScreenViewModel();
            gameplayViewModel.OnStateChanged += ChangeState;
            winScreenViewModel.OnRestart += RestartGame;
            loseScreenViewModel.OnRestart += RestartGame;
        }     
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}