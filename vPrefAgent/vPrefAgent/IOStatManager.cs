using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace IOStat
{
    class IOStatManager
    {
        private static readonly IOStatManager instance = new IOStatManager();
        private static PerformanceCounterCategory diskCounterHandler_ = null;
        private IOStatManager() { }
        private PerformanceCounter[] diskCounters_ = null;

        public static IOStatManager getInstance
        {
            get
            {
                return instance;
            }
        }

        ~IOStatManager()
        {
            Console.WriteLine("IOStatManager is closing");
        }

        private PerformanceCounterCategory getDiskCounterHandler_()
        {
            if (diskCounterHandler_ == null)
            {
                diskCounterHandler_ = new PerformanceCounterCategory("logicaldisk", Environment.MachineName);
                Console.WriteLine("Created new disk counter handler");
                String[] instanceNames = diskCounterHandler_.GetInstanceNames();
                for (int i = 0; i < instanceNames.Length; i++)
                    if (instanceNames[i].Contains(':'))
                    {
                        diskCounters_ = diskCounterHandler_.GetCounters(instanceNames[i]);
                        Console.WriteLine("Selected logical disk: "+instanceNames[i]+", total counter count: "+diskCounters_.Length);
                        break;
                    }  
            }
            return diskCounterHandler_;
        }

        public Dictionary<String, String> getDiskInformation()
        {
            getDiskCounterHandler_();
            Dictionary<String, String> diskInfo = new Dictionary<string,string>();
            for (int i = 0; i < diskCounters_.Length; i++)
            {
                diskInfo.Add(diskCounters_[i].CounterName, diskCounters_[i].NextValue().ToString());
            }
            return diskInfo;
        }
    }
}
