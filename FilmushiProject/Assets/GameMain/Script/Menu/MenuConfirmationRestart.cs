using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuConfirmationRestart : MonoBehaviour
{

    public enum MenuConfirmationRestartState
    {
        RESTART_OK,
        BACK_NO,
        NOT_SELECT,

    }
    private string returnscene;
    private Transform backgroundTransform; //バックグラウンド座標呼び出し
    private Transform restartTransform; //リスタート確認画面の座標の呼び出し
    private Vector3 backgroundPosition; //バックグラウンド座標保有
    private Vector3 restartPosition;    //リスタート確認画面の座標保有
    private bool menurestartflg; //リスタート確認メニューへのタップを許可するフラグ
    private bool rmenurestartflg;　//メニュー画面へのタップを許可するフラグ
    MenuConfirmationRestartState mcrs; //リスタートクラスの制御変数
    private FadeImage fd_out; //フェードアウト
    // Use this for initialization
    void Start()
    {
        mcrs = MenuConfirmationRestartState.NOT_SELECT;
        fd_out = GameObject.Find("Panel").GetComponent<FadeImage>();
        backgroundTransform = GameObject.Find("black_background_Confirmation").transform;
        restartTransform = GameObject.Find("Copy of text_window_stage_select").transform;
        backgroundPosition = backgroundTransform.position;
        restartPosition = restartTransform.position;
        menurestartflg = false;
        rmenurestartflg = false;
        returnscene = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        if (menurestartflg == true && fd_out.GetEndFlag() == true)
        {
            SceneManager.LoadScene(returnscene);
            ResetPosition();
        }
    }
    //次の画面処理に行く
    public void SetNextScene(MenuConfirmationRestartState setnum)
    {
        if (menurestartflg == true)
        {
            mcrs = setnum;
            switch (mcrs)
            {
                //もう一度同じステージシーンへ行く
                case MenuConfirmationRestartState.RESTART_OK:
                        fd_out.FadeStart();
                    break;
                //メニュー画面へ行く
                case MenuConfirmationRestartState.BACK_NO:
                    rmenurestartflg = true;
                    ResetPosition();
                    break;

            }
        }
    }

    private void ResetPosition()
    {
        menurestartflg = false;
        backgroundPosition.x = 100;
        restartPosition.x = 100;
        backgroundTransform.position = backgroundPosition;
        restartTransform.position = restartPosition;
    }

    public void SetMenuRestartFlg(bool setmenuflg)
    {
        menurestartflg = setmenuflg;
    }

    public bool GetMenu()
    {
        if (rmenurestartflg == true)
        {
            rmenurestartflg = false;
            return true;
        }
        else
        {
            return false;
        }
    }
    public MenuConfirmationRestartState GetM() { return mcrs; }

}