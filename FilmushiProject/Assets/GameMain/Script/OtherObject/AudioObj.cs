using UnityEngine;

public class AudioObj : MonoBehaviour
{
    private SourceAudio sourceAudio;
    private CustomAudioClip[] audioClip;

    private enum AudioList
    {
        AUDIO_GAME,
        AUDIO_GOAL,
        AUDIO_MAX
    }

    // Use this for initialization
    private void Start()
    {
        this.audioClip = new CustomAudioClip[(int)AudioList.AUDIO_MAX];
        this.audioClip[(int)AudioList.AUDIO_GAME].Clip = Resources.Load("Audio/BGM/BGM_Gamemain", typeof(AudioClip)) as AudioClip;
        this.audioClip[(int)AudioList.AUDIO_GAME].Vol = 1.0f;

        this.audioClip[(int)AudioList.AUDIO_GOAL].Clip = Resources.Load("Audio/BGM/BGM_Goal", typeof(AudioClip)) as AudioClip;
        this.audioClip[(int)AudioList.AUDIO_GOAL].Vol = 0.6f;

        sourceAudio = this.gameObject.AddComponent<SourceAudio>();
        sourceAudio.m_Audio = this.audioClip;

        sourceAudio.PlayBGM((int)AudioList.AUDIO_GAME, true);
    }

    public void PlayGameBGM()
    {
        sourceAudio.StopBGM();
        sourceAudio.PlayBGM((int)AudioList.AUDIO_GAME, true);
    }

    public void PlayGoalBGM()
    {
        sourceAudio.StopBGM();
        sourceAudio.PlayBGM((int)AudioList.AUDIO_GOAL, false);
    }
}