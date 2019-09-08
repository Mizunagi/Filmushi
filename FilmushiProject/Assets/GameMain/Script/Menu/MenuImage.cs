using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuImage : MonoBehaviour
{
    private MenuManager menuManager;
    public MenuManager.MenuState thismenustate;
    private float buttonnowtime;
    private bool changecolorflg;
    private GameObject mumabuttoncolor;
    private GameObject mumabuttoncolor2;
    private GameObject mumabuttoncolor3;

    private enum AudioList
    {
        AUDIO_YES,
        AUDIO_NO,
        AUDIO_MAX
    }

    private CustomAudioClip[] audioclip;
    private SourceAudio sourceAudio;

    // Use this for initialization
    private void Start()
    {
        buttonnowtime = 0.0f;
        changecolorflg = false;
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        //色を変更するゲームオブジェクトを入手
        mumabuttoncolor = GameObject.Find("text_stage_select");
        mumabuttoncolor2 = GameObject.Find("text_restart");
        mumabuttoncolor3 = GameObject.Find("text_back");
        //今の色コンソールに出力
        Debug.Log(mumabuttoncolor.GetComponent<Renderer>().material.color);
        Debug.Log(mumabuttoncolor2.GetComponent<Renderer>().material.color);
        Debug.Log(mumabuttoncolor3.GetComponent<Renderer>().material.color);

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
                case MenuManager.MenuState.SELECTSTAGE:
                    //色を変更する
                    mumabuttoncolor.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 1f);
                    Debug.Log(mumabuttoncolor.GetComponent<Renderer>().material.color);
                    break;

                case MenuManager.MenuState.RESTART:
                    //色を変更する
                    mumabuttoncolor2.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 1f);
                    Debug.Log(mumabuttoncolor2.GetComponent<Renderer>().material.color);
                    break;

                case MenuManager.MenuState.KEEP_BACK:
                    //色を変更する
                    mumabuttoncolor3.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 1f);
                    Debug.Log(mumabuttoncolor3.GetComponent<Renderer>().material.color);
                    break;
            }
        }
    }

    private void OnMouseUpAsButton()
    {
        changecolorflg = true;
        switch (thismenustate)
        {
            case MenuManager.MenuState.SELECTSTAGE:
                //色を変更する
                mumabuttoncolor.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                //変更後を出力
                Debug.Log(mumabuttoncolor.GetComponent<Renderer>().material.color);
                //Sound
                this.sourceAudio.PlaySE((int)AudioList.AUDIO_YES);
                break;

            case MenuManager.MenuState.RESTART:
                //色を変更する
                mumabuttoncolor2.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                //変更後を出力
                Debug.Log(mumabuttoncolor2.GetComponent<Renderer>().material.color);
                //Sound
                this.sourceAudio.PlaySE((int)AudioList.AUDIO_YES);
                break;

            case MenuManager.MenuState.KEEP_BACK:
                //色を変更する
                mumabuttoncolor3.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                Debug.Log(mumabuttoncolor3.GetComponent<Renderer>().material.color);
                this.sourceAudio.PlaySE((int)AudioList.AUDIO_NO);
                break;
        }
        menuManager.SelectMenu(thismenustate);
    }
}