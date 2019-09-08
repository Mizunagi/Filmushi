using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuConfirmationRestartImage : MonoBehaviour
{
    private MenuConfirmationRestart mcr;    //確認画面リセット処理
    public MenuConfirmationRestart.MenuConfirmationRestartState thismenustate;//確認画面リセット処理のenum取得
    private float buttonnowtime;
    private bool changecolorflg;
    private GameObject rsbuttoncolor;
    private GameObject rsbuttoncolor2;

    private enum AudioList
    {
        AUDIO_YES,
        AUDIO_NO,
        AUDIO_MAX
    }

    //Sound
    private CustomAudioClip[] audioclip;

    private SourceAudio sourceAudio;

    // Use this for initialization
    private void Start()
    {
        buttonnowtime = 0.0f;
        changecolorflg = false;
        mcr = GameObject.Find("MenuManager").GetComponent<MenuConfirmationRestart>();
        //色を変更するゲームオブジェクトを入手
        rsbuttoncolor = GameObject.Find("Copy of button_yes_1");
        rsbuttoncolor2 = GameObject.Find("Copy of button_no_1");
        //今の色コンソールに出力
        Debug.Log(rsbuttoncolor.GetComponent<Renderer>().material.color);
        Debug.Log(rsbuttoncolor2.GetComponent<Renderer>().material.color);

        this.audioclip = new CustomAudioClip[(int)AudioList.AUDIO_MAX];
        this.audioclip[(int)AudioList.AUDIO_YES].Clip = Resources.Load("Audio/SE/Button_Yes", typeof(AudioClip)) as AudioClip;
        this.audioclip[(int)AudioList.AUDIO_YES].Vol = 1.0f;
        this.audioclip[(int)AudioList.AUDIO_NO].Clip = Resources.Load("Audio/SE/Button_No", typeof(AudioClip)) as AudioClip;
        this.audioclip[(int)AudioList.AUDIO_NO].Vol = 1.0f;

        this.sourceAudio = this.gameObject.AddComponent<SourceAudio>();
        this.sourceAudio.m_Audio = this.audioclip;
    }

    // Update is called once per frame
    private void Update()
    {
        if (changecolorflg == true)
        {
            buttonnowtime += Time.deltaTime;
        }
        if (buttonnowtime > 0.3f)
        {
            buttonnowtime = 0.0f;
            changecolorflg = false;
            switch (thismenustate)
            {
                case MenuConfirmationRestart.MenuConfirmationRestartState.RESTART_OK:
                    //色を変更する
                    rsbuttoncolor.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 1f);
                    Debug.Log(rsbuttoncolor.GetComponent<Renderer>().material.color);
                    break;

                case MenuConfirmationRestart.MenuConfirmationRestartState.BACK_NO:
                    //色を変更する
                    rsbuttoncolor2.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 1f);
                    Debug.Log(rsbuttoncolor2.GetComponent<Renderer>().material.color);
                    break;
            }
        }
    }

    private void OnMouseUpAsButton()
    {
        changecolorflg = true;
        switch (thismenustate)
        {
            case MenuConfirmationRestart.MenuConfirmationRestartState.RESTART_OK:
                //色を変更する
                rsbuttoncolor.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                //変更後を出力
                Debug.Log(rsbuttoncolor.GetComponent<Renderer>().material.color);
                //Sound
                this.sourceAudio.PlaySE((int)AudioList.AUDIO_YES);
                break;

            case MenuConfirmationRestart.MenuConfirmationRestartState.BACK_NO:
                //色を変更する
                rsbuttoncolor2.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                //変更後を出力
                Debug.Log(rsbuttoncolor2.GetComponent<Renderer>().material.color);
                //Sound
                this.sourceAudio.PlaySE((int)AudioList.AUDIO_NO);
                break;
        }
        mcr.SetNextScene(thismenustate);
    }
}