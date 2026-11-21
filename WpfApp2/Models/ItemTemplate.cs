using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;
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
                field = value;
                OnPropertyChanged(nameof(TypeSpeed));
            }
        }
        public Color ConvertedColor
        {
            get => (Color)ColorConverter.ConvertFromString(TextColor);
        }
        public string TextColor { get; set; }
        [JsonIgnore]
        public Brush Brush
        {
            get => new SolidColorBrush(ConvertedColor);
        }
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
