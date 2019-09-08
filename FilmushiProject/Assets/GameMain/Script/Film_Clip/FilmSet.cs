using System.Collections.Generic;
using UnityEngine;

public class FilmSet : MonoBehaviour
{
    private Transform tf;
    private List<GameObject> filmList = new List<GameObject>();

    // Use this for initialization
    private void Awake()
    {
        tf = transform;
        for (int i = 0; i < tf.childCount; i++)
        {
            filmList.Add(tf.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    //指定フィルムを取得
    public GameObject GetFilm(int index)
    {
        //範囲外ならnullを返す
        if (index < 0 || index >= filmList.Count)
        {
            return null;
        }
        return filmList[index];
    }

    //ランダムなフィルムを取得
    public GameObject GetRandomFilm()
    {
        return filmList[Random.Range(0, filmList.Count)];
    }
}