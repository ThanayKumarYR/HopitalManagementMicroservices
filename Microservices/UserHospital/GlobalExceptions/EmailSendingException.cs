namespace UserHospital.GlobalExceptions
{
    public class EmailSendingException : Exception
    {
        public EmailSendingException(string message):base(message) { }
    }
}
