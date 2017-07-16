using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DotaLass.API;
using System.Windows.Data;
using System.Windows.Controls;

namespace DotaLass.FieldManagement.FieldGenerators.Fields
{
    public class StringField : FieldBase
    {
        public StringField(Window window, string fieldPath, string fieldName, double fieldWidth) : base(window, fieldPath, fieldName, fieldWidth)
        {
        }

        public override UIElement GenerateField(PlayerDisplay playerDisplay)
        {
            Binding binding = new Binding(Path);
            Label label = new Label() { Width = Width, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };

            label.DataContext = playerDisplay;
            label.SetBinding(Label.ContentProperty, binding);

            return BorderControl(label);
        }
    }
}
