using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace WpfApp2
{
    public class FireproofOption : INotifyPropertyChanged
    {
        public int QuestionNumber { get; init; }
        public string Reward { get; init; } = "";
        public bool IsSelected
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class FireproofViewModel
    {
        private FireproofOption? selected;

        public Action<int> OnFireproofChosen;
        public ObservableCollection<FireproofOption> Options { get; }
        public ICommand SelectCommand { get; }
        public ICommand StartCommand { get; }

        public FireproofViewModel()
        {
            Options = new ObservableCollection<FireproofOption>(
                PrizeLadder.Sums.SkipLast(1).Select((reward, index) => new FireproofOption
                {
                    QuestionNumber = index + 1,
                    Reward = reward.ToString("N0")
                }));

            SelectCommand = new RelayCommand(Select, item => item is FireproofOption);
            StartCommand = new RelayCommand(_ => Start(), _ => selected != null);
        }

        private void Select(object parameter)
        {
            if (parameter is not FireproofOption option)
                return;

            foreach (var item in Options)
            {
                item.IsSelected = item == option;
            }
            selected = option;
        }

        private void Start()
        {
            if (selected == null)
                return;

            OnFireproofChosen?.Invoke(selected.QuestionNumber);
        }
    }
}
