using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Threading;
using System.Net.Http;

namespace kafka_producer_.net
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static async System.Threading.Tasks.Task Main(string[] args)
        {


            for (int i = 0; i < 40; i++)
            {
                try
                {
                    Random rnd = new Random();
                    int rand = rnd.Next(10, 500);
                    KafkaModel _message = new KafkaModel { _id = 0, _message = $"Event {rand}", _timestamp = DateTime.Now };
                    var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(_message), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("http://40.126.229.114/KafkaPost", content);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Console.WriteLine($"Event {_message._message}");
                    }
                    else
                    {
                        Console.WriteLine("Some Error");
                    }
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Error {ex.Message}");
                }
            }
        }
        public class KafkaModel
        {
            public int _id { get; set; }
            public string _message { get; set; }
            public DateTime _timestamp { get; set; }
        }
    }
}
