using UnityEngine;

public class ViewAcorn_StageSelect : MonoBehaviour
{
    /// <summary>
    ///空どんぐりのスプライト
    /// </summary>
    public Sprite SpriteEmptyAcorn;

    /// <summary>
    ///どんぐりのプレハブ
    /// </summary>
    public Sprite SpriteAcorn;

    /// <summary>
    //表示させる空どんぐりの数
    /// </summary>
    private int m_cntMaxAcorn = 0;

    /// <summary>
    ///表示させるどんぐりの数
    /// </summary>
    private int m_cntViewAcorn = 0;

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

        for (int i = 0; i < (int)AcornsNum.ACORNS_MAX; i++)
        {
            this.emptyAcorns[i].GetComponent<SpriteRenderer>().sprite = null;
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
        for (int i = 0; i < m_cntViewAcorn; i++)
        {
            //スプライト差し替え
            emptyAcorns[i].GetComponent<SpriteRenderer>().sprite = this.SpriteAcorn;
        }
    }
}