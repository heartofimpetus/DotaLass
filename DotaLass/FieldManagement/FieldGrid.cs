using DotaLass.API;
using DotaLass.FieldManagement.FieldGenerators;
using DotaLass.FieldManagement.FieldGenerators.Fields;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DotaLass.FieldManagement
{
    public class FieldGrid
    {
        private Grid Grid { get; }
        private Window Window { get; }
        private List<PlayerDisplay> PlayerDisplays { get; }

        public List<FieldInfo> FieldInfos { get; }

        private int[] HeaderRowIndexes { get; }
        private int[] FieldRowIndexes { get; }

        public FieldGrid(Window window, Grid grid, List<PlayerDisplay> playerDisplays)
        {
            Grid = grid;
            Window = window;
            PlayerDisplays = playerDisplays;

            FieldInfos = new List<FieldInfo>();

            HeaderRowIndexes = new int[2];
            FieldRowIndexes = new int[10];

            CreateFields();
            GenerateGrid();
        }

        private FieldInfo CreateFieldInfo(string key, bool visible)
        {
            switch (key)
            {
                case "Profile": return new FieldInfo(visible, new ProfileLinkField(Window));
                case "Notes": return new FieldInfo(visible, new NotesField(Window));
                case "Solo MMR": return new FieldInfo(visible, new StringField(Window, nameof(PlayerDisplay.DisplayData.SoloMMR), "Solo MMR", 120));
                case "Estimate MMR": return new FieldInfo(visible, new StringField(Window, nameof(PlayerDisplay.DisplayData.EstimateMMR), "Estimate MMR", 120));
                case "Winrate": return new FieldInfo(visible, new FloatField(Window, nameof(PlayerDisplay.DisplayData.Winrate), "Winrate", 100, "0.#%"));
                case "K": return new FieldInfo(visible, new FloatField(Window, nameof(PlayerDisplay.DisplayData.AverageKills), "K", 50));
                case "D": return new FieldInfo(visible, new FloatField(Window, nameof(PlayerDisplay.DisplayData.AverageDeaths), "D", 50));
                case "A": return new FieldInfo(visible, new FloatField(Window, nameof(PlayerDisplay.DisplayData.AverageAssists), "A", 50));
                case "XPM": return new FieldInfo(visible, new FloatField(Window, nameof(PlayerDisplay.DisplayData.AverageXPM), "XPM", 75));
                case "GPM": return new FieldInfo(visible, new FloatField(Window, nameof(PlayerDisplay.DisplayData.AverageGPM), "GPM", 75));
                case "DMG": return new FieldInfo(visible, new FloatField(Window, nameof(PlayerDisplay.DisplayData.AverageHeroDamage), "DMG", 100));
                case "BLD": return new FieldInfo(visible, new FloatField(Window, nameof(PlayerDisplay.DisplayData.AverageTowerDamage), "BLD", 75));
                case "HEAL": return new FieldInfo(visible, new FloatField(Window, nameof(PlayerDisplay.DisplayData.AverageHeroHealing), "HEAL", 75));
                case "LH": return new FieldInfo(visible, new FloatField(Window, nameof(PlayerDisplay.DisplayData.AverageLastHits), "LH", 75));
                case "Recent Matches": return new FieldInfo(visible, new HeroIconsField(Window, "Recent Matches"));
                default: return null;
            }
        }

        private void CreateFields()
        {
            foreach (var fieldSetting in Settings.Instance.FieldSettings)
            {
                FieldInfo fieldInfo = CreateFieldInfo(fieldSetting.Item1, fieldSetting.Item2);

                if (fieldInfo != null)
                    FieldInfos.Add(fieldInfo);
            }
        }

        private void GenerateGrid()
        {
            GenerateGridColumns();
            GenerateHeaders();
            GeneratePlayerFields();

            UpdateColumns();
        }

        private void GenerateGridColumns()
        {
            for (int j = 0; j < FieldInfos.Count; j++)
            {
                Grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            HeaderRowIndexes[0] = 0;
            HeaderRowIndexes[1] = 6;

            for (int i = 0; i < 5; i++)
            {
                FieldRowIndexes[i] = i + 1;
            }
            for (int i = 5; i < 10; i++)
            {
                FieldRowIndexes[i] = i + 2;
            }
        }
        private void GenerateHeaders()
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < FieldInfos.Count; j++)
                {
                    var headerElement = FieldInfos[j].Field.GenerateHeader();

                    headerElement.SetValue(Grid.RowProperty, HeaderRowIndexes[i]);

                    Grid.Children.Add(headerElement);
                    FieldInfos[j].HeaderElements.Add(headerElement);
                }
            }
        }
        private void GeneratePlayerFields()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < FieldInfos.Count; j++)
                {
                    var fieldElement = FieldInfos[j].Field.CreateField(PlayerDisplays[i]);

                    fieldElement.SetValue(Grid.RowProperty, FieldRowIndexes[i]);

                    Grid.Children.Add(fieldElement);
                    FieldInfos[j].FieldElements.Add(fieldElement);
                }
            }
        }

        public void UpdateColumns()
        {
            for (int i = 0; i < FieldInfos.Count; i++)
            {
                var fieldInfo = FieldInfos[i];
                var column = Grid.ColumnDefinitions[i];

                if (fieldInfo.Visible)
                {
                    if (!Double.IsNaN(fieldInfo.Field.Width))
                        column.Width = new GridLength(fieldInfo.Field.Width);
                    else
                        column.Width = GridLength.Auto;
                }
                else
                {
                    column.Width = new GridLength(0);
                }
            }

            for (int i = 0; i < FieldInfos.Count; i++)
            {
                FieldInfos[i].AlignElements(i);
            }
        }

        public void UpdateSettings()
        {
            List<Tuple<string, bool>> fieldSettings = new List<Tuple<string, bool>>();

            foreach (var fieldInfo in FieldInfos)
            {
                fieldSettings.Add(new Tuple<string, bool>(fieldInfo.Field.Name, fieldInfo.Visible));
            }

            Settings.Instance.FieldSettings = fieldSettings;
        }

        public class FieldInfo
        {
            public FieldBase Field { get; }
            public bool Visible { get; set; }

            public List<UIElement> HeaderElements { get; set; }
            public List<UIElement> FieldElements { get; set; }

            public FieldInfo(bool visible, FieldBase fieldInfo)
            {
                Field = fieldInfo;
                Visible = visible;

                HeaderElements = new List<UIElement>();
                FieldElements = new List<UIElement>();
            }

            public void AlignElements(int columnIndex)
            {
                foreach (var header in HeaderElements)
                {
                    header.SetValue(Grid.ColumnProperty, columnIndex);
                }

                foreach (var field in FieldElements)
                {
                    field.SetValue(Grid.ColumnProperty, columnIndex);
                }
            }
        }
    }
}
