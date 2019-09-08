using UnityEngine;

public static class MonoBehaviourExtension
{
    //MonoBehaviourの拡張メソッドを記述
    //一時停止
    public static void Pause(this MonoBehaviour monoBehaviour)
    {
        monoBehaviour.enabled = false;
    }

    //一時停止解除
    public static void Resume(this MonoBehaviour monoBehaviour)
    {
        monoBehaviour.enabled = true;
    }
}