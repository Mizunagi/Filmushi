using UnityEngine;

[System.Flags]
public enum LockState
{
    STATE_UNLOCKED=1,
    STATE_CLEARED=1<<1,
}

[System.Serializable]
public struct SaveData
{
    public string stageName;
    public float cleartime;
    public int maxAcorns;
    public int cntAcorns;
    public LockState lockState;
};

public enum Stages
{
    STAGE_T1,
    STAGE_T2,
    STAGE_T3,
    STAGE_1,
    STAGE_2,
    STAGE_3,
    STAGE_4,
    STAGE_5,
    STAGE_6,
    STAGE_7,
    STAGE_8,
    STAGE_9,
    STAGE_10,
    STAGE_11,
    STAGE_12,
    STAGE_E1,
    STAGE_E2,
    STAGE_E3,
    STAGE_MAX
}

public class SaveDataManager : Singleton<SaveDataManager>
{
    //ファイル場所
    private string savedataPath;

    private SaveData[] InitSavedata;

    //セーブデータ一覧
    private SaveData[] savedata;

    public bool RemakeSaveData = true;

    //一時保存空間
    public SaveData temp { set; get; }

    private void Awake()
    {
        this.savedataPath = Application.persistentDataPath + "/FilmushiSave.xml";
    }

    private void CreateSaveData()
    {
        this.InitSavedata = new SaveData[19];
        InitSavedata[(int)Stages.STAGE_T1].stageName = "StageTutorial1Scene";
        InitSavedata[(int)Stages.STAGE_T1].cleartime = 200.0f;
        InitSavedata[(int)Stages.STAGE_T1].maxAcorns = 0;
        InitSavedata[(int)Stages.STAGE_T1].cntAcorns = 0;
        InitSavedata[(int)Stages.STAGE_T1].lockState = LockState.STATE_UNLOCKED;

        InitSavedata[(int)Stages.STAGE_T2].stageName = "StageTutorial2Scene";
        InitSavedata[(int)Stages.STAGE_T2].cleartime = 200.0f;
        InitSavedata[(int)Stages.STAGE_T2].maxAcorns = 0;
        InitSavedata[(int)Stages.STAGE_T2].cntAcorns = 0;
        InitSavedata[(int)Stages.STAGE_T2].lockState = LockState.STATE_UNLOCKED;

        InitSavedata[(int)Stages.STAGE_T3].stageName = "StageTutorial3Scene";
        InitSavedata[(int)Stages.STAGE_T3].cleartime = 200.0f;
        InitSavedata[(int)Stages.STAGE_T3].maxAcorns = 0;
        InitSavedata[(int)Stages.STAGE_T3].cntAcorns = 0;
        InitSavedata[(int)Stages.STAGE_T3].lockState = LockState.STATE_UNLOCKED;

        InitSavedata[(int)Stages.STAGE_1].stageName = "Stage1Scene";
        InitSavedata[(int)Stages.STAGE_1].cleartime = 300.0f;
        InitSavedata[(int)Stages.STAGE_1].maxAcorns = 3;
        InitSavedata[(int)Stages.STAGE_1].cntAcorns = 0;
        InitSavedata[(int)Stages.STAGE_1].lockState = LockState.STATE_UNLOCKED;

        InitSavedata[(int)Stages.STAGE_2].stageName = "Stage2Scene";
        InitSavedata[(int)Stages.STAGE_2].cleartime = 300.0f;
        InitSavedata[(int)Stages.STAGE_2].maxAcorns = 4;
        InitSavedata[(int)Stages.STAGE_2].cntAcorns = 0;
        InitSavedata[(int)Stages.STAGE_2].lockState = LockState.STATE_UNLOCKED;

        InitSavedata[(int)Stages.STAGE_3].stageName = "Stage3Scene";
        InitSavedata[(int)Stages.STAGE_3].cleartime = 300.0f;
        InitSavedata[(int)Stages.STAGE_3].maxAcorns = 2;
        InitSavedata[(int)Stages.STAGE_3].cntAcorns = 0;
        InitSavedata[(int)Stages.STAGE_3].lockState = LockState.STATE_UNLOCKED;

        InitSavedata[(int)Stages.STAGE_4].stageName = "Stage4Scene";
        InitSavedata[(int)Stages.STAGE_4].cleartime = 400.0f;
        InitSavedata[(int)Stages.STAGE_4].maxAcorns = 2;
        InitSavedata[(int)Stages.STAGE_4].cntAcorns = 0;
        InitSavedata[(int)Stages.STAGE_4].lockState = LockState.STATE_UNLOCKED;

        InitSavedata[(int)Stages.STAGE_5].stageName = "Stage5Scene";
        InitSavedata[(int)Stages.STAGE_5].cleartime = 400.0f;
        InitSavedata[(int)Stages.STAGE_5].maxAcorns = 3;
        InitSavedata[(int)Stages.STAGE_5].cntAcorns = 0;
        InitSavedata[(int)Stages.STAGE_5].lockState = LockState.STATE_UNLOCKED;

        InitSavedata[(int)Stages.STAGE_6].stageName = "Stage6Scene";
        InitSavedata[(int)Stages.STAGE_6].cleartime = 400.0f;
        InitSavedata[(int)Stages.STAGE_6].maxAcorns = 0;
        InitSavedata[(int)Stages.STAGE_6].cntAcorns = 0;
        InitSavedata[(int)Stages.STAGE_6].lockState = LockState.STATE_UNLOCKED;

        InitSavedata[(int)Stages.STAGE_7].stageName = "Stage7Scene";
        InitSavedata[(int)Stages.STAGE_7].cleartime = 500.0f;
        InitSavedata[(int)Stages.STAGE_7].maxAcorns = 3;
        InitSavedata[(int)Stages.STAGE_7].cntAcorns = 0;
        InitSavedata[(int)Stages.STAGE_7].lockState = LockState.STATE_UNLOCKED;

        InitSavedata[(int)Stages.STAGE_8].stageName = "Stage8Scene";
        InitSavedata[(int)Stages.STAGE_8].cleartime = 500.0f;
        InitSavedata[(int)Stages.STAGE_8].maxAcorns = 5;
        InitSavedata[(int)Stages.STAGE_8].cntAcorns = 0;
        InitSavedata[(int)Stages.STAGE_8].lockState = LockState.STATE_UNLOCKED;

        InitSavedata[(int)Stages.STAGE_9].stageName = "Stage9Scene";
        InitSavedata[(int)Stages.STAGE_9].cleartime = 500.0f;
        InitSavedata[(int)Stages.STAGE_9].maxAcorns = 2;
        InitSavedata[(int)Stages.STAGE_9].cntAcorns = 0;
        InitSavedata[(int)Stages.STAGE_9].lockState = LockState.STATE_UNLOCKED;

        InitSavedata[(int)Stages.STAGE_10].stageName = "Stage10Scene";
        InitSavedata[(int)Stages.STAGE_10].cleartime = 600.0f;
        InitSavedata[(int)Stages.STAGE_10].maxAcorns = 0;
        InitSavedata[(int)Stages.STAGE_10].cntAcorns = 0;
        InitSavedata[(int)Stages.STAGE_10].lockState = LockState.STATE_UNLOCKED;

        InitSavedata[(int)Stages.STAGE_11].stageName = "Stage11Scene";
        InitSavedata[(int)Stages.STAGE_11].cleartime = 600.0f;
        InitSavedata[(int)Stages.STAGE_11].maxAcorns = 0;
        InitSavedata[(int)Stages.STAGE_11].cntAcorns = 0;
        InitSavedata[(int)Stages.STAGE_11].lockState = LockState.STATE_UNLOCKED;

        InitSavedata[(int)Stages.STAGE_12].stageName = "Stage12Scene";
        InitSavedata[(int)Stages.STAGE_12].cleartime = 600.0f;
        InitSavedata[(int)Stages.STAGE_12].maxAcorns = 10;
        InitSavedata[(int)Stages.STAGE_12].cntAcorns = 0;
        InitSavedata[(int)Stages.STAGE_12].lockState = LockState.STATE_UNLOCKED;

        InitSavedata[(int)Stages.STAGE_E1].stageName = "StageEX1Scene";
        InitSavedata[(int)Stages.STAGE_E1].cleartime = 700.0f;
        InitSavedata[(int)Stages.STAGE_E1].maxAcorns = 10;
        InitSavedata[(int)Stages.STAGE_E1].cntAcorns = 0;
        InitSavedata[(int)Stages.STAGE_E1].lockState = LockState.STATE_UNLOCKED;

        InitSavedata[(int)Stages.STAGE_E2].stageName = "StageEX2Scene";
        InitSavedata[(int)Stages.STAGE_E2].cleartime = 700.0f;
        InitSavedata[(int)Stages.STAGE_E2].maxAcorns = 3;
        InitSavedata[(int)Stages.STAGE_E2].cntAcorns = 0;
        InitSavedata[(int)Stages.STAGE_E2].lockState = LockState.STATE_UNLOCKED;

        InitSavedata[(int)Stages.STAGE_E3].stageName = "StageEX3Scene";
        InitSavedata[(int)Stages.STAGE_E3].cleartime = 700.0f;
        InitSavedata[(int)Stages.STAGE_E3].maxAcorns = 10;
        InitSavedata[(int)Stages.STAGE_E3].cntAcorns = 0;
        InitSavedata[(int)Stages.STAGE_E3].lockState = LockState.STATE_UNLOCKED;

        XmlUtility.Seialize<SaveData[]>(this.savedataPath, this.InitSavedata);
        Debug.Log("Create new savedata:" + this.savedataPath);
    }

