using System;
using System.Collections.Generic;
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
using JustTag2.Tagging;
using JustTag2.Views;

namespace JustTag2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainPage mainPage;

        public MainWindow()
        {
            // Configure dependency injection
            mainPage = new MainPage
            (
                taggingService: new JsonTaggingService
                (
                    fs: new System.IO.Abstractions.FileSystem()
                )
            );

            InitializeComponent();
            this.Content = mainPage;
        }
    }
}
