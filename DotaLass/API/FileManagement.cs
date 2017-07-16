using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotaLass.API
{
    public static class FileManagement
    {
        private static string _ServerLog;
        public static string ServerLog
        {
            get
            {
                if (_ServerLog == null)
                {
                    foreach (var path in SteamAppDirectories)
                    {
                        if (File.Exists(path + "\\common\\dota 2 beta\\game\\dota\\server_log.txt"))
                        {
                            _ServerLog = path + "\\common\\dota 2 beta\\game\\dota\\server_log.txt";
                            break;
                        }
                    }
                    if (_ServerLog == null)
                        throw new Exception("ServerLog not found");
                }

                return _ServerLog;
            }
        }

        private static string _SteamInstallPath;
        private static string SteamInstallPath
        {
            get
            {
                if (_SteamInstallPath == null)
                    _SteamInstallPath = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Valve\\Steam", "InstallPath", "") as string;

                return _SteamInstallPath;
            }
        }

        private static List<string> SteamAppDirectories
        {
            get
            {
                List<string> steamAppDirectories = new List<string>() { SteamInstallPath + "\\steamapps" };

                string[] lines = File.ReadAllLines(SteamInstallPath + "\\steamapps\\libraryfolders.vdf");

                for (int i = 4; i < lines.Length - 1; i++)
                {
                    int index = lines[i].IndexOfNth("\"", 3);
                    steamAppDirectories.Add(lines[i].Substring(index + 1, lines[i].Length - (index + 2)) + "\\steamapps");
                }

                return steamAppDirectories;
            }
        }

        private static int IndexOfNth(this string str, string value, int nth = 1)
        {
            if (nth <= 0)
                throw new ArgumentException("Can not find the zeroth index of substring in string. Must start with 1");
            int offset = str.IndexOf(value);
            for (int i = 1; i < nth; i++)
            {
                if (offset == -1) return -1;
                offset = str.IndexOf(value, offset + 1);
            }
            return offset;
        }
    }
}
