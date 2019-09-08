using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegCollision : MonoBehaviour {
    Player pl;
    BoxCollider2D boxCollider2d;
    ContactFilter2D contactFilter2d=new ContactFilter2D();

    private void Awake()
    {
        
    }
    // Use this for initialization
    void Start () {
        pl = GameObject.Find("Player").gameObject.GetComponent<Player>();
        boxCollider2d = GetComponent<BoxCollider2D>();

        contactFilter2d.SetLayerMask(LayerMask.GetMask("Leaf", "Branch","RedBranch"));
        
    }
	
	// Update is called once per frame
	void Update () {

	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Branch" || collision.gameObject.tag=="RedBranch") && !collision.GetComponent<Collider2D>().isTrigger)
        {
            pl.SetJumpFlg(true);
            pl.SetAnimJumpFlg(false);
            //pl.SetGroundRotation(collision.transform.localEulerAngles.z);
        }
        
        else if (collision.gameObject.tag == "RedBranch" && collision.GetComponent<Collider2D>().isTrigger)
        {
            //他に当たっている足場がないか探して、当たっている物があれば無視する
            //他に当たっている足場がなければジャンプフラグをfalseにする
            Collider2D[] result = new Collider2D[3];
            boxCollider2d.OverlapCollider(contactFilter2d, result);
            bool _flag = false;
            foreach(var col in result)
            {
                if (col!=null && !col.isTrigger)
                {
                    _flag = true;
                }
            }
            pl.SetJumpFlg(_flag);
        }
        if (collision.gameObject.tag == "Leaf")
        {
            pl.SetJumpFlg(true);
            pl.SetAnimJumpFlg(false);
            
            
            //フィルムの足場に乗ったことを通知する
            collision.gameObject.GetComponent<Leaf>().SetOnPlayerFlag(true);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //コライダーがポーズ状態なら処理を行わない
        if (GetComponent<BoxCollider2D>().enabled == false)
        {
            return;
        }

        if ((collision.gameObject.tag == "Branch" || collision.gameObject.tag == "RedBranch") && !collision.GetComponent<Collider2D>().isTrigger)
        {
            pl.SetJumpFlg(false);

        }

        if (collision.gameObject.tag == "Leaf")
        {
            pl.SetJumpFlg(false);

            //フィルムの足場から離れたことを通知する
            collision.gameObject.GetComponent<Leaf>().SetOnPlayerFlag(false);
        }
    }
}
