using UnityEngine;
using System.Collections;

public class AnimationComponent : CachedObject
{
    private bool m_bPlaying;
    private float m_speed;
    private float m_lastDistance;
    private Vector3 m_directional;
    private Vector3 m_target;
    void Awake()
    {
        Cached();
        m_bPlaying = false;
        m_speed = 0.0f;
        m_lastDistance = 0.0f;
        m_directional = Vector3.zero;
        m_target = Vector3.zero;
    }

    public void AnimationTo(Vector3 target, float time)
    {
        if (m_bPlaying) return;

        m_target = target;
        m_lastDistance = (target - CTransform.position).magnitude;
        m_speed = m_lastDistance / time;
        m_directional = (target - CTransform.position).normalized;
        m_bPlaying = true;
    }

    public bool IsPlaying()
    {
        return m_bPlaying;
    }

    private void AnimationUpdate()
    {
        CTransform.position += m_directional * m_speed * Time.deltaTime;
        float new_distance = (m_target - CTransform.position).magnitude;
        if (new_distance > m_lastDistance)
        {
            CTransform.position = m_target; 
            m_bPlaying = false;
        }
        m_lastDistance = new_distance;
    }
   
    void Update()
    {
        if (m_bPlaying)
            AnimationUpdate();
    }
 
}
