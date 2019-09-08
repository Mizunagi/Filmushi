using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private enum PlayerStateFlg
    {
        WAIT,
        RIGHT,
        LEFT,
    }

    private enum PlayerAnimStateFlg
    {
        WAIT,
        WAIK,
        FURIMUKI,
        FIVEWAIT,
        FIVEWAIT2,
    }

    private float speed;        //移動量を調整
    public float retentionspeed; //移動量を保持する
    private float direction;     //移動方向を調整
    public float jumpspeed;      //ジャンプ量を調整
    public float maxfallout;     //ステージ落下死の座標
    private float nowtime; //今の時間
    public float maxtime;  //最大時間
    public float eyestarttime;      //目のパチパチアニメーションの開始時間
    private float waitnowtime = 0.0f;   //目のパチパチアニメーションの切り替え時間
    public float deadendtime;           //死亡後の終わり時間;
    private float deadtime = 0.0f;      //死亡後の時間;
    public float touchendtime = 0.0f;      //の時間;
    private float touchtime = 0.0f;      //の時間;
    private Vector3 playerstartPos; //敵に当たった時や落下した初期座標にリセットするための座標を取得
    private Quaternion playerstartRotation; //敵に当たった時や落下した初期回転にリセットするための座標を取得
    private Vector2 tapstartPos;     //タップした時点でのプレイヤーの座標
    private Vector2 tapendPos;       //タップした座標
    private Vector3 oldPos;          //プレイヤーの1フレーム前の座標
    private Vector3 nowPos;          //プレイヤーの座標
    private Vector3 startscale;      //プレイヤーのスタートスケール
    private Vector3 nowscale;       //プレイヤースケール
    private Vector2 changemousePos;  //マウス座標をワールド変換するための仮変数
    private Vector2 velocity;
    private Vector2 startvelocity;
    private float startangularvelocity;
    private Transform tf;
    private Rigidbody2D rb2d;
    private Animator anim;          //アニメーション遷移変数
    public Material mat;
    public Texture filmushi_blink;
    public Texture filmushi_normal;
    private Vector2 tapstartget;         //タップされた始まり位置を取得する
    private Vector2 tapendget;         //タップされた終わり位置を取得する
    public LayerMask BackGround;
    private PlayerStateFlg psf;
    private PlayerAnimStateFlg pasf;
    private bool touchtimeflg;   //タッチを取得する時間制御フラグ
    private bool animwalkflg;     //アニメーション歩くフラグ
    private bool animfurimukiflg; //アニメーション振り向きフラグ
    private bool animjumpflg;     //アニメーションジャンプフラグ
    private bool animwaitflg;    //アニメーション５秒待機フラグ
    private bool animwait2flg;   //アニメーション５秒待機パターン２フラグ
    private bool animdieflg;    //アニメーション死亡フラグ
    private bool animclearflg;    //アニメーションクリアフラグ
    private bool switchwaitflg;  //５秒待機の切り替えフラグ
    private bool switchfivewaitflg;  //５秒待機の切り替えフラグ
    private bool tapflg;        //タップかどうかを確かめるフラグ
    private bool onetapflg;     //タップが一度されたかどうかを確かめるフラグ
    private bool swipeflg;      //スワイプがされたかどうかを確かめるフラグ
    private bool jumpflg;       //ジャンプを許可しているかを確かめるフラグ
    private bool enemycolflg;   //エネミーに当たったかどうかを確かめるフラグ
    private bool playendflg;   //プレイを終了するフラグ

    private enum AudioList
    {
        AUDIO_JUMP,
        AUDIO_DEAD,
        AUDIO_MAX
    }

    private SourceAudio soueceAudio;
    private CustomAudioClip[] audioClip;

    //澤田が追加した項目
    private Collider2D stageCollider;

    private Collider2D clipCollider;
    private Collider2D capsuleCollider; //これは自分の
    public float swipeJudgeDistance;    //スワイプ判定になるのに必要なタップ距離
    public GameObject tapEffect;
    private Transform tapEffectTransform;
    private float groundRotation;       //地面の角度
    private List<Vector2> inputPosBuffer = new List<Vector2>(); //以前のフレームでのタッチ座標
    public float maxSpeedDistance;      //最大速度になるために必要なスワイプ距離
    private ContactFilter2D groundFilter;

    public GameObject butterfly;
    private GameObject hitParticle;
    private GameObject pickUpParticle;
    private FadeImage fd_out;

    // Use this for initializations
    private void Awake()
    {
        tf = GetComponent<Transform>();
        rb2d = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        InitStart();
        playerstartPos = tf.position;
        startscale = tf.localScale;
        nowscale = startscale;
        startvelocity = rb2d.velocity;
        startangularvelocity = rb2d.angularVelocity;

        stageCollider = GameObject.Find("StageBackGround").GetComponent<BoxCollider2D>();
        clipCollider = GameObject.Find("clip_L").GetComponent<BoxCollider2D>();

        CreateTapEffect();
        tapEffectTransform.GetComponent<Renderer>().enabled = false;

        groundFilter.SetLayerMask(LayerMask.GetMask("Leaf", "Branch", "RedBranch"));    //足場になりうるレイヤー

        hitParticle = GameObject.Find("EffectManager").GetComponent<EffectManager>().hitEffect;
        pickUpParticle = GameObject.Find("EffectManager").GetComponent<EffectManager>().acornPickUpEffect;

        fd_out = GameObject.Find("Panel").GetComponent<FadeImage>();

        this.audioClip = new CustomAudioClip[(int)AudioList.AUDIO_MAX];
        this.audioClip[(int)AudioList.AUDIO_JUMP].Clip = Resources.Load("Audio/SE/Filmushi_Jump", typeof(AudioClip)) as AudioClip;
        this.audioClip[(int)AudioList.AUDIO_JUMP].Vol = 1.0f;
        this.audioClip[(int)AudioList.AUDIO_DEAD].Clip = Resources.Load("Audio/SE/Filmushi_Down", typeof(AudioClip)) as AudioClip;
        this.audioClip[(int)AudioList.AUDIO_DEAD].Vol = 1.0f;

        this.soueceAudio = this.gameObject.AddComponent<SourceAudio>();
        this.soueceAudio.m_Audio = this.audioClip;
    }

    // Update is called once per frame
    private void Update()
    {
        //死んだ時のリスタート
        if (fd_out.GetEndFlag())
        {
            var nowScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(nowScene);
        }

        nowPos = tf.position;

        DetectionTap();
        WallControl();
        TransitionMove();
        TransitionAnim();

        anim.SetBool("jumping", animjumpflg);
        anim.SetBool("waiting", animwaitflg);
        anim.SetBool("waiting2", animwait2flg);
        anim.SetBool("dieflg", animdieflg);
        anim.SetBool("clearflg", animclearflg);
        //スワイプの入力検知はここに書く
        if (swipeflg == true)
        {
            if (jumpflg == true)
            {
                mat.SetTexture("_MainTex", filmushi_normal);
                Jump();
                animjumpflg = true;

                soueceAudio.PlaySE((int)AudioList.AUDIO_JUMP);
            }
            swipeflg = false;
        }
        //プレイヤーがステージの外に落下した場合
        if (tf.position.y < -(maxfallout) && !jumpflg && rb2d.velocity.y < 0)
        {
            //PlayerStart();
            print("落下死亡");
            tf.position = Vector3.up * 1000;
            soueceAudio.PlaySE((int)AudioList.AUDIO_DEAD); //落下時の音（仮）
            fd_out.FadeStart();
        }
        //プレイヤーがエネミーに当たった場合
        if (enemycolflg == true)
        {
            animdieflg = true;
            print("敵に当たったので死亡");
            if (deadtime == 1.0f)
            {
                //ふぃるむしが死んだ瞬間がほしい
                soueceAudio.PlaySE((int)AudioList.AUDIO_DEAD);
            }
            if (deadtime >= deadendtime)
            {
                //PlayerStart();
                //deadtime = 0.0f;
                //enemycolflg = false;
                fd_out.FadeStart(); //死んだ一定時間後フェードアウトしてリスタート
            }
            deadtime += 1.0f;
        }
        //プレイヤーが止まっているとき
        if (pasf == PlayerAnimStateFlg.WAIT)
        {
            if (switchwaitflg == true && nowtime > eyestarttime && nowtime < maxtime)
            {
                mat.SetTexture("_MainTex", filmushi_normal);
                if (waitnowtime > 25.0f)
                {
                    switchwaitflg = false;
                    waitnowtime = 0.0f;
                }
            }
            else if (switchwaitflg == false)
            {
                mat.SetTexture("_MainTex", filmushi_blink);
                if (waitnowtime > 25.0f)
                {
                    switchwaitflg = true;
                    waitnowtime = 0.0f;
                }
            }
            waitnowtime += 1.0f;
        }
        //もしプレイヤーが5秒止まっている場合
        if (pasf == PlayerAnimStateFlg.WAIT && nowtime >= maxtime)
        {
            if (switchfivewaitflg == true)
            {
                pasf = PlayerAnimStateFlg.FIVEWAIT;
                switchfivewaitflg = false;
            }
            else
            {
                pasf = PlayerAnimStateFlg.FIVEWAIT2;
                switchfivewaitflg = true;
            }
        }
        else if (pasf != PlayerAnimStateFlg.WAIT)
        {
            nowtime = 0.0f;
        }
        if (nowtime <= maxtime)
        {
            nowtime += 1.0f;
        }
        tf.localScale = nowscale;
    }

    private void FixedUpdate()
    {
        velocity = rb2d.velocity;
        Move(speed, direction);
        rb2d.velocity = velocity;

        AngleControl();
        //FrictionControll();
    }

    private void FrictionControll()
    {
        //摩擦いらんかも
        if (jumpflg == true)
        {
            rb2d.sharedMaterial.friction = 0; //old =1
        }
        else
        {
            rb2d.sharedMaterial.friction = 0;
        }
    }

    private void AngleControl()
    {
        bool error_rot = false;
        if (jumpflg == true)
        {
            if (rb2d.rotation > 50 || rb2d.rotation < -50)
            {
                error_rot = true;
            }
            //rb2d.angularVelocity = (groundRotation - rb2d.rotation) * 10;   //着地時に素早く足場と同じrotationへ移行させる
            rb2d.rotation = Mathf.Clamp(rb2d.rotation, -45, 45);
        }
        else
        {
            rb2d.rotation = Mathf.Clamp(rb2d.rotation, -40, 40);
            if (rb2d.rotation < 0)
            {
                rb2d.rotation += 9;
                if (rb2d.rotation > 0)
                {
                    rb2d.rotation = 0;
                }
            }
            else if (rb2d.rotation > 0)
            {
                rb2d.rotation -= 9;
                if (rb2d.rotation < 0)
                {
                    rb2d.rotation = 0;
                }
            }
        }

        if ((rb2d.rotation > 44.0f || rb2d.rotation < -44.0f) && !error_rot)
        {
            rb2d.gravityScale = 0;
            rb2d.angularVelocity = 0;
        }
        else
        {
            rb2d.gravityScale = 1;
        }
    }

    private void WallControl()
    {
    }

    private void TransitionMove()
    {
        switch (psf)
        {
            case PlayerStateFlg.WAIT:
                speed = 0.0f;
                break;

            case PlayerStateFlg.RIGHT:
                //speed = retentionspeed;
                direction = 1;
                //if (tf.position.x >= tapEffectTransform.position.x)
                //{
                //    psf = PlayerStateFlg.WAIT;
                //    pasf = PlayerAnimStateFlg.WAIT;
                //    onetapflg = false;
                //    speed = 0;
                //    nowPos = tf.position;
                //    nowPos.x = tapEffectTransform.position.x-0.1f;
                //    tf.position = nowPos;
                //    tapEffectTransform.GetComponent<Renderer>().enabled = false;
                //}
                //else
                //{
                //    speed = retentionspeed;
                //    direction = 1;
                //}
                break;

            case PlayerStateFlg.LEFT:
                //speed = retentionspeed;
                direction = -1;
                //if (tf.position.x <= tapEffectTransform.position.x)
                //{
                //    psf = PlayerStateFlg.WAIT;
                //    pasf = PlayerAnimStateFlg.WAIT;
                //    onetapflg = false;
                //    speed = 0;
                //    nowPos = tf.position;
                //    nowPos.x = tapEffectTransform.position.x+0.1f;
                //    tf.position = nowPos;
                //    tapEffectTransform.GetComponent<Renderer>().enabled = false;
                //}
                //else
                //{
                //    speed = retentionspeed;
                //    direction = -1;
                //}
                break;
        }
    }

    private void TransitionAnim()
    {
        switch (pasf)
        {
            case PlayerAnimStateFlg.WAIT:
                animwalkflg = false;
                animfurimukiflg = false;
                animwaitflg = false;
                animwait2flg = false;
                break;

            case PlayerAnimStateFlg.WAIK:
                mat.SetTexture("_MainTex", filmushi_normal);
                animwalkflg = true;
                break;

            case PlayerAnimStateFlg.FURIMUKI:
                mat.SetTexture("_MainTex", filmushi_normal);
                furimuki();
                break;

            case PlayerAnimStateFlg.FIVEWAIT:
                mat.SetTexture("_MainTex", filmushi_normal);
                animwalkflg = false;
                animfurimukiflg = false;
                animwaitflg = true;
                animwait2flg = false;
                break;

            case PlayerAnimStateFlg.FIVEWAIT2:
                mat.SetTexture("_MainTex", filmushi_normal);
                animwalkflg = false;
                animfurimukiflg = false;
                animwaitflg = false;
                animwait2flg = true;
                break;
        }
    }

    //移動用入力検知
    private void DetectionTap()
    {
        //死んでたりたら止める
        if (animdieflg)
        {
            psf = PlayerStateFlg.WAIT;
            pasf = PlayerAnimStateFlg.WAIT;
            tapEffectTransform.GetComponent<Renderer>().enabled = false;

            touchtimeflg = false;
            inputPosBuffer.Clear();
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            //タッチ座標取得
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //ステージ背景の中かつクリップ上ではない場所をクリックした時検知開始
            if (stageCollider.OverlapPoint(mousePos) && !clipCollider.OverlapPoint(mousePos))
            {
                tapstartPos = mousePos;
                //タッチしたらマーカー表示
                tapEffectTransform.GetComponent<Renderer>().enabled = true;
                tapEffectTransform.position = mousePos;
                tapEffectTransform.Translate(Vector3.back * 5);
                touchtimeflg = true;
            }
        }
        if (touchtimeflg)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //現在のタッチ座標をバッファへ格納
            inputPosBuffer.Add(mousePos);
            Vector2 judgePos = new Vector2();
            //バッファが大きくなりすぎないように古すぎるものは削除
            if (inputPosBuffer.Count > 10)
            {
                inputPosBuffer.RemoveAt(0);
            }
            //ジャンプのための座標比較用
            judgePos = inputPosBuffer[0];
            //以前のフレームより一定以上↑へスワイプした時ジャンプ中でなければジャンプする
            if (mousePos.y > judgePos.y + swipeJudgeDistance && !animjumpflg)
            {
                swipeflg = true;
                animwalkflg = false;
                animfurimukiflg = false;
                inputPosBuffer.Clear();
            }
            //タッチ座標が開始位置より一定以上右なら右へ移動する
            if (tapstartPos.x < mousePos.x - 0.1f)
            {
                //開始位置との距離によってスピードを変える
                if (mousePos.x - tapstartPos.x > maxSpeedDistance)
                {
                    speed = retentionspeed;
                }
                else
                {
                    speed = retentionspeed * (mousePos.x - tapstartPos.x) / maxSpeedDistance;
                }
                tapEffectTransform.localEulerAngles = Vector3.forward * 90;
                psf = PlayerStateFlg.RIGHT;
                //プレイヤーが左を向いていた場合に右に向くように処理する
                if (nowscale.x < 0.0f)
                    pasf = PlayerAnimStateFlg.FURIMUKI;
                else
                    pasf = PlayerAnimStateFlg.WAIK;
            }
            //タッチ座標が開始位置より一定以上左なら左へ移動する
            else if (tapstartPos.x > mousePos.x + 0.1f)
            {
                //開始位置との距離によってスピードを変える
                if (tapstartPos.x - mousePos.x > maxSpeedDistance)
                {
                    speed = retentionspeed;
                }
                else
                {
                    speed = retentionspeed * (tapstartPos.x - mousePos.x) / maxSpeedDistance;
                }
                tapEffectTransform.localEulerAngles = Vector3.back * 90;
                psf = PlayerStateFlg.LEFT;
                //プレイヤーが右を向いていた場合に左に向くように処理する
                if (nowscale.x > 0.0f)
                    pasf = PlayerAnimStateFlg.FURIMUKI;
                else
                    pasf = PlayerAnimStateFlg.WAIK;
            }
            //タッチ位置から動いてなければ止まる
            else
            {
                psf = PlayerStateFlg.WAIT;
                pasf = PlayerAnimStateFlg.WAIT;
                tapEffectTransform.localEulerAngles = Vector3.zero;
            }
            //離したら止まる
            if (Input.GetMouseButtonUp(0))
            {
                psf = PlayerStateFlg.WAIT;
                pasf = PlayerAnimStateFlg.WAIT;
                tapEffectTransform.GetComponent<Renderer>().enabled = false;

                touchtimeflg = false;
                inputPosBuffer.Clear();
            }
        }
    }

    private void DetectionTapOld3()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //タッチ座標取得
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //ステージ背景の中かつクリップ上ではない場所をクリックした時検知開始
            if (stageCollider.OverlapPoint(mousePos) && !clipCollider.OverlapPoint(mousePos))
            {
                touchtimeflg = true;
            }
        }
        if (touchtimeflg)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            inputPosBuffer.Add(mousePos);
            Vector2 judgePos = new Vector2();
            if (inputPosBuffer.Count < 10)
            {
                judgePos = inputPosBuffer[0];
            }
            else
            {
                judgePos = inputPosBuffer[inputPosBuffer.Count - 10];
            }
            //タッチ位置より一定以上↑へスワイプした時ジャンプする
            if (mousePos.y > judgePos.y + swipeJudgeDistance)
            {
                swipeflg = true;
                animwalkflg = false;
                animfurimukiflg = false;
                inputPosBuffer.Clear();
            }
            //タップ位置がプレイヤーより右なら右へ移動する
            if (tf.position.x < mousePos.x - 0.1f)
            {
                onetapflg = true;
                tapEffectTransform.GetComponent<Renderer>().enabled = true;
                tapEffectTransform.position = mousePos;
                tapEffectTransform.Translate(Vector3.back * 5);
                psf = PlayerStateFlg.RIGHT;
                //プレイヤーが左を向いていた場合に右に向くように処理する
                if (nowscale.x < 0.0f)
                    pasf = PlayerAnimStateFlg.FURIMUKI;
                else
                    pasf = PlayerAnimStateFlg.WAIK;
            }
            //タップ位置がプレイヤーより左なら左へ移動する
            else if (tf.position.x > mousePos.x + 0.1f)
            {
                onetapflg = true;
                tapEffectTransform.GetComponent<Renderer>().enabled = true;
                tapEffectTransform.position = mousePos;
                tapEffectTransform.Translate(Vector3.back * 5);
                psf = PlayerStateFlg.LEFT;
                //プレイヤーが右を向いていた場合に左に向くように処理する
                if (nowscale.x > 0.0f)
                    pasf = PlayerAnimStateFlg.FURIMUKI;
                else
                    pasf = PlayerAnimStateFlg.WAIK;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            onetapflg = false;
            psf = PlayerStateFlg.WAIT;
            pasf = PlayerAnimStateFlg.WAIT;
            tapEffectTransform.GetComponent<Renderer>().enabled = false;

            touchtimeflg = false;
            inputPosBuffer.Clear();
        }
    }

    private void DetectionTapOld2()
    {
        //タッチ取得
        if (Input.GetMouseButtonDown(0))
        {
            //タッチ座標取得
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //ステージ背景の中かつクリップ上ではない場所をクリックした時検知開始
            if (stageCollider.OverlapPoint(mousePos) && !clipCollider.OverlapPoint(mousePos))
            {
                touchtimeflg = true;
                tapstartPos = mousePos;
            }
        }
        //タッチ中フラグ
        if (touchtimeflg)
        {
            touchtime += Time.deltaTime;
            //時間超過でタップ判定を消す
            if (touchtime >= touchendtime)
            {
                touchtime = 0.0f;
                touchtimeflg = false;
                return;
            }
            else
            {
                //現在のタッチ座標取得
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //タッチ位置より一定以上↑へスワイプした時ジャンプする
                if (mousePos.y > tapstartPos.y + swipeJudgeDistance)
                {
                    swipeflg = true;
                    animwalkflg = false;
                    animfurimukiflg = false;
                    touchtime = 0.0f;
                    touchtimeflg = false;
                }
                //タッチ位置と近い位置で指が離れた時タップしたと判定する
                else if (Input.GetMouseButtonUp(0) && Vector2.Distance(tapstartPos, mousePos) < swipeJudgeDistance)
                {
                    touchtime = 0.0f;
                    touchtimeflg = false;
                    //タップ時処理
                    //既にタップ位置へ移動中なら移動を中止する
                    if (onetapflg)
                    {
                        onetapflg = false;
                        psf = PlayerStateFlg.WAIT;
                        pasf = PlayerAnimStateFlg.WAIT;
                        tapEffectTransform.GetComponent<Renderer>().enabled = false;
                    }
                    //タップ位置がプレイヤーより右なら右へ移動する
                    else if (tf.position.x < mousePos.x)
                    {
                        onetapflg = true;
                        tapEffectTransform.GetComponent<Renderer>().enabled = true;
                        tapEffectTransform.position = mousePos;
                        tapEffectTransform.Translate(Vector3.back * 5);
                        psf = PlayerStateFlg.RIGHT;
                        //プレイヤーが左を向いていた場合に右に向くように処理する
                        if (nowscale.x < 0.0f)
                            pasf = PlayerAnimStateFlg.FURIMUKI;
                        else
                            pasf = PlayerAnimStateFlg.WAIK;
                    }
                    //タップ位置がプレイヤーより左なら左へ移動する
                    else
                    {
                        onetapflg = true;
                        tapEffectTransform.GetComponent<Renderer>().enabled = true;
                        tapEffectTransform.position = mousePos;
                        tapEffectTransform.Translate(Vector3.back * 5);
                        psf = PlayerStateFlg.LEFT;
                        //プレイヤーが右を向いていた場合に左に向くように処理する
                        if (nowscale.x > 0.0f)
                            pasf = PlayerAnimStateFlg.FURIMUKI;
                        else
                            pasf = PlayerAnimStateFlg.WAIK;
                    }
                }
            }
        }
    }

    //タップ、スワイプ検知処理
    private void DetectionTapOld()
    {
        //タップをする
        if (Input.GetMouseButtonDown(0))
        {
            //最初にタップされた座標を取得
            changemousePos = Input.mousePosition;
            tapstartPos = Camera.main.ScreenToWorldPoint(changemousePos);
            tapstartget = new Vector2(tapstartPos.x, tapstartPos.y);
            //タップされたらカウントフラグをtrueにする
            touchtimeflg = true;
        }
        //タップされたらカウント開始
        if (touchtimeflg == true)
        {
            touchtime += 1.0f;
        }
        //一定時間が過ぎると処理に入る
        if (touchtime >= touchendtime)
        {
            //カウントフラグをfalseにする
            touchtimeflg = false;
            //カウント経過後にタップされた座標を取得
            changemousePos = Input.mousePosition;
            tapendPos = Camera.main.ScreenToWorldPoint(changemousePos);
            tapendget = new Vector2(tapendPos.x, tapendPos.y);
            //最初にタップされた座標とカウント後にタップされた座標がステージ内なら処理に入る。そしてゴールや死亡をしなければこの処理に入る
            if (Physics2D.Raycast(tapstartget, -Vector2.up, 0.001f, BackGround) && Physics2D.Raycast(tapendget, -Vector2.up, 0.001f, BackGround) && playendflg == true)
            {
                //最初にタップされたy座標よりカウント後のタップy座標の方が高ければジャンプ処理に入る
                if (tapstartPos.y < tapendPos.y)
                {
                    swipeflg = true;
                    animwalkflg = false;
                    animfurimukiflg = false;
                    touchtime = 0.0f;
                }
                //最初にタップされたy座標とカウント後のタップy座標が一緒であれば歩行処理に入る
                else if (tapstartPos.y == tapendPos.y)
                {
                    tapflg = true;
                }
                //タップとプレイヤーの位置が一緒の場合は止まる
                if (tapflg == true && onetapflg == false && nowPos.x == tapendPos.x)
                {
                    tapflg = false;
                    onetapflg = true;
                    touchtime = 0.0f;
                    psf = PlayerStateFlg.WAIT;
                    pasf = PlayerAnimStateFlg.WAIT;
                }
                //タップの位置がプレイヤーの位置より右にある場合は右に移動する
                else if (tapflg == true && onetapflg == false && nowPos.x < tapendPos.x)
                {
                    tapflg = false;
                    onetapflg = true;
                    touchtime = 0.0f;
                    psf = PlayerStateFlg.RIGHT;
                    //プレイヤーが左を向いていた場合に右に向くように処理する
                    if (nowscale.x < 0.0f)
                        pasf = PlayerAnimStateFlg.FURIMUKI;
                    else
                        pasf = PlayerAnimStateFlg.WAIK;
                }
                //タップの位置がプレイヤーの位置より左にある場合は左に移動する
                else if (tapflg == true && onetapflg == false && nowPos.x > tapendPos.x)
                {
                    psf = PlayerStateFlg.LEFT;
                    tapflg = false;
                    onetapflg = true;
                    touchtime = 0.0f;
                    //プレイヤーが右を向いていた場合に左に向くように処理する
                    if (nowscale.x > 0.0f)
                        pasf = PlayerAnimStateFlg.FURIMUKI;
                    else
                        pasf = PlayerAnimStateFlg.WAIK;
                }
                //タップが一度押されている場合はプレイヤーは停止する
                else if (tapflg == true && onetapflg == true)
                {
                    psf = PlayerStateFlg.WAIT;
                    pasf = PlayerAnimStateFlg.WAIT;
                    touchtime = 0.0f;
                    onetapflg = false;
                    tapflg = false;
                }
            }
        }
    }

    //プレイヤーが振り向く
    private void furimuki()
    {
        animfurimukiflg = true;
        if (animfurimukiflg == true)
        {
            nowscale = tf.localScale;
            nowscale.x *= -1.0f;
            pasf = PlayerAnimStateFlg.WAIK;
        }
    }

    //プレイヤーが左右移動する
    private void Move(float movespeed, float dir)
    {
        //地上に居る時の処理（ただし、おかしな角度になっている時は落ちる
        if (!animjumpflg && jumpflg == true && rb2d.rotation < 50 && rb2d.rotation > -50)
        {
            velocity.x = movespeed * dir * Mathf.Cos(rb2d.rotation * Mathf.Deg2Rad);
            velocity.y = movespeed * dir * Mathf.Sin(rb2d.rotation * Mathf.Deg2Rad);
        }
        else
        {
            velocity.x = movespeed * dir;
        }

        anim.SetBool("moving", animwalkflg);
        anim.SetBool("furimukiflg", animfurimukiflg);
    }

    //プレイヤーがジャンプする
    private void Jump()
    {
        if (animjumpflg)
        {
            return;
        }

        //rb2d.AddForce(Vector2.up * jumpspeed);
        rb2d.velocity = Vector2.up * jumpspeed / 50;
    }

    //ジャンプ操作を承認する関数
    public void SetJumpFlg(bool setflg)
    {
        jumpflg = setflg;
    }

    //ジャンプアニメーションを承認する関数
    public void SetAnimJumpFlg(bool setflg)
    {
        animjumpflg = setflg;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !animdieflg)
        {
            enemycolflg = true;
            playendflg = false;
            psf = PlayerStateFlg.WAIT;

            rb2d.Pause();

            print("敵に当たった");

            var _particle = Instantiate(hitParticle);
            _particle.transform.position = (Vector3)collision.contacts[0].point + Vector3.back * 5;
            _particle.GetComponent<ParticleSystem>().Play();
        }

        //if (jumpflg&&(collision.gameObject.tag == "Branch" || collision.gameObject.tag == "Leaf" || collision.gameObject.tag=="RedBranch"))
        //{
        //    if (collision.contacts.Length == 1 && collision.contacts[0].normal.y>=0.5f && collision.contacts[0].normal.y<=0.99f)
        //    {
        //        var _work_pos = tf.position;   //一旦Transformを退避
        //        var _work_rot = tf.rotation;
        //        //斜面に合わせて傾ける
        //        float _groundRotation = Mathf.Atan2(collision.contacts[0].normal.y, collision.contacts[0].normal.x) * Mathf.Rad2Deg - 90.0f;
        //        tf.RotateAround(collision.contacts[0].point, Vector3.forward,_groundRotation);
        //        Collider2D[] result = new Collider2D[2];
        //        //他の足場にも接触するようなら傾きを戻す
        //        if (capsuleCollider.OverlapCollider(groundFilter, result) > 1)
        //        {
        //            tf.position = _work_pos;
        //            tf.rotation = _work_rot;
        //        }

        //        //SetGroundRotation(_groundRotation);
        //    }
        //    Vector2 groundRotationNormal = Vector2.zero;
        //    int cnt = 0;
        //    foreach (var col in collision.contacts)
        //    {
        //        if (col.normal.y >= 0.5f)
        //        {
        //            LayerMask layer = LayerMask.GetMask("Branch");

        //            RaycastHit2D ray = Physics2D.Raycast(col.point, Vector2.down, 0.01f, layer);
        //            if (ray.transform == null)
        //            {
        //                layer = LayerMask.GetMask("Leaf");
        //                ray = Physics2D.Raycast(col.point, Vector2.down, 0.01f, layer);
        //            }

        //            //ray = Physics2D.Raycast(col.point, Vector2.down,,LayerMask.NameToLayer("Branch"));
        //            groundRotationNormal += ray.normal;
        //            cnt++;
        //            print(ray.transform.gameObject);
        //            print(ray.normal);
        //        }

        //    }

        //    groundRotationNormal /= (float)cnt;
        //    float _rotation = Mathf.Rad2Deg * Mathf.Atan2(groundRotationNormal.y, groundRotationNormal.x) - 90;
        //    SetGroundRotation(_rotation);
        //}
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemycolflg = true;
            playendflg = false;
            psf = PlayerStateFlg.WAIT;

            rb2d.Pause();

            print("敵に当たった");

            var _particle = Instantiate(hitParticle);
            _particle.transform.position = (Vector3)collision.transform.position + Vector3.back * 5;
            _particle.GetComponent<ParticleSystem>().Play();
        }
        if (collision.gameObject.tag == "Goal")
        {
            animclearflg = true;
            playendflg = false;
            //PlayerStart();
            psf = PlayerStateFlg.WAIT;
            print("ゴールに当たった");
            tf.parent = null;
            ChangeButterfly();
            tapEffectTransform.GetComponent<Renderer>().enabled = false;
        }

        /*
        if (collision.gameObject.tag == "Leaf")
        {
            if (collision.GetComponent<Leaf>().GetFallFlag() && collision.transform.position.y > tf.position.y)
            {
                enemycolflg = true;
                playendflg = false;
                psf = PlayerStateFlg.WAIT;
                print("落下足場に当たった");
            }
        }*/
    }

    //初期値共通まとめ関数
    private void InitStart()
    {
        psf = PlayerStateFlg.WAIT;
        pasf = PlayerAnimStateFlg.WAIT;
        mat.SetTexture("_MainTex", filmushi_normal);
        nowtime = 0.0f;
        deadtime = 0.0f;
        waitnowtime = 0.0f;
        touchtime = 0.0f;
        groundRotation = 0.0f;
        touchtimeflg = false;
        animwalkflg = false;
        animclearflg = false;
        animfurimukiflg = false;
        animjumpflg = false;
        animwaitflg = false;
        animwait2flg = false;
        animdieflg = false;
        switchwaitflg = true;
        tapflg = false;
        onetapflg = false;
        swipeflg = false;
        jumpflg = false;
        enemycolflg = false;
        playendflg = true;
    }

    //プレイヤーを初期位置に戻す
    private void PlayerStart()
    {
        InitStart();
        nowscale = startscale;
        tf.position = playerstartPos;
        playerstartRotation.x = 0.0f;
        playerstartRotation.y = 0.0f;
        playerstartRotation.z = 0.0f;
        rb2d.velocity = startvelocity;
        rb2d.angularVelocity = startangularvelocity;
        tf.rotation = playerstartRotation;
        tapEffectTransform.GetComponent<Renderer>().enabled = false;
    }

    public void SetGroundRotation(float zRotation)
    {
        groundRotation = zRotation;
    }

    private void CreateTapEffect()
    {
        if (tapEffectTransform != null)
        {
            DestroyTapEffect();
        }
        tapEffectTransform = Instantiate(tapEffect).transform;
    }

    private void DestroyTapEffect()
    {
        if (tapEffectTransform == null)
        {
            return;
        }
        Destroy(tapEffectTransform.gameObject);
    }

    private void ChangeButterfly()
    {
        Destroy(tf.Find("Filmushi").gameObject);
        var but = Instantiate(butterfly);
        but.transform.position = tf.position;
        but.transform.parent = tf;

        var _particle = Instantiate(pickUpParticle);
        _particle.transform.position = tf.position;
        _particle.GetComponent<ParticleSystem>().Play();

        GameObject.Find("PauseManager").GetComponent<PauseManager>().Pause(this.gameObject);
    }
}