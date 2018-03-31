using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationA
{
    class Integrator
    {
        private String executablePath;

        public Integrator(String executablePath) {
            this.executablePath = executablePath;
        }

        public String getImagePath(String cedula) {
            Process process = new Process();
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = executablePath;
            process.StartInfo.Arguments = cedula;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();

            if (process.ExitCode == 0) {
                return process.StandardOutput.ReadToEnd();
            } else {
                throw new Exception(process.StandardOutput.ReadToEnd());
            }
        }

        public String getExecutablePath() {
            return this.executablePath;
        }
    }
}
