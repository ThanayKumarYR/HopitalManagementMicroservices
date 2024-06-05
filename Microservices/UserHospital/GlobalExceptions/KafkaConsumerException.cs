namespace UserHospital.GlobalExceptions
{
    public class KafkaConsumerException : Exception
    {
        public KafkaConsumerException(string message):base(message) { }
    }
}
