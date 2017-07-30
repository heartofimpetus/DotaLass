using DotaLass.API;
using FontAwesome.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for NotesWindow.xaml
    /// </summary>
    public partial class NotesWindow : Window
    {
        private string PlayerID { get; set; }
        private PlayerNotes.Note Note { get; set; }

        public NotesWindow(string playerID, ImageAwesome sourceImage)
        {
            InitializeComponent();

            PlayerID = playerID;
            Note = PlayerNotes.Instance[playerID];

            InitializeColours(sourceImage);
            InitializeText();
        }

        private void InitializeColours(ImageAwesome sourceImage)
        {
            BorderSelected.SetValue(Grid.ColumnProperty, Note.ColourIndex);

            for (int i = 0; i < PlayerNotes.Colors.Length; i++)
            {
                int index = i;

                GridColour.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

                Rectangle rect = new Rectangle() { Fill = new SolidColorBrush(PlayerNotes.Colors[i]), Margin = new Thickness(5) };
                rect.SetValue(Grid.ColumnProperty, i);

                GridColour.Children.Add(rect);

                rect.MouseDown += (o, a) =>
                {
                    if (a.ChangedButton == MouseButton.Left)
                    {
                        BorderSelected.SetValue(Grid.ColumnProperty, index);
                        Note.ColourIndex = index;

                        sourceImage.Foreground = new SolidColorBrush(Note.Colour);
                    }
                };
            }
        }

        private const string DefaultText = "Enter notes here...";
        private void InitializeText()
        {
            if (!string.IsNullOrEmpty(Note.Text))
            {
                TxtNotes.Text = Note.Text;
            }
            else
            {
                TxtNotes.Text = DefaultText;
            }

            TxtNotes.GotFocus += (o, a) =>
            {
                if (TxtNotes.Text == DefaultText)
                    TxtNotes.Text = "";
            };
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNotes.Text != DefaultText)
                Note.Text = TxtNotes.Text;
            else
                Note.Text = "";

            PlayerNotes.Instance[PlayerID] = Note;

            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
