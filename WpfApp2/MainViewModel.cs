using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp2
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private MainModel mod = new MainModel();
        private const int WinningRound = 11;
        private bool isAnswering;
        public bool WinScreenState
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(WinScreenState));
            }
        }
        public bool LoseScreenState
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(LoseScreenState));
            }
        }
        public Brush Answer1Background
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Answer1Background));
            }
        }        
        public Brush Answer2Background
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Answer2Background));
            }
        }

        public Brush Answer3Background
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Answer3Background));
            }
        }

        public Brush Answer4Background
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Answer4Background));
            }
        }
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
        private Color LerpColor(Color start, Color end, double t)
        {
            byte a = (byte)(start.A + (end.A - start.A) * t);
            byte r = (byte)(start.R + (end.R - start.R) * t);
            byte g = (byte)(start.G + (end.G - start.G) * t);
            byte b = (byte)(start.B + (end.B - start.B) * t);            
            return Color.FromArgb(a, r, g, b);
        }
        private async Task PlayButtonAnimation(Action<Brush> brushCallBack, bool isCorrect)
        {
            double t = 0;
            if (isCorrect) //анимация верного ответа
            {
                Color start = Color.FromRgb(11, 47, 120); //переход из тёмно синего в светло зелёный
                Color end = Color.FromRgb(18, 200, 0);
                while (t < 1) 
                {                    
                    brushCallBack(new SolidColorBrush(LerpColor(start, end, t)));
                    t += 0.03;
                    await Task.Delay(15);
                }

                t = 0;
                start = end;
                end = Color.FromRgb(0, 140, 9); //моргание оттенков зелёного
                while (t < 1)
                {
                    brushCallBack(new SolidColorBrush(LerpColor(start, end, t)));
                    t += 0.03;
                    await Task.Delay(27);
                }
                while (t > 0)
                {
                    brushCallBack(new SolidColorBrush(LerpColor(start, end, t)));
                    t -= 0.03;
                    await Task.Delay(17);
                }
                while (t < 1)
                {
                    brushCallBack(new SolidColorBrush(LerpColor(start, end, t)));
                    t += 0.03;
                    await Task.Delay(22);
                }
                while (t > 0)
                {
                    brushCallBack(new SolidColorBrush(LerpColor(start, end, t)));
                    t -= 0.03;
                    await Task.Delay(12);
                }
            }

            else
            {
                Color start = Color.FromRgb(11, 47, 120); 
                Color end = Color.FromRgb(255, 0, 0);
                while (t < 1)
                {
                    brushCallBack(new SolidColorBrush(LerpColor(start, end, t)));
                    t += 0.03;
                    await Task.Delay(15);
                }

                t = 0;
                start = end;
                end = Color.FromRgb(140, 0, 0); 
                while (t < 1)
                {
                    brushCallBack(new SolidColorBrush(LerpColor(start, end, t)));
                    t += 0.03;
                    await Task.Delay(27);
                }
                while (t > 0)
                {
                    brushCallBack(new SolidColorBrush(LerpColor(start, end, t)));
                    t -= 0.03;
                    await Task.Delay(17);
                }
                while (t < 1)
                {
                    brushCallBack(new SolidColorBrush(LerpColor(start, end, t)));
                    t += 0.03;
                    await Task.Delay(22);
                }
                while (t > 0)
                {
                    brushCallBack(new SolidColorBrush(LerpColor(start, end, t)));
                    t -= 0.03;
                    await Task.Delay(12);
                }
            }
        }
        private async Task SetAnswerColor(string answer, bool isCorrect)
        {            
            //Brush color = isCorrect ? Brushes.Green : Brushes.Red;
            if (answer == Answer1) await PlayButtonAnimation(b => Answer1Background = b, isCorrect);
            else if (answer == Answer2) await PlayButtonAnimation(b => Answer2Background = b, isCorrect);
            else if (answer == Answer3) await PlayButtonAnimation(b => Answer3Background = b, isCorrect);
            else if (answer == Answer4) await PlayButtonAnimation(b => Answer4Background = b, isCorrect);
        }
        private async void OnSubmit(object parameter)
        {
            if (isAnswering) return;
            isAnswering = true;
            string answer = parameter.ToString()!;
            bool isCorrect = mod.GetAnswer(answer);

            // Подсвечиваем выбранный ответ
            await SetAnswerColor(answer, isCorrect);

            if (isCorrect)
            {
                if (int.Parse(mod.CurrentRound) == WinningRound)
                {
                    WinScreenState = true;
                    //MessageBox.Show("Вы стали миллионером! Вы победили!","Игра окончена",MessageBoxButton.OK,MessageBoxImage.Information);                    
                    return;
                }
                PrintRound = mod.CurrentRound;
                // Возвращаем кнопкам исходный градиент
                Answer1Background = null;
                Answer2Background = null;
                Answer3Background = null;
                Answer4Background = null;
                FieldAnswers();
            }
            else
            {
                LoseScreenState = true;
                //MessageBox.Show("Вы проиграли", "Игра окончена", MessageBoxButton.OK, MessageBoxImage.Error);                
                return;
            }
            isAnswering = false;
        }
        private bool CanSubmit(object parameter)
        {
            return !isAnswering;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}