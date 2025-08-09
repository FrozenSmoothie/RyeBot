using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RyeBot.Services
{
    public class PythonScriptService
    {
        private readonly string _pythonExePath;

        public PythonScriptService(IConfigurationRoot config)
        {
            // Set the path to your python.exe executable
            _pythonExePath = config["python:pathToExecutable"];
        }

        public async Task<string> RunPythonScriptAsync(string scriptPath)
        {
            string executableDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine("Executable Directory: " + executableDirectory);

            // Create a process start info
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = _pythonExePath,
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
                string pythonScriptResult = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();

                // Wait for the process to exit asynchronously
                await process.WaitForExitAsync();

                // Display error if any
                if (!string.IsNullOrEmpty(error))
                {
                    Console.WriteLine("Error: " + error);
                }

                return pythonScriptResult;
            }
        }
    }
}

