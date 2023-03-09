using ProgramLauncherDatabase.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Linq;
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
using System.Windows.Threading;

namespace ProgramLauncherDatabase.Pages
{
    /// <summary>
    /// Interaction logic for AppDetailsPage.xaml
    /// </summary>
    public partial class AppDetailsPage : Wpf.Ui.Controls.UiPage
    {
        System.Drawing.Icon icn = null;

        public AppDetailsPage()
        {
            InitializeComponent();

            SetAppDetails();
            SetAppEditValues();

            DispatcherTimer timer = new DispatcherTimer();  
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += timer_tick;
            timer.Start();
        }

        void SetAppEditValues()
        {
            AppNameBoxEdit.Text = CurrentAppDefinitions.AppName;
            AppPathBoxEdit.Text = CurrentAppDefinitions.AppExecutablePath;
            AppNotesBoxEdit.Text = CurrentAppDefinitions.AppNotes;
            AppCategoryBoxEdit.Text = CurrentAppDefinitions.AppCategory;
            LaunchArgumentsBoxEdit.Text = CurrentAppDefinitions.AppLaunchArguents;
        }

        void SetAppIcon()
        {
            if (!string.IsNullOrEmpty(CurrentAppDefinitions.AppExecutablePath)) { icn = System.Drawing.Icon.ExtractAssociatedIcon(CurrentAppDefinitions.AppExecutablePath);}
            if (icn != null)
            {
                AppImage.Source = Imaging.CreateBitmapSourceFromHIcon(icn.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            else { return;}
        }

        void SetAppDetails()
        {
            DBReader reader = new DBReader();
            AppIDBlock.Text = reader.GetAppIDByName(CurrentAppDefinitions.AppName);
            AppNameBlock.Text = CurrentAppDefinitions.AppName;
            AppPathBlock.Text = CurrentAppDefinitions.AppExecutablePath;
            AppNotesBox.Text = CurrentAppDefinitions.AppNotes;
            AppCategoryBadge.Content = CurrentAppDefinitions.AppCategory;
            AppLaunchArgsBox.Text = CurrentAppDefinitions.AppLaunchArguents;

            SetAppIcon();
        }

        private void timer_tick(object sender, EventArgs e)
        {
            //this will refresh things
            SetAppDetails();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Config.GlobalFrame.Navigate(new AllAppsPage());
        }

        private void ExploreBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = ".EXE Files (*.exe)|*.exe|All files (*.*)|*.*";
            openFileDialog.ShowDialog();
            openFileDialog.Multiselect = false;

            AppPathBoxEdit.Text = openFileDialog.FileName;
        }

        private void UpdateDBBtn_Click(object sender, RoutedEventArgs e)
        {
            DBUpdater updater = new DBUpdater();
            updater.UpdateApp(AppIDBlock.Text, AppNameBoxEdit.Text, AppPathBoxEdit.Text, AppCategoryBoxEdit.Text, AppNotesBoxEdit.Text, LaunchArgumentsBoxEdit.Text);
            Config.GlobalFrame.Navigate(new AllAppsPage());
        }
    }

    public static class CurrentAppDefinitions
    {
        public static string AppName { get; set; }
        public static string AppExecutablePath { get; set;}
        public static string AppCategory { get; set;}
        public static string AppNotes { get; set;}
        public static string AppLaunchArguents { get; set;}
    }
}
