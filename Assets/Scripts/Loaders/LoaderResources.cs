using UnityEngine;

public class LoaderResources<T> : Loader<T> where T : Object
{   
    public override T Load(string path)
    {
        return Resources.Load<T>(path);
    }

    public override T[] LoadAll(string path)
    {
        return Resources.LoadAll<T>(path);
    }
}