using UnityEngine;

public class StageImage : MonoBehaviour
{
    private enum SWAYSTATUS
    {
        NOT_SWAY,
        SWAY1,
        SWAY2,
        SWAY3,
        SWAY_MAX,
    }

    private enum AudioList
    {
        AUDIO_BUTTON,
        AUDIO_MAX
    }

    private Vector3 defaultPosition;
    private Quaternion defaultRotation;
    public Vector3 landingPosition; //ふぃるむしが着地する座標
    private Vector3 clipPosition;           //クリップ座標
    private bool selectFlag = false;          //選択中フラグ　使うか・・・？
    private bool swayFlag = false;          //揺れ中フラグ
    private float swayDegreeMax;            //揺れる角度最大値
    private float swayDegreeMin;            //揺れる角度最小値
    private float swayTime1;                //揺れる時間
    private float swayTime2;                //折り返しで揺れる時間
    private float swayTime3;                //更に折り返し揺れる時間
    private float swaySpeed;                //揺れる速度（オイラー角度）
    private StageSelectManager stageSelectManager;
    private StageSelectPlayer stageSelectPlayer;
    private float nowTime = 0;
    private Transform tf;
    private Vector3 nowPos;
    public StageSelectManager.SelectStage thisStage;    //このステージを示すenum
    private SWAYSTATUS swayStatus;

    private SourceAudio sourceAudio;
    private CustomAudioClip[] audioClip;

    private SpriteRenderer lockSprite;


    private void Awake()
    {
        tf = transform;
        lockSprite = tf.GetChild(0).GetComponent<SpriteRenderer>();
    }
    // Use this for initialization
    private void Start()
    {
        stageSelectManager = GameObject.Find("StageSelectManager").GetComponent<StageSelectManager>();
        stageSelectPlayer = GameObject.Find("StageSelectPlayer").GetComponent<StageSelectPlayer>();
        swayDegreeMax = stageSelectManager.swayDegreeMax;
        swayDegreeMin = stageSelectManager.swayDegreeMin;
        swayTime1 = stageSelectManager.swayTime1;
        swayTime2 = stageSelectManager.swayTime2;
        swayTime3 = stageSelectManager.swayTime3;
        swaySpeed = 0;
        swayStatus = SWAYSTATUS.NOT_SWAY;
        
        defaultPosition = tf.position;
        defaultRotation = tf.rotation;
        nowPos = tf.position;
        clipPosition = tf.parent.position;

        this.audioClip = new CustomAudioClip[(int)AudioList.AUDIO_MAX];
        this.audioClip[0].Clip = Resources.Load("Audio/SE/Button_Yes", typeof(AudioClip)) as AudioClip;
        this.audioClip[0].Vol = 1.0f;

        this.sourceAudio = this.gameObject.AddComponent<SourceAudio>();
        this.sourceAudio.m_Audio = this.audioClip;

        
    }

    // Update is called once per frame
    private void Update()
    {

        if (swayFlag)
        {
            nowTime += Time.deltaTime;
            switch (swayStatus)
            {
                case SWAYSTATUS.SWAY1:
                    if (nowTime >= swayTime1)
                    {
                        tf.RotateAround(clipPosition, Vector3.forward, swaySpeed * (Time.deltaTime - (nowTime - swayTime1)));
                        nowTime = 0;
                        swaySpeed = (swayDegreeMin - swayDegreeMax) / swayTime2;
                        swayStatus = SWAYSTATUS.SWAY2;
                        break;
                    }
                    tf.RotateAround(clipPosition, Vector3.forward, swaySpeed * Time.deltaTime);
                    break;

                case SWAYSTATUS.SWAY2:
                    if (nowTime >= swayTime2)
                    {
                        tf.RotateAround(clipPosition, Vector3.forward, swaySpeed * (Time.deltaTime - (nowTime - swayTime2)));
                        nowTime = 0;
                        swaySpeed = -swayDegreeMin / swayTime3;
                        swayStatus = SWAYSTATUS.SWAY3;
                        break;
                    }
                    tf.RotateAround(clipPosition, Vector3.forward, swaySpeed * Time.deltaTime);
                    break;

                case SWAYSTATUS.SWAY3:
                    if (nowTime >= swayTime3)
                    {
                        tf.position = defaultPosition;
                        tf.rotation = defaultRotation;
                        swayStatus = SWAYSTATUS.NOT_SWAY;
                        stageSelectManager.ChangeStageDetail();
                        break;
                    }
                    tf.RotateAround(clipPosition, Vector3.forward, swaySpeed * Time.deltaTime);
                    break;

                default:
                    break;
            }
        }
    }

    private void OnMouseUpAsButton()
    {
        if (lockSprite!=null && lockSprite.enabled)
        {
            return;
        }
        if (stageSelectPlayer.Jump(landingPosition, thisStage))
        {
            clipPosition = tf.parent.position;  //clip位置更新
            defaultPosition = tf.position;      //イメージのTransformも更新
            defaultRotation = tf.rotation;
            sourceAudio.PlaySE((int)AudioList.AUDIO_BUTTON);
            selectFlag = true;
        }
    }

    //ふぃるむし着地時揺れ
    public void SwayLanding()
    {
        selectFlag = true;
        swayFlag = true;
        nowTime = 0;
        swaySpeed = swayDegreeMax / swayTime1;
        swayStatus = SWAYSTATUS.SWAY1;
        stageSelectPlayer.transform.SetParent(tf);
    }

    public void DeleteLockSprite()
    {
        print(lockSprite);
        if (lockSprite == null)
        {
            return;
        }
        
        lockSprite.enabled = false;
    }

    public void FallLockSprite()
    {
        lockSprite.transform.Translate(Vector3.back);
        lockSprite.gameObject.AddComponent<UnLockDirection>();

    }
}