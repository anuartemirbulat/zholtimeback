namespace Services.Interfaces;

public interface IPrototype<T>
{
    T Clone();
}
