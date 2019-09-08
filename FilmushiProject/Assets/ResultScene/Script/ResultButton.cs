using UnityEngine;

public class ResultButton : MonoBehaviour
{
    private static bool enable = true;

    public GameObject instance;
    public Sprite message;

    //遷移先シーン
    public string TransitionScene;

    //遷移先シーンをマネージャから取得
    public bool GetSceneFromManager;

    private SourceAudio sourceAudio;
    private CustomAudioClip[] audioClip;

    private void Start()
    {
        enable = true;

        this.audioClip = new CustomAudioClip[1];
        this.audioClip[0].Clip = Resources.Load("Audio/SE/Button_Yes", typeof(AudioClip)) as AudioClip;
        this.audioClip[0].Vol = 1.0f;

        this.sourceAudio = this.gameObject.AddComponent<SourceAudio>();
        this.sourceAudio.m_Audio = this.audioClip;
    }

    public void Enable()
    {
        enable = true;
    }

    public void Disable()
    {
        enable = false;
    }

    private void OnMouseUpAsButton()
    {
        if (enable)
        {
            Debug.Log("touched:" + this.name);
            this.Disable();
            this.sourceAudio.PlaySE(0);

            GameObject obj = Instantiate(this.instance);
            obj.transform.Find("Message").GetComponent<SpriteRenderer>().sprite = this.message;

            CheckButton.SetResultButtonObj(this.gameObject);

            if (this.GetSceneFromManager)
            {
                //遷移先のステージの名前を取得する
                if (this.name == "Retry")
                    this.TransitionScene = GameObject.Find("ResultManager").GetComponent<ResultManger>().GetTransitScene(ResultManger.TransitMode.RETRY);
                else if (this.name == "Next")
                    this.TransitionScene = GameObject.Find("ResultManager").GetComponent<ResultManger>().GetTransitScene(ResultManger.TransitMode.NEXT);

                CheckButton.SetStansitionScene(this.TransitionScene);
            }
            else
            {
                CheckButton.SetStansitionScene(this.TransitionScene);
            }
        }
        else
        {
            Debug.LogWarning("Not enable touch");
        }
    }
}