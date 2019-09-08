using System.Collections.Generic;
using UnityEngine;

public class Clip : MonoBehaviour
{
    public Vector3 centerPosition;      //クリップ位置（中心）
    public Vector3 rightPosition;       //クリップ位置（右側）
    public Vector3 leftPosition;        //クリップ位置（左側）
    public int InitPosType;
    private float centerRightDivideX;           //中央か右側かの区別用
    private float centerLeftDivideX;            //中央か左側かの区別用
    private List<GameObject> insertFilmList = new List<GameObject>(); //挿んだフィルムのリスト
    public float maxHP;                 //耐久度上限
    private static float nowHP;                        //現在の耐久度
    private Transform tf;
    private BoxCollider2D clipCollider;
    private BoxCollider2D pinCollider;
    private bool selectFlag = false;              //クリップ選択中フラグ
    private bool redFilmFlag = false;             //赤フィルムが存在するかどうかのフラグ
    private List<GameObject> insertRedFilmList = new List<GameObject>();//挿んだ赤フィルムのリスト
    public float swipeJudgeDistance;

    // Use this for initialization
    private void Awake()
    {
        tf = transform;
        clipCollider = GetComponent<BoxCollider2D>();
        pinCollider = tf.GetChild(0).GetComponent<BoxCollider2D>();
        centerRightDivideX = (centerPosition.x + rightPosition.x) / 2;  //クリップ移動用
        centerLeftDivideX = (centerPosition.x + leftPosition.x) / 2;    //クリップ移動用
        swipeJudgeDistance = 3;

        Init();
    }

    // Update is called once per frame
    private void Update()
    {
        //コライダー上クリックで選択状態へ
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (clipCollider.OverlapPoint(mousePos) || pinCollider.OverlapPoint(mousePos))
            {
                selectFlag = true;
            }
        }
        //クリップ選択中なら移動
        if (selectFlag)
        {
            MoveClip();
        }
        //test
        if (Input.GetKeyDown(KeyCode.F))
        {
            FallAllFilm();
        }
    }

    //初期化処理(リセットでも呼ぶ)
    public void Init()
    {
        switch (InitPosType)
        {
            case 0:
                tf.position = centerPosition;
                break;

            case 1:
                tf.position = rightPosition;
                break;

            case 2:
                tf.position = leftPosition;
                break;

            default:
                tf.position = centerPosition;
                break;
        }

        nowHP = maxHP;
        foreach (var film in insertFilmList)
        {
            DestroyObject(film);
        }
        insertFilmList.Clear();
    }

    //クリップ移動
    private void MoveClip()
    {
        if (centerLeftDivideX > Camera.main.ScreenToWorldPoint(Input.mousePosition).x)
        {
            tf.position = leftPosition;
        }
        else if (centerRightDivideX > Camera.main.ScreenToWorldPoint(Input.mousePosition).x)
        {
            tf.position = centerPosition;
        }
        else
        {
            tf.position = rightPosition;
        }
        //マウス左ボタンが離れたら移動完了して選択フラグをfalseに
        if (Input.GetMouseButtonUp(0))
        {
            selectFlag = false;
        }
        //落とす処理 クリップから一定以上下にスワイプで落として選択中フラグをfalseに
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y + swipeJudgeDistance < tf.position.y)
        {
            FallAllFilm();
            selectFlag = false;
        }
    }

    //フィルムを挟む
    public void InsertFilm(GameObject film)
    {
        //フィルムを子オブジェクト化
        film.transform.parent = tf;
        //一応リストにも追加
        insertFilmList.Add(film);
        //フィルムに挿入後フィルムのスクリプト追加
        film.transform.gameObject.AddComponent<InsertFilm>();
        //HP変化
        nowHP = maxHP - insertFilmList.Count;

        CheckRedFilm(); //赤フィルムのチェック
    }

    //フィルムが落ちた時にフィルムから呼ぶ
    public void FallFilm(GameObject film)
    {
        film.transform.parent = null;
        insertFilmList.Remove(film);
        nowHP = maxHP - insertFilmList.Count;

        CheckRedFilm(); //赤フィルムのチェック
    }

    //全フィルムを落とす
    public void FallAllFilm()
    {
        var filmList = GetComponentsInChildren<InsertFilm>();
        foreach (var film in filmList)
        {
            film.FallFilm();
        }
        nowHP = maxHP;

        CheckRedFilm(); //赤フィルムのチェック
    }

    //現在の赤フィルムの状態を調べる
    private void CheckRedFilm()
    {
        insertRedFilmList.Clear();
        var redFilmScripts = GetComponentsInChildren<RedFilm>();
        print(redFilmScripts.Length);
        if (redFilmScripts.Length == 0)
        {
            redFilmFlag = false;
        }
        else
        {
            redFilmFlag = true;
            foreach (var red in redFilmScripts)
            {
                insertRedFilmList.Add(red.gameObject);
            }
        }
    }

    public bool GetFixedFlag()
    {
        //子に固定完了してないものがあればfalseを返す
        foreach (var film in tf.GetComponentsInChildren<InsertFilm>())
        {
            if (!film.GetFixedFlag())
            {
                return false;
            }
        }

        return true;
    }

    public bool GetRedFilmFlag()
    {
        return redFilmFlag;
    }

    public List<GameObject> GetRedFilms()
    {
        return insertRedFilmList;
    }

    public float GetHP()
    {
        return nowHP;
    }

    public void HPAdd()
    {
        print("ClipADD");
        nowHP = maxHP;
        print(nowHP);
        print(maxHP);
    }
}