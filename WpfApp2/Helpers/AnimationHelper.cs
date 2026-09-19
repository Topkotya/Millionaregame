using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace WpfApp2
{
    public class AnimationHelper
    {
        private Color LerpColor(Color start, Color end, double t)
        {
            byte a = (byte)(start.A + (end.A - start.A) * t);
            byte r = (byte)(start.R + (end.R - start.R) * t);
            byte g = (byte)(start.G + (end.G - start.G) * t);
            byte b = (byte)(start.B + (end.B - start.B) * t);
            return Color.FromArgb(a, r, g, b);
        }
        public async Task PlayHintAnimation(Action<Brush> brushCallBack)
        {
            double t = 0;

            Color blue = Color.FromRgb(11, 47, 120);
            Color gold = Color.FromRgb(250, 176, 5);
            Color darkGold = Color.FromRgb(190, 130, 0);

            // Синий -> золотой
            while (t < 1)
            {
                brushCallBack(new SolidColorBrush(LerpColor(blue, gold, t)));
                t += 0.04;
                await Task.Delay(15);
            }

            // Золотой -> тёмно-золотой
            t = 0;
            while (t < 1)
            {
                brushCallBack(new SolidColorBrush(LerpColor(gold, darkGold, t)));
                t += 0.04;
                await Task.Delay(25);
            }

            // Тёмно-золотой -> золотой
            while (t > 0)
            {
                brushCallBack(new SolidColorBrush(LerpColor(gold, darkGold, t)));
                t -= 0.04;
                await Task.Delay(20);
            }

            // Золотой -> синий
            t = 0;
            while (t < 1)
            {
                brushCallBack(new SolidColorBrush(LerpColor(gold, blue, t)));
                t += 0.04;
                await Task.Delay(15);
            }

            brushCallBack(new SolidColorBrush(blue));
        }
        public async Task PlayButtonAnimation(Action<Brush> brushCallBack, bool isCorrect)
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

    }
}
