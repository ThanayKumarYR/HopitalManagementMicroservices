using Confluent.Kafka;

namespace UserHospital.Helper
{
    public class KafkaProducer
    {
        public static ProducerConfig GetProducerConfig()
        {
            return new ProducerConfig
            {
                BootstrapServers = "localhost:9092" // Kafka broker(s) address
            };
        }
    }
}
