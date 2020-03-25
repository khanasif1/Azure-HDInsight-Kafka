using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace kafka_consumer_.net
{
    class Program
    {
        static void Main(string[] args)
        {
            // The Kafka endpoint address
            string kafkaEndpoint = ConfigurationManager.AppSettings["broaker"].ToString(); 
            //"wn0-corpka.f3l4t1p4pmae3jfkg24ryvt0xa.px.internal.cloudapp.net:9092,wn1-corpka.f3l4t1p4pmae3jfkg24ryvt0xa.px.internal.cloudapp.net:9092";

            // The Kafka topic we'll be using
            string kafkaTopic = ConfigurationManager.AppSettings["kafkatopic"].ToString();
            //"iottopic";


            // Create the consumer configuration
            var consumerConfig = new Dictionary<string, object>
            {
                { "group.id", "myconsumer" },
                { "bootstrap.servers", kafkaEndpoint },
                {"auto.offset.reset" ,"earliest" }
            };

            // Create the consumerdetails
            using (var consumer = new Consumer<Null, string>(consumerConfig, null, new StringDeserializer(Encoding.UTF8)))
            {
                // Subscribe to the OnMessage event
                consumer.OnMessage += (obj, msg) =>
                {
                    Console.WriteLine($"Received: {msg.Value}");
                    try
                    {
                        using (var client = new WebClient())
                        {
                            if (!string.IsNullOrEmpty(msg.Value))
                            {
                                string message =msg.Value.Replace("Event ", "");
                                string url= ConfigurationManager.AppSettings["uiurl"].ToString();
                                client.DownloadString($"{url}/{message}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine($"Error: {ex.Message}");
                    }
                };
                // Subscribe to the Kafka topic
                consumer.Subscribe(new List<string>() { kafkaTopic });
                // Handle Cancel Keypress 
                var cancelled = false;
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cancelled = true;
                };
                Console.WriteLine("Ctrl-C to exit.");
                // Poll for messages
                while (!cancelled)
                {
                    consumer.Poll(TimeSpan.FromMinutes(1));
                }
            }
        }
        private static void Listener()
        {

        }
    }
}
