using System;
using System.ComponentModel;
using System.Windows.Input;

namespace WpfApp2
{
    public class LoseScreenViewModel : INotifyPropertyChanged
    {
        public Action OnRestart;
        public ICommand RestartCommand { get; }
        public string Message
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Message));
            }
        } = "";
        public string Money
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Money));
            }
        } = "";

        public LoseScreenViewModel()
        {
            RestartCommand = new RelayCommand(Restart);
        }

        public void Update(string correctAnswer, int winnings)
        {
            Message = correctAnswer.Length > 0 ? $"Правильный ответ: «{correctAnswer}»" : "Правильный ответ был другим...";
            Money = winnings > 0 ? "Вы уходите с " + PrizeLadder.Format(winnings) : "Вы уходите ни с чем";
        }

        private void Restart(object parameter)
        {
            OnRestart.Invoke();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
