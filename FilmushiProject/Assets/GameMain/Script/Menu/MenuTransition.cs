using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTransition : MonoBehaviour
{
    private Transform menuTransform;                //メニュー画面の座標の呼び出しし
    private Vector3 menuPosition;                   //メニュー画面の座標保有
    private MenuManager menumanager;                //メニュークラス呼び出し
    private PauseManager pause;                     //ポーズクラスを呼び出し
    private float buttonnowtime;
    private GameObject buttoncolor;
    private GameObject timestop;
    private StartFont startfont;
    private FadeImage fd_out; //フェードアウト
    StageManager stageMG;

    //Sound
    private enum AudioList
    {
        AUDIO_BUTTON,
        AUDIO_MAX
    }

    private SourceAudio sourceAudio;
    private CustomAudioClip[] audioClip;

    // Use this for initialization
    private void Start()
    {
        print("start");
        buttonnowtime = 0.0f;
        startfont = GameObject.Find("StartFont").GetComponent<StartFont>();
        menumanager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        menuTransform = GameObject.Find("menuscreen").transform;
        pause = GameObject.Find("PauseManager").GetComponent<PauseManager>();
        fd_out = GameObject.Find("Panel").GetComponent<FadeImage>();
        timestop = GameObject.Find("StageManager").transform.Find("ViewTimeMain").gameObject;
        stageMG = GameObject.Find("StageManager").GetComponent<StageManager>();
        //値をunityの実変数からもらう
        menuPosition = menuTransform.position;
        //色を変更するゲームオブジェクトを入手
        buttoncolor = GameObject.Find("menu_button_game");
        //今の色コンソールに出力
        Debug.Log(buttoncolor.GetComponent<Renderer>().material.color);

        this.audioClip = new CustomAudioClip[(int)AudioList.AUDIO_MAX];
        this.audioClip[(int)AudioList.AUDIO_BUTTON].Clip = Resources.Load("Audio/SE/Button_Yes", typeof(AudioClip)) as AudioClip;
        this.audioClip[(int)AudioList.AUDIO_BUTTON].Vol = 1.0f;

        this.sourceAudio = this.gameObject.AddComponent<SourceAudio>();
        this.sourceAudio.m_Audio = this.audioClip;
    }

    // Update is called once per frame
    private void Update()
    {
        buttonnowtime += Time.deltaTime;
        //時間経過後元の色に戻る
        if (buttonnowtime > 0.3f)
        {
            //色を変更
            buttoncolor.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 1f);
            //変更後の色コンソールに出力
            //Debug.Log(buttoncolor.GetComponent<Renderer>().material.color);
            buttonnowtime = 0.0f;
        }
    }

    //メニュー用の画像呼び出し関数
    private void MenuCall()
    {
        //値を入れる
        menuPosition.x = 0;

        //値をunityの実変数に返す
        menuTransform.position = menuPosition;
        timestop.GetComponent<ViewTimeMainScene>().cntingflg = false;
        //ポーズを起動する
        stageMG.STAchangeMENUPAUSE();
        //pause.PauseTagObject("Player");
        //pause.PauseTagObject("Enemy");
        //pause.PauseTagObject("Film");
        //pause.Pause(timestop, PauseManager.MONO);
        //メニュー画面のボタンの入力を許可する
        menumanager.SetMenuFlg(true);
    }

    //ボタンが押されたらメニュー用の画像を呼び出す
    private void OnMouseUpAsButton()
    {
        //色を変更
        buttoncolor.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        //変更後の色コンソールに出力
        Debug.Log(buttoncolor.GetComponent<Renderer>().material.color);
        //PlaySound
        this.sourceAudio.PlaySE((int)AudioList.AUDIO_BUTTON);

        //メニューに遷移したときにプレイ画面のメニューボタンを押せないようにする
        if (fd_out.GetStartFlag()==false&&startfont.Startflg() == true && menumanager.GetMenu() == false)
        {
            MenuCall();
        }
    }
}