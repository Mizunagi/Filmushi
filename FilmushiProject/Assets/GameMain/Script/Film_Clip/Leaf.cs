using UnityEngine;

public class Leaf : MonoBehaviour
{
    private Transform tf;
    private GameObject film;
    private bool onPlayerFlag;   //プレイヤーが乗っているフラグ
    private float countOnPlayerTime;    //プレイヤーと接してる時間
    private float onPlayerFallTime;     //何秒プレイヤが乗っていたら落ちるか
    private bool fallFlag;              //落下中フラグ
    private BoxCollider2D boxCollider2D;
    private FilmManager filmManager;
    public bool FallOFFFlg;


    // Use this for initialization
    private void Start()
    {
        tf = transform;
        film = tf.parent.gameObject;
        onPlayerFlag = false;
        countOnPlayerTime = 0;
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;
        fallFlag = false;
    }

    // Update is called once per frame
    private void Update()
    {
        //プレイヤーが上に乗っていたらカウント
        if (onPlayerFlag)
        {
            countOnPlayerTime += Time.deltaTime;
            if (countOnPlayerTime >= onPlayerFallTime)
            {
                if(!FallOFFFlg) DownParentFilm();
            }
        }
        else if (countOnPlayerTime > 0 && !onPlayerFlag)   //カウントが進んでいてプレイヤーが乗っていなかったらカウントリセット
        {
            countOnPlayerTime = 0;
        }
    }

    //プレイヤーが何秒乗ったら落ちるかの時間をセット
    public void SetOnPlayerFallTime(float time)
    {
        onPlayerFallTime = time;
    }

    //フィルムの傾きを確定した時他オブジェクトとの当たり判定を発生させる
    public void FixedFilm()
    {
        boxCollider2D.enabled = true;
    }

    //フィルムを1段下げる
    private void DownParentFilm()
    {
        print("down");
        film.GetComponent<InsertFilm>().DownFilm();
    }   

    //フィルム落下時処理
    public void FallFilm()
    {
        fallFlag = true;
        boxCollider2D.isTrigger = true;
    }

    //落下中フラグを返す
    public bool GetFallFlag()
    {
        return fallFlag;
    }
    //プレイヤーが乗ってるかどうかのフラグ
    public void SetOnPlayerFlag(bool flag)
    {
        onPlayerFlag = flag;
    }
}