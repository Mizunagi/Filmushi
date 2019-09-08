using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorialback2 : MonoBehaviour {

    Tutorialsprite2 sprite;

    // Use this for initialization
    void Start()
    {
        sprite = transform.GetComponentInParent<Tutorialsprite2>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseUpAsButton()
    {
        sprite.SpriteClause();
    }
}
