using UnityEngine;

public class ViewAcorn : MonoBehaviour
{
    /// <summary>
    ///空どんぐりのスプライト
    /// </summary>
    public Sprite SpriteEmptyAcorn;

    /// <summary>
    ///どんぐりのプレハブ
    /// </summary>
    public GameObject PrefavAcorn;

    public GameObject ViewMaxParticle;

    /// <summary>
    ///次のどんぐりを表示させる時間
    /// </summary>
    public float ViewingDeltaTime = 0.0f;

    /// <summary>
    ///前のどんぐり表示からの経過時間
    /// </summary>
    private float m_PassageTime = 0.0f;

    /// <summary>
    ///どんぐり表示フラグ
    /// </summary>
    private bool mb_ViewingAcorn = false;

    /// <summary>
    ///スキップフラグ
    /// </summary>
    public bool mb_Skip = false;

    /// <summary>
    ///終了フラグ
    /// </summary>
    private bool mb_EndFlg = false;

    /// <summary>
    //表示させる空どんぐりの数
    /// </summary>
    private int m_cntMaxAcorn = 0;

    /// <summary>
    ///表示させるどんぐりの数
    /// </summary>
    private int m_cntViewAcorn = 0;

    /// <summary>
    ///現在表示しているどんぐりの数
    /// </summary>
    private int m_cntViewingAcorn = 0;

    private SourceAudio sourceAudio;
    private CustomAudioClip[] audioClip;

    //デバッグ用
    public int DebugMaxAcorn = 0;

    public int DebugViewAcorn = 0;
    public bool DebugFlg = false;

    /// <summary>
    //子要素のからどんぐりのインスタンス
    private GameObject[] emptyAcorns;

    private enum AcornsNum
    {
        ACORNS_ZERO,
        ACORNS_ONE,
        ACORNS_TWO,
        ACORNS_THREE,
        ACORNS_FOUR,
        ACORNS_FIVE,
        ACORNS_SIX,
        ACORNS_SEVEN,
        ACORNS_EIGHT,
        ACORNS_NINE,
        ACORNS_MAX,
    }

