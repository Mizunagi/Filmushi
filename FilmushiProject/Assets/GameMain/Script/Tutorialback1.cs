using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorialback1 : MonoBehaviour {
    Tutorialsprite1 sprite;

    // Use this for initialization
    void Start()
    {
        sprite = transform.GetComponentInParent<Tutorialsprite1>();

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
