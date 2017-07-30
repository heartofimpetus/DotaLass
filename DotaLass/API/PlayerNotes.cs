using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DotaLass.API
{
    public class PlayerNotes
    {
        public static Color[] Colors = new Color[] { (Color)ColorConverter.ConvertFromString("LightGray"),
                                                     (Color)ColorConverter.ConvertFromString("Green"),
                                                     (Color)ColorConverter.ConvertFromString("DodgerBlue"),
                                                     (Color)ColorConverter.ConvertFromString("Gold"),
                                                     (Color)ColorConverter.ConvertFromString("Firebrick")};

        [JsonProperty]
        private Dictionary<string, Note> Notes { get; set; }

        public PlayerNotes()
        {
            Notes = new Dictionary<string, Note>();
        }

        public Note this[string playerId]
        {
            get
            {
                if (Notes.ContainsKey(playerId))
                    return Notes[playerId];
                else
                    return new Note();
            }
            set
            {
                if (value.ColourIndex > 0 || !string.IsNullOrEmpty(value.Text))
                {
                    if (Notes.ContainsKey(playerId))
                        Notes[playerId] = value;
                    else
                        Notes.Add(playerId, value);
                }
                else
                {
                    if (Notes.ContainsKey(playerId))
                        Notes.Remove(playerId);
                }
            }
        }

        private static PlayerNotes _Instance;
        public static PlayerNotes Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = LoadNotes();
                }

                return _Instance;
            }
        }

        private static string NotesPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + "/Notes.json";
            }
        }

        private static PlayerNotes LoadNotes()
        {
            using (System.IO.StreamReader file = new System.IO.StreamReader(NotesPath, false))
            {
                string json = file.ReadToEnd();

                return JsonConvert.DeserializeObject<PlayerNotes>(json);
            }
        }
        public static void SaveNotes()
        {
            var json = JsonConvert.SerializeObject(Instance, Formatting.Indented);

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(NotesPath, false))
            {
                file.WriteLine(json);
            }
        }

        public class Note
        {
            public int ColourIndex { get; set; }
            public string Text { get; set; }

            [JsonIgnore]
            public Color Colour
            {
                get
                {
                    return Colors[ColourIndex];
                }
            }
        }
    }
}
