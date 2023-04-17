using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace SwiftLauncher.Downloader
{
    public class Downloader
    {
        string AppPackageID { get; set;}
        public Downloader(string ID) 
        {
            AppPackageID = ID;
        }

        public async Task<bool> Download()
        {
            bool bDownloadSuccessfull = false;
            Process shell = new Process();

            shell.StartInfo.FileName = "winget";
            shell.StartInfo.Arguments = "-install " + AppPackageID.Replace(" ", "");

            shell.StartInfo.CreateNoWindow = true;
            shell.StartInfo.RedirectStandardOutput = true;
            shell.StartInfo.UseShellExecute = false;

            shell.Start();

            string output = await shell.StandardOutput.ReadToEndAsync();

            await shell.WaitForExitAsync();

            return bDownloadSuccessfull;
        }
    }
}
