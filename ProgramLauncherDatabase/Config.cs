using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Controls.Window;

namespace SulfurLauncher
{
    public class Config
    {
        public static SQLiteConnection ESQLiteConnection;
        public static bool bIsConnectedToDB = false;
        public static string SQLiteConnectionPath = "Data Source=LauncherDatabase.db;Version=3;";

        public static NavigationView GlobalNavigation;
        //public static Frame GlobalFrame;
        public static FluentWindow MainWindow;
        public static Dialog WhatsNewDialog;

        public static string NavHeaderText { get; set; }

        public static List<string> AllAppsIDsList = new List<string>();
        public static List<string> AllAppsNamesList = new List<string>();

        public static List<string> AllGamesIDsList = new List<string>();
        public static List<string> AllGamesNamesList = new List<string>();

        public static string CurrentlySelectedAppID = string.Empty;

        public static bool bShouldRefreshAllContacts = false;

        public static bool bIsQuickLauncherVisible { get; set; }

        public static string CurrentID = string.Empty;
        public static bool bOnlyStartQuickLauncher = false;

        public static string VersionString = "0.6.7";

        #region Funcs
        public static void InitDBConnection()
        {
            ESQLiteConnection = new SQLiteConnection(Config.SQLiteConnectionPath);
            ESQLiteConnection.Open();
        }

        public static void DisconnectFromDB()
        {
            ESQLiteConnection.Close();
            bIsConnectedToDB = false;
        }

        public static void RestartApp()
        {
            System.Windows.Forms.Application.Restart();
            System.Windows.Application.Current.Shutdown();
        }
        #endregion
    }
}
