using Microsoft.Azure.Devices.Client;
using System;
using System.Configuration;
using System.Text;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace AzureMQTTSendReceive
{
    public class MQTT
    {
        private MqttClient client = null;
        private DeviceClient deviceClient = null;
        private Constants.eMode mode = Constants.eMode.Undefined;

        public MQTT (Constants.eMode mode)
        {
            this.mode = mode;
        }

        private MqttClient MQTTClient
        {
            get
            {
                if (client == null)
                {
                    string brokerAddress = ConfigurationManager.AppSettings[Constants.MQTT_BROKER_ADDRESS];
                    client = new MqttClient(brokerAddress);
                }
                return client;
            }
        }

        public void SendMQTTMessage(string mqttMessage)
        {
            MQTTConnect();
            ushort msgId = MQTTClient.Publish(Constants.MQTT_TOPIC, // topic
                              Encoding.UTF8.GetBytes(mqttMessage), // message body
                              MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
                              false); // retained
            Console.WriteLine("Sent to MQTT Broker: {0}, Return Id: {1}", mqttMessage, msgId.ToString());
        }

        public void RecieveMQTTMessages()
        {
            // Subscribe to an MQTT topic
            MQTTClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            MQTTConnect();
            var code = MQTTClient.Subscribe(new string[]
                { Constants.MQTT_TOPIC },
                new byte[] {
                     MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE
                }
            );

            Console.WriteLine("Subscribed to topic '{0}.  Subscription id: {1}", Constants.MQTT_TOPIC, code.ToString());
        }

        private void MQTTConnect()
        {
            if (!MQTTClient.IsConnected)
            {
                // MQTT Broker configuration parameters.  If anonymous connections are allowed, username and passord can
                // be set to an empty string ("")
                string brokerUserName = ConfigurationManager.AppSettings[Constants.MQTT_USERNAME];
                string brokerPassword = ConfigurationManager.AppSettings[Constants.MQTT_PASSWORD];
                string strKeepAlivePeriod = ConfigurationManager.AppSettings[Constants.MQTT_KEEP_ALIVE_PERIOD];

                //TODO: Possible parse exception
                ushort keepAlivePeriod = ushort.Parse(strKeepAlivePeriod);
 
                // Unique id for MQTT session
                string id = mode.ToString() + Guid.NewGuid().ToString();

                // Connect
                byte code = MQTTClient.Connect(id, brokerUserName, brokerPassword, true, keepAlivePeriod);
                if (code > 0)
                {
                    // Error connecting to the MQTT Broker.  Show server error code.
                    Console.WriteLine("Connection to MQTT Broker failed with code {0}", code);
                }
            }
        }

        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string message = System.Text.Encoding.UTF8.GetString(e.Message);
            DeviceClient deviceClient = GetDeviceClient();
            SendMessage(deviceClient, message);
        }

        private DeviceClient GetDeviceClient()
        {
            if (deviceClient == null)
            {
                string connectionString = ConfigurationManager.ConnectionStrings[Constants.IOT_HUB_DEVICE_NAME].ConnectionString;
                deviceClient = DeviceClient.CreateFromConnectionString(connectionString, Microsoft.Azure.Devices.Client.TransportType.Mqtt);
            }
            return deviceClient;
        }

        private void SendMessage(DeviceClient deviceClient, string message)
        {
            try
            {
                var azureMessage = new Message(Encoding.ASCII.GetBytes(message));

                // Note that this is an awaitable method call.
                deviceClient.SendEventAsync(azureMessage);
                Console.WriteLine("Sent to IoT Hub: {0}", message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
                Console.WriteLine("Stack Trace: {0}", e.StackTrace);
            }
        }
    }
}
