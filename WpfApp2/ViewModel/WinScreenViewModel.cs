using System;
using System.ComponentModel;
using System.Windows.Input;

namespace WpfApp2
{
    public class WinScreenViewModel : INotifyPropertyChanged
    {
        public Action OnRestart;
        public ICommand RestartCommand { get; }
        public string Header
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Header));
            }
        } = "ПОЗДРАВЛЯЕМ!";
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

        public WinScreenViewModel() 
        {
            RestartCommand = new RelayCommand(Restart);
        }

        public void Update(int winnings, bool tookMoney)
        {
            Header = tookMoney ? "ВЫ ЗАБРАЛИ ДЕНЬГИ" : "ПОЗДРАВЛЯЕМ!";
            Message = tookMoney ? "Вы решили не рисковать и забрали выигрыш" : "Вы ответили на все 15 вопросов!";
            Money = PrizeLadder.Format(winnings);
        }

        private void Restart(object parameter)
        {
            OnRestart.Invoke();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
