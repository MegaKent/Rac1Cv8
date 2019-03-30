using System;
using System.Diagnostics;


namespace Rac1Cv8
{
    public class InfoBase
    {
        public string UID { get; private set; }
        public string Name { get; private set; }
        public string DBMS { get; private set; }
        public string DBServer { get; private set; }
        public string DBName { get; private set; }
        public string DBUser { get; private set; }
        public int SecurityLever { get; private set; }
        public int LicenseDistribution { get; private set; }
        public bool ScheduledJobsDeny { get; private set; }
        public bool SessionDeny { get; private set; }
        public DateTime DeniedFrom { get; private set; }
        public string DeniedMessage { get; private set; }
        public string DeniedParameter { get; private set; }
        public DateTime DeniedTo { get; private set; }
        public string PermissionCode { get; private set; }
        public string ExternalSessionMngrConnStr { get; private set; }
        public string ExternalSessionMngrRequired{ get; private set; }
        public string SecurityProfileName { get; private set; }
        public string SafeModeSecProfileMode { get; private set; }
        public string Descr { get; private set; }

        private string RacPath;
        private string IBUser;
        private string IBPwd;
        private string ConnStr;
        private string ClusterUID;
        private string ClusterUser;
        private string ClusterPwd;

        public bool isAuthenticated { get; private set; }

        public InfoBase()
        {
            
        }

        public InfoBase(string ClusterUID, string[] props, string RacPath, string ConnStr, string ClusterUser, string ClusterPwd)
        {
            InitializeProperties(props);
            this.ClusterUID     = ClusterUID;
            this.RacPath        = RacPath;
            this.ConnStr        = ConnStr;
            this.ClusterUser    = ClusterUser;
            this.ClusterPwd     = ClusterPwd;
        }

        private void InitializeProperties(string[] props)
        {
            UID     = props[0];
            Name    = props[1];
            Descr   = props[2];
        }

        public void Authenticate(string InfoBaseUser, string InfoBasePwd)
        {
            string[] props = new string[20];
            string cmd = RacCmdBuilder.InfoBaseAuthCmd(ConnStr, ClusterUID, ClusterUser, ClusterPwd, UID, InfoBaseUser, InfoBasePwd);

            ProcessStartInfo start       = new ProcessStartInfo(this.RacPath, cmd);
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
                    props = Parser.InfoBaseProperties(
                        ClusterUID, 
                        process.StandardOutput,
                        RacPath, ConnStr,
                        ClusterUser,
                        ClusterPwd,
                        InfoBaseUser,
                        InfoBasePwd);
                }
            }

            if (props != null)
            {
                this.DBMS                                   = props[2];
                this.DBServer                               = props[3];
                this.DBName                                 = props[4];
                this.DBUser                                 = props[5];
                this.SecurityLever                          = int.TryParse(props[6], out int _SecurityLevel) ? _SecurityLevel: -1;
                this.LicenseDistribution                    = (props[7] == "allow") ? 1 : 0;
                this.ScheduledJobsDeny                      = (props[8] == "on") ? true : false;
                this.SessionDeny                            = (props[9] == "on") ? true : false;
                this.DeniedMessage                          = props[11];
                this.DeniedParameter                        = props[12];
                this.PermissionCode                         = props[14];
                this.ExternalSessionMngrConnStr             = props[15];
                this.ExternalSessionMngrRequired            = props[16];
                this.SecurityLever                          = (props[17] == "yes") ? 1: 0;
                this.SafeModeSecProfileMode                 = props[18];
                
                if (DateTime.TryParse(props[10], out DateTime _DeniedFrom))
                {
                    this.DeniedFrom = _DeniedFrom;
                }

                if (DateTime.TryParse(props[13], out DateTime _DeniedTo))
                {
                    this.DeniedTo = _DeniedTo;
                }

            }
            
            this.isAuthenticated = true;
            this.IBUser = InfoBaseUser;
            this.IBPwd = InfoBasePwd;
        }

        public void LockInfoBase(
            DateTime DeniedFrom, 
            DateTime DeniedTo, 
            string DeniedMessage, 
            string PermissionCode, 
            bool ScheduledJobsDeny,
            bool SessionDeny = true)
        {
            if (!this.isAuthenticated)
            {
                throw new Exception("User is not authenticated!");
            }

            string cmd = RacCmdBuilder.LockInfoBaseCmd(ConnStr, ClusterUID, ClusterUser, ClusterPwd, UID, IBUser, IBPwd, DeniedFrom, DeniedTo, DeniedMessage, PermissionCode, ScheduledJobsDeny, SessionDeny);

            ProcessStartInfo start       = new ProcessStartInfo(this.RacPath, cmd);
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
        }

        public void LockInfoBase(
           string DeniedMessage,
           string PermissionCode,
           bool ScheduledJobsDeny,
           bool SessionDeny)
        {
            if (!this.isAuthenticated)
            {
                throw new Exception("User is not authenticated!");
            }

            string cmd = RacCmdBuilder.LockInfoBaseCmd(ConnStr,ClusterUID,ClusterUser,ClusterPwd,UID,IBUser,IBPwd,DeniedFrom,DeniedTo,DeniedMessage,PermissionCode,ScheduledJobsDeny, SessionDeny);

            ProcessStartInfo start       = new ProcessStartInfo(this.RacPath, cmd);
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
        }


        public void UnLockInfoBase(bool ScheduledJobsAllow = true)
        {
            if (! this.isAuthenticated)
            {
                throw new Exception("User is not authenticated!");
            }

            string cmd = RacCmdBuilder.UnLockInfoBaseCmd(ConnStr, ClusterUID, ClusterUser, ClusterPwd, UID, IBUser, IBPwd, ScheduledJobsAllow);

            ProcessStartInfo start       = new ProcessStartInfo(this.RacPath, cmd);
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

        }

    }
}

/*
infobase : e3cdb45e-369b-405b-a77d-b83773562d23
name     : do
descr    : 

*///InfoBase short (without auth) stream pattern

/*
infobase                                   : 133b4935-faa6-43d9-8f69-1d40186a86ff
name                                       : fin
dbms                                       : MSSQLServer
db-server                                  : cdc-1cl-sql-lst
db-name                                    : FIN2
db-user                                    : 
security-level                             : 0
license-distribution                       : allow
scheduled-jobs-deny                        : off
sessions-deny                              : off
denied-from                                : 
denied-message                             : 
denied-parameter                           : 
denied-to                                  : 
permission-code                            : 
external-session-manager-connection-string : 
external-session-manager-required          : no
security-profile-name                      : 
safe-mode-security-profile-name            : 
descr                                      : 

*///InfoBase full (with auth) stream pattern
