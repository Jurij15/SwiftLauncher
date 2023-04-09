using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace SulfurLauncher
{
    public class Config
    {
        public static SQLiteConnection ESQLiteConnection;
        public static bool bIsConnectedToDB = false;
        public static string SQLiteConnectionPath = "Data Source=LauncherDatabase.db;Version=3;";

        public static NavigationCompact GlobalNavigation;
        public static Frame GlobalFrame;
        public static Dialog BulkAddDialog;
        public static UiWindow MainWindow;

        public static List<string> AllAppsIDsList = new List<string>();
        public static List<string> AllAppsNamesList = new List<string>();

        public static List<string> AllGamesIDsList = new List<string>();
        public static List<string> AllGamesNamesList = new List<string>();

        public static string CurrentlySelectedAppID = string.Empty;

        public static bool bShouldRefreshAllContacts = false;

        public static string CurrentID = string.Empty;
        public static bool bOnlyStartQuickLauncher = false;

        public static double VersionDouble = 0.6;

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
