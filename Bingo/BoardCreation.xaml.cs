using Bingo.BusinessLogic;
using Bingo.Entities;
using Bingo.Exceptions;
using Bingo.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using Brushes = System.Windows.Media.Brushes;
using FontFamily = System.Windows.Media.FontFamily;
using Color = System.Windows.Media.Color;
using Image = System.Windows.Controls.Image;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace Bingo
{
    /// <summary>
    /// Interaction logic for BoardCreation.xaml
    /// </summary>
    public partial class BoardCreation : Page, IBoardCreation
    {
        private IBoardCreationBLL boardCreationBLL;
        private List<ComboBoxItem> _colorList = new List<ComboBoxItem>();
        private List<ComboBoxItem> _colorList2 = new List<ComboBoxItem>();
        private List<ComboBoxItem> _colorList3 = new List<ComboBoxItem>();
        private double height;
        private double width;
        private int _entryCount = 0;
        private BitmapImage darkSource = new BitmapImage(new Uri("img/DarkMode.png", UriKind.Relative));
        private BitmapImage lightSource = new BitmapImage(new Uri("img/LightMode.png", UriKind.Relative));

        public List<ComboBoxItem> ColorList { get { return _colorList; } set { _colorList = value; } }
        public List<ComboBoxItem> ColorList2 { get { return _colorList2; } set { _colorList2 = value; } }
        public List<ComboBoxItem> ColorList3 { get { return _colorList3; } set { _colorList3 = value; } }
        public int EntryCount { get { return _entryCount; } set { _entryCount = value; } }

        public BoardCreation(IBoardCreationBLL boardCreationBLL)
        {
            this.boardCreationBLL = boardCreationBLL;
            SetColorList(ColorList);
            SetColorList(ColorList2);
            SetColorList(ColorList3);
            InitializeComponent();
            CreateBoardPreview();
        }


        #region Size Changes
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ChangeSizes();
        }
        private void ChangeSizes()
        {
            var parent = (MainWindow)Parent;
            height = parent.ActualHeight;
            width = parent.ActualWidth;
            if (this.IsLoaded)
            {
                Heading.Margin = new Thickness(0, height / 12, 0, height / 12);
                Heading.FontSize = width / 25;

                Config.Margin = new Thickness(0, height / 30, 0, height / 25);

                MainConfigs.CornerRadius = new CornerRadius(height / 10);

                boardConfig.Margin = new Thickness(width / 40, height / 50, width / 100, height / 50);
                boardConfig.Width = (width + height) / 7;
                boardConfigHeading.FontSize = width / 60;

                ChooseName.Margin = new Thickness(width / 100, height / 30, width / 100, height / 50);
                Name.FontSize = width / 100;
                Name.Width = width / 11;
                ChosenNameGrid.Margin = new Thickness(width / 50, 2, 0, 2);
                ChosenName.FontSize = width / 150;
                Placeholder.FontSize = width / 170;

                ChooseSize.Margin = new Thickness(width / 100, height / 30, width / 100, height / 50);
                Size.FontSize = width / 100;
                Size.Width = width / 11;
                boardSize.Margin = new Thickness(width / 50, 2, 0, 2);
                boardSize.FontSize = width / 150;

                ChooseBackgroundColor.Margin = new Thickness(width / 100, height / 30, width / 100, height / 50);
                BackgroundColor.FontSize = width / 100;
                BackgroundColor.Width = width / 11;
                gridBackground.Margin = new Thickness(width / 50, 2, 0, 2);
                gridBackground.FontSize = width / 150;

                ChooseLightingMode.Margin = new Thickness(width / 100, height / 30, width / 100, 0);
                Lighting.FontSize = width / 100;
                Lighting.Width = width / 11;
                LightingMode.Margin = new Thickness(width / 18, 0, 0, 2);
                LightingMode.Height = width / 60;
                LightingMode.Width = Lighting.Height;

                ChooseBorderColor.Margin = new Thickness(width / 100, height / 30, width / 100, height / 50);
                BorderColor.FontSize = width / 100;
                BorderColor.Width = width / 11;
                borderColor.Margin = new Thickness(width / 50, 2, 0, 2);
                borderColor.FontSize = width / 150;

                ChooseFontColor.Margin = new Thickness(width / 100, height / 30, width / 100, height / 50);
                FontColor.FontSize = width / 100;
                FontColor.Width = width / 11;
                fontColor.Margin = new Thickness(width / 50, 2, 0, 2);
                fontColor.FontSize = width / 150;

                ChooseFontFamily.Margin = new Thickness(width / 100, height / 30, width / 100, height / 50);
                FontFam.FontSize = width / 100;
                FontFam.Width = width / 11;
                Font.Margin = new Thickness(width / 50, 2, 0, 2);
                Font.FontSize = width / 150;

                entryList.Width = width / 6;
                EntriesHeading.FontSize = width / 60;
                EntriesInfo.Margin = new Thickness(width / 100, height / 30, width / 70, height / 30);
                EntriesInfo.MaxWidth = width / 7;
                EntriesInfo.FontSize = width / 150;
                EntriesTextBox.Width = entryList.Width - 40;
                EntriesTextBox.Height = height / 4;
                EntriesTextBox.FontSize = EntriesTextBox.Width / 20;

                Arrow.Width = width / 12;
                Arrow.Height = Arrow.Width;

                PreviewBorder.MinHeight = height / 2.9;
                PreviewBorder.MinWidth = PreviewBorder.MinHeight;
                PreviewBorder.Margin = new Thickness(0, height / 8, 0, 0);

                PreviewHeading.FontSize = width / 70;
                PlayGame.FontSize = width / 50;

                boardPreview.Height = height / 4.5;
                boardPreview.Width = boardPreview.Height;
            }
        }
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChangeSizes();
        }
        #endregion


        #region PagePreparation
        private void SetColorList(List<ComboBoxItem> comboBoxItems)
        {
            var dict = new Dictionary<string, Color>();
            dict.Add("Amagno-Leuchtrot", Color.FromArgb(0xff, 0xeb, 0x41, 0x3c));
            dict.Add("Amagno-Leuchtblau", Color.FromArgb(0xff, 0x14, 0x64, 0xf0));
            dict.Add("Amagno-Hellgrau", Color.FromArgb(0xff, 0xee, 0xee, 0xee));
            dict.Add("Amagno-Schwarz", Color.FromArgb(0xff, 0x44, 0x44, 0x44));
            dict.Add("Amagno-Orange", Color.FromArgb(0xff, 0xff, 0xc3, 0x0f));
            dict.Add("Amagno-Purpur", Color.FromArgb(0xff, 0xc8, 0x4b, 0xc8));
            dict.Add("Amagno-Grün", Color.FromArgb(0xff, 0x32, 0xaa, 0x5a));

            foreach (var color in dict)
            {
                var item = new ComboBoxItem();

                var brush = new SolidColorBrush();
                brush.Color = color.Value;
                item.Background = brush;
                item.Content = color.Key;
                item.FontSize = 12;
                item.FontFamily = new FontFamily("Inter SemiBold");
                item.Foreground = WhiteOrBlackText(brush);
                item.VerticalContentAlignment = VerticalAlignment.Center;
                item.HorizontalContentAlignment = HorizontalAlignment.Left;

                comboBoxItems.Add(item);
            }


            var colors = typeof(Colors).GetProperties();

            foreach (var color in colors)
            {
                var item = new ComboBoxItem();

                var brush = new SolidColorBrush();
                brush.Color = (Color)color.GetValue(null, null);
                item.Background = brush;
                item.Content = color.Name;
                item.FontSize = 12;
                item.FontFamily = new FontFamily("Inter SemiBold");
                item.Foreground = WhiteOrBlackText(brush);
                item.VerticalContentAlignment = VerticalAlignment.Center;
                item.HorizontalContentAlignment = HorizontalAlignment.Left;

                comboBoxItems.Add(item);
            }
        }
        private SolidColorBrush WhiteOrBlackText(SolidColorBrush brush)
        {
            var foregroundBrush = new SolidColorBrush();

            var color = brush.Color;

            const double threshold = (3 * 255) / 2;
            int sum = color.R + color.G + color.B;

            if (threshold < sum)
            {
                foregroundBrush.Color = Colors.Black;
                return foregroundBrush;
            }
            else
            {
                foregroundBrush.Color = Colors.White;
                return foregroundBrush;
            }
        }
        private void ResetSettings()
        {
            ChosenName.Text = "";
            boardSize.SelectedIndex = 0;
            gridBackground.SelectedIndex = 3;
            borderColor.SelectedIndex = 0;
            fontColor.SelectedIndex = 4;
            Font.SelectedIndex = 0;
            EntriesTextBox.Text = "";
        }
        #endregion


        #region Preview
        private void CreateBoardPreview()
        {
            boardPreview.Children.Clear();
            boardPreview.ColumnDefinitions.Clear();
            boardPreview.RowDefinitions.Clear();
            SetEntryCount();

            var size = (ComboBoxItem)boardSize.SelectedItem;

            switch (size.Content.ToString())
            {
                case "4x4":
                    GridCreation(4);
                    break;
                case "5x5":
                    GridCreation(5);
                    break;
                case "6x6":
                    GridCreation(6);
                    break;
                default:
                    throw new Exception();
            }
        }
        private void GridCreation(int size)
        {
            var backgroundColor = (ComboBoxItem)gridBackground.SelectedItem;
            var fontcolor = (ComboBoxItem)fontColor.SelectedItem;
            var gridcolor = (ComboBoxItem)borderColor.SelectedItem;
            var fontfamily = (FontFamily)Font.SelectedItem;

            boardPreview.Background = backgroundColor.Background;

            int l = 1;

            for (int i = 0; i < size; i++)
            {
                boardPreview.ColumnDefinitions.Add(new ColumnDefinition());
                boardPreview.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var tb = new TextBlock();
                    Grid.SetRow(tb, i);
                    Grid.SetColumn(tb, j);
                    tb.Text = l.ToString();
                    tb.Background = Brushes.Transparent;
                    tb.Foreground = fontcolor.Background;
                    tb.HorizontalAlignment = HorizontalAlignment.Center;
                    tb.FontFamily = fontfamily;
                    tb.VerticalAlignment = VerticalAlignment.Center;
                    tb.IsHitTestVisible = false;
                    tb.FontSize = 20;
                    l++;

                    var rect = new Rectangle();
                    Grid.SetRow(rect, i);
                    Grid.SetColumn(rect, j);
                    rect.Fill = Brushes.Transparent;
                    rect.StrokeThickness = 2;
                    rect.Stroke = gridcolor.Background;

                    boardPreview.Children.Add(rect);
                    boardPreview.Children.Add(tb);
                }
            }

            var rectangle = new Rectangle();
            Grid.SetColumn(rectangle, 0);
            Grid.SetColumnSpan(rectangle, size);
            Grid.SetRow(rectangle, 0);
            Grid.SetRowSpan(rectangle, size);
            rectangle.Fill = Brushes.Transparent;
            rectangle.StrokeThickness = 4;
            var borderItem = gridcolor;
            rectangle.Stroke = borderItem.Background;

            boardPreview.Children.Add(rectangle);
        }
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(this.IsLoaded)
            {
                CreateBoardPreview();
            }
        }
        #endregion


        #region Play Game
        private void OnPlayGame(object sender, RoutedEventArgs e)
        {
            try
            {
                var backgroundColor = (ComboBoxItem)gridBackground.SelectedItem;
                var fontcolor = (ComboBoxItem)fontColor.SelectedItem;
                var gridcolor = (ComboBoxItem)borderColor.SelectedItem;
                var fontfamily = (FontFamily)Font.SelectedItem;                
                var size = (ComboBoxItem)boardSize.SelectedItem;

                if (string.IsNullOrEmpty(ChosenName.Text) || string.IsNullOrEmpty(ChosenName.Text))
                {
                    throw new EmptyBoardNameException("Du musst dem Board einen Namen geben, bevor du das Spiel startest.");
                }

                CustomBoard.BoardSize size1 = CustomBoard.BoardSize.Medium;
                switch (size.Content.ToString())
                {
                    case "4x4": 
                        size1 = CustomBoard.BoardSize.Small;
                        break;
                    case "5x5":
                        size1 = CustomBoard.BoardSize.Medium;
                        break;
                    case "6x6":
                        size1 = CustomBoard.BoardSize.Big;
                        break;
                }

                IsColourDoubled(backgroundColor.Background.ToString(), gridcolor.Background.ToString(), fontcolor.Background.ToString());
                if (boardCreationBLL.CreateGame(fontfamily.ToString(), fontcolor.Background.ToString(), gridcolor.Background.ToString(),
                    backgroundColor.Background.ToString(), ChosenName.Text, size1, EntriesTextBox.Text + "\r\n", IsLightMode()))
                {
                    var par = (MainWindow)Parent;
                    var service = par.serviceProvider.GetService<IPlayBingo>();
                    par.Navigate(service);
                    ResetSettings();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void IsColourDoubled(string background, string grid, string font)
        {
            if (background == grid || background == font || grid == font)
            {
                throw new ColorException("Keine der ausgewählten Kanten-, Hintergrund- und Schriftfarbe darf sich doppeln.");
            }
        }
        #endregion


        #region EntryList
        private void UpdateWordCount()
        {
            var entries = 0;

            for (int i = 0; i < EntriesTextBox.LineCount; i++)
            {
                var text = EntriesTextBox.GetLineText(i);
                if (!String.IsNullOrEmpty(text) && !(text == "\n") && !String.IsNullOrWhiteSpace(text))
                {
                    entries++;
                }
            }
            EntryCount = entries;
            SetEntryCount();
        }
        private void SetEntryCount()
        {
            var size = (ComboBoxItem)boardSize.SelectedItem;

            switch (size.Content.ToString())
            {
                case "4x4":
                    EntriesCounter.Text = $"{EntryCount}/ min. 16";
                    break;
                case "5x5":
                    EntriesCounter.Text = $"{EntryCount}/ min. 25";
                    break;
                case "6x6":
                    EntriesCounter.Text = $"{EntryCount}/ min. 36";
                    break;
                default:
                    throw new Exception();
            }
        }
        private void OnFileLoaded(object sender, DragEventArgs e)
        {
            try
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (!files[0].EndsWith(".txt"))
                {
                    throw new FileException("Ausschließlich .txt-Dateien können eingelesen werden. Bitte verwende keine anderen Dateien.");
                }

                var fileText = "";
                var file = File.ReadAllLines(files[0]);
                foreach (var line in file)
                {
                    fileText += line + "\r\n";
                }
                EntriesTextBox.Text += fileText;
                UpdateWordCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void OnPreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateWordCount();
        }
        private void OnSearchFile(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Filter = "Textdateien|*.txt"
            };
            if (dialog.ShowDialog() == true)
            {
                var fileText = "";
                var file = File.ReadAllLines(dialog.FileName);
                foreach (var line in file)
                {
                    fileText += line + "\r\n";
                }
                EntriesTextBox.Text += fileText;
            }
        }
        #endregion

        private void OnLightingModeChanged(object sender, RoutedEventArgs e)
        {
            var lightModeImage = new Image();
            lightModeImage.Source = lightSource;
            var darkModeImage = new Image();
            darkModeImage.Source = darkSource;
            
            var image = LightingMode.Content as Image;
            if (image.Source.ToString().Contains(lightSource.ToString()))
            {
                LightingMode.Content = darkModeImage;
                ChangeColors(false);
            }
            else
            {
                LightingMode.Content = lightModeImage;
                ChangeColors(true);
            }
        }

        private void ChangeColors(bool isLightMode)
        {
            if (isLightMode)
            {
                this.Background = Brushes.White;
                Heading.Foreground = Brushes.Black;
            }
            else
            {
                var brush = new SolidColorBrush();
                brush.Color = Color.FromArgb(0xff, 0x44, 0x44, 0x44);
                this.Background = brush;
                Heading.Foreground = Brushes.White;
            }
        }

        private bool IsLightMode()
        {
            var lightModeImage = new Image();
            lightModeImage.Source = lightSource;
            if (LightingMode.Content == lightModeImage)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public interface IBoardCreation { }
}