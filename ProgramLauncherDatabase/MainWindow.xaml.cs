using ProgramLauncherDatabase.Database;
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

namespace ProgramLauncherDatabase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Wpf.Ui.Controls.UiWindow
    { 
        public MainWindow()
        {
            InitializeComponent();
            Config.InitDBConnection();
            Config.GlobalNavigation = MainWindowNavStore;
            Config.GlobalFrame = RootFrame;

            Wpf.Ui.Appearance.Watcher.Watch(this, Wpf.Ui.Appearance.BackgroundType.Mica, true);
        }

        private void ThemeButtonNavigation_Click(object sender, RoutedEventArgs e)
        {
            if (Wpf.Ui.Appearance.Theme.GetAppTheme() == Wpf.Ui.Appearance.ThemeType.Dark)
            {
                Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Light);
            }
            else if (Wpf.Ui.Appearance.Theme.GetAppTheme() == Wpf.Ui.Appearance.ThemeType.Light)
            {
                Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark);
            }
        }

        private void UiWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
