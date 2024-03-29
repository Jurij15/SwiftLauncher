﻿using SwiftLauncher.Database;
using SwiftLauncher.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
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
using System.Xml.Serialization;
using System.IO;
using System.Net;
using SwiftLauncher.Helpers;
using System.Windows.Automation.Peers;
using System.Data.Entity;
using System.IO.Packaging;
using SwiftLauncher.Classes;

namespace SwiftLauncher.Pages
{
    /// <summary>
    /// Interaction logic for AllAppsPage.xaml
    /// </summary>
    public partial class AllAppsPage : Page
    {
        bool bPageLoaded = false;
        bool bShowAppDetails = true;
        #region Cards and rows, columns
        void CreateCard(string AccountName, string AppCategory, string AppPath)
        {
            Wpf.Ui.Controls.CardAction NewCard = new Wpf.Ui.Controls.CardAction();
            StackPanel cardHeaderPanel = new StackPanel();
            Wpf.Ui.Controls.Button DirectLaunchBtn = new Wpf.Ui.Controls.Button();
            Wpf.Ui.Controls.Button OptionsBtn  = new Wpf.Ui.Controls.Button();

            Wpf.Ui.Controls.SymbolIcon icon = new Wpf.Ui.Controls.SymbolIcon();
            TextBlock tb = new TextBlock();
            TextBlock CategoryBox = new TextBlock();
            System.Drawing.Icon icn = null;
            Image img = new Image();

            bool bShouldShowIcon = false;

            try
            {
                if (!string.IsNullOrEmpty(AppPath)) { icn = System.Drawing.Icon.ExtractAssociatedIcon(AppPath); bShouldShowIcon = true; }
            }
            catch (Exception ex)
            {
                //an exception occured
                bShouldShowIcon=false;
            }

            if (bShouldShowIcon)
            {
                icon.Visibility = Visibility.Collapsed;
                img.Visibility = Visibility.Visible;
                img.Source = Imaging.CreateBitmapSourceFromHIcon(icn.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                img.Height = 50;
                img.Width = 70;

                RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.Fant);
            }
            else
            {
                icon.Symbol = Wpf.Ui.Common.SymbolRegular.Apps24;
                icon.Visibility = Visibility.Visible;
                img.Visibility = Visibility.Collapsed;
            }

            tb.Text = AccountName;
            tb.FontWeight = FontWeights.SemiBold;
            tb.Name = "AppNameBox";
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            //tb.MaxWidth = 130;
            tb.TextTrimming = TextTrimming.CharacterEllipsis;
            tb.TextWrapping = TextWrapping.NoWrap;

            CategoryBox.Foreground = Brushes.LightGray;
            CategoryBox.Text = AppCategory;
            CategoryBox.FontSize = 12;
            CategoryBox.HorizontalAlignment = HorizontalAlignment.Center;

            DirectLaunchBtn.Appearance = Wpf.Ui.Controls.ControlAppearance.Primary;
            DirectLaunchBtn.Content = "Launch";
            DirectLaunchBtn.Name = StringsHelper.ReplaceDisallowedChars(AccountName);
            DirectLaunchBtn.VerticalAlignment = VerticalAlignment.Bottom;
            DirectLaunchBtn.HorizontalAlignment = HorizontalAlignment.Center;
            DirectLaunchBtn.Click += DirectLaunch_Click;
            DirectLaunchBtn.Margin = new Thickness(2,4,2, 0);
            DirectLaunchBtn.ToolTip = "Launch " + AccountName;

            OptionsBtn.Appearance = Wpf.Ui.Controls.ControlAppearance.Secondary;
            OptionsBtn.Icon = Wpf.Ui.Common.SymbolRegular.MoreVertical16;
            OptionsBtn.VerticalAlignment = VerticalAlignment.Top;
            OptionsBtn.HorizontalAlignment = HorizontalAlignment.Right;
            OptionsBtn.Height = 40;
            OptionsBtn.Width = 30;
            OptionsBtn.Margin = new Thickness(-15, -20, -12, -10);
            OptionsBtn.Click += OptionsBtn_Click;

            cardHeaderPanel.Children.Add(OptionsBtn);
            cardHeaderPanel.Children.Add(img);
            cardHeaderPanel.Children.Add(icon);
            cardHeaderPanel.Children.Add(tb);
            cardHeaderPanel.Children.Add(CategoryBox);
            cardHeaderPanel.Children.Add(DirectLaunchBtn);

            NewCard.Content = cardHeaderPanel;

            NewCard.Click += CardClicked_Handler;

            NewCard.MaxHeight = 250;
            NewCard.MaxWidth = 230;

            //change these over h=200 w=180 if showing options button!
            //default h=170 w=150
            NewCard.Height = 170; 
            NewCard.Width = 150;

            NewCard.ToolTip = "View details about " + AccountName;

            NewCard.Margin = new Thickness(4, 4, 4, 4);
            NewCard.IsChevronVisible = false;

            //NewCard.Height = 120;
            //NewCard.Width = 120;

            //RootWrapPanel.Children.Add(NewCard);
            RootWrapPanel.Children.Add(NewCard);
        }

