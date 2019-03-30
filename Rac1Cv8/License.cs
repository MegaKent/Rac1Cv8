using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac1Cv8
{
    public class License
    {
        public string SessionUID { get; private set; }
        public string UserName { get; private set; }
        public string Host { get; private set; }
        public string AppId { get; private set; }
        public string FullName { get; private set; }
        public string Series { get; private set; }
        public bool IssuedByServer { get; private set; }
        public string LicenseTtype { get; private set; }
        public bool Net { get; private set; }
        public int MaxUsersAll { get; private set; }
        public int MaxUsersCur { get; private set; }
        public string RmngrAddress { get; private set; }
        public int RmngrPort { get; private set; }
        public int RmngrPid { get; private set; }
        public string ShortPresentation { get; private set; }
        public string FullPresentation { get; private set; }

        public License()
        {

        }

        public License(string[] properties)
        {
            InitializeProperties(properties);
        }

        private void InitializeProperties(string[] properties)
        {
            SessionUID                      = properties[0];
            UserName                        = properties[1];
            Host                            = properties[2];
            AppId                           = properties[3];
            FullName                        = properties[4];
            Series                          = properties[5];
            IssuedByServer                  = (properties[6] == "yes") ? true : false;
            LicenseTtype                    = properties[7];
            Net                             = (properties[8] == "yes") ? true : false;
            MaxUsersAll                     = int.TryParse(properties[9], out int _MaxUsersAll) ? _MaxUsersAll : -1;
            MaxUsersCur                     = int.TryParse(properties[10], out int _MaxUsersCur) ? _MaxUsersCur : -1;
            RmngrAddress                    = properties[11];
            RmngrPort                       = int.TryParse(properties[12], out int _RmngrPort) ? _RmngrPort : -1;
            RmngrPid                        = int.TryParse(properties[13], out int _RmngrPid) ? _RmngrPid : -1;
            ShortPresentation               = properties[14];
            FullPresentation                = properties[15];
        }
    }
}


/*
session            : de14a618-75fc-47b0-93e0-56d4fccdfe1c
user-name          : administrator
host               : 
app-id             : 1CV8C
full-name          : "file://C:/ProgramData/1C/licenses/20180629133259.lic"
series             : "8100078895"
issued-by-server   : no
license-type       : soft
net                : no
max-users-all      : 1
max-users-cur      : 1
rmngr-address      : 
rmngr-port         : 0
rmngr-pid          : 5684
short-presentation : "Клиент, 8100078895 1 1"
full-presentation  : "Клиент, 5684, 8100078895 1 1, file://C:/ProgramData/1C/licenses/20180629133259.lic"

*///License stream pattern