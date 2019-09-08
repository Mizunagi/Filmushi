using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnLockDirection : MonoBehaviour {

    PauseManager pauseManager;
    float timeCount;
    Transform tf;
    bool fallFlag;
    GameObject pageSet;

	// Use this for initialization
	void Start () {
        pauseManager = GameObject.Find("PauseManager").GetComponent<PauseManager>();
        timeCount = 0;
        tf = transform;
        fallFlag = false;
        pageSet = GameObject.Find("PageSet");
        pauseManager.PausePrefab(pageSet);
        pauseManager.Resume(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        timeCount += Time.deltaTime;
        if (timeCount < 1)
        {
            Vector3 nowPos = tf.localPosition;
            nowPos.x = Mathf.PingPong(Time.time, 0.1f) - 0.05f;
            nowPos.y -= 0.003f;
            tf.localPosition = nowPos;
        }
        else if(timeCount>1 && timeCount < 1.5f)
        {
            Vector3 nowPos = tf.localPosition;
            nowPos.x = 0;
            tf.localPosition = nowPos;
        }
        else if (timeCount > 1.5f && !fallFlag)
        {
            gameObject.AddComponent<Rigidbody2D>();
            fallFlag = true;
            pauseManager.ResumePrefab(pageSet);
        }
        if(fallFlag && tf.position.y < -100)
        {
            Destroy(this.gameObject);
        }
	}
}
