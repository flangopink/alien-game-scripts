using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingAlien : MonoBehaviour
{

    [SerializeField] Vector3 m_from = new Vector3(0.0F, 0.0F, 10.0F);
    [SerializeField] Vector3 m_to = new Vector3(0.0F, 0.0F, -10.0F);
    [SerializeField] float m_frequency = 0.5F;

    void Update()
    {
        Quaternion from = Quaternion.Euler(m_from);
        Quaternion to = Quaternion.Euler(m_to);

        float lerp = 0.5F * (1.0F + Mathf.Sin(Mathf.PI * Time.realtimeSinceStartup * m_frequency));
        transform.localRotation = Quaternion.Lerp(from, to, lerp);
    }
}
