using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

/// <summary>
/// CAVEAT: This sample is to demonstrate azure IoT client concepts only and is not a guide design principles or style. 
/// Proper exception management is omitted for brevity. Please practice sound engineering practices when writing production code.
/// </summary>
namespace AzureMQTTSendReceive
{
    class Program
    {
        static void Main(string[] args)
        {
            Constants.eMode mode = GetRunMode(args);
            if (mode == Constants.eMode.Undefined)
            {
                Console.WriteLine("Use 'MQTTSendReceive <mode>'.  Valid modes are 'Send' and 'Receive'");
                Console.ReadKey();
                return;
            }

            MQTT mqtt = new MQTT(mode);
            switch (mode)
            {
                case Constants.eMode.Send:
                    Console.WriteLine("Send Mode");
                    while (true)
                    {
                        string stringMessage = JsonConvert.SerializeObject(new MQTTMessage());
                        mqtt.SendMQTTMessage(stringMessage);
                        Thread.Sleep(Constants.SLEEP_TIME_BETWEEN_SENDS);
                    }
                case Constants.eMode.Receive:
                    Console.WriteLine("Receive Mode");
                    mqtt.RecieveMQTTMessages();
                    // Wait indefinately for messages to come in.  
                    Thread.Sleep(Timeout.Infinite);
                    break;
            }
        }

        private static Constants.eMode GetRunMode(string[] args)
        {
            Constants.eMode mode = Constants.eMode.Undefined;
            if (args.Length == 1)
            {
                switch (args[0].ToLower())
                {
                    case Constants.RECEIVE_MODE:
                        mode = Constants.eMode.Receive;
                        break;
                    case Constants.SEND_MODE:
                        mode = Constants.eMode.Send;
                        break;
                }
            }
            return mode;
        }
    }
}
