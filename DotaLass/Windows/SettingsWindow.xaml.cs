using DotaLass.FieldManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DotaLass.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private MainWindow MainWindow { get; }
        private FieldGrid FieldGrid { get; }

        public SettingsWindow(MainWindow mainWindow, FieldGrid fieldGrid)
        {
            InitializeComponent();

            MainWindow = mainWindow;
            FieldGrid = fieldGrid;

            PopulateListBox();
        }

        List<CheckBox> CheckBoxes;
        private void PopulateListBox()
        {
            CheckBoxes = new List<CheckBox>();

            ListBoxFields.Items.Clear();

            for (int i = 0; i < FieldGrid.FieldInfos.Count; i++)
            {
                Grid grid = new Grid();

                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition());

                CheckBox check = new CheckBox()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(1)
                };

                CheckBoxes.Add(check);

                int index = i;

                check.Checked += (o, a) =>
                {
                    FieldGrid.FieldInfos[index].Visible = true;
                    FieldGrid.UpdateColumns();
                    MainWindow.AutoSizeWindow();
                };

                check.Unchecked += (o, a) =>
                {
                    FieldGrid.FieldInfos[index].Visible = false;
                    FieldGrid.UpdateColumns();
                    MainWindow.AutoSizeWindow();
                };

                Label label = new Label()
                {
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    VerticalContentAlignment = VerticalAlignment.Center
                };

                check.SetValue(Grid.ColumnProperty, 0);
                label.SetValue(Grid.ColumnProperty, 1);

                grid.Children.Add(check);
                grid.Children.Add(label);

                Border border = new Border()
                {
                    Child = grid,
                    Style = (Style)this.FindResource("BorderStyle"),
                    Background = new SolidColorBrush(new Color() { A = 0 })
                };

                ListBoxFields.Items.Add(border);
            }

            SyncCheckBoxes();
        }

        private void SyncCheckBoxes()
        {
            for (int i = 0; i < FieldGrid.FieldInfos.Count; i++)
            {
                CheckBoxes[i].Content = FieldGrid.FieldInfos[i].Field.Name;
                CheckBoxes[i].IsChecked = FieldGrid.FieldInfos[i].Visible;
            }
        }
        
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void BtnMoveUp_Click(object sender, RoutedEventArgs e)
        {
            int index = ListBoxFields.SelectedIndex;

            if (index > 0)
            {
                FieldGrid.FieldInfo field = FieldGrid.FieldInfos[index];

                FieldGrid.FieldInfos.RemoveAt(index);
                FieldGrid.FieldInfos.Insert(index - 1, field);

                ListBoxFields.SelectedIndex--;

                SyncCheckBoxes();

                CheckBoxes[ListBoxFields.SelectedIndex].BringIntoView();

                FieldGrid.UpdateColumns();
                MainWindow.AutoSizeWindow();
            }
        }

        private void BtnMoveDown_Click(object sender, RoutedEventArgs e)
        {
            int index = ListBoxFields.SelectedIndex;

            if (index + 1 < FieldGrid.FieldInfos.Count && index >= 0)
            {
                FieldGrid.FieldInfo field = FieldGrid.FieldInfos[index];

                FieldGrid.FieldInfos.RemoveAt(index);
                FieldGrid.FieldInfos.Insert(index + 1, field);

                ListBoxFields.SelectedIndex++;

                SyncCheckBoxes();

                CheckBoxes[ListBoxFields.SelectedIndex].BringIntoView();

                FieldGrid.UpdateColumns();
                MainWindow.AutoSizeWindow();
            }
        }
    }
}
