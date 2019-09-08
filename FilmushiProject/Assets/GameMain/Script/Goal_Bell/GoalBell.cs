using System.Collections.Generic;
using UnityEngine;

public class GoalBell : MonoBehaviour
{
    public GameObject GoalObj;
    public GameObject BellObj;
    public Vector3 GoalPos;
    public List<Vector3> BellPosList;
    private GameStage gamestage;

    private static int BellCount;

    // Use this for initialization
    private void Start()
    {
        Vector3 workpos = new Vector3();
        GameObject obj;

        BellCount = BellPosList.Count;
        //print(BellCount);

        for (int i = 0; i < BellCount; i++)
        {
            workpos.Set(BellPosList[i].x, BellPosList[i].y + 0.22f, 1);
            obj = Instantiate(BellObj, workpos, Quaternion.identity) as GameObject;
            obj.transform.parent = transform;
        }

        if (BellCount <= 0)
        {
            workpos.Set(GoalPos.x, GoalPos.y, 1);
            obj = Instantiate(GoalObj, workpos, Quaternion.identity) as GameObject;
            obj.transform.parent = transform;
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    /*********************
     *ベルの数、数える
     *********************/

    public void CountSub()
    {
        Vector3 workpos = new Vector3();
        GameObject obj;

        //print(BellCount);//ログ
        BellCount--;

        //print(BellCount);//ログ

        if (BellCount < 1)
        {
            //print("true");//ログ
            workpos.Set(GoalPos.x, GoalPos.y, 1);
            obj = Instantiate(GoalObj, workpos, Quaternion.identity) as GameObject;
            obj.transform.parent = transform;
        }
    }

    /********************************
     *ステージにゴールしたよと送る
     ********************************/

    public void SendGameStageGOAL()
    {
        gamestage = transform.GetComponentInParent<GameStage>();
        if (gamestage.GetGameStageSta == 0)
        {
            //print("ゴールベルsend");
            gamestage.SendStageMGGOAL();
        }
    }
}