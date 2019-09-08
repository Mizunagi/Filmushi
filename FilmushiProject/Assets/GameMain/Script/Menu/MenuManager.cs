using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    public enum MenuState
    {
        SELECTSTAGE,
        RESTART,
        KEEP_BACK,
        NOT_SELECT,

    }

    private Transform backgroundTransform; //バックグラウンド座標呼び出し
    private Transform stageselectTransform; //ステージセレクト確認画面の座標の呼び出し
    private Transform restartTransform; //リスタート確認画面の座標の呼び出し
    private Transform menutextTransform;//メニュー画面の座標の呼び出し
    private Vector3 backgroundPosition; //バックグラウンド座標保有
    private Vector3 selectPosition;     //ステージセレクト確認画面の座標保有
    private Vector3 restartPosition;    //リスタート確認画面の座標保有
    private Vector3 menuPosition;        //メニュー画面の座標保有
    private bool menuflg;               //メニューへのタップを許可するフラグ
    private bool startmenuflg;          //メニューを表示するのを許可するフラグ
    private MenuConfirmationRestart menurestartManager; //リスタートクラス呼び出し
    private MenuConfirmationSelect menuselectManager; //ステージセレクトクラス呼び出し
    private PauseManager pause;                     //ポーズクラスを呼び出し
    private GameObject timestart;
    MenuState ms;
    StageManager stageMG;

    // Use this for initialization
    void Start () {
        ms = MenuState.NOT_SELECT;
        //クラス取得
        menurestartManager = GameObject.Find("MenuManager").GetComponent<MenuConfirmationRestart>();
        menuselectManager = GameObject.Find("MenuManager").GetComponent<MenuConfirmationSelect>();
        pause = GameObject.Find("PauseManager").GetComponent<PauseManager>();
        timestart = GameObject.Find("StageManager").transform.Find("ViewTimeMain").gameObject;
        stageMG = GameObject.Find("StageManager").GetComponent<StageManager>();
        //座標取得
        backgroundTransform = GameObject.Find("black_background_Confirmation").transform;
        stageselectTransform = GameObject.Find("Copy of text_window").transform;
        restartTransform = GameObject.Find("Copy of text_window_stage_select").transform;
        menutextTransform = GameObject.Find("menuscreen").transform;
        //transformの座標をvector3の値保有変数に渡す
        backgroundPosition =backgroundTransform.position;
        selectPosition = stageselectTransform.position;
        restartPosition = restartTransform.position;
        menuPosition = menutextTransform.position;
 
    }
	
	// Update is called once per frame
	void Update () {
        //確認メニューからメニューに遷移したときmenuを表示する
        if (menurestartManager.GetMenu() == true || menuselectManager.GetMenu() == true)
        {
           menuflg = true;
        }

    }
    //メニュー画面のセレクト関数
    public void SelectMenu(MenuState setnum)
    {
        if (menuflg == true)
        {
           startmenuflg = true;
            ms = setnum;
            switch (ms)
            {
                //ステージセレクト確認画面へ行く
                case MenuState.SELECTSTAGE:

                    backgroundPosition.x = 0;
                    selectPosition.x = 0;
                    backgroundTransform.position = backgroundPosition;
                    stageselectTransform.position = selectPosition;
                    menuselectManager.SetMenuSelectFlg(true);
                    menurestartManager.SetMenuRestartFlg(false);
                    menuflg = false;
                    break;
                //リスタート確認画面へ行く
                case MenuState.RESTART:
                    backgroundPosition.x = 0;
                    restartPosition.x = 0;
                    backgroundTransform.position = backgroundPosition;
                    restartTransform.position = restartPosition;
                    menurestartManager.SetMenuRestartFlg(true);
                    menuselectManager.SetMenuSelectFlg(false);
                    menuflg = false;
                    break;
                //プレイ画面へ行く
                case MenuState.KEEP_BACK:
                    menuPosition.x = 100;
                    menutextTransform.position = menuPosition;
                    ResetMenu();
                    break;

            }
        }

    }
    public void SetMenuFlg(bool setmenuflg)
    {
        menuflg = setmenuflg;
    }

    public bool GetMenu()
    {
        if (startmenuflg == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //メニュー処理をリセットする関数
    private void ResetMenu()
    {
        ms = MenuState.NOT_SELECT;
        menuflg = false;
        startmenuflg = false;
        //ポーズを解除する
        stageMG.STAchangeMAIN();
        //pause.ResumeTagObject("Player");
        //pause.ResumeTagObject("Enemy");
        //pause.ResumeTagObject("Film");
        //pause.Resume(timestart, PauseManager.MONO);
        timestart.GetComponent<ViewTimeMainScene>().cntingflg = true;
    }
}
