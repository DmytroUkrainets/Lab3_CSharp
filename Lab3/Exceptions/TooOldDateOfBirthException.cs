namespace Lab3.Exceptions
{
    public class TooOldDateOfBirthException : Exception
    {
        public TooOldDateOfBirthException() : base("Дата народження занадто стара.") { }
    }
}
