using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spaballe : MonoBehaviour
{
    public float m_Speed = 0.01f;
    private Transform m_Transform;
    private Vector3 m_NowPos;                 //現在位置

    // Use this for initialization
    private void Start()
    {
        m_Transform = this.GetComponent<Transform>();
        m_NowPos = m_Transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 vec = m_Transform.localRotation.eulerAngles;
        float muki;
        if (m_Transform.localScale.x >= 0.0f) { muki = 1.0f; } else { muki = -1.0f; }
        m_NowPos.x -= muki * Mathf.Cos((vec.z) * Mathf.PI / 180) * m_Speed;
        m_NowPos.y += muki * Mathf.Sin((vec.z) * Mathf.PI / 180) * m_Speed;
        m_Transform.position = m_NowPos;
    }

    private void syoumetu()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name != "StageBackGround")
        {
            switch (collision.gameObject.tag)
            {
                case "Enemy":
                case "Player":
                case "Branch":
                case "RedBranch":
                case "Leaf":
                case "EnemyBullet":
                    if (!collision.isTrigger)
                    {
                        Debug.Log("Spider Bullet Hited : " + collision.name);
                        syoumetu();
                    }
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!GetComponent<Collider2D>().enabled)
        {
            return;
        }
        switch (collision.gameObject.name)
        {
            case "StageBackGround":
                if (collision.GetComponent<Collider2D>().enabled)
                {
                    syoumetu();
                }
                break;
        }
    }

    //public void SetPos(Vector3 p)
    //{
    //    m_Transform.position = p;
    //}

    //public void SetRotation(Vector3 v)
    //{
    //    m_Transform.localRotation = Quaternion.Euler(v);
    //}
}