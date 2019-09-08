using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFont : MonoBehaviour {

    public float Speed;//移動ポジション
    StageManager stageMG;
    float stopfontpos = -35.0f;//ストップｘポジション
    private bool startendflg;
    public float WaitTime;
    float starttime;
    float nowtime;

    // Use this for initialization
    void Start () {
        startendflg = false;
        starttime = Time.time;
        nowtime = starttime;
    }
	
	// Update is called once per frame
	void Update () {
        stageMG = transform.GetComponentInParent<StageManager>();
        int sta = stageMG.GetSTAGESTA;
        Vector3 velocity = new Vector3(0,1,0);

        if (sta == 0)//Start
        {
            if(nowtime - starttime < WaitTime)
            {
                nowtime += Time.deltaTime;
            }
            else
            {
                transform.position -= velocity * Speed * Time.deltaTime;//移動
                                                                        //止めるため
                if (transform.position.y < stopfontpos)//ストップ超える
                {
                    stageMG.STAchangeMAIN();
                    //Destroy(gameObject);//Start文字消そう
                    startendflg = true;
                }
            }
            
        }

        //止めるためコメントアウト
        //if (transform.position.y < stopfontpos)//ストップ超える
        //{
        //    stageMG.STAchangeMAIN();
        //    //Destroy(gameObject);//Start文字消そう
        //    startendflg = true;
        //}

    }
    public bool Startflg()
    {
        return startendflg;
    }

}
