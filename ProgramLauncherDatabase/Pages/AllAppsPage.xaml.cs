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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace ProgramLauncherDatabase.Pages
{
    /// <summary>
    /// Interaction logic for AllAppsPage.xaml
    /// </summary>
    public partial class AllAppsPage : Wpf.Ui.Controls.UiPage
    {
        #region Cards and rows, columns
        //this is my solution for adding in cards dynammically 

        int OpenedRows = 0;
        int MaxRows = 0;
        int OpenedColumnsInRow = 0;
        public void AddMoreRows()
        {
            RowDefinition rd = new RowDefinition();
            RowDefinition rd2 = new RowDefinition();
            RowDefinition rd3 = new RowDefinition();
            RowDefinition rd4 = new RowDefinition();
            RootWrapPanel.RowDefinitions.Add(rd);
            RootWrapPanel.RowDefinitions.Add(rd2);
            RootWrapPanel.RowDefinitions.Add(rd3);
            RootWrapPanel.RowDefinitions.Add(rd4);
            MaxRows += 4;
        }
        void CreateCard(string AccountName, string AppCategory, string AppPath)
        {
            Wpf.Ui.Controls.CardAction NewCard = new Wpf.Ui.Controls.CardAction();
            StackPanel cardHeaderPanel = new StackPanel();

            Wpf.Ui.Controls.SymbolIcon icon = new Wpf.Ui.Controls.SymbolIcon();
            TextBlock tb = new TextBlock();
            TextBlock CategoryBox = new TextBlock();
            System.Drawing.Icon icn = null;
            Image img = new Image();

            bool bShouldShowIcon = false;

            if (!string.IsNullOrEmpty(AppPath)) { icn = System.Drawing.Icon.ExtractAssociatedIcon(AppPath); bShouldShowIcon = true; }

            if (bShouldShowIcon)
            {
                icon.Visibility = Visibility.Collapsed;
                img.Visibility = Visibility.Visible;
                img.Source = Imaging.CreateBitmapSourceFromHIcon(icn.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            else
            {
                icon.Symbol = Wpf.Ui.Common.SymbolRegular.Apps24;
                icon.Visibility = Visibility.Visible;
                img.Visibility = Visibility.Collapsed;
            }

            tb.Text = AccountName;
            tb.Name = "AppNameBox";

            CategoryBox.Foreground = Brushes.DimGray;
            CategoryBox.Text = AppCategory;
            CategoryBox.FontSize = 12;

            cardHeaderPanel.Children.Add(img);
            cardHeaderPanel.Children.Add(icon);
            cardHeaderPanel.Children.Add(tb);
            cardHeaderPanel.Children.Add(CategoryBox);

            NewCard.Content = cardHeaderPanel;

            NewCard.Click += CardClicked_Handler;

            NewCard.Margin = new Thickness(2, 2, 2, 2);

            //TODO update this so its actually dynamic
            if (OpenedColumnsInRow == 0)
            {
                OpenedRows++;
                AddMoreRows();
                OpenedColumnsInRow++;
                Grid.SetColumn(NewCard, 0);
            }
            else if (OpenedColumnsInRow == 1)
            {
                OpenedColumnsInRow++;
                Grid.SetColumn(NewCard, 1);
            }
            else if (OpenedColumnsInRow == 2)
            {
                OpenedColumnsInRow++;
                Grid.SetColumn(NewCard, 2);
            }
            else if (OpenedColumnsInRow == 3)
            {
                OpenedColumnsInRow++;
                Grid.SetColumn(NewCard, 3);
                OpenedColumnsInRow = 0;
            }
            Grid.SetRow(NewCard, OpenedRows);
            RootWrapPanel.Children.Add(NewCard);
        }
        #endregion

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

        void AddCardsForEveryApp()
        {
            RootWrapPanel.Children.Clear();
            foreach (var id in Config.AllAppsIDsList)
            {
                DBReader reader = new DBReader();
                CreateCard(reader.GetAppNameByID(id), reader.GetAppCategotyByID(id), reader.GetAppExecutablePathByID(id));
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
            DBReader reader = new DBReader();
            FreeArrays();
            reader.AddAllAppIDsToArray();
            reader.AddAllAppNamesToArray();
            AddCardsForEveryApp();
        }

        private void AddAppCard_Click(object sender, RoutedEventArgs e)
        {
            Config.GlobalFrame.Navigate(new AddAppPage());
        }

        private void CardClicked_Handler(object sender, RoutedEventArgs e)
        {
            DBReader reader = new DBReader();
            string AppName = FigureOutAppName((Wpf.Ui.Controls.CardAction)sender);
            string AppID = reader.GetAppIDByName(AppName);
            
            //set app properties
            CurrentAppDefinitions.AppName = AppName;
            CurrentAppDefinitions.AppExecutablePath = reader.GetAppExecutablePathByID(AppID);
            CurrentAppDefinitions.AppNotes = reader.GetAppNotesByID(AppID);
            CurrentAppDefinitions.AppCategory = reader.GetAppCategotyByID(AppID);
            CurrentAppDefinitions.AppLaunchArguents = reader.GetAppLaunchArgumentsByID(AppID);

            Config.GlobalFrame.Navigate(new AppDetailsPage());
        }
    }
}
