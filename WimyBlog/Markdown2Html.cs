using System.IO;
using System.Diagnostics;

namespace WimyBlog
{
    public class Markdown2Html
    {
        public static bool HasConvertableEnvironment()
        {
            try
            {
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.FileName = "pandoc.exe";
                startInfo.Arguments = "--help";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                return true;
            } catch (System.ComponentModel.Win32Exception)
            {
                // Cannot find pandoc.exe in PATH
            }
            return false;
        }

        public static string Convert(string content)
        {
            string temp_filename = Path.GetTempFileName();
            using (var stream = File.CreateText(temp_filename))
            {
                stream.Write(content);
            }

            string converted_filename = Path.GetTempFileName();
            string cmd = string.Format("pandoc -f gfm+hard_line_breaks -t html {0} -o {1}",
                                       temp_filename, converted_filename);
            ExecuteCommandLine(cmd);

            string converted_content;
            using (var stream = File.OpenText(converted_filename))
            {
                converted_content = stream.ReadToEnd();
            }
            File.Delete(temp_filename);
            return converted_content.Trim();
        }

        private static void ExecuteCommandLine(string cmd)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + cmd;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }
    }
}
