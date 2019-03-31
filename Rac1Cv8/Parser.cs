using System;
using System.Collections.Generic;
using System.IO;

namespace Rac1Cv8
{
    partial class Parser
    {
        //PropCount  = obj properties count
        //ValueIndex = char index after ":"

        private static int ClusterPropCount       = 13;
        private static int ClusterValueIndex      = 32;
        private static int SessionPropCount       = 44;
        private static int SessionValueIndex      = 35;
        private static int LicensesPropCount      = 16;
        private static int LicenseValueIndex      = 21;
        private static int InfoBasePropCount      = 3;
        private static int InfoBaseValueIndex     = 11;
        private static int InfoBasePropCountFull  = 20;
        private static int InfoBaseValueIndexFull = 45;
        private static int WorkingProcPropCount   = 20;
        private static int WorkingProcValueIndex  = 23;
        private static int ServerPropCount        = 13;
        private static int ServerValueIndex       = 38;
        private static int RulePropCount          = 6;
        private static int RuleValueIndex         = 18;

        public static List<Cluster> ParseClusters(StreamReader stream, string RacPath, string ConnStr)
        {
            List<Cluster> clusters = new List<Cluster>();

            string[] str = new string[ClusterPropCount];
            int counter = 0;

            while (!stream.EndOfStream)
            {
                var output = stream.ReadLine();

                if (counter == ClusterPropCount)
                {
                    stream.ReadLine();
                    clusters.Add(new Cluster(str, RacPath, ConnStr));
                    counter = 0;
                    str = new string[ClusterPropCount];
                    continue;
                }

                if (output[output.Length - 1] != '"')
                {
                    str[counter] = output.Substring(ClusterValueIndex, output.Length - ClusterValueIndex);
                }
                else
                {
                    str[counter] = (output.Substring(ClusterValueIndex + 1, output.Length - (ClusterValueIndex + 2))).Replace("\"\"", "\"");
                }

                counter++;
            }

            if (clusters.Count == 0)
            {
                return null;
            }

            return clusters;
        }

        public static List<Session> ParseSessions(
            string ClusterUID,
            StreamReader stream,
            string RacPath,
            string ConnStr,
            string ClusterUser,
            string ClusterPwd,
            List<License> Licenses,
            List<InfoBase> InfoBases)
        {
            List<Session> Sessions = new List<Session>();

            string[] str = new string[SessionPropCount];
            int counter = 0;

            while (!stream.EndOfStream)
            {
                var output = stream.ReadLine();

                if (counter == SessionPropCount)
                {
                    License lic = new License();
                    InfoBase infobase = new InfoBase();

                    if (Licenses != null)
                    {
                        foreach (License license in Licenses)
                        {
                            if (license.SessionUID == str[0])
                            {
                                lic = license;
                                break;
                            }
                        }
                    }

                    foreach (InfoBase ib in InfoBases)
                    {
                        if (ib.UID == str[2])
                        {
                            infobase = ib;
                            break;
                        }
                    }

                    Sessions.Add(new Session(ClusterUID, str, lic, RacPath, ConnStr, ClusterUser, ClusterPwd, infobase));
                    counter = 0;
                    str = new string[SessionPropCount];
                    continue;
                }

                if (output[output.Length - 1] != '"')
                {
                    str[counter] = output.Substring(SessionValueIndex, output.Length - SessionValueIndex);
                }
                else
                {
                    str[counter] = (output.Substring(SessionValueIndex + 1, output.Length - (SessionValueIndex + 2))).Replace("\"\"", "\"");
                }

                counter++;
            }

            if (Sessions.Count == 0)
            {
                return null;
            }

            return Sessions;

        }

        public static List<License> ParseLicenses(
            StreamReader stream,
            string RacPath,
            string ConnStr,
            string ClusterUser,
            string ClusterPwd)
        {
            List<License> Licenses = new List<License>();

            string[] str = new string[LicensesPropCount];
            int counter = 0;

            while (!stream.EndOfStream)
            {
                var output = stream.ReadLine();

                if (counter == LicensesPropCount)
                {
                    Licenses.Add(new License(str));
                    counter = 0;
                    str = new string[LicensesPropCount];
                    continue;
                }

                if (output[output.Length - 1] != '"')
                {
                    str[counter] = output.Substring(LicenseValueIndex, output.Length - LicenseValueIndex);
                }
                else
                {
                    str[counter] = (output.Substring(LicenseValueIndex + 1, output.Length - (LicenseValueIndex + 2))).Replace("\"\"", "\"");
                }

                counter++;
            }

            if (Licenses.Count == 0)
            {
                return null;
            }

            return Licenses;
        }