    public bool LoadSaveData()
    {
        //ファイル存在チェック
        if (System.IO.File.Exists(savedataPath))
        {
            //読み込み
            if (this.RemakeSaveData)
            {
                CreateSaveData();
            }
            savedata = XmlUtility.Deserialize<SaveData[]>(savedataPath);
            Debug.Log("Load savedata Path:" + this.savedataPath);
        }
        else
        {
            Debug.LogWarning("Not exist savedata");
            this.CreateSaveData();
            this.LoadSaveData();

            return false;
        }
        return true;
    }

    public bool WriteSaveData()
    {
        //iCloudに保存させない
        //UnityEngine.iOS.Device.SetNoBackupFlag(savedataPath);
        //書き出し
        XmlUtility.Seialize<SaveData[]>(savedataPath, savedata);
        Debug.Log("Write savedata Path:" + this.savedataPath);

        return true;
    }

    public SaveData GetSaveData(string stageName)
    {
        for (int i = 0; i < savedata.Length; i++)
        {
            if (stageName == savedata[i].stageName)
            {
                return savedata[i];
            }
        }
        SaveData none = new SaveData();
        return none;
    }

    public SaveData GetSaveData(int stageIndex)
    {
        for (int i = 0; i < savedata.Length; i++)
        {
            if (stageIndex == i)
            {
                return savedata[i];
            }
        }
        SaveData none = new SaveData();
        return none;
    }

    public void SetSaveData(SaveData data)
    {
        for (int i = 0; i < savedata.Length; i++)
        {
            if (data.stageName == savedata[i].stageName)
            {
                savedata[i] = data;
            }
        }
    }

    public SaveData[] GetAllSaveData()
    {
        SaveData[] dest = new SaveData[(int)Stages.STAGE_MAX];
        for (int i = 0; i < (int)Stages.STAGE_MAX; i++)
        {
            dest[i] = savedata[i];
        }
        return dest;
    }

    public SaveData GetNextSaveData(string beforeStageName)
    {
        SaveData dest = new SaveData();
        for (int i = 0; i < (int)Stages.STAGE_MAX-1; i++)
        {
            if (this.savedata[i].stageName == beforeStageName)
            {
                dest = this.savedata[i + 1];
                return dest;
            }
        }
        return dest;
    }

    public SaveData GetNextSaveData(int stageIndex)
    {
        SaveData dest = new SaveData();
        for (int i = 0; i < (int)Stages.STAGE_MAX - 1; i++)
        {
            if (i == stageIndex)
            {
                dest = this.savedata[i + 1];
                return dest;
            }
        }
        return dest;
    }

}