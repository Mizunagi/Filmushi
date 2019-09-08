using UnityEngine;

public class Goal : MonoBehaviour
{
    private GoalBell goalbell;
    private PauseManager pausemanager;
    private GameObject timecouter;
    private GameObject particle;

    // Use this for initialization
    private void Start()
    {
        this.pausemanager = GameObject.Find("PauseManager").GetComponent<PauseManager>();
        this.timecouter = GameObject.Find("StageManager").transform.Find("ViewTimeMain").gameObject;
        this.particle = Instantiate(GameObject.Find("EffectManager").GetComponent<EffectManager>().wingSparkleEffect);
        this.particle.transform.position = transform.position;
        this.particle.GetComponent<ParticleSystem>().Play();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.pausemanager.Pause(this.timecouter, PauseManager.MONO);
            print("GoalHit");
            goalbell = transform.GetComponentInParent<GoalBell>();
            goalbell.SendGameStageGOAL();
            GetComponent<Renderer>().enabled = false;
            particle.GetComponent<ParticleSystem>().Stop();

            GameObject.Find("AudioObj").GetComponent<AudioObj>().PlayGoalBGM();
        }
    }
}