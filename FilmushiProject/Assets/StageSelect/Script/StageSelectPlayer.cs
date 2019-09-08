using UnityEngine;

public class StageSelectPlayer : MonoBehaviour
{
    //ジャンプしたり振り返ったりする
    //ただし自発的に入力を検知して動くわけではなく、マネージャやステージ画像から呼ばれて動く

    private Vector3 defaultPosition;        //初期位置
    public Vector3 startVector;     //ジャンプ開始ベクトル
    public Vector3 endVector;       //ジャンプ終点ベクトル
    private Vector3 startPosition;          //ジャンプ開始位置
    private Vector3 endPosition;            //ジャンプ終点位置
    public float jumpTotalTime;     //ジャンプにかかる時間
    private float jumpNowTime = 0;            //ジャンプし始めてから経過した時間
    private bool jumpFlag = false;          //ジャンプ中かどうかのフラグ
    private Transform tf;
    private Vector3 nowPos;                 //現在位置
    private StageSelectManager ssm;
    private StageSelectManager.SelectStage selectStage; //選択中ステージ

    // Use this for initialization
    private void Start()
    {
        tf = GetComponent<Transform>();
        nowPos = tf.position;
        defaultPosition = tf.position;
        startPosition = tf.position;
        ssm = GameObject.Find("StageSelectManager").GetComponent<StageSelectManager>();
        selectStage = StageSelectManager.SelectStage.NOT_SELECT;
    }

    // Update is called once per frame
    private void Update()
    {
        if (jumpFlag)
        {
            jumpNowTime += Time.deltaTime;
            float t = jumpNowTime / jumpTotalTime;

            if (t >= 1) //着地処理へ
            {
                FinishJump();
                return;
            }
            //エルミート曲線の軌道で
            nowPos.x = (t - 1) * (t - 1) * (2 * t + 1) * startPosition.x + t * t * (3 - 2 * t) * endPosition.x + (1 - t) * (1 - t) * t * startVector.x + (t - 1) * t * t * endVector.x;
            nowPos.y = (t - 1) * (t - 1) * (2 * t + 1) * startPosition.y + t * t * (3 - 2 * t) * endPosition.y + (1 - t) * (1 - t) * t * startVector.y + (t - 1) * t * t * endVector.y;

            tf.position = nowPos;
        }
    }

    //ジャンプ開始処理
    //引数　着地位置、着地先ステージ番号
    public bool Jump(Vector3 landingPos, StageSelectManager.SelectStage ss)
    {
        //ジャンプ中、もしくはステージ選択中に他のステージの選択は無視
        if (jumpFlag || (nowPos != defaultPosition && landingPos != defaultPosition))
        {
            return false;
        }

        if (landingPos == defaultPosition)  //戻ってくる時は左右反転
        {
            Invert();
        }
        jumpFlag = true;
        jumpNowTime = 0;
        startPosition = tf.position;
        endPosition = landingPos;
        selectStage = ss;

        return true;
    }
    //ページ遷移用ジャンプ（プレイヤーが移動する場合）
    public bool JumpPage(Vector3 landingPos)
    {
        //ジャンプ中、もしくはステージ選択中に他のステージの選択は無視
        if (jumpFlag || (nowPos != defaultPosition && landingPos != defaultPosition))
        {
            return false;
        }
        if (landingPos.x < defaultPosition.x)  //戻ってくる時は左右反転
        {
            Invert();
        }

        jumpFlag = true;
        jumpNowTime = 0;
        startPosition = tf.position;
        endPosition = landingPos;
        selectStage = StageSelectManager.SelectStage.NOT_SELECT;

        return true;
    }
    //ページ遷移用ジャンプ（ページ側の座標を動かす場合）
    public bool JumpPage(float vector)
    {
        //ジャンプ中、もしくはステージ選択中は無視
        if (jumpFlag || nowPos != defaultPosition)
        {
            return false;
        }
        if (vector<0)  //左のページへの遷移では左右反転
        {
            Invert();
        }

        jumpFlag = true;
        jumpNowTime = 0;
        startPosition = tf.position;
        endPosition = tf.position;
        selectStage = StageSelectManager.SelectStage.NOT_SELECT;

        return true;
    }

    //着地処理
    private void FinishJump()
    {
        nowPos = endPosition;
        tf.position = endPosition;
        jumpFlag = false;
        if (tf.position == defaultPosition)
        {
            SetRightVector();   //定位置に戻ってきたら右を向く
        }
        ssm.SelectJumpFinish(selectStage);  //選択したステージに着地したことをマネージャに告知
    }

    //左右反転
    private void Invert()
    {
        Vector3 _nowScale = tf.localScale;
        _nowScale.x *= -1;
        tf.localScale = _nowScale;
    }

    private void SetRightVector()
    {
        //右を向いてたら何もしない
        if (tf.localScale.x > 0)
        {
            return;
        }
        //左を向いてたら反転
        else
        {
            Invert();
        }
    }
}