using System;
using System.Diagnostics;
using System.IO;

namespace ValuesFromFolderPlugin
{
    class Program
    {
        // Helm Env variables
        const string helmBinEnv = "HELM_BIN";

        /// <summary>
        /// Method called when command is invoked.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var helmPath = Environment.GetEnvironmentVariable(helmBinEnv);
            // Build the command to run
            var cmd = BuildHelmCommand(args);
            // Start the process which will execute the command.
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processStartInfo.FileName = helmPath;
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = cmd;
            Process process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
        }

        static string BuildHelmCommand(string[] args)
        {
            string cmd = string.Empty;
            bool isNextFolderPath = false;
            foreach(var arg in args)
            {
                if(arg == "--folder")
                {
                    isNextFolderPath = true;
                }
                else
                {
                    if(isNextFolderPath)
                    {
                        foreach(var file in Directory.GetFiles(arg, "*.yaml", SearchOption.AllDirectories))
                        {
                            cmd = $"{cmd} -f {file}";
                        }

                        isNextFolderPath = false;
                    }
                    else
                    {
                        cmd = $"{cmd} {arg}";
                    }
                }
            }
            return cmd;
        }
    }
}
