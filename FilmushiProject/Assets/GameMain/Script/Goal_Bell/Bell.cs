using UnityEngine;

public class Bell : MonoBehaviour
{
    private GoalBell goalscript;
    private GameObject particle;
    private SpriteRenderer bellTexture;
    private bool hitFlag;
    public float vanishTime;
    private float timeCount;

    private enum AudioList
    {
        AUDIO_BELL,
        AUDIO_MAX
    }

    private SourceAudio sourceAudio;
    private CustomAudioClip[] audioClip;

    // Use this for initialization
    private void Start()
    {
        bellTexture = transform.GetComponentInChildren<SpriteRenderer>();
        hitFlag = false;
        timeCount = 0;

        this.audioClip = new CustomAudioClip[(int)AudioList.AUDIO_MAX];
        this.audioClip[(int)AudioList.AUDIO_BELL].Clip = Resources.Load("Audio/SE/Bell", typeof(AudioClip)) as AudioClip;
        this.audioClip[(int)AudioList.AUDIO_BELL].Vol = 1.0f;

        this.sourceAudio = this.gameObject.AddComponent<SourceAudio>();
        this.sourceAudio.m_Audio = this.audioClip;
    }

    // Update is called once per frame
    private void Update()
    {
        if (hitFlag)
        {
            timeCount += Time.deltaTime;
            bellTexture.color = new Color(1, 1, 1, 1 - (timeCount / vanishTime));
            if (timeCount >= vanishTime)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //既に当たっていたらリターン
        if (hitFlag)
        {
            return;
        }
        if (other.gameObject.tag == "Player" && !other.isTrigger)
        {
            goalscript = transform.parent.GetComponent<GoalBell>();
            goalscript.CountSub();

            this.sourceAudio.PlaySE((int)AudioList.AUDIO_BELL);

            particle = Instantiate(GameObject.Find("EffectManager").GetComponent<EffectManager>().bellSparkleEffect);
            particle.transform.position = transform.position + Vector3.back * 3.0f + Vector3.down * 0.22f;
            particle.GetComponent<ParticleSystem>().Play();
            particle.transform.parent = transform;

            hitFlag = true;
        }
    }
}