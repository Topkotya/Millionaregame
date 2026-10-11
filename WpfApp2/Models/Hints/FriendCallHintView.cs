using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Input;

namespace WpfApp2.Models.Hints
{
    public class FriendCallHintView : HintView
    {
        public Brush FriendCallBackground
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(FriendCallBackground));
            }
        }
        public bool FriendCallHelpActive
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(FriendCallHelpActive));
            }
        }
        public FriendCallHintView(Action<object> onCommand)
        {
            FriendCallHelpCommand = new RelayCommand(onCommand);
        }
        public ICommand FriendCallHelpCommand { get; }
    }
}