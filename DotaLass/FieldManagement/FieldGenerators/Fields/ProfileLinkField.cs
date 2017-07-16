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

namespace DotaLass.FieldManagement.FieldGenerators.Fields
{
    public class ProfileLinkField : FieldBase
    {
        public ProfileLinkField(Window window) : base(window, nameof(PlayerDisplay.DisplayData.ID), "Profile", double.NaN)
        {
        }

        protected override UIElement CreateFieldElement(PlayerDisplay playerDisplay)
        {
            FontAwesome.WPF.ImageAwesome image = new FontAwesome.WPF.ImageAwesome()
            {
                Width = 16,
                Height = 16,
                Icon = FontAwesome.WPF.FontAwesomeIcon.ExternalLink
            };

            image.MouseDown += (o, a) =>
            {
                playerDisplay.OpenProfile();
            };

            return image;
        }
    }
}
