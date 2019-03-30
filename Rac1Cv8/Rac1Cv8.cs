using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Rac1Cv8
{
    public class Rac1Cv8
    {
        public string RacPath;
        public string ConnStr;
        public bool isConnected { get; private set; } = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public Rac1Cv8()
        {

        }

        /// <summary>
        /// Connect to Ras. Default ras port 1545. Invoke rac.exe with parameter ConnStr
        /// </summary>
        public void Connect(string RacPath, string ConnStr)
        {
            ProcessStartInfo start       = new ProcessStartInfo(RacPath,ConnStr);
            start.UseShellExecute        = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError  = true;
            start.CreateNoWindow         = true;

            using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(start))
            {
                if (!process.StandardError.EndOfStream)
                {
                    throw new Exception(process.StandardError.ReadToEnd());
                }
                else
                {
                    this.RacPath = RacPath;
                    this.ConnStr = ConnStr;
                }
            }

            isConnected = true;
        }

        public void Connect()
        {
            if (this.RacPath == null)
            {
                throw new Exception(".RacPath can not be null! ");
            }

            if (this.ConnStr == null)
            {
                throw new Exception(".ConnStr can not be null! ");
            }

            ProcessStartInfo start       = new ProcessStartInfo(this.RacPath, this.ConnStr);
            start.UseShellExecute        = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError  = true;
            start.CreateNoWindow         = true;

            using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(start))
            {
                if (!process.StandardError.EndOfStream)
                {
                    throw new Exception(process.StandardError.ReadToEnd());
                }
            }

            isConnected = true;

        }

        public List<Cluster> GetClusters()
        {
            if (!this.isConnected)
            {
                throw new Exception(".Rac1Cv8 is not connected!");
            }

            List<Cluster> clusters       = new List<Cluster>();

            ProcessStartInfo start       = new ProcessStartInfo(this.RacPath, RacCmdBuilder.GetClusterCmd(ConnStr));
            start.UseShellExecute        = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError  = true;
            start.CreateNoWindow         = true;

            using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(start))
            {
                if (!process.StandardError.EndOfStream)
                {
                    throw new Exception(process.StandardError.ReadToEnd());
                }
                else
                {
                    return Parser.ParseClusters(process.StandardOutput, RacPath, ConnStr);
                }
            }
        }

    }
}
