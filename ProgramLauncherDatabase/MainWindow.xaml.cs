using SulfurLauncher.Database;
using SulfurLauncher.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace SulfurLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Wpf.Ui.Controls.UiWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            //OnStartup();

            Config.InitDBConnection();
            Config.GlobalNavigation = MainWindowNavStore;
            Config.GlobalFrame = RootFrame;
            Config.BulkAddDialog = BulkAddDialog;
            Config.MainWindow = this;

            Wpf.Ui.Appearance.Watcher.Watch(this, Wpf.Ui.Appearance.BackgroundType.Mica, true);

            Settings.FilterLaunchArguments();

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

        private void BulkAddDialog_ButtonLeftClick(object sender, RoutedEventArgs e)
        {
            //add all apps before restarting
            Config.RestartApp();
        }

        private void BulkAddDialog_ButtonRightClick(object sender, RoutedEventArgs e)
        {
            BulkAddDialog.Hide();
        }

        private void UiWindow_Initialized(object sender, EventArgs e)
        {
            //MessageBox.Show("here");
        }

        private void TitleBar_HelpClicked(object sender, RoutedEventArgs e)
        {
            TitleBar_MinimizeClicked(null, null);
        }

        private void TitleBar_MinimizeClicked(object sender, RoutedEventArgs e)
        {
            QuickLauncherWindow test = new QuickLauncherWindow(Settings.GetPosition());
            this.Owner = null;
            test.Owner = null;
            test.ShowInTaskbar = false;
            this.Hide();
            //MessageBox.Show("showing");
            test.Show();
        }

        private void UiWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Config.bOnlyStartQuickLauncher)
            {
                QuickLauncherWindow test = new QuickLauncherWindow(Settings.GetPosition());
                this.Owner = null;
                test.Owner = null;
                test.ShowInTaskbar = false;
                this.Hide();
                //MessageBox.Show("showing");
                test.Show();
            }
        }
    }
}
