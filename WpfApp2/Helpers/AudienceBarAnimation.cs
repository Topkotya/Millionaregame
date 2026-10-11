using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Media;

namespace WpfApp2
{
    public class AudienceBarAnimation
    {        
        Random random = new Random();
        ObservableCollection<AudienceVoteItem> bars;
        public AudienceBarAnimation(ObservableCollection<AudienceVoteItem> AudienceVotes)
        {
            bars = AudienceVotes;
        }
        private int Lerp(int start, int end, double t)
        {
            int value = (int)(start + (end - start) * t);
            return value;
        }
        private Color LerpColor(Color start, Color end, double t)
        {
            byte a = (byte)(start.A + (end.A - start.A) * t);
            byte r = (byte)(start.R + (end.R - start.R) * t);
            byte g = (byte)(start.G + (end.G - start.G) * t);
            byte b = (byte)(start.B + (end.B - start.B) * t);
            return Color.FromArgb(a, r, g, b);
        }
        public async Task PlayAnimation(int[] barPercentage)
        {
            double t = 0;
            int maxPercentage = barPercentage.Max();            
            Color loseColor = Color.FromRgb(100, 10, 1);
            Color winColor = Color.FromRgb(250, 176, 5);

            while (t < 1)
            {                
                bars[0].BarColor = new SolidColorBrush(LerpColor(loseColor, winColor, t * barPercentage[0] / maxPercentage));
                bars[1].BarColor = new SolidColorBrush(LerpColor(loseColor, winColor, t * barPercentage[1] / maxPercentage));
                bars[2].BarColor = new SolidColorBrush(LerpColor(loseColor, winColor, t * barPercentage[2] / maxPercentage));
                bars[3].BarColor = new SolidColorBrush(LerpColor(loseColor, winColor, t * barPercentage[3] / maxPercentage));

                bars[0].BarHeight = Lerp(0, barPercentage[0], t);
                bars[1].BarHeight = Lerp(0, barPercentage[1], t);
                bars[2].BarHeight = Lerp(0, barPercentage[2], t);
                bars[3].BarHeight = Lerp(0, barPercentage[3], t);

                bars[0].Percent = Lerp(0, barPercentage[0], t);
                bars[1].Percent = Lerp(0, barPercentage[1], t);
                bars[2].Percent = Lerp(0, barPercentage[2], t);
                bars[3].Percent = Lerp(0, barPercentage[3], t);
                t += 0.01;                
                await Task.Delay(20);
            }      
        }
    }
}