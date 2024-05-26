namespace Jackal
{
    public interface IClonable<T> where T : class
    {
        T Clone();
    }
}