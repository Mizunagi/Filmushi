using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderDetectBox : MonoBehaviour
{
    private GameObject _parent;

    private void Start()
    {
        _parent = this.transform.parent.gameObject;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _parent.GetComponent<spa>().cntdn();
        }
    }
}