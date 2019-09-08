using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager1 : MonoBehaviour
{
    public List<GameObject> Cursorobjlist;//Cursorを表示するポジションかつ表示するCursorの数と順番
    private StageManager stageMG;//ポーズするため
    private PauseManager PauseMG;
    private int nowCursorno;
    private int maxCursor;
    private GameObject nowCursorobj;
    private bool spriteFlg;
    private GameObject Film;

    // Use this for initialization
    private void Start()
    {
        maxCursor = Cursorobjlist.Count;
        nowCursorno = 0;

        stageMG = GameObject.Find("StageManager").GetComponent<StageManager>();
        PauseMG = new PauseManager();
        spriteFlg = false;
        Film = stageMG.GetComponentInChildren<FilmHolder>().gameObject;
        Film.active = false;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 workpos = new Vector3();

        if (stageMG.GetSTAGESTA == 1 && spriteFlg == false)
        {
            workpos.Set(Cursorobjlist[nowCursorno].transform.position.x, Cursorobjlist[nowCursorno].transform.position.y, Cursorobjlist[nowCursorno].transform.position.z);
            nowCursorobj = Instantiate(Cursorobjlist[nowCursorno], workpos, Quaternion.identity) as GameObject;
            nowCursorobj.transform.parent = transform;
            spriteFlg = true;

            stageMG.STAchangePAUSE();

            if (nowCursorno == 2)
            {
                Film.active = true;
            }
        }

        if (stageMG.GetSTAGESTA == (int)StageManager.STAGESTA.GOAL &&
            this.nowCursorno < this.maxCursor - 1 &&
            this.nowCursorobj.GetComponent<SpriteRenderer>().enabled)
        {
            this.nowCursorobj.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void CursorHIT()
    {
        if (nowCursorno < maxCursor - 1)
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