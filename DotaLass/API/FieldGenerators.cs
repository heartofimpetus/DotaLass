using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DotaLass.API
{
    public static class FieldGenerators
    {
        private const double FieldWidth = 250;

        public static Border GenerateSoloMMRField(Window window, PlayerDisplay[] PlayerDisplays, int index)
        {
            Binding binding = new Binding($"{nameof(PlayerDisplays)}[{ index }].{nameof(PlayerDisplay.SoloMMR)}");
            Label label = new Label() { Width = FieldWidth, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };
            label.SetBinding(Label.ContentProperty, binding);

            return BorderControl(window, label);
        }

        public static Border GenerateEstimateMMRField(Window window, PlayerDisplay[] PlayerDisplays, int index)
        {
            Binding binding = new Binding($"{nameof(PlayerDisplays)}[{ index }].{nameof(PlayerDisplay.EstimateMMR)}");
            Label label = new Label() { Width = FieldWidth, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };
            label.SetBinding(Label.ContentProperty, binding);

            return BorderControl(window, label);
        }

        private static Border BorderControl(Window window, Control control)
        {
            Border border = new Border();
            border.Child = control;
            border.Style = (Style)window.FindResource("BorderStyle");
            return border;
        }

    }
}
