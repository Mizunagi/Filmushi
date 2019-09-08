using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class TitleManager : MonoBehaviour
{
    private bool startAnimeStartFlag = true;
    private bool startAnimeFinishFlag = false;
    private bool changeSceneAnimeStartFlag = false;
    private bool changeSceneAnimeFinishFlag = false;

    public string nextScene;
    public Material filmushiMaterial; // head object of the filmushi in the tile scene
    public Texture filmushi_normal;
    public Texture filmushi_sleep;
    public Texture filmushi_blink;
    public GameObject titleLeaf;
    public ParticleSystem sleepEffect;
    public GameObject touchStart;

    private GameObject mc;
    private GameObject tl;
    private GameObject pnl;
    private TranslationObject mc_script;
    private TranslationObject tl_script;
    private FadeImage fd_out;
    private GameObject ts;

    private float blinkDelay = 0.3f;
    private float time;

    private SourceAudio sourceAudio;
    private CustomAudioClip[] audioClip;

    private enum AudioList
    {
        AUDIO_BGM,
        AUDIO_BUTTON,
        AUDIO_MAX
    };

    // Use this for initialization
    private void Start()
    {
        nextScene = "StageSelectScene";

        mc = GameObject.Find("MainCamera");
        //tl = GameObject.Find("TitleLogo");
        pnl = GameObject.Find("FadePanel");
        ts = GameObject.Find("TouchStart");
        mc_script = mc.GetComponent<TranslationObject>();
        //tl_script = tl.GetComponent<TranslationObject>();
        fd_out = pnl.GetComponent<FadeImage>();

        this.audioClip = new CustomAudioClip[(int)AudioList.AUDIO_MAX];
        this.audioClip[0].Clip = Resources.Load("Audio/BGM/BGM_Title", typeof(AudioClip)) as AudioClip;
        this.audioClip[0].Vol = 1.0f;
        this.audioClip[1].Clip = Resources.Load("Audio/SE/Button_Yes", typeof(AudioClip)) as AudioClip;
        this.audioClip[1].Vol = 1.0f;

        this.sourceAudio = this.gameObject.AddComponent<SourceAudio>();
        this.sourceAudio.m_Audio = this.audioClip;

        this.sourceAudio.PlayBGM((int)AudioList.AUDIO_BGM, true);

        filmushiMaterial.SetTexture("_MainTex", filmushi_sleep);

        touchStart.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        time += Time.deltaTime;

        //ロゴ止まったらスタート演出完了
        if (titleLeaf.GetComponent<Transform>().position.y == 10)
        {
            StartAnimeFinish();
        }
        if (Input.GetMouseButtonDown(0))
        {
            //スタート演出が完了している時
            if (startAnimeFinishFlag)
            {
                ChangeSceneAnimeStart();
            }
            //スタート演出中
            else
            {
                mc_script.Skip();
                //tl_script.Skip();

                StartAnimeFinish();
            }
        }
        //フェード完了したら遷移
        if (fd_out.GetEndFlag())
        {
            SceneManager.LoadScene(nextScene);
        }
    }

    //ロゴが止まったら呼んで
    public void StartAnimeFinish()
    {
        startAnimeFinishFlag = true;
        //ts.GetComponent<TouchScreen>().StartRender();
        titleLeaf.GetComponent<Animator>().Play("TitleLeafAnimation", 0, 9); // last frame of leaf animation
        touchStart.SetActive(true);
    }

    //スタート演出完了後にタップされたら呼んで
    public void ChangeSceneAnimeStart()
    {
        filmushiMaterial.SetTexture("_MainTex", filmushi_normal);

        sleepEffect.Stop();
        sourceAudio.PlaySE((int)AudioList.AUDIO_BUTTON);
        changeSceneAnimeStartFlag = true;
        fd_out.FadeStart();
    }

    public void Blink()
    {
        if (filmushiMaterial.GetTexture("_MainTex") == filmushi_normal)
            filmushiMaterial.SetTexture("_MainTex", filmushi_blink);
        else
            filmushiMaterial.SetTexture("_MainTex", filmushi_normal);
    }
}