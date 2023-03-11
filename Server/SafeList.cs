using System.Net.Sockets;

namespace Server;

public class SafeList<T>
{
    private List<T> _internalList;
    private bool isBusy = false;
    public SafeList()
    {
        _internalList = new List<T>();
    }

    public void Add(T value)
    {
        while (isBusy);
        isBusy = true;
        _internalList.Add(value);
        isBusy = false;
    }
    public void Remove(T value)
    {
        while (isBusy);
        isBusy = true;
        _internalList.Remove(value);
        isBusy = false;
    }
    public void RemoveAt(int index)
    {
        while (isBusy);
        isBusy = true;
        _internalList.RemoveAt(index);
        isBusy = false;
    }

    public List<T> getCopyOfInternalList()
    {
        while (isBusy);
        isBusy = true;
        List<T> copyList = new List<T>(_internalList);
        isBusy = false;
        return copyList;
    }

    public T GetAt(int index)
    {
        while (isBusy) ;
        isBusy = true;
        T value = _internalList[index];
        isBusy = false;
        return value;
    }

    public int GetCount()
    {
        while (isBusy) ;
        isBusy = true;
        int value = _internalList.Count;
        isBusy = false;
        return value;
    }
}