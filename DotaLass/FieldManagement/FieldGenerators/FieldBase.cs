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

        protected abstract UIElement CreateFieldElement(PlayerDisplay playerDisplay);

        public UIElement CreateField(PlayerDisplay playerDisplay)
        {
            UIElement fieldElement = CreateFieldElement(playerDisplay);
            Border innerBorder = new Border() { Child = fieldElement };

            innerBorder.Visibility = Visibility.Hidden;

            playerDisplay.RetrievalStarted += (o, a) =>
            {
                innerBorder.Dispatcher.Invoke(() =>
                {
                    innerBorder.Visibility = Visibility.Hidden;
                });
            };

            playerDisplay.Data.PropertyChanged += (o, a) =>
            {
                if (a.PropertyName == Path)
                {
                    innerBorder.Dispatcher.Invoke(() =>
                    {
                        innerBorder.Visibility = Visibility.Visible;
                    });
                }
            };

            return BorderControl(innerBorder);
        }

        private Border BorderControl(UIElement control)
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
