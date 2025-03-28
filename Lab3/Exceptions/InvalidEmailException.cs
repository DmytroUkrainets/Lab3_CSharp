namespace Lab3.Exceptions
{
    public class InvalidEmailException : Exception
    {
        public InvalidEmailException() : base("Некоректна адреса електронної пошти.") { }
    }
}
