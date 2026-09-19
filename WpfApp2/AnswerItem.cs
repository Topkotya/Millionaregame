using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Media;

namespace WpfApp2
{
    public class AnswerItem : INotifyPropertyChanged
    {
        public string Text
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Text));
            }
        }
        public string Letter
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Letter));
            }
        }
        public string Visability
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Visability));
            }
        } = "Visible";
        public Brush Background
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Background));
            }
        }        
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
