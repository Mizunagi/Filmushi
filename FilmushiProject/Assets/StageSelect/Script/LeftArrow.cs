using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArrow : MonoBehaviour {

    StageSelectManager stageSelectManager;
    SpriteRenderer arrowSprite;

    // Use this for initialization
    void Start () {
        stageSelectManager = GameObject.Find("StageSelectManager").GetComponent<StageSelectManager>();
        arrowSprite = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (stageSelectManager.GetMinPageFlag())
        {
            arrowSprite.enabled = false;
        }
        else
        {
            arrowSprite.enabled = true;
        }
	}

    private void OnMouseUpAsButton()
    {
        stageSelectManager.MoveLeftPage();
    }
}
