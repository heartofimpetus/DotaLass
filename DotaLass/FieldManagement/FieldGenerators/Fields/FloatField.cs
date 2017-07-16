using DotaLass.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DotaLass.FieldManagement.FieldGenerators.Fields
{
    public class FloatField : FieldBase
    {
        private string StringFormat { get; }

        public FloatField(Window window, string fieldPath, string fieldName, double fieldWidth, string stringFormat = "0.00") : base(window, fieldPath, fieldName, fieldWidth)
        {
            StringFormat = stringFormat;
        }

        public override UIElement GenerateField(PlayerDisplay playerDisplay)
        {
            Binding binding = new Binding(Path);
            Label label = new Label() { Width = Width, HorizontalContentAlignment = HorizontalAlignment.Right, VerticalContentAlignment = VerticalAlignment.Center, ContentStringFormat = StringFormat };

            label.DataContext = playerDisplay;
            label.SetBinding(Label.ContentProperty, binding);

            return BorderControl(label);
        }
    }
}
