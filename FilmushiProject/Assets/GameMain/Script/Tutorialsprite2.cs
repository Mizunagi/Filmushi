using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorialsprite2 : MonoBehaviour {

    TutorialCursor2 Cursor;

    // Use this for initialization
    void Start()
    {
        Cursor = transform.GetComponentInParent<TutorialCursor2>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpriteClause()
    {
        Cursor.SpriteClause();

        Destroy(gameObject);
    }
}
