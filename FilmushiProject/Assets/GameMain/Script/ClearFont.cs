using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearFont : MonoBehaviour
{
    public GameObject GuideObj;//タップガイドプレハブ
    public float Speed;//クリア文字速度
    public float Stopypos;//クリア文字止まる位置
    public Vector3 Guidepos;//ガイドのポジション
    private bool OnGuideFlg;//ガイド出てるよフラグ
    private GameObject obj;//ガイドのオブジェクト

    //クリアデータ取得用
    private AcornManager acornmanage;

    private ViewTimeMainScene timer;

    private SaveData cleardata;

    // Use this for initialization
    private void Start()
    {
        OnGuideFlg = false;

        this.acornmanage = GameObject.Find("AcornManager").GetComponent<AcornManager>();
        this.timer = GameObject.Find("ViewTimeMain").GetComponent<ViewTimeMainScene>();
    }

    // Update is called once per frame
    private void Update()
    {
        StageManager stageMG = transform.GetComponentInParent<StageManager>();
        Vector3 velocity = new Vector3(0, -1, 0);

        if (stageMG.GetSTAGESTA == 3)//クリア演出
        {
            //動かないのでコメントアウト
            //if (transform.position.y > Stopypos)//ストップ位置より上
            //{
            //    transform.position += velocity * Speed * Time.deltaTime;//落ちる

            //}
            //else
            //{
            if (!OnGuideFlg)//ガイドが出ていない
            {
                OnGuideFlg = true;
                //ガイド表示
                obj = Instantiate(GuideObj, Guidepos, Quaternion.identity) as GameObject;
                obj.transform.parent = transform;
            }
            //}
        }

        if (OnGuideFlg)
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.cleardata.stageName = SceneManager.GetActiveScene().name;
                this.cleardata.cleartime = this.timer.GetTime();
                this.cleardata.maxAcorns = this.acornmanage.GetMAXAcorn;
                this.cleardata.cntAcorns = this.acornmanage.GetCntAcorn;
                SaveDataManager.Instance.temp = cleardata;

                //ガイド表示タップ（遷移）
                print("tatti");
                SceneManager.LoadScene("ResultScene");
            }
        }
    }
}