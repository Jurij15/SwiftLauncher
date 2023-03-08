﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace ProgramLauncherDatabase
{
    public class Config
    {
        public static SQLiteConnection ESQLiteConnection;
        public static bool bIsConnectedToDB = false;
        public static string SQLiteConnectionPath = "Data Source=LauncherDatabase.db;Version=3;";

        public static NavigationStore GlobalNavigation;
        public static Frame GlobalFrame;

        public static List<string> AllAppsIDsList = new List<string>();
        public static List<string> AllAppsNamesList = new List<string>();

        public static string CurrentlySelectedAppID = string.Empty;

        public static bool bShouldRefreshAllContacts = false;

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
        #endregion
    }
}