        private void OptionsBtn_Click(object sender, RoutedEventArgs e)
        {
            bShowAppDetails = false;
            ContextMenu ctx = new ContextMenu();

            Wpf.Ui.Controls.MenuItem DeleteAppMenuItem = new Wpf.Ui.Controls.MenuItem();
            DeleteAppMenuItem.Header = "Remove App";
            DeleteAppMenuItem.SymbolIcon = Wpf.Ui.Common.SymbolRegular.Delete24;


            ctx.Items.Add(DeleteAppMenuItem);

            ctx.IsOpen = true;

            ((Wpf.Ui.Controls.Button)sender).ContextMenu = ctx;
        }
        #endregion

        string FigureOutAppName(Wpf.Ui.Controls.CardAction UiElement)
        {
            string RetValue = string.Empty;
            if (ViewComboBox.SelectedItem == ViewNormal)
            {
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
            }
            else if (ViewComboBox.SelectedItem == ViewExpanded)
            {
                StackPanel content = (StackPanel)UiElement.Content;
                foreach (var item in content.Children)
                {
                    if (item.GetType() == typeof(Grid))
                    {
                        Grid grid = (Grid)item;
                        foreach (var iitem in grid.Children)
                        {
                            if (iitem.GetType() == typeof(StackPanel))
                            {
                                foreach (var iiitem in ((StackPanel)iitem).Children)
                                {
                                    if (iiitem.GetType() == typeof(TextBlock))
                                    {
                                        TextBlock textBlock = (TextBlock)iiitem;
                                        if (textBlock.Name == "AppNameBox")
                                        {
                                            RetValue = textBlock.Text;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (ViewComboBox.SelectedItem == ViewMax)
            {

            }
            return RetValue;
        }

        void AddCardsForEveryApp()
        {
            RootWrapPanel.Children.Clear();
            if (SortAsc.IsSelected || SortDesc.IsSelected)
            {
                foreach (var name in Config.AllAppsNamesList)
                {
                    DBReader reader = new DBReader();
                    string id = reader.GetAppIDByName(name);

                    AppCardActionClass appCardActionClass = new AppCardActionClass(id);
                    if (ViewComboBox.SelectedItem == ViewNormal)
                    {
                        RootWrapPanel.Children.Add(appCardActionClass.CreateStandardCard(Enums.CardDisplayMode.Normal, DirectLaunch_Click, OptionsBtn_Click, CardClicked_Handler));
                    }
                    else if (ViewComboBox.SelectedItem == ViewExpanded)
                    {
                        RootWrapPanel.Children.Add(appCardActionClass.CreateStandardCard(Enums.CardDisplayMode.Expanded, DirectLaunch_Click, OptionsBtn_Click, CardClicked_Handler));
                    }
                    else if (ViewComboBox.SelectedItem == ViewMax)
                    {
                        RootWrapPanel.Children.Add(appCardActionClass.CreateStandardCard(Enums.CardDisplayMode.Max, DirectLaunch_Click, OptionsBtn_Click, CardClicked_Handler));
                    }
                }
            }
            else
            {
                foreach (var id in Config.AllAppsIDsList)
                {
                    DBReader reader = new DBReader();
                    AppCardActionClass appCardActionClass = new AppCardActionClass(id);
                    if (ViewComboBox.SelectedItem == ViewNormal)
                    {
                        RootWrapPanel.Children.Add(appCardActionClass.CreateStandardCard(Enums.CardDisplayMode.Normal, DirectLaunch_Click, OptionsBtn_Click, CardClicked_Handler));
                    }
                    else if (ViewComboBox.SelectedItem == ViewExpanded)
                    {
                        RootWrapPanel.Children.Add(appCardActionClass.CreateStandardCard(Enums.CardDisplayMode.Expanded, DirectLaunch_Click, OptionsBtn_Click, CardClicked_Handler));
                    }
                    else if (ViewComboBox.SelectedItem == ViewMax)
                    {
                        RootWrapPanel.Children.Add(appCardActionClass.CreateStandardCard(Enums.CardDisplayMode.Max, DirectLaunch_Click, OptionsBtn_Click, CardClicked_Handler));
                    }
                }
            }
        }

        void FreeArrays()
        {
            Config.AllAppsIDsList.Clear();
            Config.AllAppsNamesList.Clear();
        }

        public AllAppsPage()
        {
            InitializeComponent();
            //set views (by user saved settings TODO)
            ViewComboBox.SelectedItem = ViewNormal;
        }

        void RefreshPage()
        {
            DBReader reader = new DBReader();
            FreeArrays();
            reader.AddAllAppIDsToArray();
            if (SortAsc.IsSelected)
            {
                reader.AddAllAppNamesToArrayAndSort(Enums.SortBy.Ascending);
            }
            else if (SortDesc.IsSelected)
            {
                reader.AddAllAppNamesToArrayAndSort(Enums.SortBy.Descending);
            }
            else
            {
                reader.AddAllAppNamesToArray();
            }
            AddCardsForEveryApp();
        }

        private void AddAppCard_Click(object sender, RoutedEventArgs e)
        {
            Config.GlobalNavigation.GoBack();
        }

        public void DrawBulkAddUI()
        {
            StackPanel panel = new StackPanel();
            ListView listBox = new ListView();

            listBox.SelectionMode = SelectionMode.Multiple;

            foreach (var app in StartMenuApps.GetAllApps())
            {
                listBox.Items.Add(System.IO.Path.GetFileNameWithoutExtension(app));
            }

            listBox.Margin = new Thickness(2, 2, 2, 2);

            Config.WhatsNewDialog.Content = listBox;
            Config.WhatsNewDialog.Title = "Select apps to add";
            //Config.WhatsNewDialog.Message = "These apps are installed on your computer";

            Config.WhatsNewDialog.ButtonLeftAppearance = Wpf.Ui.Controls.ControlAppearance.Primary;
            Config.WhatsNewDialog.ButtonLeftVisibility = Visibility.Visible;
            Config.WhatsNewDialog.ButtonLeftName = "Add";
            Config.WhatsNewDialog.ButtonLeftClick += BulkAddAppsDialog_ButtonLeftClick;

            Config.WhatsNewDialog.ButtonRightAppearance = Wpf.Ui.Controls.ControlAppearance.Secondary;
            Config.WhatsNewDialog.ButtonRightName = "Close";
            Config.WhatsNewDialog.ButtonRightClick += BulkAddAppsDialog_ButtonRightClick;

            Config.WhatsNewDialog.DialogHeight = 450;
            Config.WhatsNewDialog.ShowAndWaitAsync();
        }

        private void CardClicked_Handler(object sender, RoutedEventArgs e)
        {
            if (!bShowAppDetails)
            {
                //IMPORTANT!! Always set bShowAppDetails back to TRUE!
                bShowAppDetails = true;
                return;
            }
            DBReader reader = new DBReader();
            string AppName = FigureOutAppName((Wpf.Ui.Controls.CardAction)sender);
            string AppID = reader.GetAppIDByName(AppName);

            //set app properties
            CurrentAppDefinitions.AppName = AppName;
            CurrentAppDefinitions.AppExecutablePath = reader.GetAppExecutablePathByID(AppID);
            CurrentAppDefinitions.AppNotes = reader.GetAppNotesByID(AppID);
            CurrentAppDefinitions.AppCategory = reader.GetAppCategotyByID(AppID);
            CurrentAppDefinitions.AppLaunchArguents = reader.GetAppLaunchArgumentsByID(AppID);
            Config.GlobalNavigation.Navigate(typeof(AppDetailsPage));
            MessageBox.Show("it should be navigated now");
        }

        private void DirectLaunch_Click(object sender, RoutedEventArgs e)
        {
            bShowAppDetails = false;
            DBReader reader = new DBReader();
            string AppName = StringsHelper.ReturnDisallowedChars(((Wpf.Ui.Controls.Button)sender).Name);
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
            //MessageBox.Show(StringsHelper.ReturnDisallowedChars(AppName));
        }

        private void BulkAddApps_Click(object sender, RoutedEventArgs e)
        {
            DrawBulkAddUI();
        }

        private void RootWrapPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AppsCountBlock.Text = "All Apps: " + RootWrapPanel.Children.Count.ToString();
        }

        private void AutoSuggestBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void AutoSuggestBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //user started typing
            if (string.IsNullOrEmpty(SearchBox.Text))
            {
                RefreshPage();
            }
            else
            {
                DBReader reader = new DBReader();
                FreeArrays();
                reader.FillAppIDsArrayByIfNameContains(SearchBox.Text);
                AddCardsForEveryApp();
            }
        }

        private void BulkAddAppsDialog_ButtonRightClick(object sender, RoutedEventArgs e)
        {
            Config.WhatsNewDialog.Hide();
        }

        private void BulkAddAppsDialog_ButtonLeftClick(object sender, RoutedEventArgs e)
        {
            //do not hide the dialog before all apps are added
            //MessageBox.Show(Config.WhatsNewDialog.Content.GetType().ToString());
            ListView listView = (ListView)Config.WhatsNewDialog.Content;

            foreach (var item in listView.SelectedItems)
            {
                StartMenuApps.AddAppToDBByName(item.ToString());
            }

            Config.WhatsNewDialog.Hide();

            RefreshPage();
        }

        private void SortByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!bPageLoaded)
            {
                return;
            }
            RefreshPage();
        }

        private void ClearSort_Selected(object sender, RoutedEventArgs e)
        {
            SortByComboBox.SelectedItem = null;
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!bPageLoaded)
            {
                return;
            }
            else
            {
                RefreshPage();
            }
        }

        private void ViewComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshPage();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshPage();

            bPageLoaded = true;
        }
    }
}
