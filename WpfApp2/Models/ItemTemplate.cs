using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;

namespace WpfApp2
{
    public class ItemTemplate : INotifyPropertyChanged
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
        public int TypeSpeed
        {
            get => field;
            set
            {
                field = value * 2;
                OnPropertyChanged(nameof(TypeSpeed));
            }
        }
        public Brush TextColor
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(TextColor));
            }
        }
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
