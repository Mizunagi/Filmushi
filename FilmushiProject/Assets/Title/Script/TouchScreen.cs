using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchScreen : MonoBehaviour {

    private int count;
    private bool renderFlag;
    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
        renderFlag = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (renderFlag)
        {
            count++;
            if ((count / 60) % 2 > 0)
            {
                spriteRenderer.enabled = false;
            }
            else
            {
                spriteRenderer.enabled = true;
            }
        }
	}

    public void StartRender()
    {
        renderFlag = true;
    }
}
