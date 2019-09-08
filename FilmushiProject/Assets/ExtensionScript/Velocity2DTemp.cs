using UnityEngine;

public class Velocity2DTemp : MonoBehaviour
{
    //Rigidbody2Dの一時停止時速度保管用クラス
    private Vector2 velocityTemp;

    private float angularVelocityTemp;

    public Vector2 GetVelocityTemp()
    {
        return velocityTemp;
    }

    public float GetAngularVelocityTemp()
    {
        return angularVelocityTemp;
    }

    public void SetVelocity(Rigidbody2D rb2d)
    {
        velocityTemp = rb2d.velocity;
        angularVelocityTemp = rb2d.angularVelocity;
    }
}