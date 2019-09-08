using UnityEngine;

public class AudioSystem : Singleton<AudioSystem>
{
    //BGM用AudioSource
    private AudioSource bgm;

    //SE用AudioSource
    private AudioSource se;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void PlayBGM(AudioClip playAudio, float vol, bool loop)
    {
        if (bgm == null)
        {
            bgm = this.gameObject.AddComponent<AudioSource>();
            bgm.playOnAwake = false;
        }
        if (!bgm.isPlaying)
        {
            //サウンドクリップ登録
            bgm.clip = playAudio;
            bgm.volume = vol;
            //ループフラグ
            bgm.loop = loop;
            //再生(重複不可能)
            bgm.Play();
            Debug.Log("Play:BGM -> " + bgm.clip.name);
        }
        else
        {
            Debug.LogWarning("再生中です。 -> " + bgm.clip.name);
        }
    }

    public void PauseBGM()
    {
        if (bgm.clip != null)
        {
            if (bgm.isPlaying)
            {
                //ポーズ
                bgm.Pause();
                Debug.Log("Pause:BGM -> " + bgm.clip.name);
            }
            else
            {
                //ポーズ解除
                bgm.UnPause();
                Debug.Log("Resume:BGM -> " + bgm.clip.name);
            }
        }
        else
        {
            Debug.LogWarning("クリップが登録させていません。");
        }
    }

    public void StopBGM()
    {
        if (bgm.clip != null)
            if (bgm.isPlaying)
            {
                //停止
                bgm.Stop();
                //サウンドクリップ登録解除
                bgm.clip = null;
                Debug.Log("Stop:BGM");
            }
    }

    public void PlaySE(AudioClip playAudio, float vol)
    {
        if (se == null)
        {
            se = this.gameObject.AddComponent<AudioSource>();
            se.playOnAwake = false;
        }
        se.volume = vol;
        //ワンショット再生(重複可能)
        se.PlayOneShot(playAudio);
        Debug.Log("Play:SE -> " + playAudio.name);
        return;
    }
}