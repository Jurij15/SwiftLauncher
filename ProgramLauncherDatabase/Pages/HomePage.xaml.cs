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

namespace SulfurLauncher.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void TemplateCard1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddAppCard_Click(object sender, RoutedEventArgs e)
        {
            Config.GlobalFrame.Navigate(new AddAppPage());
        }

        private void ViewAllAppsCard_Click(object sender, RoutedEventArgs e)
        {
            Config.GlobalNavigation.Navigate(typeof(AllAppsPage));
        }
    }
}
