using SulfurLauncher.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SulfurLauncher.Database
{
    public class DBReader
    {
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
            SQLiteCommand SelectCommand = new SQLiteCommand( Command, Config.ESQLiteConnection);
            SQLiteDataReader reader = SelectCommand.ExecuteReader();
            while(reader.Read())
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
            string Command = "SELECT ID FROM App WHERE AppName =='" + Name+"';";
            SQLiteCommand SelectCommand = new SQLiteCommand(Command, Config.ESQLiteConnection);
            SQLiteDataReader reader = SelectCommand.ExecuteReader();
            while (reader.Read())
            {
                ReturnValue = reader.GetInt32(0).ToString();
            }

            return ReturnValue;
        }


    }
}
