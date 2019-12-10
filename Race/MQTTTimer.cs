using System;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Race
{
    public static class MQTTTimer
    {
        [FunctionName("FunctionTimer")]
        public static void Run([TimerTrigger("*/10 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            MqttClient client = new MqttClient("52.174.68.36");
            client.ProtocolVersion = uPLibrary.Networking.M2Mqtt.MqttProtocolVersion.Version_3_1;
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            client.MqttMsgPublishReceived += client_recievedMessage;

            string time = DateTime.Now.ToString("h:mm:ss tt");

            string value = "{\"teamId\": 384, \"brand\" : \"Lamborghini\", \"team\": \"Team AD\", \"startTime\": \" " + time + "\" }".ToString();

            client.Publish("howest/track/cars", Encoding.UTF8.GetBytes(value), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            client.Publish("howest/track/cars/logs", Encoding.UTF8.GetBytes(value), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);

        }
        public static void client_recievedMessage(object sender, MqttMsgPublishEventArgs e)
        {
            // Handle message received
            var message = Encoding.Default.GetString(e.Message);
            System.Console.WriteLine("Message received: " + message);
        }
 
    }
}
