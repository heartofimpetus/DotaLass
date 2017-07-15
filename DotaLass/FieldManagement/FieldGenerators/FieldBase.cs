using DotaLass.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DotaLass.FieldManagement.FieldGenerators
{
    public abstract class FieldBase
    {
        protected Window Window { get; }
        public string Path { get; }
        public string Name { get; }
        public double Width { get; }

        public FieldBase(Window window, string fieldPath, string fieldName, double fieldWidth)
        {
            Window = window;
            Path = fieldPath;
            Name = fieldName;
            Width = fieldWidth;
        }

        public FrameworkElement GenerateHeader()
        {
            Label fieldHeader = new Label()
            {
                Content = Name,
                Width = Width,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Style = (Style)Window.FindResource("HeaderStyle")
            };

            return BorderControl(fieldHeader);
        }
        public abstract UIElement GenerateField(PlayerDisplay playerDisplay);

        protected Border BorderControl(FrameworkElement control)
        {
            Border border = new Border()
            {
                Child = control,
                Style = (Style)Window.FindResource("BorderStyle")
            };

            return border;
        }
    }
}
