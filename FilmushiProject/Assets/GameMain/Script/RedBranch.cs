using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBranch : MonoBehaviour {

    enum REDBRANCHSTA
    {
        HIDDEN_ON = 0,
        HIDDEN_OFF
    }
    REDBRANCHSTA redbranchsta;
    BoxCollider2D collider;
    SpriteRenderer renderer;
    Color hidden_on_col;
    Color hidden_off_col;

    Clip clip;
    ContactFilter2D redFilmFilter=new ContactFilter2D();
    Collider2D[] result = new Collider2D[1];    //赤フィルムとの接触確認用

    // Use this for initialization
    void Start () {
        redbranchsta = REDBRANCHSTA.HIDDEN_OFF;
        collider = new BoxCollider2D();
        renderer = new SpriteRenderer();
        collider = transform.GetComponent<BoxCollider2D>();
        renderer = transform.GetComponent<SpriteRenderer>();
        hidden_on_col = new Color(1, 1, 1, 0);
        hidden_off_col = new Color(1, 1, 1, 1);

        clip = GameObject.Find("clip_L").GetComponent<Clip>();
        redFilmFilter.SetLayerMask(LayerMask.GetMask("RedFilm"));
        redFilmFilter.useTriggers = true;
    }
	
	// Update is called once per frame
	void Update () {

        //一つ以上の赤フィルムと接していたら消す
        if(collider.OverlapCollider(redFilmFilter, result) >= 1)
        {
            RedBranchSTAchangeHIDDEN_ON();
        }
        else
        {
            RedBranchSTAchangeHIDDEN_OFF();
        }


        //確認用
		//if(Input.anyKeyDown)
  //      {
  //          if(redbranchsta == 0)
  //          {
  //              RedBranchSTAchangeHIDDEN_OFF();
  //          }
  //          else
  //          {
  //              RedBranchSTAchangeHIDDEN_ON();
  //          }
            
  //      }
	}

    //赤枝ステータスゲッター
    public int GetRedBranchSTA
    {
        get { return (int)redbranchsta; }
    }

    //赤枝ステータスを表示に
    public void RedBranchSTAchangeHIDDEN_OFF()
    {
        redbranchsta = REDBRANCHSTA.HIDDEN_OFF;
        collider.isTrigger = false;
        //print(collider.isTrigger);
        renderer.color = hidden_off_col;
    }

    //赤枝ステータスを非表示に
    public void RedBranchSTAchangeHIDDEN_ON()
    {
        redbranchsta = REDBRANCHSTA.HIDDEN_ON;
        collider.isTrigger = true;
        //print(collider.isTrigger);
        renderer.color = hidden_on_col;
    }
}
