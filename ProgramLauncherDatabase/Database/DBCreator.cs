using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProgramLauncherDatabase.Database
{
    public class DBCreator
    {
        public void CreateApp(string AppName, string ExecutablePath, string AppCategory = "" ,string Notes = "", string AppLaunchArguments = "")
        {
            string CreateTableSqlCommand = "CREATE TABLE IF NOT EXISTS App (ID INTEGER PRIMARY KEY AUTOINCREMENT, AppName STRING, ExecutablePath STRING, AppCategory STRING, Notes STRING, AppLaunchArguments STRING)";
            SQLiteCommand CreateCommand = new SQLiteCommand(CreateTableSqlCommand, Config.ESQLiteConnection);
            CreateCommand.ExecuteNonQuery();

            string InsertSqlCommand = "INSERT INTO App (AppName, ExecutablePath, AppCategory, Notes, AppLaunchArguments)" + " VALUES('" + AppName + "', '" + ExecutablePath + "', '" + AppCategory + "', '" + Notes + "', '" + AppLaunchArguments +"')";
            SQLiteCommand InsertCommand = new SQLiteCommand(InsertSqlCommand, Config.ESQLiteConnection);
            InsertCommand.ExecuteNonQuery();
        }

        public async Task CreateAppAsync(string AppName, string ExecutablePath, string AppCategory = "", string Notes = "", string AppLaunchArguments = "")
        {
            string CreateTableSqlCommand = "CREATE TABLE IF NOT EXISTS App (ID INTEGER PRIMARY KEY AUTOINCREMENT, AppName STRING, ExecutablePath STRING, AppCategory STRING, Notes STRING, AppLaunchArguments STRING)";
            SQLiteCommand CreateCommand = new SQLiteCommand(CreateTableSqlCommand, Config.ESQLiteConnection);
            await CreateCommand.ExecuteNonQueryAsync();

            string InsertSqlCommand = "INSERT INTO App (AppName, ExecutablePath, AppCategory, Notes, AppLaunchArguments)" + " VALUES('" + AppName + "', '" + ExecutablePath + "', '" + AppCategory + "', '" + Notes + "', '" + AppLaunchArguments + "')";
            SQLiteCommand InsertCommand = new SQLiteCommand(InsertSqlCommand, Config.ESQLiteConnection);
            await InsertCommand.ExecuteNonQueryAsync();
        }
    }
}
