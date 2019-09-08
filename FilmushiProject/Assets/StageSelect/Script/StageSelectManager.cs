using UnityEngine;
using System;

public class StageSelectManager : MonoBehaviour
{
    public enum SelectStage
    {
        NOT_SELECT,
        TUTORIAL1,
        TUTORIAL2,
        TUTORIAL3,
        STAGE1,
        STAGE2,
        STAGE3,
        STAGE4,
        STAGE5,
        STAGE6,
        STAGE7,
        STAGE8,
        STAGE9,
        STAGE10,
        STAGE11,
        STAGE12,
        EXTRA1,
        EXTRA2,
        EXTRA3,
        STAGE_MAX
    }

    public float swayDegreeMax; //ステージイメージの揺れる角度最大値
    public float swayDegreeMin; //ステージイメージの揺れる角度最小値
    public float swayTime1;
    public float swayTime2;
    public float swayTime3;
    private SelectStage selectStage;

    //各ステージScript 後々配列化するかも
    private StageImage[] stageScript = new StageImage[(int)Stages.STAGE_MAX];

    private StageSelectPlayer stageSelectPlayer;
    private Vector3 playerDefaultPosition;
    private StartButton startButton;

    private Transform mainCamraTransform;
    private Vector3 stageSelectCameraPosition;
    private Vector3 stageDetailCameraPosition;

    private StageDetailManager stageDetail;

    private int nowPage;
    public int maxPage;
    private RightArrow rightArrow;  //右向き矢印
    private LeftArrow leftArrow;    //左向き矢印

    private Vector2 startPos;
    private bool tapFlag;
    public float swipeJudgeDistance;
    private GameObject pageSet;

    private enum AudioList
    {
        AUDIO_BGM,
        AUDIO_MAX
    }

    private SourceAudio sourceAudio;
    private CustomAudioClip[] audioClip;

    // Use this for initialization
    private void Start()
    {
        SaveDataManager.Instance.LoadSaveData();

        selectStage = SelectStage.NOT_SELECT;

        stageScript[(int)Stages.STAGE_T1] = GameObject.Find("stage_tutorial1_image").GetComponent<StageImage>();
        stageScript[(int)Stages.STAGE_T2] = GameObject.Find("stage_tutorial2_image").GetComponent<StageImage>();
        stageScript[(int)Stages.STAGE_T3] = GameObject.Find("stage_tutorial3_image").GetComponent<StageImage>();
        stageScript[(int)Stages.STAGE_1] = GameObject.Find("stage_1_image").GetComponent<StageImage>();
        stageScript[(int)Stages.STAGE_2] = GameObject.Find("stage_2_image").GetComponent<StageImage>();
        stageScript[(int)Stages.STAGE_3] = GameObject.Find("stage_3_image").GetComponent<StageImage>();
        stageScript[(int)Stages.STAGE_4] = GameObject.Find("stage_4_image").GetComponent<StageImage>();
        stageScript[(int)Stages.STAGE_5] = GameObject.Find("stage_5_image").GetComponent<StageImage>();
        stageScript[(int)Stages.STAGE_6] = GameObject.Find("stage_6_image").GetComponent<StageImage>();
        stageScript[(int)Stages.STAGE_7] = GameObject.Find("stage_7_image").GetComponent<StageImage>();
        stageScript[(int)Stages.STAGE_8] = GameObject.Find("stage_8_image").GetComponent<StageImage>();
        stageScript[(int)Stages.STAGE_9] = GameObject.Find("stage_9_image").GetComponent<StageImage>();
        stageScript[(int)Stages.STAGE_10] = GameObject.Find("stage_10_image").GetComponent<StageImage>();
        stageScript[(int)Stages.STAGE_11] = GameObject.Find("stage_11_image").GetComponent<StageImage>();
        stageScript[(int)Stages.STAGE_12] = GameObject.Find("stage_12_image").GetComponent<StageImage>();
        stageScript[(int)Stages.STAGE_E1] = GameObject.Find("stage_extra1_image").GetComponent<StageImage>();
        stageScript[(int)Stages.STAGE_E2] = GameObject.Find("stage_extra2_image").GetComponent<StageImage>();
        stageScript[(int)Stages.STAGE_E3] = GameObject.Find("stage_extra3_image").GetComponent<StageImage>();

        stageSelectPlayer = GameObject.Find("StageSelectPlayer").GetComponent<StageSelectPlayer>();
        playerDefaultPosition = stageSelectPlayer.gameObject.GetComponent<Transform>().position;
        startButton = GameObject.Find("StartButton").GetComponent<StartButton>();

        mainCamraTransform = GameObject.Find("Main Camera").GetComponent<Transform>();
        stageSelectCameraPosition = GameObject.Find("Page1").GetComponent<Transform>().position;
        stageSelectCameraPosition.z = -10;
        stageDetailCameraPosition = GameObject.Find("StageDetailBackGround").GetComponent<Transform>().position;
        stageDetailCameraPosition.z = -10;

        this.stageDetail = GameObject.Find("ViewStageDetail").GetComponent<StageDetailManager>();

        nowPage = 1;
        rightArrow = GameObject.Find("RightArrow").GetComponent<RightArrow>();
        leftArrow = GameObject.Find("LeftArrow").GetComponent<LeftArrow>();
        tapFlag = false;
        pageSet = GameObject.Find("PageSet");
        //MovePage(); //他のシーンから戻った際に初期ページ位置変更

        this.audioClip = new CustomAudioClip[(int)AudioList.AUDIO_MAX];
        this.audioClip[0].Clip = Resources.Load("Audio/BGM/BGM_StageSelect", typeof(AudioClip)) as AudioClip;
        this.audioClip[0].Vol = 1.0f;

        this.sourceAudio = this.gameObject.AddComponent<SourceAudio>();
        this.sourceAudio.m_Audio = this.audioClip;

        this.sourceAudio.PlayBGM((int)AudioList.AUDIO_BGM, true);

        CheckUnLock();
    }

