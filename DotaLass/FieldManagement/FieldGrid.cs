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

        private void CreateFields()
        {
            var blueprint = LoadBlueprint();

            Dictionary<string, FieldInfo> CoreFields = new Dictionary<string, FieldInfo>();

            CoreFields.Add(nameof(PlayerDisplay.SoloMMR), new FieldInfo(true, new StringField(Window, nameof(PlayerDisplay.SoloMMR), "Solo MMR", 150)));
            CoreFields.Add(nameof(PlayerDisplay.EstimateMMR), new FieldInfo(true, new StringField(Window, nameof(PlayerDisplay.EstimateMMR), "Estimate MMR", 150)));
            CoreFields.Add(nameof(PlayerDisplay.Winrate), new FieldInfo(true, new FloatField(Window, nameof(PlayerDisplay.Winrate), "Winrate", 100, "0.#%")));
            CoreFields.Add(nameof(PlayerDisplay.AverageKills), new FieldInfo(true, new FloatField(Window, nameof(PlayerDisplay.AverageKills), "K", 50)));
            CoreFields.Add(nameof(PlayerDisplay.AverageDeaths), new FieldInfo(true, new FloatField(Window, nameof(PlayerDisplay.AverageDeaths), "D", 50)));
            CoreFields.Add(nameof(PlayerDisplay.AverageAssists), new FieldInfo(true, new FloatField(Window, nameof(PlayerDisplay.AverageAssists), "A", 50)));
            CoreFields.Add(nameof(PlayerDisplay.AverageXPM), new FieldInfo(true, new FloatField(Window, nameof(PlayerDisplay.AverageXPM), "XPM", 75)));
            CoreFields.Add(nameof(PlayerDisplay.AverageGPM), new FieldInfo(true, new FloatField(Window, nameof(PlayerDisplay.AverageGPM), "GPM", 75)));
            CoreFields.Add(nameof(PlayerDisplay.AverageHeroDamage), new FieldInfo(true, new FloatField(Window, nameof(PlayerDisplay.AverageHeroDamage), "DMG", 100)));
            CoreFields.Add(nameof(PlayerDisplay.AverageTowerDamage), new FieldInfo(true, new FloatField(Window, nameof(PlayerDisplay.AverageTowerDamage), "BLD", 75)));
            CoreFields.Add(nameof(PlayerDisplay.AverageHeroHealing), new FieldInfo(true, new FloatField(Window, nameof(PlayerDisplay.AverageHeroHealing), "HEAL", 75)));
            CoreFields.Add(nameof(PlayerDisplay.AverageLastHits), new FieldInfo(true, new FloatField(Window, nameof(PlayerDisplay.AverageLastHits), "LH", 75)));
            CoreFields.Add(nameof(PlayerDisplay.HeroesPlayed), new FieldInfo(true, new HeroesPlayedField(Window, "Heroes")));

            for (int i = 0; i < CoreFields.Count; i++)
            {
                FieldInfos.Add(null);
            }

            foreach (var coreField in CoreFields)
            {
                FieldInfos[blueprint[coreField.Key].Item1] = coreField.Value;
                coreField.Value.Visible = blueprint[coreField.Key].Item2;
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
                    var fieldElement = FieldInfos[j].Field.GenerateField(PlayerDisplays[i]);

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

        private string BlueprintPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + "/Settings.json";
            }
        }

        public void SaveBlueprint()
        {
            Dictionary<string, Tuple<int, bool>> blueprint = new Dictionary<string, Tuple<int, bool>>();

            for (int i = 0; i < FieldInfos.Count; i++)
            {
                blueprint.Add(FieldInfos[i].Field.Path, new Tuple<int, bool>(i, FieldInfos[i].Visible));
            }

            var json = JsonConvert.SerializeObject(blueprint);

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(BlueprintPath, false))
            {
                file.WriteLine(json);
            }
        }

        public Dictionary<string, Tuple<int, bool>> LoadBlueprint()
        {
            using (System.IO.StreamReader file = new System.IO.StreamReader(BlueprintPath, false))
            {
               string json = file.ReadToEnd();

                return JsonConvert.DeserializeObject<Dictionary<string, Tuple<int, bool>>>(json);
            }
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
