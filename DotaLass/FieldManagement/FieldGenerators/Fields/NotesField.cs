using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DotaLass.API;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Windows.Media;
using DotaLass.Windows;

namespace DotaLass.FieldManagement.FieldGenerators.Fields
{
    public class NotesField : FieldBase
    {
        public NotesField(Window window, string fieldName) : base(window, nameof(PlayerDisplay.DisplayData.ID), fieldName, double.NaN)
        {
        }

        protected override UIElement CreateFieldElement(PlayerDisplay playerDisplay)
        {
            FontAwesome.WPF.ImageAwesome image = new FontAwesome.WPF.ImageAwesome()
            {
                Width = 16,
                Height = 16,
                Icon = FontAwesome.WPF.FontAwesomeIcon.StickyNoteOutline
            };

            playerDisplay.Data.PropertyChanged += (o, a) =>
            {
                if (a.PropertyName == Path)
                {
                    image.Dispatcher.Invoke(() =>
                    {
                        image.Foreground = new SolidColorBrush(PlayerNotes.Instance[playerDisplay.Data.ID].Colour);
                    });
                }
            };

            image.MouseDown += (o, a) =>
            {
                if (!string.IsNullOrEmpty(playerDisplay.Data.ID))
                {
                    NotesWindow window = new NotesWindow(playerDisplay.Data.ID, image);
                    window.ShowDialog();
                }
            };

            return image;
        }
    }
}
