using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace WpfApp2
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _statusText = "Нажмите кнопку";
        public string StatusText
        {
            get => _statusText;
            set
            {
                _statusText = value;
                OnPropertyChanged(nameof(StatusText));
            }
        }
        public ICommand SubmitCommand { get; }

        public MainViewModel()
        {
            // Передаем метод выполнения и (опционально) условие активности кнопки
            SubmitCommand = new RelayCommand(OnSubmit, CanSubmit);
        }

        // Логика, которая выполнится при нажатии
        private void OnSubmit(object parameter)
        {
            StatusText = $"Кнопка нажата! Параметр: {parameter ?? "нет"}";
        }

        // Логика проверки: должна ли кнопка быть активна (IsEnable)
        private bool CanSubmit(object parameter)
        {
            return true; // Верните false, чтобы временно отключить кнопку
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
