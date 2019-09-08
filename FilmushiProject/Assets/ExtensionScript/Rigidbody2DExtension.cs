using UnityEngine;

public static class Rigidbody2DExtension
{
    //rigidbody2Dの拡張メソッドを記述
    //一時停止
    public static void Pause(this Rigidbody2D rigidbody2D)
    {
        if (!rigidbody2D.simulated)
        {
            return;
        }
        //obj.AddComponent<Velocity2DTemp>().SetVelocity(rigidbody2D);
        rigidbody2D.simulated = false;
    }

    //一時停止解除
    public static void Resume(this Rigidbody2D rigidbody2D)
    {
        if (rigidbody2D.simulated)
        {
            return;
        }
        rigidbody2D.simulated = true;
        /*
        Velocity2DTemp tmp = obj.GetComponent<Velocity2DTemp>();
        if (!rigidbody2D.isKinematic || tmp==null)
        {
            return;
        }

        rigidbody2D.velocity = tmp.GetVelocityTemp();
        rigidbody2D.angularVelocity = tmp.GetAngularVelocityTemp();
        rigidbody2D.isKinematic = false;

        GameObject.Destroy(tmp);
        */
    }
}