    // Use this for initialization
    private void Start()
    {
        //子のインスタンスを入手
        this.emptyAcorns = new GameObject[(int)AcornsNum.ACORNS_MAX];
        for (int i = 0; i < (int)AcornsNum.ACORNS_MAX; i++)
        {
            this.emptyAcorns[i] = transform.Find("EnptyAcorn_" + i).gameObject;
        }

        this.audioClip = new CustomAudioClip[2];
        this.audioClip[0].Clip = Resources.Load("Audio/SE/Acorn", typeof(AudioClip)) as AudioClip;
        this.audioClip[0].Vol = 1.0f;
        this.audioClip[1].Clip = Resources.Load("Audio/SE/tirin1", typeof(AudioClip)) as AudioClip;
        this.audioClip[1].Vol = 1.0f;

        this.sourceAudio = this.gameObject.AddComponent<SourceAudio>();
        this.sourceAudio.m_Audio = this.audioClip;

        //デバッグ挙動
        if (this.DebugFlg)
        {
            this.ViewEmptyAcorns(this.DebugMaxAcorn);
            this.View(this.DebugViewAcorn);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //スキップ処理
        if (this.mb_Skip)
        {
            //表示処理を無効化
            this.mb_ViewingAcorn = false;
            for (int i = m_cntViewingAcorn; i < m_cntViewAcorn; i++)
            {
                //親の座標を取得
                Vector3 instancePos = this.emptyAcorns[i].transform.position;
                //空どんぐりより前に出す
                instancePos.z -= 0.1f;
                //生成&結合
                Instantiate(this.PrefavAcorn, instancePos, Quaternion.identity).transform.parent =
                    this.emptyAcorns[i].transform;
                emptyAcorns[i].GetComponent<SpriteRenderer>().enabled = false;

                //Particle
                if (i == this.m_cntMaxAcorn - 1)
                {
                    instancePos.z -= 0.1f;
                    Instantiate(this.ViewMaxParticle, instancePos, Quaternion.identity).transform.parent
                        = this.emptyAcorns[i].transform;
                }
            }

            //Sound
            if (m_cntViewAcorn < m_cntMaxAcorn)
            {
                this.sourceAudio.PlaySE(0);
            }
            else if (m_cntViewAcorn == this.m_cntMaxAcorn)
            {
                this.sourceAudio.PlaySE(1);
            }
            this.mb_Skip = false;
            this.mb_EndFlg = true;
        }
        //どんぐり表示フラグ？
        if (this.mb_ViewingAcorn)
        {
            //現在経過時間が表示間隔を超えたか？
            //現在表示どんぐり数が表示どんぐり数を超えないか？
            if (this.m_PassageTime >= this.ViewingDeltaTime &&
               this.m_cntViewingAcorn < this.m_cntViewAcorn)
            {
                //親の座標を取得
                Vector3 instancePos = this.emptyAcorns[this.m_cntViewingAcorn].transform.position;
                //空どんぐりより前に出す
                instancePos.z -= 0.1f;

                //生成&結合
                Instantiate(this.PrefavAcorn, instancePos, Quaternion.identity).transform.parent
                    = this.emptyAcorns[this.m_cntViewingAcorn].transform;
                emptyAcorns[this.m_cntViewingAcorn].GetComponent<SpriteRenderer>().enabled = false;

                //Particle
                if (this.m_cntViewingAcorn == this.m_cntMaxAcorn - 1)
                {
                    instancePos.z -= 0.1f;
                    Instantiate(this.ViewMaxParticle, instancePos, Quaternion.identity).transform.parent
                        = this.emptyAcorns[this.m_cntViewingAcorn].transform;
                }

                //Sound
                if (this.m_cntViewingAcorn < this.m_cntMaxAcorn - 1)
                {
                    this.sourceAudio.PlaySE(0);
                }
                else if (this.m_cntViewingAcorn == this.m_cntMaxAcorn - 1)
                {
                    //ステージのどんぐりをすべて取得したら最後の1個の音は違う
                    this.sourceAudio.PlaySE(1);
                }

                this.m_PassageTime = 0.0f;
                this.m_cntViewingAcorn++;

                //経過していない場合
            }
            else if (this.m_PassageTime < this.ViewingDeltaTime)
            {
                //現在経過時間を更新
                this.m_PassageTime += Time.deltaTime;
                //現在表示どんぐりが表示どんぐり数を超えた場合
            }
            else if (this.m_cntViewingAcorn >= this.m_cntViewAcorn)
            {
                //終了&リセット
                this.mb_EndFlg = true;
                this.mb_ViewingAcorn = false;
                this.m_cntViewingAcorn = 0;
                this.m_PassageTime = 0.0f;
            }
        }
    }

    //空どんぐりを表示させる
    public void ViewEmptyAcorns(int cnt)
    {
        //cntがどんぐり最大数を越えるとエラーさせる
        if (cnt > (int)AcornsNum.ACORNS_MAX)
        {
            Debug.LogError("Input over max acorns(10)");
            return;
        }

        this.m_cntMaxAcorn = cnt;
        //空どんぐり表示
        for (int i = 0; i < m_cntMaxAcorn; i++)
        {
            this.emptyAcorns[i].GetComponent<SpriteRenderer>().sprite = this.SpriteEmptyAcorn;
        }
    }

    public void View(int cnt)
    {
        //表示どんぐりが空どんぐりの数を越えるとエラーさせる
        if (cnt > this.m_cntMaxAcorn)
        {
            Debug.LogError("View acorns bigger than max acorns: Max acorns>" + this.m_cntMaxAcorn);
            return;
        }

        this.m_cntViewAcorn = cnt;
        this.mb_ViewingAcorn = true;
    }

    public bool GetEndFlg()
    {
        return mb_EndFlg;
    }

    public void Skip()
    {
        this.mb_Skip = true;
    }
}