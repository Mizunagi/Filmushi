using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFilm : MonoBehaviour {

    //赤フィルム専用処理
    BoxCollider2D boxCollider2d;
    ContactFilter2D redBranchFilter = new ContactFilter2D();
    ContactFilter2D testFilter = new ContactFilter2D();
    Collider2D[] hitCollider;
    List<Collider2D> oldHitCollider=new List<Collider2D>();
    int redBranchCount;

	// Use this for initialization
	void Start () {
        boxCollider2d = GetComponent<BoxCollider2D>();

        redBranchFilter.SetLayerMask(LayerMask.GetMask("RedBranch"));
        redBranchCount = GameObject.FindGameObjectsWithTag("RedBranch").Length;
        hitCollider = new Collider2D[redBranchCount];

        //そもそも赤枝が存在しない場合スクリプトを削除
        if (redBranchCount == 0)
        {
            Destroy(this);
        }
	}
	
	// Update is called once per frame
	void Update () {
        //当たっている赤枝も前回のフレームで当たっていた赤枝もない場合リターン
        //if(boxCollider2d.OverlapCollider(redBranchFilter, hitCollider) == 0 && oldHitCollider.Count==0)
        //{
        //    return;
        //}

        //foreach(var redBranch in hitCollider)
        //{
        //    if (redBranch == null)
        //    {
        //        break;
        //    }
        //    foreach(var old in oldHitCollider)
        //    {

        //    }
        //    redBranch.GetComponent<RedBranch>().RedBranchSTAchangeHIDDEN_ON();
        //}
	}
}
