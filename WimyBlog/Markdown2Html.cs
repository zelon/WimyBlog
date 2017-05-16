using System.IO;
using System.Diagnostics;

namespace WimyBlog
{
    public class Markdown2Html
    {
        public static string Convert(string content)
        {
            string temp_filename = Path.GetTempFileName();
            using (var stream = File.CreateText(temp_filename))
            {
                stream.Write(content);
            }

            string converted_filename = Path.GetTempFileName();
            string cmd = string.Format("pandoc -f markdown_github -t html {0} -o {1}",
                                       temp_filename, converted_filename);
            ExecuteCommandLine(cmd);

            string converted_content;
            using (var stream = File.OpenText(converted_filename))
            {
                converted_content = stream.ReadToEnd();
            }
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
