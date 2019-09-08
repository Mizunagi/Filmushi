using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtension
{
    //GameObjectの拡張メソッドを記述

    //全ての子（孫以下含む）オブジェクトを取得
    public static List<GameObject> GetAllChildren(this GameObject obj)
    {
        List<GameObject> allChildren = new List<GameObject>();  //全ての子要素を格納するリスト
        GetChildren(obj, allChildren);

        return allChildren;
    }

    //全ての子オブジェクトを取得する
    public static void GetChildren(GameObject obj, List<GameObject> allChildren)
    {
        //子が0なら終了
        if (obj.transform.childCount == 0)
        {
            return;
        }

        var children = obj.GetComponentsInChildren<Transform>();    //直接の子を全て取得

        foreach (var child in children)
        {
            if (child.gameObject == obj)
            {
                continue;
            }
            allChildren.Add(child.gameObject);  //子をリストに追加
            //if (child.transform.childCount > 0) //孫探査
            //{
            //    GetChildren(child.gameObject, allChildren);
            //}
        }
    }
}