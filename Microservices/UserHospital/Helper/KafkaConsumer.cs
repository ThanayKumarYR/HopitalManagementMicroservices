using Confluent.Kafka;

namespace UserHospital.Helper
{
    public class KafkaConsumer
    {
        public static ConsumerConfig GetConsumerConfig()
        {
            return new ConsumerConfig
            {
                BootstrapServers = "localhost:9092", // Kafka broker(s) address
                GroupId = "my-consumer-group", // Consumer group ID
                AutoOffsetReset = AutoOffsetReset.Earliest // Reset offset to the earliest message in case no offset is committed
            };
        }

    }
}
