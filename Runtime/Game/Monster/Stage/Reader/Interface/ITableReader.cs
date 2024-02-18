public interface ITableReader<T>
{
    T ReadTable(string jsonPath);
}