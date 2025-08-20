namespace Business.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public static void ThrowIfNull<T>(T t, string message)
    {
        if (t != null)
            return;
        throw new NotFoundException(message);
    }
}