using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEffectManager : MonoBehaviour {

    public GameObject tapEffect;
    private ParticleSystem tapEffectParticle;
    public GameObject swipeEffect;
    private Transform swipeEffectTransform;

    private bool touchFlag;
    private Vector2 touchPosition;
    private float touchCount;


	// Use this for initialization
	void Start () {
        tapEffectParticle = Instantiate(tapEffect).GetComponent<ParticleSystem>();
        tapEffectParticle.Stop();
        

        touchFlag = false;
        touchCount = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchFlag = true;
            tapEffectParticle.transform.position = touchPosition;
            tapEffectParticle.transform.Translate(Vector3.back * 5);
            tapEffectParticle.Play();

            swipeEffectTransform = Instantiate(swipeEffect).transform;
        }

        if (touchFlag)
        {
            Vector2 nowTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchCount += Time.deltaTime;
            

            if (touchPosition != nowTouchPosition)
            {
                if (swipeEffectTransform == null)
                {
                    touchFlag = false;
                    return;
                }
                swipeEffectTransform.position = nowTouchPosition;
                swipeEffectTransform.Translate(Vector3.back * 5);
            }
            if (Input.GetMouseButtonUp(0))
            {
                Destroy(swipeEffectTransform.gameObject);
                swipeEffectTransform = null;
                touchFlag = false;
            }
        }
	}
}
