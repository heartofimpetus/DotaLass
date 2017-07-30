using DotaLass.API;
using DotaLass.FieldManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DotaLass.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<PlayerDisplay> PlayerDisplays { get; private set; }

        private FieldGrid FieldGrid;

        public MainWindow()
        {
            InitializeComponent();

            this.Style = (Style)FindResource(typeof(Window));
            this.DataContext = this;

            PlayerDisplays = new List<PlayerDisplay>();

            for (int i = 0; i < 10; i++)
            {
                var playerDisplay = new PlayerDisplay();

                playerDisplay.RetrievalStarted += PlayerDisplay_RetrievalStarted;
                playerDisplay.RetrievalCompleted += PlayerDisplay_RetrievalCompleted;

                PlayerDisplays.Add(playerDisplay);
            }

            FieldGrid = new FieldGrid(this, DataGrid, PlayerDisplays);

            SetupFileWatcher();
        }

        private string LastLobby = null;
        private void SetupFileWatcher()
        {
            LastLobby = OpenDotaAPI.GetLastLobby(FileManagement.ServerLog);

            FileSystemWatcher watcher = new FileSystemWatcher(new FileInfo(FileManagement.ServerLog).Directory.FullName)
            {
                EnableRaisingEvents = true
            };

            watcher.Changed += (newobject, newargs) =>
            {
                try
                {
                    if (Settings.Instance.AutoRetrievePlayerData)
                    {
                        string tempLobby = OpenDotaAPI.GetLastLobby(FileManagement.ServerLog);
                        if (LastLobby != tempLobby)
                        {
                            watcher.EnableRaisingEvents = false;
                            RetrieveData();

                            LastLobby = tempLobby;
                        }
                    }
                }
                finally
                {
                    watcher.EnableRaisingEvents = true;
                }
            };
        }
        
        private void RetrieveData()
        {
            this.Dispatcher.Invoke(() =>
            {
                RefreshSpinner.Spin = true;
            });

            var playerIDs = OpenDotaAPI.GetPlayerIDs();
            
            for (int i = 0; i < 10; i++)
            {
                if (i < playerIDs.Count)
                    PlayerDisplays[i].Update(playerIDs[i]);
                else
                    PlayerDisplays[i].Update("");
            }
        }


        int runningRetrievals = 0;
        private void PlayerDisplay_RetrievalStarted(object sender, EventArgs e)
        {
            runningRetrievals++;
        }
        private void PlayerDisplay_RetrievalCompleted(object sender, EventArgs e)
        {
            runningRetrievals--;
            this.Dispatcher.Invoke(() =>
            {
                RefreshSpinner.Spin = runningRetrievals != 0;
            });
        }

        public void AutoSizeWindow()
        {
            UIElement content = this.Content as UIElement;
            
            content.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            
            this.MaxWidth = content.DesiredSize.Width;
            this.MinHeight = content.DesiredSize.Height;
            this.MaxHeight = content.DesiredSize.Height;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            FieldGrid.UpdateSettings();

            Application.Current.Shutdown();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RetrieveData();
            AutoSizeWindow();
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow(this, FieldGrid);
            settingsWindow.ShowDialog();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AutoSizeWindow();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToHorizontalOffset(scv.HorizontalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
