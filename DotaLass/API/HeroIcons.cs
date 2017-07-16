using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DotaLass.API
{
    public static class HeroIcons
    {
        private const int HeroIconCount = 114;

        private static BitmapImage[] _HeroWinIcons;
        public static BitmapImage[] HeroWinIcons
        {
            get
            {
                Uri[] imageUris = new Uri[HeroIconCount];

                for (int i = 0; i < imageUris.Length; i++)
                {
                    string iconName = (i + 1).ToString().PadLeft(3, '0');
                    imageUris[i] = new Uri($"/Resources/Images/HeroIcons/Win/{iconName}.png", UriKind.Relative);
                }

                _HeroWinIcons = imageUris.Select(x => new BitmapImage(x)).ToArray();

                return _HeroWinIcons;
            }
        }

        private static BitmapImage[] _HeroLossIcons;
        public static BitmapImage[] HeroLossIcons
        {
            get
            {
                if (_HeroLossIcons == null)
                {
                    Uri[] imageUris = new Uri[HeroIconCount];

                    for (int i = 0; i < imageUris.Length; i++)
                    {
                        string iconName = (i + 1).ToString().PadLeft(3, '0');
                        imageUris[i] = new Uri($"/Resources/Images/HeroIcons/Loss/{iconName}.png", UriKind.Relative);
                    }

                    _HeroLossIcons = imageUris.Select(x => new BitmapImage(x)).ToArray();
                }

                return _HeroLossIcons;
            }
        }

        private static BitmapImage _BlankHeroIcon;
        public static BitmapImage BlankHeroIcon
        {
            get
            {
                if (_BlankHeroIcon == null)
                {
                    var uriSource = new Uri(@"/Resources/Images/HeroIcons/Empty.png", UriKind.Relative);
                    _BlankHeroIcon = new BitmapImage(uriSource);
                }

                return _BlankHeroIcon;
            }
        }
    }
}
