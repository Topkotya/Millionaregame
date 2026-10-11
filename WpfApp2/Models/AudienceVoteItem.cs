using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using System.Text;

namespace WpfApp2
{
    public class AudienceVoteItem : INotifyPropertyChanged
    {
        public string Letter
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Letter));
            }
        }
        public int Percent
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Percent));
            }
        }
        public int BarHeight
        {
            get => field;
            set
            {
                field = value * 2;
                OnPropertyChanged(nameof(BarHeight));
            }
        }
        public Brush BarColor
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(BarColor));
            }
        }
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
