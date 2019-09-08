using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour {

    private Transform tf;
    private bool movieEndFlag;
    private Vector3 speed;
    public float accelerationF;
    private Vector3 accelerationV;
    private GameObject particle;
    private float rotation;

	// Use this for initialization
	void Start () {
        tf = transform;
        movieEndFlag = false;
        speed = Vector2.zero;
        particle = Instantiate(GameObject.Find("EffectManager").GetComponent<EffectManager>().wingFallingEffect);
        particle.transform.position = tf.position;
        particle.transform.parent = tf;
        particle.GetComponent<ParticleSystem>().Play();

        rotation = -15;
        accelerationV = new Vector3(-Mathf.Sin(rotation * Mathf.Deg2Rad) * accelerationF, Mathf.Cos(rotation * Mathf.Deg2Rad) * accelerationF, 0);
	}
	
	// Update is called once per frame
	void Update () {
        if (!movieEndFlag)
        {
            Vector3 nowPos=tf.position;
            nowPos += speed;
            tf.position = nowPos;

            speed += accelerationV;
        }
	}

    public bool GetMovieEndFlag()
    {
        return movieEndFlag;
    }
}
