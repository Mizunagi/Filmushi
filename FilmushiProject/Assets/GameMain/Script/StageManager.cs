using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public List<GameObject> stagelist;

    public enum STAGESTA
    {
        START = 0,//ステート演出
        MAIN,//ゲームメイン
        GOAL,//GAUL演出
        END,//クリア演出
        PAUSE,//ポーズがいるかも
        MENUPAUSE//メニュー用ポーズ
    }

    private static STAGESTA sta;//ステージ遷移のステータス
    public Vector3 stagepos;//ステージ表示ポジション
    private int nowstageno;//今のステージのNo
    private int cnt;//ステージリストのカウント（Max数）
    private GameStage gamestage;
    private GameObject nowobj;//表示中ステージプレハブ

    //GameObject nextobj;//次無しでもいけると思った
    private PauseManager PauseMG;

    private Clip clip;
    private GameObject clipobj;
    private GameObject menuobj;
    private GameObject time;

    //リザルトへ送るデータ
    private SaveData sendSaveData;

    private AcornManager acornmanage;

    private ViewTimeMainScene timer;

    // Use this for initialization
    private void Awake()
    {
        sta = STAGESTA.START;
        Vector3 workpos = new Vector3();
        PauseMG = GameObject.Find("PauseManager").GetComponent<PauseManager>();

        nowstageno = 0;
        cnt = stagelist.Count;

        /******************************
         *ステージ生成S
         ******************************/
        workpos.Set(stagepos.x, stagepos.y, stagepos.z);
        nowobj = Instantiate(stagelist[nowstageno], workpos, Quaternion.identity) as GameObject;
        nowobj.transform.parent = transform;
        /******************************
         *ステージ生成E
         ******************************/
    }

    private void Start()
    {
        //GameObject obj;

        menuobj = GameObject.Find("Menu");
        time = transform.Find("ViewTimeMain").gameObject;
        clipobj = GameObject.Find("clip_L");
        clip = clipobj.GetComponent<Clip>();
        /******************************
         *ポーズするS
         ******************************/
        PauseMG.PauseAllChildren(nowobj);
        PauseMG.PauseTagObject("Player");
        PauseMG.PauseAllChildren(menuobj);
        PauseMG.Pause(time, PauseManager.MONO);
        PauseMG.PauseAllChildren(clipobj, PauseManager.MONO);
        PauseMG.Pause(clipobj, PauseManager.MONO);

        this.sendSaveData.stageName = SceneManager.GetActiveScene().name;
        SaveDataManager.Instance.temp = this.sendSaveData;

        this.acornmanage = GameObject.Find("AcornManager").GetComponent<AcornManager>();
        this.timer = GameObject.Find("ViewTimeMain").GetComponent<ViewTimeMainScene>();
        /******************************
         *ポーズするE
         ******************************/
    }

    // Update is called once per frame
    private void Update()
    {
        // Vector3 velocity = new Vector3(0,-1,0);//ここで落とそうとした名残

        /********************************
         *アップデートS（今はなし）
         ********************************/
        switch (sta)
        {
            case STAGESTA.START:

                break;

            case STAGESTA.MAIN:

                break;

            case STAGESTA.GOAL:

                break;

            case STAGESTA.END:
                break;
        }
        /********************************
         *アップデートE（今はなし）
         ********************************/
    }

    /***********************
     *ステータスMAINへ
     ************************/

    public void STAchangeMAIN()
    {
        //追加。ステージが落ちてから呼ばれるからエンドに行く処理をここに追加
        if (cnt - 1 < nowstageno)
        {
            /************************************
            *ステージクリア
            *************************************/
            STAchangeEND();
        }
        else
        {
            sta = STAGESTA.MAIN;
            /********************************
             *メイン画面に行くからポーズ解除S
             ********************************/
            PauseMG.ResumeAllChildren(nowobj);
            PauseMG.ResumeTagObject("Player");
            PauseMG.ResumeTagObject("Film");
            PauseMG.ResumeAllChildren(menuobj);
            PauseMG.Resume(time, PauseManager.MONO);
            PauseMG.ResumeAllChildren(clipobj, PauseManager.MONO);
            PauseMG.Resume(clipobj, PauseManager.MONO);
            /********************************
             *メイン画面に行くからポーズ解除E
             ********************************/
        }
    }

    /********************************
    *ステータスをGAULに
    ********************************/

    public void STAchangeGOAL()
    {
        Vector3 workpos = new Vector3();

        /************************************
        *GAULに当たったステージが最後かどうか
        *************************************/
        //落とすだけになるのでコメントアウト
        //if (cnt-1 > nowstageno)
        //{
        print("ステージMGゴールセット");
        sta = STAGESTA.GOAL;
        /************************************
        *ステージのステータスをENDにS
        *************************************/
        gamestage = nowobj.GetComponent<GameStage>();
        gamestage.GameStageStaSetEND();
        /************************************
        *ステージのステータスをENDにE
        *************************************/

        /************************************
        *表のステージ、プレイヤーポーズS
        *************************************/
        PauseMG.PauseAllChildren(nowobj);
        PauseMG.PauseTagObject("Player");
        //PauseMG.ResumeTagObject("Player");
        PauseMG.PauseAllChildren(menuobj);
        PauseMG.Pause(time, PauseManager.MONO);
        //PauseMG.PauseAllChildren(clipobj);
        /************************************
        *表のステージ、プレイヤーポーズE
        *************************************/

        nowstageno++;//ステージNo更新

        clip.FallAllFilm();//ふぃるむ全部落とす
        //ステージ生成時に最後かどうか判断
        if (cnt > nowstageno)
        {
            /************************************
            *裏ステージ生成S
            *************************************/
            workpos.Set(stagepos.x, stagepos.y, stagepos.z);
            nowobj = Instantiate(stagelist[nowstageno], workpos, Quaternion.identity) as GameObject;
            nowobj.transform.parent = transform;
            /************************************
            *裏ステージ生成E
            *************************************/

            /************************************
            *裏ステージポーズS
            *************************************/
            PauseMG.PauseAllChildren(nowobj);
            /************************************
            *裏ステージポーズE
            *************************************/
        }

        //セーブデータ処理
        this.sendSaveData.stageName = SceneManager.GetActiveScene().name;
        this.sendSaveData.cleartime = this.timer.GetTime();
        this.sendSaveData.maxAcorns = this.acornmanage.GetMAXAcorn;
        this.sendSaveData.cntAcorns = this.acornmanage.GetCntAcorn;
        SaveDataManager.Instance.temp = sendSaveData;
        //エンドに行く処理はなくなるのでコメントアウト
        //else
        //{
        /************************************
        *ステージクリア
        *************************************/
        //    STAchangeEND();
        //}
    }

    /************************************
    *ステージ終了－＞クリア演出
    *************************************/

    public void STAchangeEND()
    {
        sta = STAGESTA.END;
        /************************************
        *ステージポーズS
        *************************************/
        //ポーズしなくていいのでお面とアウト
        //PauseMG.PauseAllChildren(nowobj);
        //PauseMG.PauseTagObject("Player");
        //PauseMG.PauseTagObject("Film");
        /************************************
        *ステージポーズE
        *************************************/
    }

    /************************************
    *Stage全体ポーズ
    *************************************/

    public void STAchangePAUSE()
    {
        sta = STAGESTA.PAUSE;
        /************************************
        *ステージポーズS
        *************************************/
        PauseMG.PauseAllChildren(nowobj);
        //PauseMG.PauseTagObject("Player");
        PauseMG.PauseTagObject("Film");
        PauseMG.PauseAllChildren(menuobj);
        PauseMG.Pause(time, PauseManager.MONO);
        PauseMG.PauseAllChildren(clipobj, PauseManager.MONO);
        PauseMG.Pause(clipobj, PauseManager.MONO);
        /************************************
        *ステージポーズE
        *************************************/
    }

    /************************************
    *メニュー用ポーズ
    *************************************/

    public void STAchangeMENUPAUSE()
    {
        sta = STAGESTA.MENUPAUSE;
        /************************************
        *ステージポーズS
        *************************************/
        PauseMG.PauseAllChildren(nowobj);
        //PauseMG.PauseTagObject("Player");
        PauseMG.PauseTagObject("Film");
        //PauseMG.PauseAllChildren(menuobj);
        PauseMG.Pause(time, PauseManager.MONO);
        PauseMG.PauseAllChildren(clipobj, PauseManager.MONO);
        PauseMG.Pause(clipobj, PauseManager.MONO);
        /************************************
        *ステージポーズE
        *************************************/
    }

    /************************************
    *ステージステータスゲッター
    *************************************/

    public int GetSTAGESTA
    {
        get { return (int)sta; }
    }
}