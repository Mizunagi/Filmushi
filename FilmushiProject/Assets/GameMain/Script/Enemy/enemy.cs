using UnityEngine;

public abstract class enemy : MonoBehaviour
{
    protected bool mb_Hit = false;
    protected float m_Speed = 0.00f;//0.00fを変えたら速さが変わるがUnityでの1が何pxかわからん
    protected int m_Mode = 0;
    protected Transform m_Transform;
    protected Vector3 m_InitPos;
    protected Vector3 syokikaku;
    protected Animator m_Animator;
    protected BoxCollider2D m_Collision;
    protected bool mb_Death = false;
    protected float m_HitTime = 0.0f;
    protected BoxCollider2D m_PlayerCol;
    protected Vector3 m_NowPos;                 //現在位置

    protected void EnemyUpdate()
    {
        switch (m_Mode)
        {
            case 0://移動
                {
                    //Vector2 foot;

                    //if (m_Transform.localScale.x > 0.0f) { foot.x = col.bounds.min.x; foot.y = col.bounds.min.y; }
                    //else { foot.x = col.bounds.max.x; foot.y = col.bounds.min.y; }
                    //if (!(Physics2D.Raycast(foot, -Vector2.up, 0.001f, BackGround))) { SetHit(true); print("ζ*'ヮ')ζ＜出ちゃダメかなーって"); }

                    Vector3 vec = m_Transform.localRotation.eulerAngles;
                    float muki;
                    if (m_Transform.localScale.x >= 0.0f) { muki = 1.0f; } else { muki = -1.0f; }
                    m_NowPos.x -= muki * Mathf.Cos((vec.z) * Mathf.PI / 180) * m_Speed;//初期画像が左を向いてたのでマイナス
                    m_NowPos.y += muki * Mathf.Sin((vec.z) * Mathf.PI / 180) * m_Speed;

                    m_Transform.position = m_NowPos;

                    //何か振り向くことがあったらhitにtrue
                    if (mb_Hit)
                    { Invert(); }
                    break;
                }
            case 1://死
                {
                    m_NowPos.y -= m_Speed;
                    m_Transform.position = m_NowPos;
                    m_Collision.enabled = false;
                    break;
                }
            case 2://落下
                {
                    m_NowPos.y -= m_Speed;
                    m_Transform.position = m_NowPos;
                    break;
                }
        }
        if (m_Mode != 1)
        {
            GameObject[] colLeaf = GameObject.FindGameObjectsWithTag("Leaf");
            Vector2 foot2;
            foot2.x = m_Collision.bounds.min.x;
            foot2.y = m_Collision.bounds.max.y;
            RaycastHit2D hidariue = Physics2D.Raycast(foot2, Vector2.up, 0.001f);
            Vector2 foot3;
            foot3.x = m_Collision.bounds.max.x;
            foot3.y = m_Collision.bounds.max.y;
            RaycastHit2D migiue = Physics2D.Raycast(foot3, Vector2.up, 0.001f);

            foreach (GameObject obj in colLeaf)
            {
                Leaf leaf = obj.GetComponent<Leaf>();
                if (leaf.GetFallFlag())
                {
                    if (migiue && (migiue.collider.tag == obj.tag))
                    {
                        //m_Mode = 1;
                    }
                    if (hidariue && (hidariue.collider.tag == obj.tag))
                    {
                        //m_Mode = 1;
                    }
                }
                else
                {
                    if ((hidariue || migiue) && (migiue.collider.tag == obj.tag))
                    {
                        SetHit(true);
                    }
                }
            }
        }
    }

    //左右反転
    private void Invert()
    {
        Vector3 _nowScale = m_Transform.localScale;
        _nowScale.x *= -1;
        m_Transform.localScale = _nowScale;
    }

    public void SetHit(bool h)
    {
        mb_Hit = h;
    }

    public void SetRotation(Vector3 v)
    {
        m_Transform.localRotation = Quaternion.Euler(v);
    }

    public int GetMode()
    {
        return m_Mode;
    }

    public void resetpos()
    {
        m_Transform.position = m_InitPos;
        m_Transform.localScale = syokikaku;
        m_NowPos = m_Transform.position;
        m_Mode = 0;
        // print("リセットしたよーーーー！！＞(>ヮ<)");
    }
}