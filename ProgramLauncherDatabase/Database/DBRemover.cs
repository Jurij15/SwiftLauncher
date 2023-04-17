using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftLauncher.Database
{
    public class DBRemover
    {
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
    }
}
