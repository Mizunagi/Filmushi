using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorialsprite : MonoBehaviour {
    TutorialCursor Cursor;

    // Use this for initialization
    void Start () {
        Cursor = transform.GetComponentInParent<TutorialCursor>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpriteClause()
    {
        Cursor.SpriteClause();

        Destroy(gameObject);
    }
}
