using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Collider2DExtension{

    //Collider2Dの拡張メソッドを記述
    //一時停止
    public static void Pause(this Collider2D collider2d)
    {
        collider2d.enabled = false;
    }

    //一時停止解除
    public static void Resume(this Collider2D collider2d)
    {
        collider2d.enabled = true;
    }
}
