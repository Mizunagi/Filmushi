using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStage : MonoBehaviour
{
    enum GAMESTAGESTA
    {
        NORMAL = 0,//通常
        END,//クリア－＞落ちる
        PAUSE//ポーズいるかと思ったがいらないかも
    }

    GAMESTAGESTA gamestagesta;//ステージ自体のステータス
    public float Speed;//落ちるスピード
    float endzpos = -1.5f;//落ちるときのｚポジション
    Vector3 velocity;
    float deleteypos = -35.0f;//消えるｙポジション
    StageManager satgeMG;
    PauseManager PauseMG;

    // Use this for initialization
    void Start()
    {
        gamestagesta = 0;
        velocity = new Vector3(0,-1,0);
        satgeMG = transform.GetComponentInParent<StageManager>();

    }

    // Update is called once per frame
    void Update()
    {
       // print(gamestagesta);
       switch(gamestagesta)
        {
            case GAMESTAGESTA.NORMAL:
                break;
            case GAMESTAGESTA.END:
                //print("落ち中");
                transform.position += velocity * Speed * Time.deltaTime;
                if (transform.position.y < deleteypos)
                {
                    satgeMG.STAchangeMAIN();
                    Destroy(gameObject);
                }
                break;
            case GAMESTAGESTA.PAUSE:
                break;
        }
    }

    /*********************************
     *ステージ自体のステータスENDに
     *********************************/
    public void GameStageStaSetEND()
    {
        //print("ステージEND");
        Vector3 work = new Vector3(transform.position.x, transform.position.y, endzpos);
       
        gamestagesta = GAMESTAGESTA.END;
        transform.position = work;
        
    }

    /*********************************
     *ステージ自体のステータスPAUSEに
     *********************************/
    public void GameStageStaSetPAUSE()
    {
        gamestagesta = GAMESTAGESTA.PAUSE;
    }

    /*********************************
     *ステージ自体のステータスNORMALに
     *********************************/
    public void GameStageStaSetNORMAL()
    {
        gamestagesta = GAMESTAGESTA.NORMAL;
    }

    /***************************************
     *ステージマネージャーへゴールしたと送る
     ***************************************/
    public void SendStageMGGOAL()
    {
        print("ゲームステージsend");
        satgeMG.STAchangeGOAL();
    }

    /*********************************
     *ステージ自体のステータスゲッター
     *********************************/
    public int GetGameStageSta
    {
        get { return (int)gamestagesta; }
    }
}