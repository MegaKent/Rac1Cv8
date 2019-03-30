using System;
using System.Diagnostics;

namespace Rac1Cv8
{
    public class Session
    {
        public string UID { get; private set; }
        public int Numb { get; private set; }
        //public string InfoBase { get; private set; }
        public string Connection { get; private set; }
        public string Process { get; private set; }
        public string UserName { get; private set; }
        public string Host { get; private set; }
        public string AppId { get; private set; }
        public string Locale { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime LastActiveAt { get; private set; }
        public bool Hibernate { get; private set; }
        public int PassiveSessionHibernateTime { get; private set; }
        public int HibernateSessionTerminateTime { get; private set; }
        public int BlockedByDBMS { get; private set; }
        public int BlockedByLs { get; private set; }
        public long BytesAll { get; private set; }
        public long BytesLast5Min { get; private set; }
        public int CallsAll { get; private set; }
        public long CallsLast5Min { get; private set; }
        public long DBMSBytesAll { get; private set; }
        public long DBMSBytesLast5Min { get; private set; }
        public string DBProcInfo { get; private set; }
        public int DBProcTook { get; private set; }
        public DateTime DBProcTookAt { get; private set; }
        public int DurationAll { get; private set; }
        public int DurationAllDBMS { get; private set; }
        public int DurationCurrent { get; private set; }
        public int DurationCurrentDBMS { get; private set; }
        public int DurationLast5Min { get; private set; }
        public int DurationLast5MinDBMS { get; private set; }
        public long MemoryCurrent { get; private set; }
        public long MemoryLast5Min { get; private set; }
        public long MemoryTotal { get; private set; }
        public long ReadCurrent { get; private set; }
        public long ReadLast5Min { get; private set; }
        public long ReadTotal { get; private set; }
        public long WriteCurrent { get; private set; }
        public long WriteLast5Min { get; private set; }
        public long WriteTotal { get; private set; }
        public int DurationCurrentService { get; private set; }
        public int DurationLast5MinService { get; private set; }
        public int DurationAllService { get; private set; }
        public string CurrentServiceName { get; private set; }
        public License License;
        public InfoBase InfoBase;

        public string ClusterUID { get; private set; }

        private string RacPath;
        private string ConnStr;
        private string ClusterUser;
        private string ClusterPwd;

        public Session()
        {

        }

        public Session(
            string ClusterUID,
            string[] str,               //session properties 
            License license, 
            string RacPath,
            string ConnStr, 
            string ClusterUser, 
            string ClusterPwd,
            InfoBase infobase)
        {
            InitializeProperties(str, license, infobase);
            this.RacPath        = RacPath;
            this.ConnStr        = ConnStr;
            this.ClusterUser    = ClusterUser;
            this.ClusterPwd     = ClusterPwd;
            this.ClusterUID     = ClusterUID;
        }

        public void Terminate()
        {
            string cmd = RacCmdBuilder.TerminateSessionCmd(ConnStr,ClusterUID,ClusterUser,ClusterPwd,UID);

            ProcessStartInfo start       = new ProcessStartInfo(this.RacPath, cmd);
            start.UseShellExecute        = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError  = true;
            start.CreateNoWindow         = true;

            using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(start))
            {
                if (!process.StandardError.EndOfStream)
                {
                    string excp = process.StandardError.ReadToEnd();

                    if (!excp.Contains("Сеанс с указанным идентификатором не найден"))
                    {
                        throw new Exception(excp);
                    }
                }
            }
        }

