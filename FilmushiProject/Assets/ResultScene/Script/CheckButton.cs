using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckButton : MonoBehaviour
{
    static private GameObject parent;
    static private GameObject resButton;

    //遷移先
    static private string transition;

    //ボタン有効化
    private bool buttonEnable = true;

    //遷移フラグ
    private bool transitFlg = false;

    private FadeImage fade;

    private SourceAudio sourceAudio;
    private CustomAudioClip[] audioClip;

    private void Start()
    {
        parent = this.transform.root.gameObject;
        fade = GameObject.Find("Canvas").transform.Find("Panel").GetComponent<FadeImage>();

        this.audioClip = new CustomAudioClip[2];
        this.audioClip[0].Clip = Resources.Load("Audio/SE/Button_Yes", typeof(AudioClip)) as AudioClip;
        this.audioClip[0].Vol = 1.0f;
        this.audioClip[1].Clip = Resources.Load("Audio/SE/Button_No", typeof(AudioClip)) as AudioClip;
        this.audioClip[1].Vol = 1.0f;

        this.sourceAudio = this.gameObject.AddComponent<SourceAudio>();
        this.sourceAudio.m_Audio = this.audioClip;
    }

    private void Update()
    {
        if (transitFlg)
        {
            if (fade.GetEndFlag())
            {
                SceneManager.LoadScene(transition);
            }
        }
    }

    private void OnMouseUpAsButton()
    {
        if (this.buttonEnable)
        {
            if (this.name == "Yes")
            {
                this.buttonEnable = false;
                this.sourceAudio.PlaySE(0);
                this.transitFlg = true;
                fade.FadeStart();
            }
            else if (this.name == "No")
            {
                this.buttonEnable = false;
                this.sourceAudio.PlaySE(1);
                resButton.GetComponent<ResultButton>().Enable();
                Destroy(parent);
            }
        }
    }

    static public void SetResultButtonObj(GameObject obj)
    {
        resButton = obj;
    }

    static public void SetStansitionScene(string transit)
    {
        transition = transit;
    }
}