        public static List<InfoBase> ParseInfoBases(
            string ClusterUID,
            StreamReader stream,
            string RacPath,
            string ConnStr,
            string ClusterUser,
            string ClusterPwd)
        {
            List<InfoBase> InfoBases = new List<InfoBase>();

            string[] str = new string[InfoBasePropCount];
            int counter = 0;

            while (!stream.EndOfStream)
            {
                var output = stream.ReadLine();

                if (counter == InfoBasePropCount)
                {
                    InfoBases.Add(new InfoBase(ClusterUID, str, RacPath, ConnStr, ClusterUser, ClusterPwd));
                    counter = 0;
                    str = new string[InfoBasePropCount];
                    continue;
                }

                if (output[output.Length -1] != '"')
                {
                    str[counter] = output.Substring(InfoBaseValueIndex, output.Length - InfoBaseValueIndex);
                }
                else
                {
                    str[counter] = (output.Substring(InfoBaseValueIndex + 1, output.Length - (InfoBaseValueIndex + 2))).Replace("\"\"","\"");
                }

                counter++;

            }

            if (InfoBases.Count == 0)
            {
                return null;
            }

            return InfoBases;
        }

        public static List<WorkingProcess> ParseProcesses(
            string ClusterUID,
            StreamReader stream,
            string RacPath,
            string ConnStr,
            string ClusterUser,
            string ClusterPwd)
        {
            List<WorkingProcess> Processes = new List<WorkingProcess>();

            string[] str = new string[WorkingProcPropCount];
            int counter = 0;

            while (!stream.EndOfStream)
            {
                var output = stream.ReadLine();

                if (counter == WorkingProcPropCount)
                {
                    Processes.Add(new WorkingProcess(str));
                    counter = 0;
                    str = new string[WorkingProcPropCount];
                    continue;
                }

                if (output[output.Length - 1] != '"')
                {
                    str[counter] = output.Substring(WorkingProcValueIndex, output.Length - WorkingProcValueIndex);
                }
                else
                {
                    str[counter] = (output.Substring(WorkingProcValueIndex + 1, output.Length - (WorkingProcValueIndex + 2))).Replace("\"\"", "\"");
                }

                counter++;

            }

            if (Processes.Count == 0)
            {
                return null;
            }

            return Processes;
        }

        public static string[] InfoBaseProperties(
            string ClusterUID,
            StreamReader stream,
            string RacPath,
            string ConnStr,
            string ClusterUser,
            string ClusterPwd,
            string IBuser,
            string IBPwd)
        {
            string[] str = new string[InfoBasePropCountFull];

            int counter = 0;

            while (!stream.EndOfStream)
            {
                var output = stream.ReadLine();

                if (counter == InfoBasePropCountFull)
                {
                    return str;
                }

                if (output[output.Length - 1] != '"')
                {
                    str[counter] = output.Substring(InfoBaseValueIndexFull, output.Length - InfoBaseValueIndexFull);
                }
                else
                {
                    str[counter] = (output.Substring(InfoBaseValueIndexFull + 1, output.Length - (InfoBaseValueIndexFull + 2))).Replace("\"\"", "\"");
                }

                counter++;

            }

            if (str.Length == 0)
            {
                return null;
            }
            else
            {
                return str;
            }
        }

        public static List<Server> ParseServers(
            string ClusterUID,
            StreamReader stream,
            string RacPath,
            string ConnStr,
            string ClusterUser,
            string ClusterPwd
            )
        {
            List<Server> ServerList = new List<Server>();

            string[] str = new string[ServerPropCount];
            int counter = 0;

            while (!stream.EndOfStream)
            {
                var output = stream.ReadLine();

                if (counter == ServerPropCount)
                {
                    ServerList.Add(new Server(ClusterUID, str, RacPath, ConnStr, ClusterUser, ClusterPwd));
                    counter = 0;
                    str = new string[ServerPropCount];
                    continue;
                }

                if (output[output.Length - 1] != '"')
                {
                    str[counter] = output.Substring(ServerValueIndex, output.Length - ServerValueIndex);
                }
                else
                {
                    str[counter] = (output.Substring(ServerValueIndex + 1, output.Length - (ServerValueIndex + 2))).Replace("\"\"", "\"");
                }

                counter++;

            }

            return ServerList;
        }

        public static List<Rule> ParseRules(
            string ClusterUID,
            StreamReader stream,
            string RacPath,
            string ConnStr,
            string ClusterUser,
            string ClusterPwd
            )
        {
            List<Rule> RuleList = new List<Rule>();

            string[] str = new string[RulePropCount];
            int counter = 0;

            while (!stream.EndOfStream)
            {
                var output = stream.ReadLine();

                if (counter == RulePropCount)
                {
                    RuleList.Add(new Rule(str));
                    counter = 0;
                    str = new string[RulePropCount];
                    continue;
                }

                if (output[output.Length - 1] != '"')
                {
                    str[counter] = output.Substring(RuleValueIndex, output.Length - RuleValueIndex);
                }
                else
                {
                    str[counter] = (output.Substring(RuleValueIndex + 1, output.Length - (RuleValueIndex + 2))).Replace("\"\"", "\"");
                }

                counter++;

            }

            return RuleList;
        }

    }
}
