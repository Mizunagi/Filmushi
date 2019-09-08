using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spaback : MonoBehaviour
{
    private GameObject _parent;
    private float m_Time = 0.0f;

    private void Start()
    {
        _parent = this.transform.parent.gameObject;
    }

    private void Update()
    {
        if (m_Time >= 0.0f)
        { m_Time -= 1.0f; }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (m_Time <= 0.0f)
        {
            if (collision.tag == "Player")
            {
                _parent.GetComponent<spa>().gorugo();
                m_Time = 50.0f;
            }
        }
    }
}