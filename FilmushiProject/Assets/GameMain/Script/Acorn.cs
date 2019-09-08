using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acorn : MonoBehaviour
{
    private AcornManager acornMG;
    private int acornID;
    private Acorn my;
    private GameObject particle;

    private enum AudioList
    {
        AUDIO_ACORN,
        AUDIO_MAX
    }

    private SourceAudio sourceAudio;
    private CustomAudioClip[] audioClip;

    // Use this for initialization
    private void Start()
    {
        particle = GameObject.Find("EffectManager").GetComponent<EffectManager>().acornPickUpEffect;

        this.audioClip = new CustomAudioClip[(int)AudioList.AUDIO_MAX];
        this.audioClip[(int)AudioList.AUDIO_ACORN].Clip = Resources.Load("Audio/SE/Acorn", typeof(AudioClip)) as AudioClip;
        this.audioClip[(int)AudioList.AUDIO_ACORN].Vol = 0.6f;

        this.sourceAudio = this.gameObject.AddComponent<SourceAudio>();
        this.sourceAudio.m_Audio = this.audioClip;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !collision.isTrigger)
        {
            print("ドングリhit");

            acornMG = transform.parent.GetComponent<AcornManager>();
            print("DeleteacornID" + acornID);
            acornMG.SendDestroyAcorn(acornID);

            var _particle = Instantiate(particle);
            _particle.transform.position = transform.position;
            _particle.GetComponent<ParticleSystem>().Play();

            this.sourceAudio.PlaySE((int)AudioList.AUDIO_ACORN);

            Destroy(gameObject);
        }
    }

    public void SetAcornID(int ID)
    {
        acornID = ID;
        print("acornID" + acornID);
    }

    public int GetAcornID()
    {
        return acornID;
    }
}