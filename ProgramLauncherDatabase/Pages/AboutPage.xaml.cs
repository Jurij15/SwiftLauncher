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

namespace ProgramLauncherDatabase.Pages
{
    /// <summary>
    /// Interaction logic for AboutPage.xaml
    /// </summary>
    public partial class AboutPage : Wpf.Ui.Controls.UiPage
    {
        public AboutPage()
        {
            InitializeComponent();

            AppVerBox.Text = Config.VersionDouble.ToString();
        }

        private void DarkTheme_Checked(object sender, RoutedEventArgs e)
        {
            Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark);
        }

        private void LightTheme_Checked(object sender, RoutedEventArgs e)
        {
            Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light);
        }
    }
}
