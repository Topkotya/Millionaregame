using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApp2
{
    public class FiftyFiftyHintView : HintView
    {
        public Brush FiftyFiftyBackground
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(FiftyFiftyBackground));
            }
        }
        public bool FiftyFiftyActive
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(FiftyFiftyActive));
            }
        }
        public FiftyFiftyHintView(Action<object> onCommand)
        {
            FiftyFiftyCommand = new RelayCommand(onCommand);
        }
        public ICommand FiftyFiftyCommand { get; }
    }
}