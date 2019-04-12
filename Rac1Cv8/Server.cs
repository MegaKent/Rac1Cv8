using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac1Cv8
{
    public class Server
    {
        public string UID { get; private set; }
        public string AgentHost { get; private set; }
        public int AgentPort { get; private set; }
        public List<PortRange> PortRange { get; private set; }
        public string Name { get; private set; }
        public string Using { get; private set; }
        public string DedicateManagers { get; private set; }
        public int InfobaseLimit { get; private set; }
        public long MemoryLimit { get; private set; }
        public int ConnectionsLimit { get; private set; }
        public long SafeWPMemoryLimit { get; private set; }
        public long SafeCallMemoryLimit { get; private set; }
        public int ClusterPort { get; private set; }
        public List<Rule> Rules { get; private set; }

        public string ClusterUID { get; private set; }

        private string RacPath;
        private string ConnStr;
        private string ClusterUser;
        private string ClusterPwd;

        public Server()
        {

        }

        public Server(
            string ClusterUID,
            string[] properties,               //server string properties
            string RacPath,
            string ConnStr,
            string ClusterUser,
            string ClusterPwd
            )
        {
            this.RacPath     = RacPath;
            this.ConnStr     = ConnStr;
            this.ClusterUser = ClusterUser;
            this.ClusterPwd  = ClusterPwd;
            this.ClusterUID  = ClusterUID;
            InitializeProperties(properties);
        }

        private void InitializeProperties(string[] properties)
        {
            UID                 = properties[0];
            AgentHost           = properties[1];
            AgentPort           = int.TryParse(properties[2], out int _AgentPort) ? _AgentPort : -1;
            PortRange           = GetPortRanges(properties[3]);
            Name                = properties[4];
            Using               = properties[5];
            DedicateManagers    = properties[6];
            InfobaseLimit       = int.TryParse(properties[7], out int _InfobaseLimit) ? _InfobaseLimit : -1;
            MemoryLimit         = long.TryParse(properties[8], out long _MemoryLimit) ? _MemoryLimit : -1;
            ConnectionsLimit    = int.TryParse(properties[9], out int _ConnectionsLimit) ? _ConnectionsLimit : -1;
            SafeWPMemoryLimit   = long.TryParse(properties[10], out long _SafeWPMemoryLimit) ? _SafeWPMemoryLimit : -1;
            SafeCallMemoryLimit = long.TryParse(properties[11], out long _SafeCallMemoryLimit) ? _SafeCallMemoryLimit : -1;
            ClusterPort         = int.TryParse(properties[12], out int _ClusterPort) ? _ClusterPort : -1;
            Rules               = GetRules();
        }

        private List<Rule> GetRules()
        {
            ProcessStartInfo start = new ProcessStartInfo(
                                                this.RacPath,
                                                RacCmdBuilder.GetRuleListCmd(ConnStr, ClusterUID, ClusterUser, ClusterPwd,UID));
            start.UseShellExecute        = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError  = true;
            start.CreateNoWindow         = true;

            using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(start))
            {
                return Parser.ParseRules(UID, process.StandardOutput, RacPath, ConnStr, ClusterUser, ClusterPwd);
            }
        }

        private List<PortRange> GetPortRanges(string str)
        {
            List<PortRange> ranges = new List<PortRange>();

            if (!str.Contains(' '))
            {
                ranges.Add(new PortRange(str));
            }
            else
            {
                string[] stringArray = str.Split(' ');

                foreach (string s in stringArray)
                {
                    ranges.Add(new PortRange(s));
                }
            }

            return ranges;
        }
    }

    public class PortRange
    {
        public int HighBound { get; private set; }
        public int LowBound { get; private set; }

        public PortRange(string str)
        {
            ParseStr(str);
        }

        private void ParseStr(string str)
        {
            string[] ports = str.Split(':');

            int port1 = int.TryParse(ports[0], out int _HighBound) ? _HighBound : -1;
            int port2 = int.TryParse(ports[1], out int _LowBound) ? _LowBound : -1;

            HighBound = port1 <= port2 ? port2 : port1;
            LowBound  = port1 <= port2 ? port1 : port2;
        }
    }
}



/*
server                              : a7c23698-9c66-4602-830b-c4e8f5896f82
agent-host                          : CDC-1CL-REP-01
agent-port                          : 1540
port-range                          : 1560:1591 1660:1691 1760:1791
name                                : "Центральный сервер"
using                               : main
dedicate-managers                   : none
infobases-limit                     : 8
memory-limit                        : 0
connections-limit                   : 128
safe-working-processes-memory-limit : 0
safe-call-memory-limit              : 0
cluster-port                        : 1541
 */// Server stream pattern
