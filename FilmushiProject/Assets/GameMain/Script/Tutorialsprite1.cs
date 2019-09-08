using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorialsprite1 : MonoBehaviour {

    TutorialCursor1 Cursor;

    // Use this for initialization
    void Start()
    {
        Cursor = transform.GetComponentInParent<TutorialCursor1>();

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
