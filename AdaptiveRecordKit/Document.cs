namespace AdaptiveRecordKit;

public abstract class Document<T>
{
    protected readonly string _path;
    protected readonly List<T> _listOfT;
    
    protected Document(string path)
    {
        if (Path.Exists(path))
        {
            _path = path;
            _listOfT = new List<T>();
        }
        else
        {
            throw new ArgumentException("Path does not exists");
        }
    }

    protected abstract void ParseFile();
    public abstract void SaveFile();
       

    public bool TryUpdate(Predicate<T> predicate, T updatedObj)
    {
        var index = _listOfT.FindIndex(predicate);
        if (index == -1)
        {
            return false;
        }

        _listOfT[index] = updatedObj; 
        return true;
    }

    public IReadOnlyCollection<T> GetAll()
    {
        return _listOfT;
    }

    public T? Get(Func<T, bool> predicate)
    {
        return _listOfT.FirstOrDefault(predicate);
    }

    public void Add(T obj)
    {
        _listOfT.Add(obj);
    }

    public void DeleteAll(Predicate<T> predicate)
    {
        _listOfT.RemoveAll(predicate);
    }
}
