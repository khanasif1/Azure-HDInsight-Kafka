using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace kafka_producer_api.Controllers
{
    public class KafkaPubController : ApiController
    {
        // GET: api/KafkaPub
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST: api/KafkaPub
        [HttpPost]
        [Route("KafkaPost")]
        public string Post(KafkaModel _model)
        {
            string responseMessage = string.Empty;

            responseMessage = "Start Pub **** ";
            string kafkaEndpoint = string.Empty;
            try
            {
                kafkaEndpoint = ConfigurationManager.AppSettings["broaker"].ToString();
            }
            catch (Exception ex)
            {

                responseMessage = responseMessage + $"Error : {ex.Message} ****";
            }
            responseMessage = responseMessage + $"Kafka broker {kafkaEndpoint} **** ";

            string kafkaTopic = "test";


            var producerConfig = new Dictionary<string, object> {
                { "bootstrap.servers", kafkaEndpoint },                               
            };


            using (var producer = new Producer<Null, string>(producerConfig, null, new StringSerializer(Encoding.UTF8)))
            {
                responseMessage = responseMessage + "Start send ****";
                try
                {
                    var message = $"Event {_model._message}";
                    var result = producer.ProduceAsync(kafkaTopic, null, message).GetAwaiter().GetResult();
                    Console.WriteLine($"Event {_model._message} sent on Partition: {result.Partition} with Offset: {result.Offset}");
                    responseMessage = responseMessage + "End  send ****";
                }
                catch (Exception ex)
                {
                    responseMessage = responseMessage + $"Error : {ex.Message} ****";
                }
                //}
                responseMessage = responseMessage + "End Pub **** ";
            }
            return responseMessage;
        }

    }
    public class KafkaModel
    {
        public int _id { get; set; }
        public string _message { get; set; }
        public DateTime _timestamp { get; set; }
    }
}
