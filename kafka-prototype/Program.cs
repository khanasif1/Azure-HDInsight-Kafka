using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
namespace kafka_prototype
{
    class Program
    {
        /*
         * Zookeper: zk0-pwckaf.enddump30h2enc252ekqx2sxqc.px.internal.cloudapp.net:2181,zk3-pwckaf.enddump30h2enc252ekqx2sxqc.px.internal.cloudapp.net:2181
           Broker: wn0-pwckaf.enddump30h2enc252ekqx2sxqc.px.internal.cloudapp.net:9092,wn1-pwckaf.enddump30h2enc252ekqx2sxqc.px.internal.cloudapp.net:9092
        */
        static void Main(string[] args)
        {   
            // The Kafka endpoint address
            string kafkaEndpoint = ConfigurationManager.AppSettings["broaker"].ToString();
            Console.WriteLine($"Kafka broker {kafkaEndpoint}");
            // The Kafka topic we'll be using
            string kafkaTopic = "test";

            // Create the producer configuration
            var producerConfig = new Dictionary<string, object> {
                { "bootstrap.servers", kafkaEndpoint },                
                //{ "sasl.username", "pwckafka" },
                //{ "sasl.password", "Redhat0!asif165" },
            };

            // Create the producer
            using (var producer = new Producer<Null, string>(producerConfig, null, new StringSerializer(Encoding.UTF8)))
            {
                Console.WriteLine("Start send");
                // Send 10 messages to the topic
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                       var message = $"Event {i}";
                        var result = producer.ProduceAsync(kafkaTopic, null, message).GetAwaiter().GetResult();
                        Console.WriteLine($"Event {i} sent on Partition: {result.Partition} with Offset: {result.Offset}");
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
