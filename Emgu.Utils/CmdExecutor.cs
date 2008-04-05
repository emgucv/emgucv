using System;
using System.Diagnostics;

namespace Emgu
{
    public class CmdExecutor
    {
        public static string execute(string execFileName, string arguments)
        {
            Process processor = new Process();

            processor.StartInfo.FileName = execFileName;
            processor.StartInfo.Arguments = arguments; 
            processor.StartInfo.UseShellExecute = false;
            processor.StartInfo.RedirectStandardOutput = true;
            processor.StartInfo.RedirectStandardError = true;

            //string error = string.Empty;
            try
            {
                processor.Start();
            }
            catch (Exception)
            {
                //error = e.Message;
            }

            //processor.BeginErrorReadLine();
            //String error2 = processor.StandardError.ReadToEnd();
            string output = processor.StandardOutput.ReadToEnd();
            
            processor.WaitForExit();
            processor.Close();

            return output;
        }
    }
}
