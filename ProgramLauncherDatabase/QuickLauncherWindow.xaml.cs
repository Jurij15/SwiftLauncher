using SulfurLauncher.Database;
using SulfurLauncher.Helpers;
using SulfurLauncher.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SulfurLauncher
{
    /// <summary>
    /// Interaction logic for QuickLauncherWindow.xaml
    /// </summary>
    public partial class QuickLauncherWindow : Wpf.Ui.Controls.UiWindow
    {
        void CreateCard(string AccountName, string AppPath)
        {
            Wpf.Ui.Controls.CardAction NewCard = new Wpf.Ui.Controls.CardAction();
            StackPanel cardHeaderPanel = new StackPanel();

            TextBlock tb = new TextBlock();
            System.Drawing.Icon icn = null;
            Image img = new Image();

            bool bShouldShowIcon = false;

            if (!string.IsNullOrEmpty(AppPath)) { icn = System.Drawing.Icon.ExtractAssociatedIcon(AppPath); bShouldShowIcon = true; }

            if (bShouldShowIcon)
            {
                img.Visibility = Visibility.Visible;
                img.Source = Imaging.CreateBitmapSourceFromHIcon(icn.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                img.Height = 70;
                img.Width = 90;

                RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.Fant);
            }

            tb.Text = AccountName;
            tb.FontWeight = FontWeights.SemiBold;
            tb.Name = "AppNameBox";

            cardHeaderPanel.Children.Add(img);
            cardHeaderPanel.Children.Add(tb);

            NewCard.Content = cardHeaderPanel;

            NewCard.Click += CardClicked_Handler;

            NewCard.Margin = new Thickness(4,4,4,4);
            NewCard.IsChevronVisible = false;

            //NewCard.Height = 120;
            //NewCard.Width = 120;

            //RootWrapPanel.Children.Add(NewCard);
            AppsPanel.Children.Add(NewCard);
        }
        public QuickLauncherWindow()
        {
            InitializeComponent();
            this.Width = 640;
            this.Height = 480;

            // Set the window to be centered horizontally
            this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2;

            // Set the window to be at the top of the screen
            this.Top = 0;
            //QuickTitleBar.MinimizeToTray = true;
            CreateAppsForEveryApp();
        }

        void CreateAppsForEveryApp()
        {
            DBReader Reader = new DBReader();
            foreach (var app in Settings.GetAllQuickLaunchAppIDS())
            {
                CreateCard(Reader.GetAppNameByID(app), Reader.GetAppExecutablePathByID(app));
            }
        }

        private void TaskBarIcon_LeftClick(Wpf.Ui.Controls.NotifyIcon sender, RoutedEventArgs e)
        {
            MessageBox.Show("CLicked");
            e.Handled = true;
            return;
        }

        private void UiWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /*
            this.Hide();
            Config.MainWindow.Show();
            Config.MainWindow.WindowState = WindowState.Normal;
            */
        }

        private void ReturnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Config.MainWindow.Show();
            Config.MainWindow.WindowState = WindowState.Normal;
        }

        string FigureOutAppName(Wpf.Ui.Controls.CardAction UiElement)
        {
            string RetValue = string.Empty;
            StackPanel content = (StackPanel)UiElement.Content;
            foreach (var item in content.Children)
            {
                if (item.GetType() == typeof(TextBlock))
                {
                    TextBlock textBlock = (TextBlock)item;
                    if (textBlock.Name == "AppNameBox")
                    {
                        RetValue = textBlock.Text;
                    }
                }
            }
            return RetValue;
        }

        private void ExitCompletely_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CardClicked_Handler(object sender, RoutedEventArgs e)
        {
            DBReader reader = new DBReader();
            string AppName = FigureOutAppName((Wpf.Ui.Controls.CardAction)sender);
            string AppID = reader.GetAppIDByName(AppName);
            string AppPath = reader.GetAppExecutablePathByID(AppID);
            string AppLaunchArguments = reader.GetAppLaunchArgumentsByID(AppID);

            if (!string.IsNullOrEmpty(AppLaunchArguments))
            {
                Process.Start(AppPath, AppLaunchArguments);
            }
            else
            {
                Process.Start(AppPath);
            }
        }
    }
}
