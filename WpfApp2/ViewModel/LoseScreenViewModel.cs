using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WpfApp2
{
    public class LoseScreenViewModel
    {
        public Action OnRestart;
        public ICommand RestartCommand { get; }
        public LoseScreenViewModel()
        {
            RestartCommand = new RelayCommand(Restart);
        }
        private void Restart(object parameter)
        {
            OnRestart.Invoke();
        }
    }
}
