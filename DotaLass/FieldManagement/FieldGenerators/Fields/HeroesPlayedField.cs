using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DotaLass.API;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DotaLass.FieldManagement.FieldGenerators.Fields
{
    public class HeroesPlayedField : FieldBase
    {
        public HeroesPlayedField(Window window, string fieldName) : base(window, nameof(PlayerDisplay.DisplayData.HeroesPlayed), fieldName, double.NaN)
        {
        }

        protected override UIElement CreateFieldElement(PlayerDisplay playerDisplay)
        {
            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal };

            for (int i = 0; i < 20; i++)
            {
                int index = i;

                Image image = new Image() { Source = HeroIcons.BlankHeroIcon, Margin = new Thickness(1) };

                playerDisplay.Data.PropertyChanged += (o, a) =>
                {
                    if (a.PropertyName == Path)
                    {
                        Window.Dispatcher.Invoke(() =>
                        {
                            if (playerDisplay.Data.HeroesPlayed != null)
                            {
                                if (index < playerDisplay.Data.HeroesPlayed.Count)
                                {
                                    if (playerDisplay.Data.HeroesPlayed[index].Item2)
                                        image.Source = HeroIcons.HeroWinIcons[playerDisplay.Data.HeroesPlayed[index].Item1 - 1];
                                    else
                                        image.Source = HeroIcons.HeroLossIcons[playerDisplay.Data.HeroesPlayed[index].Item1 - 1];
                                }
                            }
                        });
                    }
                };

                panel.Children.Add(image);
            }

            return panel;
        }
    }
}
