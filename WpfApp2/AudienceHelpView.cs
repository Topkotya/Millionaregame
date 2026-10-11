using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WpfApp2
{
    public class AudienceHelpView : HintView
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
        public AudienceHelpView(Action<object> onCommand)
        {
            ErrorRightCommand = new RelayCommand(onCommand);
        }
    }
}
