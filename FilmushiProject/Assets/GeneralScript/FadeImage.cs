using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour
{
    public Color startColor;
    public Color endColor;
    public float fadeTotalTime;
    private Color nowColor;
    private Color speed;
    private float fadeTime;
    private bool startFlag;
    private bool endFlag;
    private Image img;

    // Use this for initialization
    private void Start()
    {
        startFlag = false;
        endFlag = false;
        fadeTime = 0;
        //とりあえずImageでもSpriteRendererでも色変える
        img = GetComponent<Image>();
        img.color = startColor;

        nowColor = startColor;
        speed = (endColor - startColor) / fadeTotalTime;
    }

    // Update is called once per frame
    private void Update()
    {
        if (startFlag)
        {
            if (fadeTotalTime > fadeTime)
            {
                fadeTime += Time.deltaTime;
                nowColor += speed * Time.deltaTime;
                img.color = nowColor;
            }
            else
            {
                nowColor = endColor;
                img.color = nowColor;
                endFlag = true;
            }
        }
    }

    public void FadeStart()
    {
        startFlag = true;
    }
    public bool GetStartFlag()
    {
        return startFlag;
    }
    public bool GetEndFlag()
    {
        return endFlag;
    }
}