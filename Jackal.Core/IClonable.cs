namespace Jackal.Core;

public interface IClonable<T> where T : class
{
    T Clone();
}