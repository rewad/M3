using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourcesManager : MonoBehaviour
{
    private static Dictionary<string, GameObject> m_dictionaryResources;

    public static GameObject GetResource(string name_resource)
    {
        if (m_dictionaryResources == null) m_dictionaryResources = new Dictionary<string, GameObject>();

        GameObject obj = null;
        if (m_dictionaryResources.TryGetValue(name_resource, out obj))
        {
            return obj;
        }

        obj = Resources.Load(name_resource) as GameObject;
        m_dictionaryResources.Add(name_resource, obj);
        return obj;
    } 
}
