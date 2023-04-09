using SulfurLauncher.Database;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SulfurLauncher.Pages
{
    /// <summary>
    /// Interaction logic for AddAppPage.xaml
    /// </summary>
    public partial class AddAppPage : Page
    {
        async void AddAppToDB()
        {
            string AppName = AppNameBox.Text;
            string AppPath = AppPathBox.Text;
            string AppCategory = AppCategoryBox.Text;
            string AppNotes = AppNotesBox.Text;
            string AppLaunchArguments = LaunchArgumentsBox.Text;

            if(DatabaseHelper.bDoesNameAlreadyExist(AppName)) { return; /*for now*/ }
            if(string.IsNullOrEmpty(AppName)) { return; /*for now*/ }
            if(string.IsNullOrEmpty(AppPath)) { return; /*for now*/}

            DBCreator creator = new DBCreator();
            CreatingPanel.Visibility = Visibility.Visible;
            await creator.CreateAppAsync(AppName, AppPath, AppCategory, AppNotes, AppLaunchArguments);
            CreatingPanel.Visibility = Visibility.Collapsed;

            Config.GlobalFrame.Navigate(new AllAppsPage());
        }
        public AddAppPage()
        {
            InitializeComponent();
        }

        private void AddAppBtn_Click(object sender, RoutedEventArgs e)
        {
            AddAppToDB();
        }


        private void ExploreBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = ".EXE Files (*.exe)|*.exe|All files (*.*)|*.*";
            openFileDialog.ShowDialog();
            openFileDialog.Multiselect = false;

            AppPathBox.Text = openFileDialog.FileName;
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            //Config.GlobalNavigation.NavigateBack();
        }
    }
}
