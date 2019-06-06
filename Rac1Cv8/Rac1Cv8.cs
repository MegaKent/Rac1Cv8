using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            string Command  = ConnStr;

            StreamReader sr = RacInvoker.RunWithErrCheck(RacPath, Command);

            RacInvoker.CloseStreamReader(sr);

            this.RacPath     = RacPath;
            this.ConnStr     = ConnStr;
            this.isConnected = true;
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

            string Command = this.ConnStr;

            StreamReader sr = RacInvoker.RunWithErrCheck(this.RacPath, Command);

            RacInvoker.CloseStreamReader(sr);
           
            isConnected = true;

        }

        public List<Cluster> GetClusters()
        {
            if (!this.isConnected)
            {
                throw new Exception(".Rac1Cv8 is not connected!");
            }

            string Command = RacCmdBuilder.GetClusterCmd(ConnStr);

            StreamReader sr = RacInvoker.RunWithErrCheck(this.RacPath, Command);

            return Parser.ParseClusters(sr, RacPath, ConnStr);
        }
    }
}
