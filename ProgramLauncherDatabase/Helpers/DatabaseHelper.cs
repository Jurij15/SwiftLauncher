using SulfurLauncher.Database;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace SulfurLauncher.Helpers
{
    public class DatabaseHelper
    {
        public static bool bDoesNameAlreadyExist(string AppName)
        {
            if (!DatabaseHelper.tableAlreadyExists()) { return false; } //there is no table yet, create it
            bool RetValue = false;
            DBReader reader = new DBReader();
            reader.AddAllAppNamesToArray();
            foreach (var name in Config.AllAppsNamesList)
            {
                if (name == AppName)
                {
                    RetValue = true;
                }
                else
                {
                    continue;
                }
            }

            return RetValue;
        }

        public static bool tableAlreadyExists() //https://stackoverflow.com/questions/45344744/how-to-check-if-a-table-exists-in-c-sharp
        {
            var sql =
            "SELECT name FROM sqlite_master WHERE type='table' AND name='App';";
            if (Config.ESQLiteConnection.State == System.Data.ConnectionState.Open)
            {
                SQLiteCommand command = new SQLiteCommand(sql, Config.ESQLiteConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    return true;
                }
                return false;
            }
            else
            {
                throw new System.ArgumentException("Data.ConnectionState must be open");
            }
        }
    }
}
