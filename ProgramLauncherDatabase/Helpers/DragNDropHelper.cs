using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Shell;
using SulfurLauncher.Database;
using SulfurLauncher.Helpers;

namespace SwiftLauncher.Helpers
{
    public class DragNDropHelper
    {
        public static async Task AddAppByDragAndDrop(string FilePath)
        {
            string AppName = string.Empty;
            string AppExePath = string.Empty;

            if (Path.GetFileName(FilePath).Contains("lnk")) //we need to get the actuall exe path, not the shortcut
            {
                AppExePath = StringsHelper.GetLnkTarget(FilePath);
            }
            else if (Path.GetFileName(FilePath).Contains("exe"))
            {
                AppExePath = Path.GetFileName(FilePath);
            }
            else { return; }

            AppName = Path.GetFileNameWithoutExtension(AppExePath);

            DBCreator creator = new DBCreator();
            await creator.CreateAppAsync(AppName, AppExePath);
        }
    }
}
