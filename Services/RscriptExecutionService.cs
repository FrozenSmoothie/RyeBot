using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RyeBot.Services
{
    public class RscriptService
    {
        private readonly string _rscriptPath;

        public RscriptService(
            IConfigurationRoot config)
        {
            // Set the path to your Rscript executable
            _rscriptPath = config["rscript:pathToExecutable"];
        }

        public async Task<string> RunRScriptAsync(string scriptPath)
        {
            string executableDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine("Executable Directory: " + executableDirectory);

            // Create a process start info
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = _rscriptPath,
                Arguments = $"\"{scriptPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // Create a new process and start it
            using (Process process = new Process { StartInfo = psi })
            {
                process.Start();

                // Read the result and error streams asynchronously
                string rScriptResult = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();

                // Wait for the process to exit asynchronously
                await process.WaitForExitAsync();

                // Display error if any
                if (!string.IsNullOrEmpty(error))
                {
                    Console.WriteLine("Error: " + error);
                }

                return rScriptResult;
            }
        }
    }
}