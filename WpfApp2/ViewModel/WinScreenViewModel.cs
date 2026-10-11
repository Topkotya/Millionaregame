using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WpfApp2
{
    public class WinScreenViewModel
    {
        public Action OnRestart;
        public ICommand RestartCommand { get; }
        public WinScreenViewModel() 
        {
            RestartCommand = new RelayCommand(Restart);
        }
        private void Restart(object parameter)
        {
            OnRestart.Invoke();
        }
    }
}
