using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thesis.Helpers.AI.SpamDetection
{
    public static class SpamDetector
    {
        public static bool IsContentSpam(string text)
        {
            var currentPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            var pathToExe = currentPath + @"\..\..\ConsoleApp29\ConsoleApp29\bin\Debug\";
            System.IO.Directory.SetCurrentDirectory(pathToExe);
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "ConsoleApp29.exe";
            process.StartInfo.Arguments = "\"" + text  + "\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            //* Read the output (or the error)
            string output = process.StandardOutput.ReadToEnd();
            output = System.Text.RegularExpressions.Regex.Replace(output, @"\t|\n|\r", "");
            process.WaitForExit();
            return output == "spam" ? true : false;
        }
    }
}