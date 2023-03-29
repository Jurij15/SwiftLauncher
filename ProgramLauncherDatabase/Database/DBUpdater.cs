using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SulfurLauncher.Database
{
    public class DBUpdater
    {
        public void UpdateApp(string AppID, string AppName, string ExecutablePath, string AppCategory = "", string Notes = "", string AppLaunchArguments = "")
        {
            //tried using chatgpt for this, its good
            string command = "UPDATE App SET AppName = @AppName, ExecutablePath = @ExecutablePath, AppCategory = @AppCategory, Notes = @Notes, AppLaunchArguments = @AppLaunchArguments WHERE ID = @id";
            using (var cmd = new SQLiteCommand(command, Config.ESQLiteConnection))
            {
                cmd.Parameters.AddWithValue("@AppName", AppName);
                cmd.Parameters.AddWithValue("@ExecutablePath", ExecutablePath);
                cmd.Parameters.AddWithValue("@AppCategory", AppCategory);
                cmd.Parameters.AddWithValue("@Notes", Notes);
                cmd.Parameters.AddWithValue("@AppLaunchArguments", AppLaunchArguments);
                cmd.Parameters.AddWithValue("@id", AppID);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
