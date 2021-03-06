﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotaLass.API
{
    public class Settings
    {
        public bool AutoRetrievePlayerData { get; set; }

        public LinkSite PreferredSite { get; set; }

        public DateLimit RecentMatchesDateLimit { get; set; }

        public bool RankedMatchesOnly { get; set; }

        public enum LinkSite
        {
            OpenDota,
            DotaBuff
        }

        public enum DateLimit
        {
            Days30,
            Days60,
            Days90,
            AllTime
        }

        [JsonIgnore]
        public string BaseLinkAddress
        {
            get
            {
                switch (PreferredSite)
                {
                    default:
                    case LinkSite.OpenDota: return "https://www.opendota.com";
                    case LinkSite.DotaBuff: return "https://www.dotabuff.com";
                }
            }
        }

        public int GetDaysLimit()
        {
            switch (RecentMatchesDateLimit)
            {
                case DateLimit.Days30: return 30;
                case DateLimit.Days60: return 60;
                case DateLimit.Days90: return 90;
                case DateLimit.AllTime:
                default: return -1;
            }
        }
        public int GetLobbyType()
        {
            if (RankedMatchesOnly)
                return 7;
            else
                return -1;
        }

        public List<Tuple<string, bool>> FieldSettings { get; set; }

        private static string SettingsPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + "/Settings.json";
            }
        }

        private static Settings _Instance;
        public static Settings Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = LoadSettings();
                }

                return _Instance;
            }
        }

        private static Settings LoadSettings()
        {
            using (System.IO.StreamReader file = new System.IO.StreamReader(SettingsPath, false))
            {
                string json = file.ReadToEnd();

                return JsonConvert.DeserializeObject<Settings>(json);
            }
        }

        public static void SaveSettings()
        {
            var json = JsonConvert.SerializeObject(Instance, Formatting.Indented);

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(SettingsPath, false))
            {
                file.WriteLine(json);
            }
        }
    }
}
