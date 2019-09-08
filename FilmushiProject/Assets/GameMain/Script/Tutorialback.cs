using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorialback : MonoBehaviour {

    Tutorialsprite sprite;

    // Use this for initialization
    void Start () {
        sprite = transform.GetComponentInParent<Tutorialsprite>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseUpAsButton()
    {
        sprite.SpriteClause();
    }
}
