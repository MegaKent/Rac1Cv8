using System;
using System.Collections.Generic;
using System.IO;

namespace Rac1Cv8
{
    public class Cluster
    {
        /// <summary>
        /// Cluster properties. Public read access & private write access
        /// </summary>
        public string UID { get; private set;}
        public string Host { get; private set; }
        public int Port { get; private set; }
        public string Name { get; private set; }
        public int ExpitarionTimeout { get; private set; }
        public int LifetimeLimit { get; private set; }
        public int MaxMemorySize { get; private set; }
        public int MaxMemoryTimeLimit { get; private set; }
        public int SecurityLevel { get; private set; }
        public int SessionFaultToleranceLevel { get; private set; }
        public int LoadBalancingMode { get; private set; }
        public int ErorsCountThreshold { get; private set; }
        public int KillProblemProcesses { get; private set; }

        /// <summary>
        /// Service class properties
        /// </summary>
        public bool isAuthenticated { get; private set; } = false;
        private string RacPath;
        private string ConnStr;
        private string ClusterUser;
        private string ClusterPwd;

        public Cluster()
        {
        }

        public Cluster(string[] str, string RacPath, string ConnStr)
        {
            this.RacPath = RacPath;
            this.ConnStr = ConnStr;
            InitializeProperties(str);
        }

        private void InitializeProperties(string[] properties)
        {
            UID                        = properties[0];
            Host                       = properties[1];
            Name                       = properties[3];
            LoadBalancingMode          = (properties[2] == "performance") ? 0 : 1; ;
            Port                       = int.TryParse(properties[2], out int _Port) ? _Port : -1;
            ExpitarionTimeout          = int.TryParse(properties[4], out int _ExpitarionTimeout) ? _ExpitarionTimeout : -1;
            LifetimeLimit              = int.TryParse(properties[5], out int _LifetimeLimit) ? _LifetimeLimit : -1;
            MaxMemorySize              = int.TryParse(properties[6], out int _MaxMemorySize) ? _MaxMemorySize : -1;
            MaxMemoryTimeLimit         = int.TryParse(properties[7], out int _MaxMemoryTimeLimit) ? _MaxMemoryTimeLimit : -1;
            SecurityLevel              = int.TryParse(properties[8], out int _SecurityLevel) ? _SecurityLevel : -1;
            SessionFaultToleranceLevel = int.TryParse(properties[9], out  int _SessionFaultToleranceLevel) ? _SessionFaultToleranceLevel : -1;
            ErorsCountThreshold        = int.TryParse(properties[11], out int _ErorsCountThreshold) ? _ErorsCountThreshold : -1;
            KillProblemProcesses       = int.TryParse(properties[12], out int _KillProblemProcesses) ? _KillProblemProcesses : -1;

        }

        private void CheckAuthentication()
        {
            if (!this.isAuthenticated)
            {
                throw new Exception("Cluster admin is not authenticated!");
            }
        }

        private string GetInfoBaseUID(string Name)
        {
            string InfoBaseUID = string.Empty;

            List<InfoBase> InfoBaseList = GetInfoBases();

            foreach (InfoBase ib in InfoBaseList)
            {
                if (ib.Name == Name)
                {
                    return ib.UID;
                }
            }

            return InfoBaseUID;
        }

        private void CheckNewInfoBaseProps(string[] props)
        {
            string excp = string.Empty;

            if (props[0] == string.Empty)
            {
                excp += "InfoBase Name can not be empty!";
            }
            if (!(props[1] == "MSSQLServer" | props[1] == "PostgreSQL" | props[1] == "IBMDB2" | props[1] == "OracleDatabase"))
            {
                excp += "\nDBMS contains only MSSQLServer | PostgreSQL |  IBMDB2 | OracleDatabase";
            }
            if (props[2] == string.Empty)
            {
                excp += "DBServer can not be empty!";
            }
            if (props[3] == string.Empty)
            {
                excp += "DBName can not be empty!";
            }

            if (excp != string.Empty)
            {
                throw new Exception(excp);
            }
        }

        public void Authenticate(string usr, string pwd)
        {
            string Command  = RacCmdBuilder.ClusterAuthCmd(ConnStr, UID, usr, pwd);

            StreamReader sr = RacInvoker.RunWithErrCheck(this.RacPath, Command);

            RacInvoker.CloseStreamReader(sr);

            this.ClusterUser = usr;
            this.ClusterPwd = pwd;
            this.isAuthenticated = true;
        }

        public List<Session> GetSessions()
        {
            CheckAuthentication();

            List<License> Licenses = GetLicenses();
            List<InfoBase> InfoBases = GetInfoBases();

            string Command  = RacCmdBuilder.GetSessionsCmd(ConnStr, UID, ClusterUser, ClusterPwd);
            StreamReader sr = RacInvoker.Run(this.RacPath, Command);

            return Parser.ParseSessions(UID, sr, RacPath, ConnStr, ClusterUser, ClusterPwd, Licenses, InfoBases);
        }

        public List<License> GetLicenses()
        {
            CheckAuthentication();

            string Command  = RacCmdBuilder.GetLicensesCmd(ConnStr, UID, ClusterUser, ClusterPwd);

            StreamReader sr = RacInvoker.Run(this.RacPath, Command);

            return Parser.ParseLicenses(sr, RacPath, ConnStr, ClusterUser, ClusterPwd);
        }

