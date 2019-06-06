using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac1Cv8
{
    public static class RacInvoker
    {
        public static StreamReader Run(string RacFile, string RacCmd)
        {
            ProcessStartInfo start       = new ProcessStartInfo(RacFile, RacCmd);
            start.UseShellExecute        = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError  = true;
            start.CreateNoWindow         = true;

            using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(start))
            {
                return process.StandardOutput;
            }
        }

        public static StreamReader RunWithErrCheck(string RacFile, string RacCmd)
        {
            ProcessStartInfo start = new ProcessStartInfo(RacFile, RacCmd);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            start.CreateNoWindow = true;

            using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(start))
            {
                if (!process.StandardError.EndOfStream)
                {
                    throw new Exception(process.StandardError.ReadToEnd());
                }

                return process.StandardOutput;
            }
        }

        public static void CloseStreamReader(StreamReader sr)
        {
            if (sr != null)
            {
                sr.Close();
            }
        }
    }
}
