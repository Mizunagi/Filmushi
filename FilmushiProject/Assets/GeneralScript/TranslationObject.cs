using UnityEngine;

public class TranslationObject : MonoBehaviour
{
    public Vector3 startPos;    //開始座標
    public Vector3 endPos;      //終点座標
    private Vector3 nowPos;             //現在座標
    public float moveTotalTime; //移動にかかる時間
    public float startTime;     //移動し始めるまでの時間
    private float count;                //時間カウント
    private float speed;                //単位時間当たり移動速度
    private Transform tf;               //更新対象取得用
    private bool moveFinishFlag = false;//移動完了フラグ

    // Use this for initialization
    private void Start()
    {
        tf = GetComponent<Transform>();
        tf.position = startPos;
        nowPos = startPos;
        speed = (endPos.y - startPos.y) / moveTotalTime;
        count = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        count += Time.deltaTime;

        if (count < startTime || startTime < 0)
        {
            return;
        }
        if (moveTotalTime + startTime > count)
        {
            nowPos.y += speed * Time.deltaTime;
            tf.position = nowPos;
        }
        else
        {
            nowPos = endPos;
            tf.position = nowPos;
            moveFinishFlag = true;
        }
    }

    //移動を開始する
    public void MoveStart()
    {
        if (startTime < 0)
        {
            startTime = 0;
        }
        count = startTime;
    }

    //移動を完了させる
    public void Skip()
    {
        count = moveTotalTime;
    }

    public bool GetMoveFinishFlag()
    {
        return moveFinishFlag;
    }
}