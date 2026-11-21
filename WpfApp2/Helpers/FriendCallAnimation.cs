using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Media;

namespace WpfApp2.Helpers
{
    public static class FriendCallAnimation
    {
        public static async Task PlayAnimation(Action<ItemTemplate> callback, List<ItemTemplate> text)
        {
            Random rnd = new Random();
            foreach (var item in text) 
            {
                await Task.Delay(rnd.Next(1400, 3600));
                var fullText = item.Text;
                callback.Invoke(item);
                item.Text = "";
                for (int i = 0; i < fullText.Count(); i++)
                {
                    item.Text += fullText[i];
                    await Task.Delay(item.TypeSpeed * 3);
                }
            }            
        }
    }
}
