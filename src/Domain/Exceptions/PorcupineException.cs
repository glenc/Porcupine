namespace Porcupine.Domain.Exceptions;

public class PorcupineException : ApplicationException
{
    public PorcupineException(string message) : base(message) { }
}
