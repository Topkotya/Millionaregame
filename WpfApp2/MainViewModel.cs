using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace WpfApp2
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private MainModel mod = new MainModel();
        private const int WinningRound = 11;
        public string PrintRound
        {
            get => field;
            set
            {
                field = value + " Вопрос";
                OnPropertyChanged(nameof(PrintRound));
            }
        }

        public string Answer1
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Answer1));
            }
        }

        public string Answer2
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Answer2));
            }
        }

        public string Answer3
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Answer3));
            }
        }

        public string Answer4
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Answer4));
            }
        }

        public string QuestionText
        {
            get => mod.GetQuestion();
            set
            {
                field = value;
                OnPropertyChanged(nameof(QuestionText));
            }
        }

        public ICommand SubmitCommand { get; }

        private void FieldAnswers()
        {
            var answers = mod.GetAnswers();
            Answer1 = answers[0];
            Answer2 = answers[1];
            Answer3 = answers[2];
            Answer4 = answers[3];
            OnPropertyChanged(nameof(QuestionText));
        }

        public MainViewModel()
        {
            PrintRound = "1";

            FieldAnswers();

            SubmitCommand = new RelayCommand(OnSubmit, CanSubmit);
        }

        private void OnSubmit(object parameter)
        {
            string answer = parameter.ToString()!;

            if (mod.GetAnswer(answer))
            {
                if (int.Parse(mod.CurrentRound) == WinningRound)
                {
                    MessageBox.Show("Вы стали миллионером! Вы победили!", "Игра окончена", MessageBoxButton.OK, MessageBoxImage.Information);
                    Application.Current.Shutdown();
                    return;
                }
                PrintRound = mod.CurrentRound;
                FieldAnswers();
            }
            else
            {
                MessageBox.Show("Вы проиграли", "Игра окончена", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        private bool CanSubmit(object parameter)
        {
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}