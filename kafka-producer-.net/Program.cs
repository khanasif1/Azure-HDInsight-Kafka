using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Threading;

namespace kafka_producer_.net
{
    class Program
    {
        
        static void Main(string[] args)
        {
            // The Kafka endpoint address
            string kafkaEndpoint = ConfigurationManager.AppSettings["broaker"].ToString();
            Console.WriteLine($"Kafka broker {kafkaEndpoint}");
            // The Kafka topic we'll be using
            string kafkaTopic = "test";

            // Create the producer configuration
            var producerConfig = new Dictionary<string, object> {
                { "bootstrap.servers", kafkaEndpoint }                
            };

            // Create the producer
            using (var producer = new Producer<Null, string>(producerConfig, null, new StringSerializer(Encoding.UTF8)))
            {
                Console.WriteLine("Start send");
                // Send 10 messages to the topic
                for (int i = 0; i < 40; i++)
                {
                    try
                    {
                        Random rnd = new Random();
                        int rand = rnd.Next(10, 500);
                        var message = $"Event {rand}";
                        var result = producer.ProduceAsync(kafkaTopic, null, message).GetAwaiter().GetResult();
                        Console.WriteLine($"Event {rand} sent on Partition: {result.Partition} with Offset: {result.Offset}");
                        Thread.Sleep(2000);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error : {ex.Message}");
                        throw ex;
                    }
                }
                Console.WriteLine("End send");
            }
        }
    }
}
