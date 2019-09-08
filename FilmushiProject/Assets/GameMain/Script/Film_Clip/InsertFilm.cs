using UnityEngine;

public class InsertFilm : MonoBehaviour
{
    //挟んだ後のフィルム用処理群

    private Transform tf;
    private bool fixedFlag;         //傾き固定完了フラグ
    private float leanSpeed;        //傾くスピード
    private Vector3 clipPosition;   //傾くときの原点（浅いか深いかで少し変化）
    private FilmManager filmManager;
    private Leaf[] leaves;      //付属している足場
    private FilmManager.FilmDepthStatus filmDepthStatus;    //フィルムの深さステータス
    private float deepToShallowMoveTime;    //深い状態から浅い状態への移行にかかる時間
    private float deepToShallowMoveSpeed;   //↑移行速度
    private float shallowPositionY;         //浅いフィルム位置（傾きによって違う）
    private bool fallFlag;          //落下フラグ
    private float fallSpeed;        //落下速度
    private PauseManager pauseManager;

    private SourceAudio sourceAudio;
    private CustomAudioClip[] audioClip;

    private enum AudioList
    {
        AUDIO_DOWN,
        AUDIO_FALL,
        AUDIO_MAX
    }

    // Use this for initialization
    private void Start()
    {
        tf = transform;
        fixedFlag = false;
        fallFlag = false;
        fallSpeed = 0.0f;
        filmManager = GameObject.Find("FilmManager").GetComponent<FilmManager>();
        pauseManager = GameObject.Find("PauseManager").GetComponent<PauseManager>();
        leanSpeed = filmManager.leanSpeed;
        deepToShallowMoveTime = filmManager.deepToShallowMovetime;
        deepToShallowMoveSpeed = (filmManager.shallowFilmPosition.y - filmManager.deepFilmPosition.y) / deepToShallowMoveTime;
        clipPosition = tf.parent.position;

        //フィルムの深さ判定
        if (filmManager.deepFilmPosition.y - 1 <= tf.position.y)
        {
            filmDepthStatus = FilmManager.FilmDepthStatus.DEEP;
        }
        else
        {
            filmDepthStatus = FilmManager.FilmDepthStatus.SHALLOW;
            clipPosition.y -= filmManager.deepFilmPosition.y - tf.position.y;   //浅く挿んだ時は深い位置との差分原点を下げる
        }
        //子である足場を取得
        leaves = GetComponentsInChildren<Leaf>();
        foreach (var leaf in leaves)
        {
            leaf.SetOnPlayerFallTime(filmManager.onPlayerFallTime); //付属する足場にプレイヤーが何秒乗ったら落ちるかをセット
        }

        //Sound
        this.audioClip = new CustomAudioClip[(int)AudioList.AUDIO_MAX];
        this.audioClip[(int)AudioList.AUDIO_DOWN].Clip = Resources.Load("Audio/SE/Film_Down", typeof(AudioClip)) as AudioClip;
        this.audioClip[(int)AudioList.AUDIO_DOWN].Vol = 1.0f;
        this.audioClip[(int)AudioList.AUDIO_FALL].Clip = Resources.Load("Audio/SE/Film_Fall", typeof(AudioClip)) as AudioClip;
        this.audioClip[(int)AudioList.AUDIO_FALL].Vol = 1.0f;

        sourceAudio = this.gameObject.AddComponent<SourceAudio>();
        sourceAudio.m_Audio = this.audioClip;
    }

    // Update is called once per frame
    private void Update()
    {
        //固定されていなければ傾け処理
        if (!fixedFlag)
        {
            LeanFilm();
        }
        //深い状態から浅い状態への移行中
        if (filmDepthStatus == FilmManager.FilmDepthStatus.CENTER)
        {
            Vector3 nowPos = tf.position;
            nowPos.y += deepToShallowMoveSpeed * Time.deltaTime;
            if (nowPos.y < shallowPositionY)
            {
                //浅い状態への移行完了
                nowPos.y = shallowPositionY;
                filmDepthStatus = FilmManager.FilmDepthStatus.SHALLOW;
            }
            tf.position = nowPos;
        }
        //落下フラグが立っていれば落下処理（物理挙動使わない場合)
        if (fallFlag)
        {
            Vector3 nowPos = tf.position;
            fallSpeed += Physics2D.gravity.y * Time.deltaTime / 50;
            nowPos.y += fallSpeed;
            tf.position = nowPos;
        }
        //画面外へ出たら削除
        if (!GetComponent<Renderer>().isVisible)
        {
            pauseManager.RemovePauseObject(this.gameObject, "Film");
            Destroy(this.gameObject);
        }
    }

    //固定完了フラグを返す
    public bool GetFixedFlag()
    {
        return fixedFlag;
    }

    //傾ける
    private void LeanFilm()
    {
        //フィルム座標がクリップの真下(-90度)になければ傾ける
        float deg = Mathf.Atan2(tf.position.y - clipPosition.y, tf.position.x - clipPosition.x) * Mathf.Rad2Deg;
        //角度が一定以内なら確定させる
        if (deg < -89 && deg > -91)
        {
            FixedFilm();
            return;
        }
        else//傾ける
        {
            tf.RotateAround(clipPosition, Vector3.forward, (-90 - deg) * Time.deltaTime * leanSpeed);
        }
    }

    //フィルム固定
    private void FixedFilm()
    {
        fixedFlag = true;

        //付属する足場を全て取得
        foreach (var leaf in leaves)
        {
            leaf.FixedFilm();   //足場の方にも固定時の処理を走らせる
        }
        //深く挟んだ場合に浅い状態のy座標を設定
        if (filmDepthStatus == FilmManager.FilmDepthStatus.DEEP)
        {
            shallowPositionY = tf.position.y - (filmManager.deepFilmPosition.y - filmManager.shallowFilmPosition.y);
        }

        filmManager.ResumeOtherObject();    //ポーズ解除
    }

    //フィルム1段ダウン
    public void DownFilm()
    {
        switch (filmDepthStatus)
        {
            case FilmManager.FilmDepthStatus.DEEP:
                filmDepthStatus = FilmManager.FilmDepthStatus.CENTER;
                this.sourceAudio.PlaySE((int)AudioList.AUDIO_DOWN);
                break;

            case FilmManager.FilmDepthStatus.SHALLOW:
                filmDepthStatus = FilmManager.FilmDepthStatus.FALL;
                this.sourceAudio.PlaySE((int)AudioList.AUDIO_FALL);
                FallFilm();
                break;

            default:
                break;
        }
    }

    //フィルム落下
    public void FallFilm()
    {
        fallFlag = true;
        //gameObject.AddComponent<Rigidbody2D>();   //物理挙動の影響によって落とす
        //足場にも落下したことを伝える
        foreach (var leaf in leaves)
        {
            leaf.FallFilm();
        }
        //クリップにも落下したことを伝える
        tf.parent.GetComponent<Clip>().FallFilm(this.gameObject);
    }
}