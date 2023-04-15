using SulfurLauncher.Database;
using SulfurLauncher.Helpers;
using SwiftLauncher.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
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

            Config.GlobalNavigation.Navigate(typeof(AllAppsPage));
        }
        public AddAppPage()
        {
            InitializeComponent();
            backBtn.Visibility = Visibility.Collapsed;
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
            Config.GlobalNavigation.GoBack();
        }

        private async void Border_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                MessageBox.Show(files.Length.ToString());
                if (files.Length == 1)
                {
                    if (DirectAddCheck.IsChecked == true)
                    {
                        foreach (string file in files)
                        {
                            await DragNDropHelper.AddAppByDragAndDrop(file);
                        }
                    }
                    else
                    {
                        foreach (string file in files) //there is only 1 file
                        {
                            AppNameBox.Text = System.IO.Path.GetFileNameWithoutExtension(file);
                            if (file.Contains("lnk"))
                            {
                                AppPathBox.Text = StringsHelper.GetLnkTarget(file);
                            }
                            else if (file.Contains("exe"))
                            {
                                AppPathBox.Text = file;
                            }
                            else
                            {
                                e.Handled = true;
                                return;
                            }
                        }
                    }
                }
                else
                {
                    foreach (string file in files)
                    {
                        await DragNDropHelper.AddAppByDragAndDrop(file);
                    }
                }
            }
        }

        private void Border_DragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void Border_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }
    }
}
