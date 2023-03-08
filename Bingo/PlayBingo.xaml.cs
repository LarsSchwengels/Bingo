using Bingo.BusinessLogic;
using Bingo.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
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
using FontFamily = System.Windows.Media.FontFamily;
using Color = System.Drawing.Color;
using Brushes = System.Windows.Media.Brushes;
using Color2 = System.Windows.Media.Color;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace Bingo
{
    /// <summary>
    /// Interaction logic for Bingo.xaml
    /// </summary>
    public partial class PlayBingo : Page, IPlayBingo
    {
        private IGameBLL gameBLL;
        private CustomBoard customBoard;
        private List<TextBlock> textBlocks = new List<TextBlock>();
        private List<TextBlock> checkBlocks = new List<TextBlock>();
        private List<Button> buttons = new List<Button>();
        private double height = 0;
        private double width = 0;

        public PlayBingo(IGameBLL game)
        {
            gameBLL = game;
            customBoard = gameBLL.GetBoard();
            InitializeComponent();
        }

        #region WindowSize
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetSizes();
        }
        private void SetSizes()
        {
            var size = GetSizeInt();
            var parent = (MainWindow)Parent;
            height = parent.ActualHeight;
            width = parent.ActualWidth;

            Board.Height = height / 1.5;
            Board.Width = Board.Height;

            foreach (var button in buttons)
            {
                button.Width = Board.Width / size;
                button.Height = Board.Height / size;
            }

            foreach (var cB in checkBlocks)
            {
                cB.Width = Board.Width / size;
                cB.Height = Board.Height / size;
            }
        }
        #endregion


        #region Board Creation
        private void OnOpened(object sender, RoutedEventArgs e)
        {
            CreateGrid();
            SetSizes();
            EnterWordsToGrid();
            LightingMode();

            if (customBoard.Squares.Exists(x => x.IsChecked == true))
            {
                for (int i = 0; i < customBoard.Squares.Count; i++)
                {
                    if (customBoard.Squares[i].IsChecked == true)
                    {
                        var btn = buttons[i];
                        ChangeButtonForeground(btn);
                    }
                }
            }
            BingoCounter.Text = customBoard.BingoCount.ToString();
        }

        private void LightingMode()
        {
            if (customBoard.IsLightMode)
            {
                return;
            }
            else
            {
                this.Background = ColorFromString("Amagno-Schwarz");
                boardName.Foreground = Brushes.White;
                BingoCounter.Foreground = Brushes.White;
                bingos.Foreground = Brushes.White;
            }
        }

        private int GetSizeInt()
        {
            return customBoard.Size switch
            {
                CustomBoard.BoardSize.Small => 4,
                CustomBoard.BoardSize.Medium => 5,
                CustomBoard.BoardSize.Big => 6,
                _ => 0,
            };
        }
        private void CreateGrid()
        {
            customBoard = gameBLL.GetBoard();
            int size = GetSizeInt();

            boardName.Text = customBoard.Name;
            MainGrid(size);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var grid = new Grid();
                    var rect = new Rectangle();

                    var vb = SquareViewbox(size, FontFamilyFromString());
                    var button = SquareButton(size);

                    Grid.SetRow(rect, i);
                    Grid.SetColumn(rect, j);
                    rect.Fill = Brushes.Transparent;
                    rect.StrokeThickness = 4;
                    rect.Stroke = ColorFromString(customBoard.GridColour);

                    grid.Children.Add(vb);
                    grid.Children.Add(button);
                    Grid.SetRow(grid, i);
                    Grid.SetColumn(grid, j);

                    Board.Children.Add(rect);
                    Board.Children.Add(grid);
                }
            }
        }
        private void MainGrid(int size)
        {
            for (int i = 0; i < size; i++)
            {
                Board.ColumnDefinitions.Add(new ColumnDefinition());
                Board.RowDefinitions.Add(new RowDefinition());
            }
            Board.Background = ColorFromString(customBoard.BackgroundColour);

            var rectangle = new Rectangle();
            Grid.SetColumn(rectangle, 0);
            Grid.SetColumnSpan(rectangle, size);
            Grid.SetRow(rectangle, 0);
            Grid.SetRowSpan(rectangle, size);
            rectangle.Fill = Brushes.Transparent;
            rectangle.StrokeThickness = 4;
            var borderItem = ColorFromString(customBoard.GridColour);
            rectangle.Stroke = borderItem;

            Board.Children.Add(rectangle);
        }
        private Viewbox SquareViewbox(int size, FontFamily fam)
        {
            var tb = new TextBlock();
            tb.Background = Brushes.Transparent;
            tb.Foreground = ColorFromString(customBoard.FontColour);
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.FontFamily = fam;
            tb.TextAlignment = TextAlignment.Center;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.IsHitTestVisible = true;
            tb.MaxHeight = 600 / size - 20;
            tb.MaxWidth = tb.MaxHeight;
            tb.TextWrapping = TextWrapping.Wrap;
            Panel.SetZIndex(tb, 4);
            textBlocks.Add(tb);

            var vb = new Viewbox();

            vb.MaxHeight = 600 / size - 50;
            vb.MaxWidth = vb.MaxHeight;
            Panel.SetZIndex(vb, 3);
            vb.Child = tb;
            vb.Stretch = Stretch.Uniform;
            vb.IsHitTestVisible = false;

            return vb;
        }
        private Button SquareButton(int size)
        {
            var button = new Button();

            button.Content = "X";
            button.Foreground = Brushes.Transparent;
            button.Background = Brushes.Transparent;
            button.HorizontalAlignment = HorizontalAlignment.Center;
            button.VerticalAlignment = VerticalAlignment.Center;
            button.HorizontalContentAlignment = HorizontalAlignment.Center;
            button.VerticalContentAlignment = VerticalAlignment.Center;
            button.FontSize = 500 / size;
            button.Height = 600 / size - 20;
            button.Width = button.Height;
            button.Click += OnCheckOrUncheck;
            Panel.SetZIndex(button, 4);
            buttons.Add(button);

            return button;
        }
        #endregion


        #region Type Conversions
        private FontFamily FontFamilyFromString()
        {
            FontFamily fontfamily = Fonts.SystemFontFamilies.First();

            var fontConverter = new FontFamilyConverter();
            var fontFamilyUnchecked = fontConverter.ConvertFromString(customBoard.Font) as FontFamily;
            if (fontFamilyUnchecked != null)
            {
                fontfamily = fontFamilyUnchecked;
            }

            return fontfamily;
        }
        private SolidColorBrush ConvertColorFromString(string colorString)
        {
            var converter = new BrushConverter();

            var color = converter.ConvertFromString(colorString) as SolidColorBrush;
            if (color != null)
            {
                return color;
            }
            throw new Exception();
        }
        private SolidColorBrush ColorFromString(string colour)
        {
            if (colour.StartsWith("Amagno"))
            {
                var brush = new SolidColorBrush();
                switch (colour)
                {
                    case "Amagno-Leuchtrot":
                        brush.Color = Color2.FromArgb(0xff, 0xeb, 0x41, 0x3c);
                        break;
                    case "Amagno-Leuchtblau":
                        brush.Color = Color2.FromArgb(0xff, 0x14, 0x64, 0xf0);
                        break;
                    case "Amagno-Hellgrau":
                        brush.Color = Color2.FromArgb(0xff, 0xee, 0xee, 0xee);
                        break;
                    case "Amagno-Schwarz":
                        brush.Color = Color2.FromArgb(0xff, 0x44, 0x44, 0x44);
                        break;
                    case "Amagno-Orange":
                        brush.Color = Color2.FromArgb(0xff, 0xff, 0xc3, 0x0f);
                        break;
                    case "Amagno-Purpur":
                        brush.Color = Color2.FromArgb(0xff, 0xc8, 0x4b, 0xc8);
                        break;
                    case "Amagno-Grün":
                        brush.Color = Color2.FromArgb(0xff, 0x32, 0xaa, 0x5a);
                        break;
                }
                return brush;
            }
            else
            {
                return ConvertColorFromString(colour);
            }
        }
        #endregion


        #region Game Logic
        private void OnCheckOrUncheck(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                var i = buttons.IndexOf(btn);
                ChangeButtonForeground(btn);
                gameBLL.CheckOrUncheckSquare(customBoard.Squares[i]);

                customBoard = gameBLL.GetBoard();
                BingoCounter.Text = gameBLL.CheckForBingo().ToString();
            }
        }        
        private void ChangeButtonForeground(Button btn)
        {
            if (btn.Foreground == Brushes.Transparent)
            {
                if (!(customBoard.BackgroundColour == "Amagno-Rot") && !(customBoard.FontColour == "Amagno-Rot"))
                {
                    btn.Foreground = new SolidColorBrush(Color2.FromArgb(0xff, 0xeb, 0x41, 0x3c));
                }
                else if (!(customBoard.BackgroundColour == "Amagno-Blau") && !(customBoard.FontColour == "Amagno-Blau"))
                {
                    btn.Foreground = new SolidColorBrush(Color2.FromArgb(0xff, 0x14, 0x64, 0xf0));
                }
                else
                {
                    btn.Foreground = Brushes.White;
                }
            }
            else
            {
                btn.Foreground = Brushes.Transparent;
            }
        }
        private void EnterWordsToGrid()
        {
            for (int i = 0; i < customBoard.Squares.Count; i++)
            {
                textBlocks[i].Text = customBoard.Squares[i].Text;
            }
        }
        #endregion


        #region Page Options 
        private void OnRestart(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Wenn das Board neu gemischt wird, werden evtl. angehakte Kästchen zurückgesetzt. Ggfs. werden einige Einträge auch durch andere ersetzt." +
                " Möchtest du das Board trotzdem neu mischen?", "Warnung", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                RandomizeBoard();
            }
        }
        private void RandomizeBoard()
        {
            gameBLL.RandomizeBoard();
            customBoard = gameBLL.GetBoard();

            foreach (var btn in buttons)
            {
                btn.Foreground = Brushes.Transparent;
            }

            foreach (var tB in textBlocks)
            {
                tB.Text = customBoard.Squares[textBlocks.IndexOf(tB)].Text;
            }

            BingoCounter.Text = "0";
        }

        private void OnCreateNew(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Wenn du ein neues Board erstellen möchtest, geht der aktuelle Fortschritt unwiderruflich verloren. Möchtest du trotzdem fortfahren?", "Warnung", 
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                var parent = (MainWindow)Parent;
                var service = parent.serviceProvider.GetService<IBoardCreation>();
                parent.Navigate(service);
                gameBLL.DeleteCurrentSave();
            }        
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            var par = (MainWindow)Parent;
            par.Close();
        }
        #endregion
    }

    public interface IPlayBingo { }
}