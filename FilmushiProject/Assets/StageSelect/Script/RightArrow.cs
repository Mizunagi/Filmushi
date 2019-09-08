using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightArrow : MonoBehaviour {

    StageSelectManager stageSelectManager;
    SpriteRenderer arrowSprite;

    // Use this for initialization
    void Start () {
        stageSelectManager = GameObject.Find("StageSelectManager").GetComponent<StageSelectManager>();
        arrowSprite = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (stageSelectManager.GetMaxPageFlag())
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
        stageSelectManager.MoveRightPage();
    }
}