    // Update is called once per frame
    private void Update()
    {
        //ステージシーンへの遷移はStartButtonから

        //スワイプ検知してページ遷移
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tapFlag = true;
        }
        if (tapFlag)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (startPos.x > mousePos.x + swipeJudgeDistance)
            {
                MoveRightPage();
                tapFlag = false;
            }
            else if (startPos.x + swipeJudgeDistance < mousePos.x)
            {
                MoveLeftPage();
                tapFlag = false;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                tapFlag = false;
            }
        }
    }

    //ステージ選択時のジャンプの終了時処理
    public void SelectJumpFinish(SelectStage ss)
    {
        selectStage = ss;
        if (selectStage == SelectStage.NOT_SELECT)
        {
            return;
        }
        stageScript[(int)selectStage - 1].SwayLanding();    //StagesとSelectStageは1違うのでマイナスする
    }

    public void ChangeStageDetail()
    {
        this.stageDetail.ViewDetail(selectStage);
        startButton.SetNextScene(selectStage);
        mainCamraTransform.position = stageDetailCameraPosition;
    }

    public void BackStageSelect()
    {
        mainCamraTransform.position = stageSelectCameraPosition;
        selectStage = SelectStage.NOT_SELECT;
        stageSelectPlayer.transform.parent = null;
        stageSelectPlayer.Jump(playerDefaultPosition, selectStage);
    }

    public void MoveRightPage()
    {
        //既に一番右のページなら無視
        if (GetMaxPageFlag())
        {
            return;
        }
        //ふぃるむしをジャンプさせるジャンプの引数はジャンプの向き（ｘ）
        if (stageSelectPlayer.JumpPage(1))
        {
            //無事ジャンプしてくれたらページ遷移開始
            pageSet.GetComponent<TranslationPage>().MoveRight();

            nowPage += 1;
        }
    }

    public void MoveLeftPage()
    {
        //既に一番左のページなら無視
        if (GetMinPageFlag())
        {
            return;
        }
        //ふぃるむしをジャンプさせるジャンプの引数はジャンプの向き（ｘ）
        if (stageSelectPlayer.JumpPage(-1))
        {
            //無事ジャンプしてくれたらページ遷移開始
            pageSet.GetComponent<TranslationPage>().MoveLeft();

            nowPage -= 1;
        }
    }

    public bool GetMinPageFlag()
    {
        if (nowPage <= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetMaxPageFlag()
    {
        if (nowPage >= maxPage)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void MovePage()
    {
        switch (SaveDataManager.Instance.temp.stageName)
        {
            case "Stage1Scene":
            case "Stage2Scene":
            case "Stage3Scene":
            case "Stage4Scene":
                nowPage = 2;
                pageSet.GetComponent<TranslationPage>().SetStartPage(nowPage);
                break;

            case "Stage5Scene":
            case "Stage6Scene":
            case "Stage7Scene":
            case "Stage8Scene":
                nowPage = 3;
                pageSet.GetComponent<TranslationPage>().SetStartPage(nowPage);
                break;

            case "Stage9Scene":
            case "Stage10Scene":
            case "Stage11Scene":
            case "Stage12Scene":
                nowPage = 4;
                pageSet.GetComponent<TranslationPage>().SetStartPage(nowPage);
                break;
            case "StageEX1Scene":
            case "StageEX2Scene":
            case "StageEX3Scene":
                nowPage = 5;
                pageSet.GetComponent<TranslationPage>().SetStartPage(nowPage);
                break;
            default:
                nowPage = 1;
                pageSet.GetComponent<TranslationPage>().SetStartPage(nowPage);
                break;
        }
    }

    //アンロックされるステージがあるかどうかチェックする
    private bool CheckUnLock()
    {
        SaveData[] allSaveData;
        allSaveData=SaveDataManager.Instance.GetAllSaveData();
        int maxUnlocked = 0;
        for(int i=0;i<allSaveData.Length;i++)
        {
            if ((allSaveData[i].lockState & LockState.STATE_UNLOCKED) == LockState.STATE_UNLOCKED)
            {
                print(i);
                stageScript[i].DeleteLockSprite();  //既にアンロック演出済みならロックの紙を消す
                maxUnlocked = i;
            }
        }

        SaveDataManager.Instance.temp = allSaveData[maxUnlocked];
        MovePage();

        bool unlockFlag = false;    //ロック解除演出が入るフラグ
        for(int i = 0; i < allSaveData.Length - 1; i++)
        {
            //現在参照しているステージがクリア済みかつ次のステージがアンロックされていない時
            if((allSaveData[i].lockState&LockState.STATE_CLEARED)==LockState.STATE_CLEARED
                && (allSaveData[i + 1].lockState & LockState.STATE_UNLOCKED)!=LockState.STATE_UNLOCKED)
            {
                allSaveData[i + 1].lockState |= LockState.STATE_UNLOCKED;
                
                switch (i)
                {
                    case (int)Stages.STAGE_T3:
                        nowPage = 2;
                        pageSet.GetComponent<TranslationPage>().SetStartPage(nowPage);
                        break;
                    case (int)Stages.STAGE_4:
                        nowPage = 3;
                        pageSet.GetComponent<TranslationPage>().SetStartPage(nowPage);
                        break;
                    case (int)Stages.STAGE_8:
                        nowPage = 4;
                        pageSet.GetComponent<TranslationPage>().SetStartPage(nowPage);
                        break;
                    case (int)Stages.STAGE_12:
                        nowPage = 5;
                        pageSet.GetComponent<TranslationPage>().SetStartPage(nowPage);
                        break;
                    default:
                        break;
                }

                stageScript[i + 1].FallLockSprite();    //ロックの紙を落とす演出
                unlockFlag = true;

                SaveDataManager.Instance.SetSaveData(allSaveData[i + 1]);
            }
        }

        SaveDataManager.Instance.WriteSaveData();
        return unlockFlag;
    }
}