        private void InitializeProperties(string[] properties, License license, InfoBase infobase)
        {
            UID                             = properties[0];
            Numb                            = int.TryParse(properties[1], out int _Numb) ? _Numb : -1;
            InfoBase                        = infobase;
            Connection                      = properties[3];
            Process                         = properties[4];
            UserName                        = properties[5];
            Host                            = properties[6];
            AppId                           = properties[7];
            Locale                          = properties[8];
            StartedAt                       = DateTime.TryParse(properties[9], out DateTime _StartedAt) ? _StartedAt : DateTime.MinValue;
            LastActiveAt                    = DateTime.TryParse(properties[10], out DateTime _LastActiveAt) ? _LastActiveAt : DateTime.MinValue;
            Hibernate                       = (properties[11]=="yes") ? true : false ;
            PassiveSessionHibernateTime     = int.TryParse(properties[12], out int _PassiveSessionHibernateTime) ? _PassiveSessionHibernateTime : -1;
            HibernateSessionTerminateTime   = int.TryParse(properties[13], out int _HibernateSessionTerminateTime) ? _HibernateSessionTerminateTime : -1;
            BlockedByDBMS                   = int.TryParse(properties[14], out int _BlockByDBMS) ? _BlockByDBMS : -1;
            BlockedByLs                     = int.TryParse(properties[15], out int _BlockByLs) ? _BlockByLs : -1;
            BytesAll                        = long.TryParse(properties[16], out long _BytesAll) ? _BytesAll : -1;
            BytesLast5Min                   = long.TryParse(properties[17], out long _BytesLast5Min) ? _BytesLast5Min : -1;
            CallsAll                        = int.TryParse(properties[18], out int _CallsAll) ? _CallsAll : -1;
            CallsLast5Min                   = long.TryParse(properties[19], out long _CallsLast5Min) ? _CallsLast5Min : -1;
            DBMSBytesAll                    = long.TryParse(properties[20], out long _DBMSBytesAll) ? _DBMSBytesAll : -1;
            DBMSBytesLast5Min               = long.TryParse(properties[21], out long _DBMSBytesLast5Min) ? _DBMSBytesLast5Min : -1;
            DBProcInfo                      = properties[22];
            DBProcTook                      = int.TryParse(properties[23], out int _DBProcTook) ? _DBProcTook : -1;
            DBProcTookAt                    = DateTime.TryParse(properties[24], out DateTime _DBProcTookAt) ? _DBProcTookAt : DateTime.MinValue;
            DurationAll                     = int.TryParse(properties[25], out int _DurationAll) ? _DurationAll : -1;
            DurationAllDBMS                 = int.TryParse(properties[26], out int _DurationAllDBMS) ? _DurationAllDBMS : -1;
            DurationCurrent                 = int.TryParse(properties[27], out int _DurationCurrent) ? _DurationCurrent : -1;
            DurationCurrentDBMS             = int.TryParse(properties[28], out int _DurationCurrentDBMS) ? _DurationCurrentDBMS : -1;
            DurationLast5Min                = int.TryParse(properties[29], out int _DurationLast5Min) ? _DurationLast5Min : -1;
            DurationLast5MinDBMS            = int.TryParse(properties[30], out int _DurationLast5MinDBMS) ? _DurationLast5MinDBMS : -1;
            MemoryCurrent                   = long.TryParse(properties[31], out long _MemoryCurrent) ? _MemoryCurrent : -1;
            MemoryLast5Min                  = long.TryParse(properties[32], out long _MemoryLast5Min) ? _MemoryLast5Min : -1;
            MemoryTotal                     = long.TryParse(properties[33], out long _MemoryTotal) ? _MemoryTotal : -1;
            ReadCurrent                     = long.TryParse(properties[34], out long _ReadCurrent) ? _ReadCurrent : -1;
            ReadLast5Min                    = long.TryParse(properties[35], out long _ReadLast5Min) ? _ReadLast5Min : -1;
            ReadTotal                       = long.TryParse(properties[36], out long _ReadTotal) ? _ReadTotal : -1;
            WriteCurrent                    = long.TryParse(properties[37], out long _WriteCurrent) ? _WriteCurrent : -1;
            WriteLast5Min                   = long.TryParse(properties[38], out long _WriteLast5Min) ? _WriteLast5Min : -1;
            WriteTotal                      = long.TryParse(properties[39], out long _WriteTotal) ? _WriteTotal : -1;
            DurationCurrentService          = int.TryParse(properties[40], out int _DurationCurrentService) ? _DurationCurrentService : -1;
            DurationLast5MinService         = int.TryParse(properties[41], out int _DurationLast5MinService) ? _DurationLast5MinService : -1;
            DurationAllService              = int.TryParse(properties[42], out int _DurationAllService) ? _DurationAllService : -1;
            CurrentServiceName              = properties[43];

            License                         = license;
        }
    }
}

/*
session                          : 665d99fc-091d-4b25-8d73-2e821a8bd741
session-id                       : 1
infobase                         : 075f405e-5a50-4aa5-aff8-1c4e01331aa6
connection                       : 00000000-0000-0000-0000-000000000000
process                          : 00000000-0000-0000-0000-000000000000
user-name                        : administrator
host                             : 
app-id                           : 1CV8C
locale                           : ru
started-at                       : 2018-12-07T09:42:23
last-active-at                   : 2018-12-07T09:47:39
hibernate                        : no
passive-session-hibernate-time   : 1200
hibernate-session-terminate-time : 7200
blocked-by-dbms                  : 0
blocked-by-ls                    : 0
bytes-all                        : 95300
bytes-last-5min                  : 95300
calls-all                        : 130
calls-last-5min                  : 130
dbms-bytes-all                   : 6882585
dbms-bytes-last-5min             : 6882585
db-proc-info                     : 
db-proc-took                     : 0
db-proc-took-at                  : 
duration-all                     : 15858
duration-all-dbms                : 10742
duration-current                 : 0
duration-current-dbms            : 0
duration-last-5min               : 15858
duration-last-5min-dbms          : 10742
memory-current                   : 0
memory-last-5min                 : 165772912
memory-total                     : 165772912
read-current                     : 0
read-last-5min                   : 10815340
read-total                       : 10815340
write-current                    : 0
write-last-5min                  : 10991674
write-total                      : 10991674
duration-current-service         : 0
duration-last-5min-service       : 32
duration-all-service             : 32
current-service-name             : 

*///Session stream pattern