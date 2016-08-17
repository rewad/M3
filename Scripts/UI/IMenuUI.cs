using UnityEngine;
using System.Collections;

public abstract class IMenuUI : CachedObject
{ 

    void Awake()
    {
        Cached();
    }

    public abstract void OpenMenu();
    public abstract void CloseMenu();

}
