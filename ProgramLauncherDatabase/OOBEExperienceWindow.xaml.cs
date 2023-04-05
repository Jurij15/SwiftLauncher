using Microsoft.VisualBasic;
using SulfurLauncher.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
using System.Windows.Shapes;

namespace SulfurLauncher
{
    /// <summary>
    /// Interaction logic for OOBEExperienceWindow.xaml
    /// </summary>
    public partial class OOBEExperienceWindow : Wpf.Ui.Controls.UiWindow
    {
        public bool bDroppedOOBE = false;
        async Task PutTaskDelayWelcomeFirstTime()
        {
            await Task.Delay(2500);
        }

        async Task PutTaskDelayWelcomeBack()
        {
            await Task.Delay(50);
        }

        public OOBEExperienceWindow()
        {
            InitializeComponent();
            LScreen.BeginInit();
            OnStartup();
        }

        private async void OnStartup()
        {
            if (Settings.bIsFirstTimeUse())
            {
                await PutTaskDelayWelcomeFirstTime();
                CommonAppsHelper.CheckForCommonAppsAndAddThemToDatabase();
                Settings.CreateSettings();
                //RestartApp();
                LScreen.EndInit();
                this.Hide();
                //this.Close();
                MainWindow betterMainWindow = new MainWindow();
                betterMainWindow.Show();
                betterMainWindow.ShowActivated = true;
                betterMainWindow.ShowInTaskbar = true;

                this.Visibility = Visibility.Hidden;
                bDroppedOOBE = true;
            }
            else if (Config.bOnlyStartQuickLauncher) ///this will be read from a config file
            {
                LScreen.EndInit();
                //this.Hide();
                MainWindow betterMainWindow = new MainWindow();
                betterMainWindow.Show();
                betterMainWindow.ShowActivated = true;
                betterMainWindow.ShowInTaskbar = true;

                betterMainWindow.Hide();
                QuickLauncherWindow quickLauncherWindow = new QuickLauncherWindow();
                quickLauncherWindow.Show();
                this.Hide();

                bDroppedOOBE = true;
            }
            else
            {
                //await PutTaskDelayWelcomeBack();
                //Settings.SettingsValues.bShouldShowWelcomeBackWindow = false;
                LScreen.EndInit();
                //this.Hide();
                MainWindow betterMainWindow = new MainWindow();
                betterMainWindow.Owner = null;
                betterMainWindow.Show();
                betterMainWindow.ShowActivated = true;
                betterMainWindow.ShowInTaskbar = true;

                this.Hide();

                bDroppedOOBE = true;
                //this.Close();
            }
        }

        private void UiWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (bDroppedOOBE)
            {
                this.Hide();
                this.Visibility = Visibility.Hidden;
            }
        }

        private void UiWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Application.Current.Shutdown();
        }
    }
}
