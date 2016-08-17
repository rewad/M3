using UnityEngine;
using System.Collections;

public class CachedObject : MonoBehaviour
{

    protected GameObject m_gameObject;
    protected Transform m_transform;

    protected void Cached()
    {
        m_gameObject = gameObject;
        m_transform = transform;
    }
    public Transform CTransform
    {
        get
        {
            if (m_transform == null)
                m_transform = transform;
            return m_transform;
        }
        set { m_transform = value; }
    }

    public GameObject CGameObject
    {
        get
        {
            if (m_gameObject == null)
                m_gameObject = gameObject;
            return m_gameObject;
        }
        set { m_gameObject = value; }
    }
}
