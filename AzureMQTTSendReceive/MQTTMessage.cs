using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

/// <summary>
/// CAVEAT: This sample is to demonstrate azure IoT client concepts only and is not a guide design principles or style. 
/// Proper exception management is omitted for brevity. Please practice sound engineering practices when writing production code.
/// </summary>
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
