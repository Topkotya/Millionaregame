using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WpfApp2
{
    public class HintView : INotifyPropertyChanged
    {
        public string Opacity
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Opacity));
            }
        } = "1";        
        public bool IsEnabled
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        } = true;
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}