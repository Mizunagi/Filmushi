using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuConfirmationImage : MonoBehaviour
{
    private MenuConfirmationSelect mcs;//確認画面ステージセレクト処理
    public MenuConfirmationSelect.MenuConfirmationSelectState thismenustate; //確認画面ステージセレクト処理のenum取得
    private float buttonnowtime;
    private bool changecolorflg;
    private GameObject slbuttoncolor;
    private GameObject slbuttoncolor2;

    private enum AudioList
    {
        AUDIO_YES,
        AUDIO_NO,
        AUDIO_MAX
    }

    //Sound
    private CustomAudioClip[] audioClip;

    private SourceAudio sourceAudio;

    // Use this for initialization
    private void Start()
    {
        buttonnowtime = 0.0f;
        changecolorflg = false;
        mcs = GameObject.Find("MenuManager").GetComponent<MenuConfirmationSelect>();
        //色を変更するゲームオブジェクトを入手
        slbuttoncolor = GameObject.Find("Copy of button_yes_2");
        slbuttoncolor2 = GameObject.Find("Copy of button_no_2");
        //今の色コンソールに出力
        Debug.Log(slbuttoncolor.GetComponent<Renderer>().material.color);
        Debug.Log(slbuttoncolor2.GetComponent<Renderer>().material.color);

        this.audioClip = new CustomAudioClip[(int)AudioList.AUDIO_MAX];
        this.audioClip[(int)AudioList.AUDIO_YES].Clip = Resources.Load("Audio/SE/Button_Yes", typeof(AudioClip)) as AudioClip;
        this.audioClip[(int)AudioList.AUDIO_YES].Vol = 1.0f;
        this.audioClip[(int)AudioList.AUDIO_NO].Clip = Resources.Load("Audio/SE/Button_No", typeof(AudioClip)) as AudioClip;
        this.audioClip[(int)AudioList.AUDIO_NO].Vol = 1.0f;

        this.sourceAudio = this.gameObject.AddComponent<SourceAudio>();
        this.sourceAudio.m_Audio = this.audioClip;
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
                case MenuConfirmationSelect.MenuConfirmationSelectState.STAGESELECT_OK:
                    //色を変更する
                    slbuttoncolor.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 1f);
                    Debug.Log(slbuttoncolor.GetComponent<Renderer>().material.color);
                    break;

                case MenuConfirmationSelect.MenuConfirmationSelectState.BACK_NO:
                    //色を変更する
                    slbuttoncolor2.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 1f);
                    Debug.Log(slbuttoncolor2.GetComponent<Renderer>().material.color);
                    break;
            }
        }
    }

    private void OnMouseUpAsButton()
    {
        changecolorflg = true;
        switch (thismenustate)
        {
            case MenuConfirmationSelect.MenuConfirmationSelectState.STAGESELECT_OK:
                //色を変更する
                slbuttoncolor.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                Debug.Log(slbuttoncolor.GetComponent<Renderer>().material.color);
                //sound
                this.sourceAudio.PlaySE((int)AudioList.AUDIO_YES);
                break;

            case MenuConfirmationSelect.MenuConfirmationSelectState.BACK_NO:
                //色を変更する
                slbuttoncolor2.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                Debug.Log(slbuttoncolor2.GetComponent<Renderer>().material.color);
                //sound
                this.sourceAudio.PlaySE((int)AudioList.AUDIO_NO);
                break;
        }
        mcs.SetNextScene(thismenustate);
    }
}