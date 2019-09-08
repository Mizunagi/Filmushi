using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    public List<GameObject> Cursorobjlist;//Cursorを表示するポジションかつ表示するCursorの数と順番
    StageManager stageMG;//ポーズするため
    int nowCursorno;
    int maxCursor;
    GameObject nowCursorobj;
    bool spriteFlg;

    // Use this for initialization
    void Start () {
        maxCursor = Cursorobjlist.Count;
        nowCursorno = 0;

        stageMG = GameObject.Find("StageManager").GetComponent<StageManager>();
        spriteFlg = false;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 workpos = new Vector3();

        if (stageMG.GetSTAGESTA == 1 && spriteFlg == false)
        {
            workpos.Set(Cursorobjlist[nowCursorno].transform.position.x, Cursorobjlist[nowCursorno].transform.position.y, Cursorobjlist[nowCursorno].transform.position.z);
            nowCursorobj = Instantiate(Cursorobjlist[nowCursorno], workpos, Quaternion.identity) as GameObject;
            nowCursorobj.transform.parent = transform;
            spriteFlg = true;

            stageMG.STAchangePAUSE();
        }

    }

    public void CursorHIT()
    {
        if(nowCursorno < maxCursor-1)
        {
            spriteFlg = false;
            nowCursorno++;
        }
    }

    public void SpriteClause()
    {
        stageMG.STAchangeMAIN();
    }
}
