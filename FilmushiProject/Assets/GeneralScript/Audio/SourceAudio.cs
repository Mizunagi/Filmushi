using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CustomAudioClip
{
    //オーディオクリップ
    public AudioClip Clip;

    //ボリューム(0.0f~1.0f)
    public float Vol;

    public CustomAudioClip(AudioClip clip, float vol)
    {
        this.Clip = clip;
        this.Vol = vol;
    }
}

public class SourceAudio : MonoBehaviour
{
    //コンポ―ネントを使用するオブジェクトに登録すると
    //size欄が表示されるのでそこにオブジェクトが使用するサウンドの数を入力
    //するとAudioClipの欄が出現するのでそこにオブジェクトが使用する
    //サウンドデータを登録
    public CustomAudioClip[] m_Audio;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    //再生には配列番号が引数にあるのでenum等で番号を控えておくと良い。

    //BGM再生(音の重複は不可能)
    //(使用サウンド配列番号,ループフラグ)
    public void PlayBGM(int playNo, bool loop)
    {
        if (this.CheckExist(playNo))
        {
            AudioSystem.Instance.PlayBGM(m_Audio[playNo].Clip, m_Audio[playNo].Vol, loop);
        }
    }

    //BGM一時停止&再開
    public void PauseBGM()
    {
        AudioSystem.Instance.PauseBGM();
    }

    //BGM停止
    public void StopBGM()
    {
        AudioSystem.Instance.StopBGM();
    }

    //SE再生(重複可能)
    //(使用サウンド配列番号)
    public void PlaySE(int playNo)
    {
        if (this.CheckExist(playNo))
        {
            AudioSystem.Instance.PlaySE(m_Audio[playNo].Clip, m_Audio[playNo].Vol);
        }
    }

    private bool CheckExist(int No)
    {
        if (m_Audio == null)
        {
            Debug.LogWarning("サウンドのインスタンス１つもありません。");
            return false;
        }
        if (No < m_Audio.Length)
        {
            if (m_Audio[No].Clip == null)
            {
                Debug.LogWarning("クリップにサウンドがありません");
                return false;
            }
        }
        else
        {
            Debug.LogWarning("登録されていない音源です。");
            return false;
        }
        return true;
    }
}