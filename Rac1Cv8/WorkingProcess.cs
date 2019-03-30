using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac1Cv8
{
    public class WorkingProcess
    {
        public string UID { get; private set; }
        public string Host { get; private set; }
        public int Port { get; private set; }
        public int PID { get; private set; }
        public bool isEnable { get; private set; }
        public int Running { get; private set; }                     // 1 - the process is running, 0 - the process is stopped
        public DateTime StartedAt { get; private set; }
        public int Use { get; private set; }                         // 0 - not used, 1 - used, 2 - used as a reserve
        public int AvailablePerformance { get; private set; }
        public int Capacity { get; private set; }
        public int Connections { get; private set; }
        public int MemorySize { get; private set; }
        public long MemoryExcessTime { get; private set; }
        public int SelectionSize { get; private set; }
        public double AvgBackCallTime { get; private set; }
        public double AvgCallTime { get; private set; }
        public double AvgDBCallTime { get; private set; }
        public double AvgLockCallTime { get; private set; }
        public double AvgServerCallTime { get; private set; }
        public double AvgThreads { get; private set; }

        public WorkingProcess()
        {

        }

        public WorkingProcess(string[] properties)
        {
            InitializeProperties(properties);
        }

        private void InitializeProperties(string[] properties)
        {
            UID = properties[0];
            Host = properties[1];
            Port = int.TryParse(properties[2], out int _Port) ? _Port : -1;
            PID = int.TryParse(properties[3], out int _PID) ? _PID: -1;
            isEnable = (properties[4] == "yes") ? true : false;
            Running = (properties[5] == "yes") ? 1 : 0;
            StartedAt = DateTime.Parse(properties[6]);
            AvailablePerformance = int.TryParse(properties[8], out int _AvailablePerformance) ? _AvailablePerformance : -1;
            Capacity = int.TryParse(properties[9], out int _Capacity) ? _Capacity : -1;
            Connections = int.TryParse(properties[10], out int _Connections) ? _Connections : -1;
            MemorySize = int.TryParse(properties[11], out int _MemorySize) ? _MemorySize : -1;
            MemoryExcessTime = long.TryParse(properties[12], out long _MemoryExcessTime) ? _MemoryExcessTime : -1;
            SelectionSize = int.TryParse(properties[13], out int _SelectionSize) ? _SelectionSize : -1;
            AvgBackCallTime = double.TryParse(properties[14], NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out double _AvgBackCallTime) ? _AvgBackCallTime : -1;
            AvgCallTime = double.TryParse(properties[15], NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out double _AvgCallTime) ? _AvgCallTime : -1;
            AvgDBCallTime = double.TryParse(properties[16], NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out double _AvgDBCallTime) ? _AvgDBCallTime : -1;
            AvgLockCallTime = double.TryParse(properties[17], NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out double _AvgLockCallTime) ? _AvgLockCallTime : -1;
            AvgServerCallTime = double.TryParse(properties[18], NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out double _AvgServerCallTime) ? _AvgServerCallTime : -1;
            AvgThreads = double.TryParse(properties[18], NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out double _AvgThreads) ? _AvgThreads : -1;

            if (properties[7] == "not used")
            {
                Use = 0;
            }
            else if (properties[7] == "used")
            {
                Use = 1;
            }
            else
            {
                Use = 2;

            }
        }
    }
}


/*
process              : 67b66836-ec05-4aa4-b793-f94db67fa4d9
host                 : CDC-1CL-REP-01
port                 : 1563
pid                  : 12644
is-enable            : yes
running              : yes
started-at           : 2018-12-20T17:07:19
use                  : used
available-perfomance : 200
capacity             : 1000
connections          : 2
memory-size          : 82044
memory-excess-time   : 0
selection-size       : 24
avg-back-call-time   : 0.000
avg-call-time        : 0.199
avg-db-call-time     : 0.024
avg-lock-call-time   : 0.000
avg-server-call-time : 0.175
avg-threads          : 0.005

*///Process pattern
