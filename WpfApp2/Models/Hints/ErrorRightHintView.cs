using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApp2
{
    public class ErrorRightHintView : HintView
    {
        public bool ErrorRightActive
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(ErrorRightActive));
            }
        }
        public ICommand ErrorRightCommand { get; }
        public Brush ErrorRightBackground
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(ErrorRightBackground));
            }
        }
        public ErrorRightHintView(Action<object> onCommand)
        {
            ErrorRightCommand = new RelayCommand(onCommand);
        }
    }
}
