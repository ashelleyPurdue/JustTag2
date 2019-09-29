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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

using JustTag2.Views;

namespace JustTag2.Views
{
    /// <summary>
    /// Interaction logic for NavigationBar.xaml
    /// </summary>
    public partial class NavigationBar : UserControl
    {
        public MainPageViewModel ViewModel => (MainPageViewModel)DataContext;

        public NavigationBar()
        {
            InitializeComponent();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(addressBar.Text))
            {
                MessageBox.Show("Folder does not exist");
                return;
            }

            ViewModel.CurrentFolder = new DirectoryInfo(addressBar.Text);
            ViewModel.Refresh();
        }

        private void NavigationBar_EnterPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Refresh_Click(sender, null);
        }
    }
}
