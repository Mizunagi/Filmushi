using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationPage : MonoBehaviour
{
    public float pageWidth;
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 nowPos;
    public float moveTotalTime;
    private float speed;
    private bool moveFinishFlag;
    private float time;

    // Use this for initialization
    private void Start()
    {
        moveFinishFlag = true;
        time = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (moveFinishFlag)
        {
            return;
        }
        time += Time.deltaTime;

        if (moveTotalTime > time)
        {
            nowPos.x += speed * Time.deltaTime;
            transform.position = nowPos;
        }
        else
        {
            nowPos = endPos;
            transform.position = nowPos;
            moveFinishFlag = true;
        }
    }

    //右のページへ遷移する（つまりページ自体は左へ移動する
    public void MoveRight()
    {
        startPos = transform.position;
        nowPos = startPos;
        endPos = startPos + Vector3.left * pageWidth;
        time = 0;
        speed = (endPos.x - startPos.x) / moveTotalTime;
        moveFinishFlag = false;
    }

    //左のページへ遷移する（つまりページ自体は右へ移動する
    public void MoveLeft()
    {
        startPos = transform.position;
        nowPos = startPos;
        endPos = startPos + Vector3.right * pageWidth;
        time = 0;
        speed = (endPos.x - startPos.x) / moveTotalTime;
        moveFinishFlag = false;
    }

    public bool GetMoveFinishFlag()
    {
        return moveFinishFlag;
    }

    public void SetStartPage(int pageNum)
    {
        transform.position = Vector3.zero;
        switch (pageNum)
        {
            case 1:
                transform.Translate(Vector3.left * pageWidth * 0);
                break;

            case 2:
                transform.Translate(Vector3.left * pageWidth * 1);
                break;

            case 3:
                transform.Translate(Vector3.left * pageWidth * 2);
                break;

            case 4:
                transform.Translate(Vector3.left * pageWidth * 3);
                break;

            case 5:
                transform.Translate(Vector3.left * pageWidth * 4);
                break;

            default:
                break;
        }
    }
}