using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Media;

namespace WpfApp2.Helpers
{
    public static class FriendCallAnimation
    {
        public static async Task PlayAnimation(Action<List<ItemTemplate>> callback, List<ItemTemplate> text)
        {
            List<ItemTemplate> currentReplics = new List<ItemTemplate>();
            currentReplics.Add(text[0]);
            currentReplics[0].Text = "";
            callback.Invoke(currentReplics);            
            for (int i = 0; i < text[0].Text.Count() - 1; i++) 
            {
                currentReplics[0].Text += text[0].Text[i];
                await Task.Delay(currentReplics[0].TypeSpeed);
            }
            currentReplics.Add(text[1]);
            callback.Invoke(currentReplics);
        }
    }
}
