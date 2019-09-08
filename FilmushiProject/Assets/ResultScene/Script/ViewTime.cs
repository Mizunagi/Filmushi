using UnityEngine;

public class ViewTime : MonoBehaviour
{
    /// <summary>
    ///タイムスプライト
    /// </summary>
    public Sprite[] timeSprite;

    /// <summary>
    ///子要素
    /// </summary>
    private GameObject[] m_TimeChild;

    /// <summary>
    ///子要素の名前
    /// </summary>
    private string[] m_ChildString = { "One", "Ten", "Hundret", "Thousand" };

    /// <summary>
    ///表示させる時間
    /// </summary>
    private float m_ViewTime = 0.0f;

    /// <summary>
    /// ランダム表示を行う時間
    /// </summary>
    public float RandomViewTime = 0.0f;

    /// <summary>
    ///ランダム開始時間
    /// </summary>
    private float m_RandomBeginTime = 0.0f;

    /// <summary>
    ///経過時間
    /// </summary>
    private float m_RandomNowTime = 0.0f;

    /// <summary>
    /// ランダム処理フラグ
    /// </summary>
    private bool mb_RandomViewing = false;

    ///<summary>
    ///スキップフラグ
    ///</summary>
    public bool mb_Skip = false;

    ///<summary>
    ///</summary>
    private bool mb_EndFlg = false;

    private SourceAudio sourceAudio;

    /// <summary>
    ///スプライト列挙体
    /// </summary>
    private enum SpriteNum
    {
        SPRITE_TIME_ZERO,
        SPRITE_TIME_ONE,
        SPRITE_TIME_TWO,
        SPRITE_TIME_THREE,
        SPRITE_TIME_FOUR,
        SPRITE_TIME_FIVE,
        SPRITE_TIME_SIX,
        SPRITE_TIME_SEVEN,
        SPRITE_TIME_EIGHT,
        SPRITE_TIME_NINE,
        SPRITE_TIME_MAX
    }

    //デバッグ用手入力時間(1.0f以上でテスト表示モード)
    public float m_DebugTime;

    public bool mb_Debug;

    /// <summary>
    ///表示用時間
    /// </summary>
    private int[] m_ViewTimeArray;

    private enum TimePlaceNum
    {
        TIMEPLACE_ONE,
        TIMEPLACE_TEN,
        TIMEPLACE_HUNDRET,
        TIMEPLASE_THOUSAND,
        TIMEPLACE_MAX
    }

    // Use this for initialization
    private void Start()
    {
        //子要素入手
        this.m_TimeChild = new GameObject[(int)TimePlaceNum.TIMEPLACE_MAX];
        for (int i = 0; i < (int)TimePlaceNum.TIMEPLACE_MAX; i++)
        {
            this.m_TimeChild[i] = transform.Find(this.m_ChildString[i]).gameObject;
        }

        //表示用配列初期化
        this.m_ViewTimeArray = new int[(int)TimePlaceNum.TIMEPLACE_MAX];
        for (int i = 0; i < (int)TimePlaceNum.TIMEPLACE_MAX; i++)
            this.m_ViewTimeArray[i] = 0;

        this.sourceAudio = this.GetComponent<SourceAudio>();

        //デバッグ用
        if (this.mb_Debug)
        {
            if (this.m_DebugTime < 10000.0f)
            {
                this.View(m_DebugTime);
            }
            else if (this.m_DebugTime >= 10000.0f)
            {
                this.View(999.0f);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (this.mb_Skip)
        {
            this.mb_RandomViewing = false;
            this.ExtractTime(m_ViewTime, m_ViewTimeArray);
            this.ChangeSprite(this.m_ViewTimeArray);
            this.mb_Skip = false;
            this.mb_EndFlg = true;
        }
        if (this.mb_RandomViewing)
        {
            if (this.m_RandomNowTime - this.m_RandomBeginTime < this.RandomViewTime)
            {
                float rand = (float)Random.Range(1000, 10000);
                this.ExtractTime(rand, this.m_ViewTimeArray);
                this.ChangeSprite(m_ViewTimeArray);
                this.m_RandomNowTime = Time.time;
            }
            else if (this.m_RandomNowTime - this.m_RandomBeginTime >= this.RandomViewTime)
            {
                this.ExtractTime(m_ViewTime, m_ViewTimeArray);
                this.ChangeSprite(this.m_ViewTimeArray);
                this.mb_RandomViewing = false;
                this.mb_EndFlg = true;
            }
        }
    }

    private void ExtractTime(float time, int[] dest)
    {
        if (time >= 10000.0f)
        {
            time = 9999.0f;
        }
        //小数切り捨て
        float floored = Mathf.Floor(time);
        //数値取り出し
        float div = 1;
        for (int i = 0; i < (int)TimePlaceNum.TIMEPLACE_MAX; i++)
        {
            dest[i] = (int)(floored / div) % 10;
            div = div * 10;
        }
    }

    private void ChangeSprite(int[] view)
    {
        //スプライト交換

        if (view[(int)TimePlaceNum.TIMEPLASE_THOUSAND] != 0)
            this.m_TimeChild[(int)TimePlaceNum.TIMEPLASE_THOUSAND].
                    GetComponent<SpriteRenderer>().sprite
                     = this.timeSprite[view[(int)TimePlaceNum.TIMEPLASE_THOUSAND]];
        else
            this.m_TimeChild[(int)TimePlaceNum.TIMEPLASE_THOUSAND].
                    GetComponent<SpriteRenderer>().sprite
                    = null;

        if (view[(int)TimePlaceNum.TIMEPLACE_HUNDRET] != 0 || view[(int)TimePlaceNum.TIMEPLASE_THOUSAND] != 0)
            this.m_TimeChild[(int)TimePlaceNum.TIMEPLACE_HUNDRET].
                GetComponent<SpriteRenderer>().sprite
                = this.timeSprite[view[(int)TimePlaceNum.TIMEPLACE_HUNDRET]];
        else
            //0なら映さない
            this.m_TimeChild[(int)TimePlaceNum.TIMEPLACE_HUNDRET].
                GetComponent<SpriteRenderer>().sprite
                = null;

        if (view[(int)TimePlaceNum.TIMEPLACE_TEN] != 0 || view[(int)TimePlaceNum.TIMEPLACE_HUNDRET] != 0 || view[(int)TimePlaceNum.TIMEPLASE_THOUSAND] != 0)
            //0でも100の位に0以外の数字があれば表示する
            this.m_TimeChild[(int)TimePlaceNum.TIMEPLACE_TEN].
                GetComponent<SpriteRenderer>().sprite
                = this.timeSprite[view[(int)TimePlaceNum.TIMEPLACE_TEN]];
        else
            this.m_TimeChild[(int)TimePlaceNum.TIMEPLACE_TEN].
                GetComponent<SpriteRenderer>().sprite
                = null;

        this.m_TimeChild[(int)TimePlaceNum.TIMEPLACE_ONE].
            GetComponent<SpriteRenderer>().sprite
            = this.timeSprite[view[(int)TimePlaceNum.TIMEPLACE_ONE]];
    }

    public void View(float time)
    {
        m_ViewTime = time;
        m_RandomNowTime = m_RandomBeginTime = Time.time;
        mb_RandomViewing = true;
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