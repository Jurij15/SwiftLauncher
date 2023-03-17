using Microsoft.VisualBasic;
using ProgramLauncherDatabase.Helpers;
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

namespace ProgramLauncherDatabase
{
    /// <summary>
    /// Interaction logic for OOBEExperienceWindow.xaml
    /// </summary>
    public partial class OOBEExperienceWindow : Wpf.Ui.Controls.UiWindow
    {
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
                MainWindow betterMainWindow = new MainWindow();
                betterMainWindow.Show();
                betterMainWindow.ShowActivated = true;
                betterMainWindow.ShowInTaskbar = true;
            }
            else if (!Settings.bIsFirstTimeUse())
            {
                await PutTaskDelayWelcomeBack();
                //Settings.SettingsValues.bShouldShowWelcomeBackWindow = false;
                LScreen.EndInit();
                this.Hide();
                MainWindow betterMainWindow = new MainWindow();
                betterMainWindow.Show();
                betterMainWindow.ShowActivated = true;
                betterMainWindow.ShowInTaskbar = true;
            }
            else
            {
                LScreen.EndInit();
                this.Hide();
                MainWindow betterMainWindow = new MainWindow();
                betterMainWindow.Show();
                betterMainWindow.ShowActivated = true;
                betterMainWindow.ShowInTaskbar = true;
            }
        }
    }
}
