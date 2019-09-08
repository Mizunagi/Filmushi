using System.Collections.Generic;
using UnityEngine;

public class FilmHolder : MonoBehaviour
{
    private GameObject filmSet;     //このホルダーに入れるフィルムのセット
    private GameObject stockFilm;   //このホルダーに現在入っている（見えている）フィルム
    private Transform tf;
    private Transform filmTransform;
    private BoxCollider2D boxCollider2d;
    public List<int> filmIndexTabel;    //フィルムを出現させる際のテーブル
    private int nowIndex;                       //テーブル操作のためのインデックス
    public bool randomSetFlag;          //trueならランダムにフィルム取得
    private bool selectFlag;                    //このホルダーのフィルムを選択中かどうか
    private FilmManager filmManager;

    private enum AudioList
    {
        AUDIO_PIC,
        AUDIO_SET,
        AUDIO_MAX
    }

    private SourceAudio sourceAudio;
    private CustomAudioClip[] audioClip;

    // Use this for initialization
    private void Start()
    {
        tf = transform;
        filmSet = tf.GetChild(0).gameObject;    //フィルムセットを取得
        boxCollider2d = GetComponent<BoxCollider2D>();

        Init();

        filmManager = GameObject.Find("FilmManager").GetComponent<FilmManager>();

        this.audioClip = new CustomAudioClip[(int)AudioList.AUDIO_MAX];
        this.audioClip[(int)AudioList.AUDIO_PIC].Clip = Resources.Load("Audio/SE/Film_Pic", typeof(AudioClip)) as AudioClip;
        this.audioClip[(int)AudioList.AUDIO_PIC].Vol = 1.0f;
        this.audioClip[(int)AudioList.AUDIO_SET].Clip = Resources.Load("Audio/SE/Film_Set", typeof(AudioClip)) as AudioClip;
        this.audioClip[(int)AudioList.AUDIO_SET].Vol = 1.0f;

        sourceAudio = this.gameObject.AddComponent<SourceAudio>();
        sourceAudio.m_Audio = this.audioClip;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (boxCollider2d.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
            {
                SelectFilm();
            }
        }
    }

    //初期化処理（リセットでも呼ぶ）
    private void Init()
    {
        //既にストックがある場合破棄
        if (stockFilm != null)
        {
            Destroy(stockFilm);
        }

        if (randomSetFlag)
        {
            //ランダムなフィルムを取得
            stockFilm = Instantiate(filmSet.GetComponent<FilmSet>().GetRandomFilm());
        }
        else
        {
            //配列の先頭フィルムを取得
            nowIndex = 0;
            stockFilm = Instantiate(filmSet.GetComponent<FilmSet>().GetFilm(filmIndexTabel[nowIndex]));
        }
        //生成したフィルムを子にする
        filmTransform = stockFilm.transform;
        filmTransform.position = tf.position;
        var scale = tf.localScale;
        scale.x *= tf.parent.localScale.x;
        scale.y *= tf.parent.localScale.y;
        filmTransform.localScale = scale;
        filmTransform.parent = tf;
    }

    //フィルムを挟んだ時
    public void InsertFilm()
    {
        this.sourceAudio.PlaySE((int)AudioList.AUDIO_SET);
        selectFlag = false;
        //子フィルムとの親子関係を切る
        filmTransform.parent = null;
        //次のフィルムを取得
        SetNewFilm();
    }

    //フィルムを挟まずキャンセルした時
    public void CancelFilm()
    {
        selectFlag = false;
        //子フィルムを元の位置と座標に戻す
        filmTransform.parent = null;    //一旦親子関係を切って座標とサイズを調整する
        filmTransform.position = tf.position;
        var scale = tf.localScale;
        scale.x *= tf.parent.localScale.x;
        scale.y *= tf.parent.localScale.y;
        filmTransform.localScale = scale;
        filmTransform.parent = tf;
    }

    //新しいフィルムをホルダーにセットする
    private void SetNewFilm()
    {
        if (randomSetFlag)
        {
            stockFilm = Instantiate(filmSet.GetComponent<FilmSet>().GetRandomFilm());
        }
        else
        {
            nowIndex += 1;
            if (nowIndex >= filmIndexTabel.Count)
            {
                nowIndex = 0;
            }
            stockFilm = Instantiate(filmSet.GetComponent<FilmSet>().GetFilm(filmIndexTabel[nowIndex]));
        }
        filmTransform = stockFilm.transform;
        filmTransform.position = tf.position;
        var scale = tf.localScale;
        scale.x *= tf.parent.localScale.x;
        scale.y *= tf.parent.localScale.y;
        filmTransform.localScale = scale;
        filmTransform.parent = tf;
    }

    //ホルダー上でドラッグでフィルムを選択
    private void SelectFilm()
    {
        this.sourceAudio.PlaySE((int)AudioList.AUDIO_PIC);
        selectFlag = filmManager.SelectFilm(stockFilm);

        //stockFilm.GetComponent<SelectFilm>().SelectThisFilm();
        //filmTransform.position = defaultFilmSetPosition;
        //filmTransform.localScale = filmHolder.GetComponent<FilmSet>().GetFilm(0).GetComponent<Transform>().localScale;
    }
}