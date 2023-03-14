using System.Net.Sockets;

namespace Server;

//This is whole class is as a replacement for List that won't give errors if 2 threads are trying to edit/access it at the same time,
//it will practically queue up the Tasks.
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
        Console.WriteLine("Added to list of " + typeof(T).ToString());
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
        _internalList.RemoveAt(index); Console.WriteLine("Removed from a list of: " + typeof(T));
        isBusy = false;
    }

    public List<T> GetCopyOfInternalList()
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
        //if(_internalList[index] == null) Console.WriteLine(" Reading a Nul value   gr9gefciadcbpe");
        T value = _internalList[index]; Console.WriteLine("Read list of " + typeof(T).ToString());
        isBusy = false;
        if(value == null) Console.WriteLine("Returned NUll Value                            0123i12y301294713g  0hcugfowuhf" );
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