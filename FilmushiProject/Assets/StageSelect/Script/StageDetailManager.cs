using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDetailManager : MonoBehaviour
{
    private string loadDetail;
    private SaveData loadSaveData;

    private GameObject stagenemeObj;
    private GameObject timeObj;
    private GameObject AcornObj;

    public int DebugStage;
    public bool debugFlg;

    // Use this for initialization
    private void Start()
    {
        //SaveDataManager.Instance.LoadSaveData();  //StageSelectManagerへ移動
        this.stagenemeObj = this.transform.Find("StageLabel").gameObject;
        this.timeObj = this.transform.Find("ViewTime").gameObject;
        this.AcornObj = this.transform.Find("ViewAcorn").gameObject;
    }

    public void ViewDetail(StageSelectManager.SelectStage ss)
    {
        if (this.debugFlg)
        {
            ss = (StageSelectManager.SelectStage)this.DebugStage;
        }

        this.loadDetail = null;

        switch (ss)
        {
            case StageSelectManager.SelectStage.TUTORIAL1:
                loadDetail = "StageTutorial1Scene";
                break;

            case StageSelectManager.SelectStage.TUTORIAL2:
                loadDetail = "StageTutorial2Scene";
                break;

            case StageSelectManager.SelectStage.TUTORIAL3:
                loadDetail = "StageTutorial3Scene";
                break;

            case StageSelectManager.SelectStage.STAGE1:
                loadDetail = "Stage1Scene";
                break;

            case StageSelectManager.SelectStage.STAGE2:
                loadDetail = "Stage2Scene";
                break;

            case StageSelectManager.SelectStage.STAGE3:
                loadDetail = "Stage3Scene";
                break;

            case StageSelectManager.SelectStage.STAGE4:
                loadDetail = "Stage4Scene";
                break;

            case StageSelectManager.SelectStage.STAGE5:
                loadDetail = "Stage5Scene";
                break;

            case StageSelectManager.SelectStage.STAGE6:
                loadDetail = "Stage6Scene";
                break;

            case StageSelectManager.SelectStage.STAGE7:
                loadDetail = "Stage7Scene";
                break;

            case StageSelectManager.SelectStage.STAGE8:
                loadDetail = "Stage8Scene";
                break;

            case StageSelectManager.SelectStage.STAGE9:
                loadDetail = "Stage9Scene";
                break;

            case StageSelectManager.SelectStage.STAGE10:
                loadDetail = "Stage10Scene";
                break;

            case StageSelectManager.SelectStage.STAGE11:
                loadDetail = "Stage11Scene";
                break;

            case StageSelectManager.SelectStage.STAGE12:
                loadDetail = "Stage12Scene";
                break;

            case StageSelectManager.SelectStage.EXTRA1:
                loadDetail = "StageEX1Scene";
                break;

            case StageSelectManager.SelectStage.EXTRA2:
                loadDetail = "StageEX2Scene";
                break;

            case StageSelectManager.SelectStage.EXTRA3:
                loadDetail = "StageEX3Scene";
                break;

            default:
                break;
        }

        this.loadSaveData.cleartime = 0.0f;
        this.loadSaveData.cntAcorns = 0;
        this.loadSaveData.maxAcorns = 0;
        this.loadSaveData.stageName = null;

        this.loadSaveData = SaveDataManager.Instance.GetSaveData(this.loadDetail);
        this.stagenemeObj.GetComponent<ViewStageName>().View(ss);
        this.timeObj.GetComponent<ViewTime_StageSelect>().View(loadSaveData.cleartime);
        this.AcornObj.GetComponent<ViewAcorn_StageSelect>().ViewEmptyAcorns(loadSaveData.maxAcorns);
        this.AcornObj.GetComponent<ViewAcorn_StageSelect>().View(loadSaveData.cntAcorns);
    }
}