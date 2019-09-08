using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleBackButton : MonoBehaviour
{
    private enum AudioList
    {
        AUDIO_BUTTON,
        AUDIO_MAX
    }

    private SourceAudio sourceAudio;
    private CustomAudioClip[] audioClip;

    public string prevScene = "TitleScene";

    // Use this for initialization
    private void Start()
    {
        this.audioClip = new CustomAudioClip[(int)AudioList.AUDIO_MAX];
        this.audioClip[0].Clip = Resources.Load("Audio/SE/Button_No", typeof(AudioClip)) as AudioClip;
        this.audioClip[0].Vol = 1.0f;

        this.sourceAudio = this.gameObject.AddComponent<SourceAudio>();
        this.sourceAudio.m_Audio = this.audioClip;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnMouseUpAsButton()
    {
        sourceAudio.PlaySE((int)AudioList.AUDIO_BUTTON);
        SceneManager.LoadScene(prevScene);
    }
}