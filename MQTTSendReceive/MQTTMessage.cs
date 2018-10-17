using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AzureMQTTSendReceive
{
    public class MQTTMessage
    {
        private static PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private static PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");

        public float CPU
        {
            get
            {
                return cpuCounter.NextValue();
            }
        }

        public float Memory
        {
            get
            {
                return ramCounter.NextValue();
            }
        }
    }
}