        public List<InfoBase> GetInfoBases()
        {
            CheckAuthentication();

            string Command   = RacCmdBuilder.GetInfoBaseListCmd(ConnStr, UID, ClusterUser, ClusterPwd);

            StreamReader sr  = RacInvoker.Run(this.RacPath, Command);
           
            return Parser.ParseInfoBases(UID, sr, RacPath, ConnStr, ClusterUser, ClusterPwd);
        }

        public List<WorkingProcess> GetProcesses()
        {
            CheckAuthentication();

            string Command  = RacCmdBuilder.GetProcessesCmd(ConnStr, UID, ClusterUser, ClusterPwd);

            StreamReader sr = RacInvoker.Run(this.RacPath, Command);

            return Parser.ParseProcesses(UID, sr, RacPath, ConnStr, ClusterUser, ClusterPwd);
        }

        public List<Server> GetServers()
        {
            CheckAuthentication();

            string Command  = RacCmdBuilder.GetServerListCmd(ConnStr, UID, ClusterUser, ClusterPwd);

            StreamReader sr = RacInvoker.Run(this.RacPath, Command);

            return Parser.ParseServers(UID, sr, RacPath, ConnStr, ClusterUser, ClusterPwd);
        }

        public void CreateInfoBase(
            string Name,
            string DBMS,
            string DBServer,
            string DBName,
            bool CreateDataBase,
            string Locale = "ru",
            string DBUser = "",
            string DBPass = "",
            string Descr = "",
            int DateOffset = 2000,
            int SecurityLevel = 0,
            bool ScheduledJobsDeny = false,
            bool LicenseDistribution = true
            )
        {
            CheckAuthentication();

            string[] props = { Name, DBMS, DBServer, DBName, CreateDataBase ? "yes":"no", Locale, DBUser, DBPass, Descr,
                DateOffset.ToString(), SecurityLevel.ToString(),
                ScheduledJobsDeny ? "on":"off", LicenseDistribution ? "allow":"deny"
            };

            CheckNewInfoBaseProps(props);

            string Command  = RacCmdBuilder.CreateInfoBaseCmd(ConnStr, UID, ClusterUser, ClusterPwd, props);

            StreamReader sr = RacInvoker.RunWithErrCheck(this.RacPath, Command);

            RacInvoker.CloseStreamReader(sr);
        }

        public void DeleteInfoBase(string Name, bool DropDatabase,  string InfoBaseUser, string InfoBasePwd, bool ClearDatabase = false)
        {
            CheckAuthentication();

            string InfoBaseUID = GetInfoBaseUID(Name);

            if (InfoBaseUID == string.Empty)
            {
                throw new Exception("Infobase "+ Name +" not exist!");
            }

            string Command  = RacCmdBuilder.DeleteInfoBaseCmd(ConnStr, UID, ClusterUser, ClusterPwd, InfoBaseUID, InfoBaseUser, InfoBasePwd, DropDatabase, ClearDatabase);

            StreamReader sr = RacInvoker.RunWithErrCheck(this.RacPath, Command);

            RacInvoker.CloseStreamReader(sr);

        }
        
        //work after remove database
        public void DeleteInfoBase(string Name)
        {
            CheckAuthentication();

            string InfoBaseUID = GetInfoBaseUID(Name);

            if (InfoBaseUID == string.Empty)
            {
                throw new Exception("Infobase " + Name + " not exist!");
            }

            string Command  = RacCmdBuilder.DeleteInfoBaseCmd(ConnStr, UID, ClusterUser, ClusterPwd, InfoBaseUID, "", "", false, false);

            StreamReader sr = RacInvoker.RunWithErrCheck(this.RacPath, Command);

            RacInvoker.CloseStreamReader(sr);
        }

        public void SetClusterRecycling(int seconds)
        {
            CheckAuthentication();

            string Command  = RacCmdBuilder.GetClusterRecyclingCmd(ConnStr, UID, ClusterUser, ClusterPwd, seconds);

            StreamReader sr = RacInvoker.RunWithErrCheck(this.RacPath, Command);

            RacInvoker.CloseStreamReader(sr);
        }

        public void TerminateSessions(string InfoBase, bool DesignerTo = false)
        {
            CheckAuthentication();

            List<Session> All_Sessions = GetSessions();
            List<Session> sessions;

            if (All_Sessions == null)
            {
                return;
            }

            if (DesignerTo)
            {
                sessions = All_Sessions.FindAll(s => s.InfoBase.Name.ToLower() == InfoBase.ToLower() & s.AppId != "Designer");
            }
            else
            {
                sessions = All_Sessions.FindAll(s => s.InfoBase.Name.ToLower() == InfoBase.ToLower());
            }

            foreach (Session s in sessions)
            {
                s.Terminate();
            }
        }

    }
}


/*
cluster                       : 84b23dbd-1c59-4afa-9573-b8254fd1d805
host                          : CDC-1CL-REP-01
port                          : 1541
name                          : "CDC-1CL-REP-01"
expiration-timeout            : 600
lifetime-limit                : 86400
max-memory-size               : 0
max-memory-time-limit         : 0
security-level                : 0
session-fault-tolerance-level : 0
load-balancing-mode           : performance
errors-count-threshold        : 0
kill-problem-processes        : 0

*/// Cluster stream pattern
