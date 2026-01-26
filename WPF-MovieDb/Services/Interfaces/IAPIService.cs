namespace WPF_MovieDb.Services;


public interface IAPIService<TTable> where TTable : class
{
    void Add(TTable entry);
    TTable? Get(int id);
    List<TTable> GetAll();
    void Remove(int id);
}
