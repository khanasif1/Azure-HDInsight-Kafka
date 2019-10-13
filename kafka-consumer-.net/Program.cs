using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using System;
using System.Collections.Generic;
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
            string kafkaEndpoint = "wn0-pwc-ka.hadn2mhy1dvunibxjea5kqt21h.px.internal.cloudapp.net:9092,wn1-pwc-ka.hadn2mhy1dvunibxjea5kqt21h.px.internal.cloudapp.net:9092";

            // The Kafka topic we'll be using
            string kafkaTopic = "test";


            // Create the consumer configuration
            var consumerConfig = new Dictionary<string, object>
            {
                { "group.id", "myconsumer" },
                { "bootstrap.servers", kafkaEndpoint },
                {"auto.offset.reset" ,"earliest" }
            };

            // Create the consumer
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
                                client.DownloadString($"https://kafka-web.azurewebsites.net/api/values/{message}");
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
