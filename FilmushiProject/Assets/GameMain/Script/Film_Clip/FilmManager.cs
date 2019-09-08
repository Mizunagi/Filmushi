using System.Collections.Generic;
using UnityEngine;

public class FilmManager : MonoBehaviour
{
    //フィルムの共通ステータスもここに
    public enum FilmDepthStatus
    {
        NOT_INSERT,
        DEEP,
        CENTER,
        SHALLOW,
        FALL,
    }

    public Vector3 largeFilmScale;      //拡大した時のフィルムのスケール
    public Vector3 smallFilmScale;      //縮小した時のフィルムのスケール
    public Vector3 deepFilmPosition;    //深く挟んだ時のフィルムの座標
    public Vector3 shallowFilmPosition; //浅く挟んだ時のフィルムの座標
    public Vector2 filmSize;            //フィルムのサイズ
    public Vector2 leafSize;            //フィルムの足場のサイズ
    public float deepToShallowMovetime; //深いから浅いへの移行にかかる時間

    public float onPlayerFallTime;      //プレイヤーが乗ってて落ちるまでの時間

    private Vector3 mousePos;
    private Vector3 mousePosOld;
    private bool onStageFlag;
    private Collider2D stageCollider;
    private GameObject selectFilm;
    private Transform selectFilmTransform;
    private Clip clip;
    public float leanSpeed;             //フィルムの傾く速度
    private PauseManager pauseManager;

    public float swipeJudgeDistance=2;
    private List<Vector2> inputPosBuffer = new List<Vector2>(); //以前のフレームでのタッチ座標

    // Use this for initialization
    private void Start()
    {
        stageCollider = GameObject.Find("StageBackGround").GetComponent<BoxCollider2D>();
        clip = GameObject.Find("clip_L").GetComponent<Clip>();
        selectFilm = null;
        selectFilmTransform = null;
        mousePos = new Vector3(0, 0, 0);
        pauseManager = GameObject.Find("PauseManager").GetComponent<PauseManager>();
        //ポーズマネージャに挿入中のフィルムのリストを作る
        pauseManager.CreatePauseObjectList("Film");
    }

    // Update is called once per frame
    private void Update()
    {
        //マウス座標取得
        //mousePosOld = mousePos;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //現在のタッチ座標をバッファへ格納
        inputPosBuffer.Add(mousePos);
        Vector2 judgePos = new Vector2();
        //バッファが大きくなりすぎないように古すぎるものは削除
        if (inputPosBuffer.Count > 10)
        {
            inputPosBuffer.RemoveAt(0);
        }
        judgePos = inputPosBuffer[0];
        //選択中フィルムがある時マウス座標によってフィルムを移動する
        if (selectFilm != null)
        {
            //マウス座標がステージ上にあるかどうか判定する
            if (stageCollider.OverlapPoint(mousePos))
            {
                //前回のフレームがステージ外だった場合切り替え
                if (!onStageFlag)
                {
                    onStageFlag = true;
                    IncreaseSelectFilmSize();   //フィルム拡大
                    selectFilmTransform.position = deepFilmPosition;    //深く挿む
                }
                else if (mousePos.y > judgePos.y+swipeJudgeDistance)//前回のフレームでもステージ上で前回フレームよりマウスが上に移動した場合
                {
                    selectFilmTransform.position = deepFilmPosition;    //深く挿む
                }
                else if (mousePos.y < judgePos.y-swipeJudgeDistance)//前回のフレームでもステージ上で前回フレームよりマウスが下に移動した場合
                {
                    selectFilmTransform.position = shallowFilmPosition; //浅く挿む
                }
                //離れたら挿む
                if (Input.GetMouseButtonUp(0))
                {
                    InsertFilm();
                }
            }
            else
            {
                mousePos.z = selectFilmTransform.position.z;    //z座標はフィルムに合わせる
                selectFilmTransform.position = mousePos;
                //前回のフレームがステージ上だった場合切り替え
                if (onStageFlag)
                {
                    onStageFlag = false;
                    DecreaseSelectFilmSize();   //フィルム縮小
                }
                //離れたらキャンセル
                if (Input.GetMouseButtonUp(0))
                {
                    CancelFilm();
                }
            }
        }
    }

    //マウス座標がステージ上にあるかどうかのフラグを返す
    public bool GetOnStageFlag()
    {
        return onStageFlag;
    }

    //引数のフィルムを選択状態にする
    public bool SelectFilm(GameObject film)
    {
        onStageFlag = false;
        //既にフィルムを選択中ならfalseを返す
        if (selectFilm != null)
        {
            return false;
        }
        //挟まっているフィルムの枚数が上限に達していればfalseを返す
        if (clip.GetHP() <=0)
        {
            return false;
        }
        //固定完了していないフィルムがあればfalseを返す
        if (!clip.GetFixedFlag())
        {
            return false;
        }
        selectFilm = film;
        selectFilmTransform = selectFilm.transform;
        //他のオブジェクトを一時停止する
        PauseOtherObject();

        return true;
    }

    //選択中フィルムをクリップに挟む
    private void InsertFilm()
    {
        //ポーズマネージャに登録する
        pauseManager.AddPauseObject(selectFilm, "Film",PauseManager.ALL^PauseManager.COL);
        //ホルダーとの親子関係を切る
        selectFilmTransform.parent.gameObject.GetComponent<FilmHolder>().InsertFilm();
        //クリップの子にする
        clip.InsertFilm(selectFilm);
        //選択中フィルムをnullに
        selectFilm = null;
        selectFilmTransform = null;
        //他のオブジェクトの一時停止を解除する
        //ResumeOtherObject();
    }

    //選択中フィルムをホルダーに戻す
    private void CancelFilm()
    {
        selectFilmTransform.parent.gameObject.GetComponent<FilmHolder>().CancelFilm();
        selectFilm = null;
        selectFilmTransform = null;
        //他のオブジェクトの一時停止を解除する
        ResumeOtherObject();
    }

    //選択中フィルム拡大
    private void IncreaseSelectFilmSize()
    {
        //選択中フィルムがなければ戻る
        if (selectFilm == null)
        {
            
            return;
        }
        selectFilmTransform.localScale = largeFilmScale;
    }

    //選択中フィルム縮小
    private void DecreaseSelectFilmSize()
    {
        //選択中フィルムがなければ戻る
        if (selectFilm == null)
        {
            return;
        }
        selectFilmTransform.localScale = smallFilmScale;
    }

    //操作中フィルム以外のオブジェクトを一時停止する
    public void PauseOtherObject()
    {
        pauseManager.PauseTagObject("Enemy");
        pauseManager.PauseTagObject("Player");
        pauseManager.PauseTagObject("Bell");
        pauseManager.PauseTagObject("Goal");
        pauseManager.PauseTagObject("Clip");
        //pauseManager.PauseTagObject("RedBranch");
        pauseManager.PauseListObject("Film");
        pauseManager.PauseTagObject("Leaf");
    }

    //フィルム以外のオブジェクトの一時停止を解除する
    public void ResumeOtherObject()
    {
        pauseManager.ResumeTagObject("Enemy");
        pauseManager.ResumeTagObject("Player");
        pauseManager.ResumeTagObject("Bell");
        pauseManager.ResumeTagObject("Goal");
        pauseManager.ResumeTagObject("Clip");
        //pauseManager.ResumeTagObject("RedBranch");
        pauseManager.ResumeListObject("Film");
        pauseManager.ResumeTagObject("Leaf");
    }
}