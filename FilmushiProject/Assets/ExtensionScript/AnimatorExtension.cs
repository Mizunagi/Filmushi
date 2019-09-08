using UnityEngine;

public static class AnimatorExtension
{
    //Animator拡張メソッドを記述
    public static void Pause(this Animator anime)
    {
        anime.speed = 0;
    }

    public static void Resume(this Animator anime)
    {
        anime.speed = 1;
    }
}