using SwiftLauncher.Enums;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftLauncher.Classes.DatabaseClasses
{
    public class DBInteractor
    {
        public DBInteractor() 
        {

        }

        public static string GetAppPropertyFromDBByID(string ID, DatabaseAppProperty Property)
        {
            string ReturnValue = "";
            string Command = "";

            switch (Property)
            {
                case DatabaseAppProperty.ID:
                    ReturnValue = ID;
                    return ReturnValue;
                case DatabaseAppProperty.Name:
                    Command = "SELECT AppName FROM App WHERE ID ==" + ID;
                    break;
                case DatabaseAppProperty.ExecutablePath:
                    Command = "SELECT ExecutablePath FROM App WHERE ID ==" + ID;
                    break;
                case DatabaseAppProperty.ApplicationCategory:
                    Command = "SELECT AppCategory FROM App WHERE ID ==" + ID;
                    break;
                case DatabaseAppProperty.ApplicationNotes:
                    Command = "SELECT Notes FROM App WHERE ID ==" + ID;
                    break;
                case DatabaseAppProperty.ApplicationLaunchArguments:
                    Command = "SELECT AppLaunchArguments FROM App WHERE ID ==" + ID;
                    break;
                case DatabaseAppProperty.ApplicationLaunchAsAdmin:
                    Command = "SELECT AppLaunchAsAdmin FROM App WHERE ID ==" + ID;
                    break;
                case DatabaseAppProperty.ApplicationShowInQuickLauncher:
                    Command = "SELECT AppShowInQuickLauncher FROM App WHERE ID ==" + ID;
                    break;
            }

            SQLiteCommand SelectCommand = new SQLiteCommand(Command, Config.ESQLiteConnection);
            SQLiteDataReader reader = SelectCommand.ExecuteReader();
            while (reader.Read())
            {
                ReturnValue = reader.GetString(0).ToString();
            }

            return ReturnValue;
        }

        //The rest of this is just moved from the old Database folder, for better consistency
        #region Readers
        #region Array stuff
        public void AddAllAppIDsToArray()
        {
            string Command = "SELECT ID FROM App";
            SQLiteCommand SelectCommand = new SQLiteCommand(Command, Config.ESQLiteConnection);
            SQLiteDataReader reader = SelectCommand.ExecuteReader();
            while (reader.Read())
            {
                Config.AllAppsIDsList.Add(Convert.ToString(reader["ID"]));
            }
        }

        public void AddAllAppNamesToArray()
        {
            string Command = "SELECT AppName FROM App";
            SQLiteCommand SelectCommand = new SQLiteCommand(Command, Config.ESQLiteConnection);
            SQLiteDataReader reader = SelectCommand.ExecuteReader();
            while (reader.Read())
            {
                Config.AllAppsNamesList.Add(Convert.ToString(reader["AppName"]));
            }
        }

        public void AddAllAppNamesToArrayAndSort(SortBy Sort)
        {
            string Command = string.Empty;
            if (Sort == SortBy.Ascending)
            {
                Command = "SELECT AppName FROM App ORDER BY AppName ASC";
            }
            else
            {
                Command = "SELECT AppName FROM App ORDER BY AppName DESC";
            }
            SQLiteCommand SelectCommand = new SQLiteCommand(Command, Config.ESQLiteConnection);
            SQLiteDataReader reader = SelectCommand.ExecuteReader();
            while (reader.Read())
            {
                Config.AllAppsNamesList.Add(Convert.ToString(reader["AppName"]));
            }
        }

        //games
        public void AddAllGamesIDsToArray()
        {
            string Name = "Games";
            string Command = "SELECT ID FROM App WHERE AppCategory =='" + Name + "';";
            SQLiteCommand SelectCommand = new SQLiteCommand(Command, Config.ESQLiteConnection);
            SQLiteDataReader reader = SelectCommand.ExecuteReader();
            while (reader.Read())
            {
                Config.AllGamesIDsList.Add(Convert.ToString(reader["ID"]));
            }
        }

        public void AddAllGamesNamesToArray()
        {
            string Name = "Games";
            string Command = "SELECT AppName FROM App WHERE AppCategory =='" + Name + "';";
            SQLiteCommand SelectCommand = new SQLiteCommand(Command, Config.ESQLiteConnection);
            SQLiteDataReader reader = SelectCommand.ExecuteReader();
            while (reader.Read())
            {
                Config.AllGamesNamesList.Add(Convert.ToString(reader["AppName"]));
            }
        }

        #endregion

        public string GetAppNameByID(string ID)
        {
            string ReturnValue = "";
            string Command = "SELECT AppName FROM App WHERE ID ==" + ID;
            SQLiteCommand SelectCommand = new SQLiteCommand(Command, Config.ESQLiteConnection);
            SQLiteDataReader reader = SelectCommand.ExecuteReader();
            while (reader.Read())
            {
                ReturnValue = reader.GetString(0);
            }

            return ReturnValue;
        }

        public string GetAppCategotyByID(string ID)
        {
            string ReturnValue = "";
            string Command = "SELECT AppCategory FROM App WHERE ID ==" + ID;
            SQLiteCommand SelectCommand = new SQLiteCommand(Command, Config.ESQLiteConnection);
            SQLiteDataReader reader = SelectCommand.ExecuteReader();
            while (reader.Read())
            {
                ReturnValue = reader.GetString(0);
            }

            return ReturnValue;
        }

        public string GetAppExecutablePathByID(string ID)
        {
            string ReturnValue = "";
            string Command = "SELECT ExecutablePath FROM App WHERE ID ==" + ID;
            SQLiteCommand SelectCommand = new SQLiteCommand(Command, Config.ESQLiteConnection);
            SQLiteDataReader reader = SelectCommand.ExecuteReader();
            while (reader.Read())
            {
                ReturnValue = reader.GetString(0);
            }

            return ReturnValue;
        }

        public string GetAppNotesByID(string ID)
        {
            string ReturnValue = "";
            string Command = "SELECT Notes FROM App WHERE ID ==" + ID;
            SQLiteCommand SelectCommand = new SQLiteCommand(Command, Config.ESQLiteConnection);
            SQLiteDataReader reader = SelectCommand.ExecuteReader();
            while (reader.Read())
            {
                ReturnValue = reader.GetString(0);
            }

            return ReturnValue;
        }

        public string GetAppLaunchArgumentsByID(string ID)
        {
            string ReturnValue = "";
            string Command = "SELECT AppLaunchArguments FROM App WHERE ID ==" + ID;
            SQLiteCommand SelectCommand = new SQLiteCommand(Command, Config.ESQLiteConnection);
            SQLiteDataReader reader = SelectCommand.ExecuteReader();
            while (reader.Read())
            {
                ReturnValue = reader.GetString(0);
            }

            return ReturnValue;
        }

        public string GetAppIDByName(string Name)
        {
            string ReturnValue = "";
            string Command = "SELECT ID FROM App WHERE AppName =='" + Name + "';";
            SQLiteCommand SelectCommand = new SQLiteCommand(Command, Config.ESQLiteConnection);
            SQLiteDataReader reader = SelectCommand.ExecuteReader();
            while (reader.Read())
            {
                ReturnValue = reader.GetInt32(0).ToString();
            }

            return ReturnValue;
        }

        public void FillAppIDsArrayByIfNameContains(string Contains)
        {
            string ReturnValue = "";
            string Command = "SELECT ID FROM App WHERE AppName LIKE '%" + Contains + "%';";
            SQLiteCommand SelectCommand = new SQLiteCommand(Command, Config.ESQLiteConnection);
            SQLiteDataReader reader = SelectCommand.ExecuteReader();
            while (reader.Read())
            {
                Config.AllAppsIDsList.Add(Convert.ToString(reader["ID"]));
            }
        }
        #endregion

        #region Writers
        public void CreateApp(string AppName, string ExecutablePath, string AppCategory = "", string Notes = "", string AppLaunchArguments = "", bool AppLaunchAsAdmin = false, bool AppShowInQuickLauncher = false)
        {
            string CreateTableSqlCommand = "CREATE TABLE IF NOT EXISTS App (ID INTEGER PRIMARY KEY AUTOINCREMENT, AppName STRING, ExecutablePath STRING, AppCategory STRING, Notes STRING, AppLaunchArguments STRING, AppLaunchAsAdmin STRING, AppShowInQuickLauncher STRING)";
            SQLiteCommand CreateCommand = new SQLiteCommand(CreateTableSqlCommand, Config.ESQLiteConnection);
            CreateCommand.ExecuteNonQuery();

            string InsertSqlCommand = "INSERT INTO App (AppName, ExecutablePath, AppCategory, Notes, AppLaunchArguments)" + " VALUES('" + AppName + "', '" + ExecutablePath + "', '" + AppCategory + "', '" + Notes + "', '" + AppLaunchArguments + "', '" + AppLaunchAsAdmin.ToString() + "', '" + "', '" + AppShowInQuickLauncher.ToString() + "')";
            SQLiteCommand InsertCommand = new SQLiteCommand(InsertSqlCommand, Config.ESQLiteConnection);
            InsertCommand.ExecuteNonQuery();
        }

        public async Task CreateAppAsync(string AppName, string ExecutablePath, string AppCategory = "", string Notes = "", string AppLaunchArguments = "", bool AppLaunchAsAdmin = false, bool AppShowInQuickLauncher = false)
        {
            string CreateTableSqlCommand = "CREATE TABLE IF NOT EXISTS App (ID INTEGER PRIMARY KEY AUTOINCREMENT, AppName STRING, ExecutablePath STRING, AppCategory STRING, Notes STRING, AppLaunchArguments STRING, AppLaunchAsAdmin STRING, AppShowInQuickLauncher STRING)";
            SQLiteCommand CreateCommand = new SQLiteCommand(CreateTableSqlCommand, Config.ESQLiteConnection);
            await CreateCommand.ExecuteNonQueryAsync();

            string InsertSqlCommand = "INSERT INTO App (AppName, ExecutablePath, AppCategory, Notes, AppLaunchArguments)" + " VALUES('" + AppName + "', '" + ExecutablePath + "', '" + AppCategory + "', '" + Notes + "', '" + AppLaunchArguments + "', '" + AppLaunchAsAdmin.ToString() + "', '" + "', '" + AppShowInQuickLauncher.ToString() + "')";
            SQLiteCommand InsertCommand = new SQLiteCommand(InsertSqlCommand, Config.ESQLiteConnection);
            await InsertCommand.ExecuteNonQueryAsync();
        }

        public void UpdateApp(string AppID, string AppName, string ExecutablePath, string AppCategory = "", string Notes = "", string AppLaunchArguments = "", bool AppLaunchAsAdmin = false, bool AppShowInQuickLauncher = false)
        {
            //tried using chatgpt for this, its good
            string command = "UPDATE App SET AppName = @AppName, ExecutablePath = @ExecutablePath, AppCategory = @AppCategory, Notes = @Notes, AppLaunchArguments = @AppLaunchArguments, AppLaunchAsAdmin = @AppLaunchAsAdmin, AppShowInQuickLauncher = @AppShowInQuickLauncher WHERE ID = @id";
            using (var cmd = new SQLiteCommand(command, Config.ESQLiteConnection))
            {
                cmd.Parameters.AddWithValue("@AppName", AppName);
                cmd.Parameters.AddWithValue("@ExecutablePath", ExecutablePath);
                cmd.Parameters.AddWithValue("@AppCategory", AppCategory);
                cmd.Parameters.AddWithValue("@Notes", Notes);
                cmd.Parameters.AddWithValue("@AppLaunchArguments", AppLaunchArguments);
                cmd.Parameters.AddWithValue("@id", AppID);
                cmd.Parameters.AddWithValue("@AppLaunchAsAdmin", AppLaunchAsAdmin.ToString());
                cmd.Parameters.AddWithValue("@AppShowInQuickLauncher", AppShowInQuickLauncher.ToString());
                cmd.ExecuteNonQuery();
            }
        }

        public static void RemoveAppFromDatabaseByID(string ID)
        {
            string Command = "Delete FROM App WHERE ID ==" + ID;
            SQLiteCommand SelectCommand = new SQLiteCommand(Command, Config.ESQLiteConnection);
            SelectCommand.ExecuteNonQuery();
        }

        public static async void RemoveAppFromDatabaseByIDAsync(string ID)
        {
            string Command = "Delete FROM App WHERE ID ==" + ID;
            SQLiteCommand SelectCommand = new SQLiteCommand(Command, Config.ESQLiteConnection);
            await SelectCommand.ExecuteNonQueryAsync();
        }
        #endregion
    }
}
