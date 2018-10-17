using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureMQTTSendReceive
{
    public class Constants
    {
        public enum eMode
        {
            Send,
            Receive,
            Undefined
        }
        public const string RECEIVE_MODE = "receive";
        public const string SEND_MODE = "send";
        public const string IOT_HUB_DEVICE_NAME = "MQTTDevice";
        public const string MQTT_BROKER_ADDRESS = "MQTTBrokerAddress";
        public const string MQTT_USERNAME = "MQTTUsername";
        public const string MQTT_PASSWORD = "MQTTPassword";
        public const string MQTT_KEEP_ALIVE_PERIOD = "MQTTKeepAlivePeriod";
        public const string MQTT_TOPIC = "/MyMqttTopic";
        public const int SLEEP_TIME_BETWEEN_SENDS = 1000;
    }
}
