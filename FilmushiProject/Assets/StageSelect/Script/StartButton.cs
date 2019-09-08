using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    private enum AudioList
    {
        AUDIO_BUTTON,
        AUDIO_MAX
    }

    private string nextScene = "";
    private FadeImage fd_out;

    private SourceAudio sourceAudio;
    private CustomAudioClip[] audioClip;

    // Use this for initialization
    private void Start()
    {
        fd_out = GameObject.Find("Panel").GetComponent<FadeImage>();

        this.audioClip = new CustomAudioClip[(int)AudioList.AUDIO_MAX];
        this.audioClip[0].Clip = Resources.Load("Audio/SE/Button_Yes", typeof(AudioClip)) as AudioClip;
        this.audioClip[0].Vol = 1.0f;

        this.sourceAudio = this.gameObject.AddComponent<SourceAudio>();
        this.sourceAudio.m_Audio = audioClip;
    }

    // Update is called once per frame
    private void Update()
    {
        if (fd_out.GetEndFlag())
        {
            SceneManager.LoadScene(nextScene);
        }
    }

    public void SetNextScene(StageSelectManager.SelectStage ss)
    {
        switch (ss)
        {
            case StageSelectManager.SelectStage.TUTORIAL1:
                nextScene = "StageTutorial1Scene";
                break;

            case StageSelectManager.SelectStage.TUTORIAL2:
                nextScene = "StageTutorial2Scene";
                break;

            case StageSelectManager.SelectStage.TUTORIAL3:
                nextScene = "StageTutorial3Scene";
                break;

            case StageSelectManager.SelectStage.STAGE1:
                nextScene = "Stage1Scene";
                break;

            case StageSelectManager.SelectStage.STAGE2:
                nextScene = "Stage2Scene";
                break;

            case StageSelectManager.SelectStage.STAGE3:
                nextScene = "Stage3Scene";
                break;

            case StageSelectManager.SelectStage.STAGE4:
                nextScene = "Stage4Scene";
                break;

            case StageSelectManager.SelectStage.STAGE5:
                nextScene = "Stage5Scene";
                break;

            case StageSelectManager.SelectStage.STAGE6:
                nextScene = "Stage6Scene";
                break;

            case StageSelectManager.SelectStage.STAGE7:
                nextScene = "Stage7Scene";
                break;

            case StageSelectManager.SelectStage.STAGE8:
                nextScene = "Stage8Scene";
                break;

            case StageSelectManager.SelectStage.STAGE9:
                nextScene = "Stage9Scene";
                break;

            case StageSelectManager.SelectStage.STAGE10:
                nextScene = "Stage10Scene";
                break;

            case StageSelectManager.SelectStage.STAGE11:
                nextScene = "Stage11Scene";
                break;

            case StageSelectManager.SelectStage.STAGE12:
                nextScene = "Stage12Scene";
                break;

            case StageSelectManager.SelectStage.EXTRA1:
                nextScene = "StageEX1Scene";
                break;

            case StageSelectManager.SelectStage.EXTRA2:
                nextScene = "StageEX2Scene";
                break;

            case StageSelectManager.SelectStage.EXTRA3:
                nextScene = "StageEX3Scene";
                break;

            default:
                break;
        }
    }

    private void OnMouseUpAsButton()
    {
        sourceAudio.PlaySE((int)AudioList.AUDIO_BUTTON);
        fd_out.FadeStart();
    }
}