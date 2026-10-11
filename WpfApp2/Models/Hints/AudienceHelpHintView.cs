using System;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApp2
{
    public class AudienceHelpHintView : HintView
    {
        public Brush AudienceHelpBackground
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(AudienceHelpBackground));
            }
        }
        public bool AudienceHelpActive
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(AudienceHelpActive));
            }
        }
        public AudienceHelpHintView(Action<object> onCommand)
        {
            AudienceHelpCommand = new RelayCommand(onCommand);
        }
        public ICommand AudienceHelpCommand { get; }
    }
}