using UnityEngine;

public abstract class Loader<T> where T : Object
{
    public abstract T Load(string path);
    public abstract T[] LoadAll(string path);
}