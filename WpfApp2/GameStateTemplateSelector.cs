using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2
{
    public class GameStateTemplateSelector : DataTemplateSelector
    {
        public DataTemplate GameplayTemplate { get; set; }
        public DataTemplate WinTemplate { get; set; }
        public DataTemplate LoseTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is EGameState state)
            {
                return state switch
                {
                    EGameState.Win => WinTemplate,
                    EGameState.Lose => LoseTemplate,
                    _ => GameplayTemplate
                };
            }
            return GameplayTemplate;
        }
    }
}
