using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spa : enemy
{
    public Material mat;
    public Texture dead_tex;
    public Texture nomal_tex;
    public float spaSP = 0.0f;
    public float spafallspeed = 0.1f;
    public LayerMask Branch;
    private int cnt_bullet;
    private bool hit;
    public GameObject spaballe;
    private float nowtime;
    private float Stime;
    public float shottime;
    public float tamahaba = 2.5f;
    public float tamatakasa = 0.8f;

    // Use this for initialization
    private void Start()
    {
        m_Animator = this.GetComponent<Animator>();
        m_Collision = this.GetComponent<BoxCollider2D>();
        m_Transform = this.GetComponent<Transform>();
        m_PlayerCol = GameObject.Find("Player").GetComponent<BoxCollider2D>();
        m_NowPos = m_InitPos = m_Transform.position;
        syokikaku = m_Transform.localScale;
        m_Mode = 0;
        Stime = nowtime = Time.deltaTime;
        m_Speed = spaSP;
        m_Mode = 0;
        mat.SetTexture("_MainTex", nomal_tex);
    }

    // Update is called once per frame
    private void Update()
    {
        hit = false;

        GameObject[] colBranch = GameObject.FindGameObjectsWithTag("Branch");
        Vector2 kaku;
        if (m_Collision.transform.localScale.x < 0.0f)
        {
            //kaku = (Quaternion.Euler(Vector2.right) * m_Collision.transform.localRotation).eulerAngles;
            kaku = Vector2.right;
        }
        else
        {
            //kaku = (Quaternion.Euler(Vector2.left) * m_Collision.transform.localRotation).eulerAngles;
            kaku = Vector2.left;
        }
        ////////////////////////////////////////////////////////////
        Vector2 foot2;
        foot2.x = m_Collision.bounds.center.x;
        foot2.y = m_Collision.bounds.min.y;
        Vector2 mu = (Quaternion.Euler(Vector2.down) * m_Collision.transform.localRotation).eulerAngles;
        RaycastHit2D c = Physics2D.Raycast(foot2, mu, 0.0001f, Branch);

        foreach (GameObject obj in colBranch)
        {
            if (!mb_Death /*&& (c.collider.tag == obj.tag)*/&& c == true)
            {
                ///////////////////////dev.cs SetR()/////////////////////////////

                Vector3 setr;
                setr = c.transform.localRotation.eulerAngles;
                setr.y = 180;
                setr *= -1;
                base.SetRotation(setr);

                ////////////////////////////////////////////////////////
                m_Speed = spaSP;
                m_Mode = 0;
                //m_Animator.SetBool("jimen", true);
            }
            else
            {
                m_Speed = spafallspeed;
                if (m_Mode != 1)
                {
                    m_Mode = 2;
                    //  m_Animator.SetBool("jimen", false);
                }
            }
        }

        if (m_HitTime >= 0.0f)
        { m_HitTime -= 1.0f; }
        this.EnemyUpdate();
        base.SetHit(hit);
        if (m_Mode == 1)
        {
            m_Speed = spafallspeed;
            //base.SetMode(1);
            mb_Death = true;
            mat.SetTexture("_MainTex", dead_tex);
        }
        m_Animator.SetBool("spadeath", mb_Death);
        if (Input.GetAxis("Horizontal") != 0)
        {
            //    resetpos(); }
            //if (Input.GetAxis("Jump") != 0) {
            shot();
        }
    }

    public void SHit()
    {
        if (m_HitTime <= 0.0f)
        {
            hit = true;
            m_HitTime = 50.0f;
        }
    }

    public void resetpos()
    {
        m_Speed = spaSP;
        mat.SetTexture("_MainTex", nomal_tex);

        mb_Death = false;
        base.resetpos();
    }

    private void shot()
    {
        Vector3 workpos = new Vector3();
        GameObject obj2;
        float mu = tamahaba;
        if (m_Transform.localScale.x < 0) { mu *= -1; }

        workpos.Set(m_Transform.position.x - mu, m_Transform.position.y + tamatakasa, m_Transform.position.z);

        obj2 = Instantiate(spaballe, workpos, m_Transform.localRotation) as GameObject;
        obj2.transform.parent = transform;
        m_Animator.SetTrigger("shot");
    }

    public void gorugo()
    {
        base.SetHit(true);
        //print("ζ*'ヮ')ζ＜おれのうしろにたつな");
    }

    public void cntdn()
    {
        nowtime += Time.deltaTime;
        if (nowtime >= shottime) { shot(); nowtime = 0.0f; }
    }

    ////////////////////////////////////////////
}