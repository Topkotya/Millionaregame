using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WpfApp2
{
    public class IntroScreenViewModel
    {
        public Action<EGameState> OnIntroEndend;
        public ICommand StartGameCommand { get; }
        public IntroScreenViewModel()
        {
            StartGameCommand = new RelayCommand(Start);
        }
        private void Start(object parameter)
        {
            OnIntroEndend.Invoke(EGameState.Playing);
        }
    }
}
