using Confluent.Kafka;

namespace Ecommerce.OrderService.kafka
{
    public interface IKafkaProducer
    {
        // Define methods for producing messages to Kafka topics
        Task ProduceAsync(string topic, Message<string,string> message);
    }
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<string, string> _producer;
        public KafkaProducer()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092", // Adjust to your Kafka server
                Acks = Acks.All
            };
            _producer = new ProducerBuilder<string, string>(config).Build();
        }
        public  Task ProduceAsync(string topic, Message<string, string> message)
        {
           return _producer.ProduceAsync(topic, message);
     
        }
    }
}
