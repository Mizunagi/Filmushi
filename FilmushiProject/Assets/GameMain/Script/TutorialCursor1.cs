using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCursor1 : MonoBehaviour
{
    public GameObject spriteobj;
    public float MaxWaitTime;
    private TutorialManager1 TutorialMG;
    private GameObject spriteInstance;
    float waittime;
    bool hitflg;

    // Use this for initialization
    private void Start()
    {
        TutorialMG = transform.GetComponentInParent<TutorialManager1>();
        Vector3 workpos = new Vector3();

        workpos.Set(0.0f, -5.0f, -10.0f);
        spriteInstance = Instantiate(spriteobj, workpos, Quaternion.identity) as GameObject;
        spriteInstance.transform.parent = transform;
        waittime = 0;
        hitflg = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (hitflg)
        {
            if (MaxWaitTime > waittime)
            {
                waittime += Time.deltaTime;
            }
            else
            {
                TutorialMG.CursorHIT();

                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !other.isTrigger)
        {
            //ここに当たった音入れて
            hitflg = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            //TutorialMG.CursorHIT();

            //Destroy(gameObject);
        }
    }

    public void SpriteClause()
    {
        TutorialMG.SpriteClause();
    }
}