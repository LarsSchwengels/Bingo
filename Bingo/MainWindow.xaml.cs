using Bingo.BusinessLogic;
using Bingo.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
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

namespace Bingo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        internal ServiceProvider serviceProvider = new ServiceCollection()
            .AddTransient<IBoardCreation, BoardCreation>()
            .AddTransient<IBoardCreationBLL, BoardCreationBLL>()
            .AddSingleton<IGameBLL, GameBLL>()
            .AddTransient<IPlayBingo, PlayBingo>()
            .AddSingleton<IGameRepo, GameRepo>()
            .BuildServiceProvider();


        public MainWindow()
        {
            InitializeComponent();

            var families = new InstalledFontCollection();
            if (families.Families.FirstOrDefault(f => f.Name == "Inter") == null || families.Families.FirstOrDefault(f => f.Name == "Kumbh Sans") == null ||
                families.Families.FirstOrDefault(f => f.Name == "Inter SemiBold") == null)
            {
                MessageBox.Show("Die im Installationsordner enthaltenen Sprachpakete Khumb Sans und Inter müssen vor dem Start der Anwendung installiert worden sein.", 
                    "Warnung", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            if (File.Exists(@"../CurrentSave.json"))
            {
                var service = serviceProvider.GetService<IPlayBingo>();
                Navigate(service);
            }
            else
            {
                var service = serviceProvider.GetService<IBoardCreation>();
                Navigate(service);
            }
        }
    }
}