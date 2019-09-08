using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whitewall : MonoBehaviour {

    public Vector3 waitpos;//待ちポジション
    public Vector3 onpos;//上にかかるポジション
    StageManager stagemanager;

    // Use this for initialization
    void Start () {
        transform.position = waitpos;
        stagemanager = transform.GetComponentInParent<StageManager>();
    }
	
	// Update is called once per frame
	void Update () {
        int sta = stagemanager.GetSTAGESTA;

        if(sta == 0 || sta == 3)//Start、END時
        {
            transform.position = onpos;
        }
        else
        {
            transform.position = waitpos;
        }
    }
}
