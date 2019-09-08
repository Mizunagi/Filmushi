using UnityEngine;

public class ResultManger : MonoBehaviour
{
    private enum ResultState
    {
        STATE_RESULT_START,
        STATE_TIMEPROD_START,
        STATE_TIMEPROD_PROD,     //クリアタイム演出
        STATE_TIMEPROD_SKIP,
        STATE_TIMEPROD_COMPARE,
        STATE_TIMEPROD_END,
        STATE_ACORNPROD_START,
        STATE_ACORNPROD_PROD,    //どんぐり演出
        STATE_ACORNPROD_SKIP,
        STATE_ACORNPROD_COMPARE,
        STATE_ACORNPROD_END,
        STATE_OPEN_BUTTON,
        STATE_END_PROD,
        STATE_RESULT_SKIP,
        STATE_WAIT,    //入力待ち
    }

    public enum TransitMode
    {
        RETRY,
        NEXT
    }

    private ResultState state;

    private SaveData cleardata;
    private SaveData Hiscore;
    private SaveData Output;

    //時間演出関係=========================
    public GameObject TimeView;

    private ViewTime timeviewInstance;

    public GameObject TimeHiscore;
    //時間演出関係=========================

    //どんぐり演出関係=====================
    public GameObject AcornView;

    private ViewAcorn acornviewInstance;

    public GameObject AcornHiscore;
    //どんぐり演出関係=====================

    //ボタン
    public GameObject resultButtons;

    private bool prodSkip = false;

    //デバッグ用
    public SaveData debugdata;

    public SaveData debugTempdata;

    public bool debugEnable;
    public bool debugTemp = false;

    private SourceAudio sourceAudio;
    private CustomAudioClip[] audioClip;

    // Use this for initialization
    private void Start()
    {
        //デバッグモード
        if (!this.debugEnable)
        {
            if (debugTemp)
            {
                //デバッグ用一時データ
                Output = cleardata = this.debugTempdata;
                SaveDataManager.Instance.LoadSaveData();
            }
            else
            {
                Output = cleardata = SaveDataManager.Instance.temp;
            }
            this.Hiscore = SaveDataManager.Instance.GetSaveData(cleardata.stageName);
        }
        else
        {
            cleardata = debugdata;
        }
        this.timeviewInstance = Instantiate(this.TimeView).GetComponent<ViewTime>();
        this.acornviewInstance = Instantiate(this.AcornView).GetComponent<ViewAcorn>();

        this.audioClip = new CustomAudioClip[1];
        this.audioClip[0].Clip = Resources.Load("Audio/BGM/BGM_Result", typeof(AudioClip)) as AudioClip;
        this.audioClip[0].Vol = 1.0f;

        this.sourceAudio = this.gameObject.AddComponent<SourceAudio>();
        this.sourceAudio.m_Audio = this.audioClip;

        this.sourceAudio.PlayBGM(0, true);
    }

    // Update is called once per frame
    private void Update()
    {
        switch (this.state)
        {
            case ResultState.STATE_RESULT_START:
                this.acornviewInstance.ViewEmptyAcorns(this.cleardata.maxAcorns);
                this.state = ResultState.STATE_TIMEPROD_START;
                break;

            case ResultState.STATE_TIMEPROD_START:
                //時間演出開始
                this.timeviewInstance.View(this.cleardata.cleartime);
                this.state = ResultState.STATE_TIMEPROD_PROD;
                break;

            case ResultState.STATE_TIMEPROD_PROD:
                if (this.timeviewInstance.GetEndFlg())
                {
                    //演出が完了したら次の状態へ遷移
                    this.state = ResultState.STATE_TIMEPROD_COMPARE;
                }
                else if (Input.GetMouseButton(0))
                {
                    //演出中に画面を押されると演出をスキップ
                    this.state = ResultState.STATE_TIMEPROD_SKIP;
                    this.prodSkip = true;
                }
                break;

            case ResultState.STATE_TIMEPROD_SKIP:
                //スキップ
                this.timeviewInstance.Skip();
                this.state = ResultState.STATE_TIMEPROD_COMPARE;
                break;

            case ResultState.STATE_TIMEPROD_COMPARE:
                //ハイスコア表示
                if (cleardata.cleartime < Hiscore.cleartime)
                {
                    Instantiate(this.TimeHiscore);
                    //クリアハイスコアを更新
                    Output.cleartime = cleardata.cleartime;
                }
                else
                {
                    //ハイスコアのクリアタイムのままに
                    Output.cleartime = Hiscore.cleartime;
                }
                this.state = ResultState.STATE_TIMEPROD_END;
                break;

            case ResultState.STATE_TIMEPROD_END:
                this.state = ResultState.STATE_ACORNPROD_START;
                break;

            case ResultState.STATE_ACORNPROD_START:
                //どんぐり演出開始
                this.acornviewInstance.View(this.cleardata.cntAcorns);
                if (this.prodSkip)
                {
                    this.state = ResultState.STATE_ACORNPROD_SKIP;
                }
                else
                {
                    this.state = ResultState.STATE_ACORNPROD_PROD;
                }
                break;

            case ResultState.STATE_ACORNPROD_PROD:
                if (this.acornviewInstance.GetEndFlg())
                {
                    //演出が完了したら次の状態へ遷移
                    this.state = ResultState.STATE_ACORNPROD_COMPARE;
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    //演出中に画面を押されると演出をスキップ
                    this.state = ResultState.STATE_ACORNPROD_SKIP;
                }
                break;

            case ResultState.STATE_ACORNPROD_SKIP:
                //スキップ
                this.acornviewInstance.Skip();
                this.state = ResultState.STATE_ACORNPROD_COMPARE;
                break;

            case ResultState.STATE_ACORNPROD_COMPARE:
                if (cleardata.maxAcorns == Hiscore.maxAcorns &&
                    cleardata.cntAcorns >= Hiscore.cntAcorns)
                {
                    //スコア更新
                    //Instantiate(this.AcornHiscore);
                    Output.cntAcorns = cleardata.cntAcorns;
                }
                else
                {
                    //ハイスコアをそのまま保存
                    Output.cntAcorns = Hiscore.cntAcorns;
                }
                this.state = ResultState.STATE_ACORNPROD_END;
                break;

            case ResultState.STATE_ACORNPROD_END:
                Output.lockState |= LockState.STATE_CLEARED | LockState.STATE_UNLOCKED;
                this.state = ResultState.STATE_END_PROD;
                break;

            case ResultState.STATE_END_PROD:
                if (!this.debugEnable)
                {
                    SaveDataManager.Instance.SetSaveData(this.Output);
                }
                this.state = ResultState.STATE_OPEN_BUTTON;
                break;

            case ResultState.STATE_OPEN_BUTTON:
                //ボタン表示
                Instantiate(this.resultButtons);
                this.state = ResultState.STATE_WAIT;
                break;

            case ResultState.STATE_WAIT:
                if (Input.GetKeyDown(KeyCode.S))
                {
                    Output.maxAcorns = cleardata.maxAcorns;
                    SaveDataManager.Instance.SetSaveData(this.Output);
                }
                break;
        }
    }

    private void OnDestroy()
    {
        //シーン消滅時にっセーブデータを保存
        SaveDataManager.Instance.WriteSaveData();
    }

    public string GetTransitScene(TransitMode mode)
    {
        if (mode == TransitMode.RETRY)
        {
            return cleardata.stageName;
        }
        else if (mode == TransitMode.NEXT)
        {
            return SaveDataManager.Instance.GetNextSaveData(cleardata.stageName).stageName;
        }
        return null;
    }
}