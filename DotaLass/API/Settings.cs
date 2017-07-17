using Newtonsoft.Json;
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

        public enum LinkSite
        {
            OpenDota,
            DotaBuff
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

        public static void SaveBlueprint()
        {
            var json = JsonConvert.SerializeObject(Instance, Formatting.Indented);

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(SettingsPath, false))
            {
                file.WriteLine(json);
            }
        }
    }
}
