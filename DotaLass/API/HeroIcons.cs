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

        private static Dictionary<int, BitmapImage> _HeroWinIcons;
        private static Dictionary<int, BitmapImage> HeroWinIcons
        {
            get
            {
                if (_HeroWinIcons == null)
                {
                    _HeroWinIcons = new Dictionary<int, BitmapImage>();

                    var dir = Directory.GetCurrentDirectory() + "/Resources/Images/HeroIcons/Win/";

                    var files = Directory.GetFiles(dir, "*.png", SearchOption.AllDirectories);

                    foreach (var item in files)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(item);

                        int id;
                        if (int.TryParse(fileName, out id))
                        {
                            _HeroWinIcons[id] = new BitmapImage(new Uri(item));
                        }
                    }
                }

                return _HeroWinIcons;
            }
        }

        public static BitmapImage GetHeroWinIcon(int id)
        {
            BitmapImage output;
            if (HeroWinIcons.TryGetValue(id, out output))
            {
                return output;
            }
            else
            {
                return BlankHeroIcon;
            }
        }

        private static Dictionary<int, BitmapImage> _HeroLossIcons;
        private static Dictionary<int, BitmapImage> HeroLossIcons
        {
            get
            {
                if (_HeroLossIcons == null)
                {
                    _HeroLossIcons = new Dictionary<int, BitmapImage>();

                    var dir = Directory.GetCurrentDirectory() + "/Resources/Images/HeroIcons/Loss/";

                    var files = Directory.GetFiles(dir, "*.png", SearchOption.AllDirectories);

                    foreach (var item in files)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(item);

                        int id;
                        if (int.TryParse(fileName, out id))
                        {
                            _HeroLossIcons[id] = new BitmapImage(new Uri(item));
                        }
                    }
                }

                return _HeroLossIcons;
            }
        }

        public static BitmapImage GetHeroLossIcon(int id)
        {
            BitmapImage output;
            if (HeroLossIcons.TryGetValue(id, out output))
            {
                return output;
            }
            else
            {
                return BlankHeroIcon;
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
