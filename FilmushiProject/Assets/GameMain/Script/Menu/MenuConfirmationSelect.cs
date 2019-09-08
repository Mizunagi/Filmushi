using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuConfirmationSelect : MonoBehaviour {
    public enum MenuConfirmationSelectState
    {
        STAGESELECT_OK,
        BACK_NO,
        NOT_SELECT,

    }

    private Transform backgroundTransform;              //バックグラウンド座標呼び出し
    private Transform stageselectTransform;             //ステージセレクト確認画面の座標の呼び出し
    private Vector3 backgroundPosition;                 //バックグラウンド座標保有
    private Vector3 selectPosition;                     //ステージセレクト確認画面の座標保有
    private bool menuselectflg;　    //ステージセレクト確認メニューへのタップを許可するフラグ
    private bool rmenuselectflg;    ////メニュー画面へのタップを許可するフラグ
    private FadeImage fd_out; //フェードアウト


    MenuConfirmationSelectState mcss; //ステージクラスの制御変数
    // Use this for initialization
    void Start () {
        mcss = MenuConfirmationSelectState.NOT_SELECT;
        fd_out = GameObject.Find("Panel").GetComponent<FadeImage>();
        backgroundTransform = GameObject.Find("black_background_Confirmation").transform;
        stageselectTransform = GameObject.Find("Copy of text_window").transform;
        backgroundPosition = backgroundTransform.position;
        selectPosition = stageselectTransform.position;
        menuselectflg = false;
        rmenuselectflg = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (menuselectflg == true&&fd_out.GetEndFlag() == true)
        {
            SceneManager.LoadScene("StageSelectScene");
            ResetePosition();
        }
    }
    //次の画面処理に行く
    public void SetNextScene(MenuConfirmationSelectState setnum)
    {
        if (menuselectflg == true)
        {
            mcss = setnum;
            switch (mcss)
            {
                //ステージセレクト画面へ行く
                case MenuConfirmationSelectState.STAGESELECT_OK:
                    fd_out.FadeStart();
                    break;
                //メニュー画面へ戻る
                case MenuConfirmationSelectState.BACK_NO:
                    rmenuselectflg = true;
                    ResetePosition();
                    break;

            }
        }
    }

    private void ResetePosition()
    {
        menuselectflg = false;
        backgroundPosition.x = 100;
        selectPosition.x = 100;
        backgroundTransform.position = backgroundPosition;
        stageselectTransform.position = selectPosition;
    }

    public void SetMenuSelectFlg(bool setmenuflg)
    {
        menuselectflg= setmenuflg;
    }

    public bool GetMenu()
    {
        if (rmenuselectflg == true)
        {
            rmenuselectflg = false;
            return true;
        }
        else
        {
            return false;
        }
    }
}
