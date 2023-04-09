using SulfurLauncher.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Appearance;

namespace SulfurLauncher.Pages
{
    /// <summary>
    /// Interaction logic for AboutPage.xaml
    /// </summary>
    public partial class AboutPage : Page
    {
        public AboutPage()
        {
            InitializeComponent();

            AppVerBox.Text = Config.VersionDouble.ToString();

            string QuickLauncherPOS = Settings.GetPosition();
            if (QuickLauncherPOS == "TOP")
            {
                TopPosition.IsChecked = true;
            }
            else if(QuickLauncherPOS == "BOTTOM")
            {
                BottomPosition.IsChecked = true;
            }
            else
            {
                Settings.ChangePosition("TOP");
            }

            if (Wpf.Ui.Appearance.Theme.GetAppTheme() == ThemeType.Dark)
                DarkTheme.IsChecked = true;
            else
                LightTheme.IsChecked = true;

            ThemeCard.Visibility = Visibility.Collapsed; //due to a bug that idk how exactly to fix, i will just hide this, no need to really have this here
        }

        private void DarkTheme_Checked(object sender, RoutedEventArgs e)
        {
            Wpf.Ui.Appearance.Theme.Apply(ThemeType.Dark);
        }

        private void LightTheme_Checked(object sender, RoutedEventArgs e)
        {
            Wpf.Ui.Appearance.Theme.Apply(ThemeType.Light);
        }

        private void TopPosition_Checked(object sender, RoutedEventArgs e)
        {
            Settings.ChangePosition("TOP");
        }

        private void BottomPosition_Checked(object sender, RoutedEventArgs e)
        {
            Settings.ChangePosition("BOTTOM");
        }
    }
}
