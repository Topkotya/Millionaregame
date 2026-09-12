using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WpfApp2
{
    public class RewardView : INotifyPropertyChanged
    {
        public string Reward
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Reward));
            }
        }
        public ERewardState State
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(State));
            }
        } = ERewardState.locked;
        public event PropertyChangedEventHandler? PropertyChanged;        
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}