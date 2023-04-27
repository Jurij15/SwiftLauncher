using SwiftLauncher.Database;
using SwiftLauncher.Helpers;
using SwiftLauncher.Pages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SwiftLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Wpf.Ui.Controls.Window.FluentWindow
    {

        public MainWindow()
        {
            InitializeComponent();

            Config.InitDBConnection();
            Config.MainWindow = this;
            Config.WhatsNewDialog = WhatsnewDialog;
            Config.GlobalNavigation = MainWindowNavStore;

            Wpf.Ui.Appearance.Watcher.Watch(this);

            Settings.FilterLaunchArguments();

            MWindowTitleBar.MinimizeToTray = true;

            if (Config.bShowDownloadTestPage)
            {
                DownloadAppsPage.Visibility = Visibility.Visible;
            }
            else
            {
                DownloadAppsPage.Visibility = Visibility.Collapsed;
            }
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
            Config.bIsQuickLauncherVisible = true;
            test.Owner = null;
            test.ShowInTaskbar = false;
            MWindowTitleBar.MinimizeToTray = true;
            this.Hide();
            //MessageBox.Show("showing");
            test.Show();
        }

        private void UiWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(Config.bOnlyStartQuickLauncher.ToString());
            if (Config.bOnlyStartQuickLauncher)
            {
                QuickLauncherWindow test = new QuickLauncherWindow(Settings.GetPosition());
                this.WindowState = WindowState.Minimized;
                Config.bIsQuickLauncherVisible = true;
                this.Owner = null;
                test.Owner = null;
                test.ShowInTaskbar = false;
                MWindowTitleBar.MinimizeToTray = true;
                this.Hide();
                //MessageBox.Show("showing");
                test.Show();
            }
        }

        private void MainWindowNavStore_PaneClosed(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("closed pane");
        }

        private void FluentWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Config.bIsQuickLauncherVisible)
            {
                e.Cancel = true;
                return;
            }
            else
            {
                Application.Current.Shutdown();
                //Environment.Exit(0);
            }
        }

        private void MWindowTitleBar_CloseClicked(object sender, RoutedEventArgs e)
        {
            FluentWindow_Closing(null, null);
        }

        private void FluentWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindowNavStore.Navigate(typeof(HomePage));
            //MessageBox.Show(Config.bOnlyStartQuickLauncher.ToString());
            if (Config.bOnlyStartQuickLauncher)
            {
                QuickLauncherWindow test = new QuickLauncherWindow(Settings.GetPosition());
                this.WindowState = WindowState.Minimized;
                Config.bIsQuickLauncherVisible = true;
                this.Owner = null;
                test.Owner = null;
                test.ShowInTaskbar = false;
                MWindowTitleBar.MinimizeToTray = true;
                this.Hide();
                //MessageBox.Show("showing");
                test.Show();
            }
        }

        private void MainWindowNavStore_PaneOpened(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("open pane");
        }

        private void MainWindowNavStore_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void DragNDropPreviewDialog_Drop(object sender, DragEventArgs e)
        {
            MessageBox.Show("Dropped!");
        }

        private void WhatsnewDialog_ButtonRightClick(object sender, RoutedEventArgs e)
        {
            WhatsnewDialog.Hide();
        }

        private void FluentWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //MessageBox.Show(this.Width.ToString());
        }
    }
}
