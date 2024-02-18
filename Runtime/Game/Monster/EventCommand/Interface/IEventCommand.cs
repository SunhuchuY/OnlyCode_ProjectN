
public interface IEventCommand<T> where T : struct
{
    Monster Owner { get; }
    T Add { get; }
    void Event();
}