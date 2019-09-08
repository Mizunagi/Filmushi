using UnityEngine;

public class SelectBackButton : MonoBehaviour
{
    private enum AudioList
    {
        AUDIO_BUTTON,
        AUDIO_MAX
    }

    private StageSelectManager stageSelectManager;

    private SourceAudio sourceAudio;
    private CustomAudioClip[] audioClip;

    // Use this for initialization
    private void Start()
    {
        stageSelectManager = GameObject.Find("StageSelectManager").GetComponent<StageSelectManager>();

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
        stageSelectManager.BackStageSelect();
    }
}