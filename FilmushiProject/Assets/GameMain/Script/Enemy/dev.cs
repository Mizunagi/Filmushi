using UnityEngine;

public class dev : enemy
{
    public Material mat;
    public Texture devilmushi_dead;
    public Texture devilmushi_normal;
    public float devSP = 0.01f;
    public float devfallspeed = 0.01f;
    public LayerMask BackGround;
    public LayerMask Branch;

    private bool hit;

    private BoxCollider2D rightFoot;
    private BoxCollider2D leftFoot;
    private BoxCollider2D head;

    // Use this for initialization
    private void Start()
    {
        m_Animator = this.GetComponent<Animator>();
        m_Collision = this.GetComponent<BoxCollider2D>();
        m_Transform = this.GetComponent<Transform>();
        m_PlayerCol = GameObject.Find("Player").GetComponent<BoxCollider2D>();
        rightFoot = transform.Find("RightFoot").GetComponent<BoxCollider2D>();
        leftFoot = transform.Find("LeftFoot").GetComponent<BoxCollider2D>();
        head = transform.Find("devhead").GetComponent<BoxCollider2D>();

        m_NowPos = m_InitPos = m_Transform.position;
        syokikaku = m_Transform.localScale;
        m_Speed = devSP;
        m_Mode = 0;

        mat.SetTexture("_MainTex", devilmushi_normal);
    }

    // Update is called once per frame
    private void Update()
    {
        hit = false;
        Vector2 foot;
        //*********************************出ちゃダメなとき***************************************
        //if (base.transform.localScale.x > 0.0f) { foot.x = leftFoot.bounds.min.x; foot.y = leftFoot.bounds.min.y; }
        //else { foot.x = rightFoot.bounds.max.x; foot.y = rightFoot.bounds.min.y; }
        foot.x = leftFoot.bounds.center.x;
        foot.y = leftFoot.bounds.min.y;
        if (!(Physics2D.Raycast(foot, Vector2.down, 0.001f, BackGround)))
        {
            this.SHit();
            //    print("ζ*'ヮ')ζ＜出ちゃダメかなーって");
        }
        //************************************************************************
        bool kiasiba;
        bool kijimenn;
        bool akaasiba;
        bool akajimenn;
        GameObject[] colBranch = GameObject.FindGameObjectsWithTag("Branch");
        // Vector2 foot;
        Vector2 foot2;
        foot2.x = (leftFoot.bounds.center.x + rightFoot.bounds.center.x) / 2;
        foot2.y = (leftFoot.bounds.center.y + rightFoot.bounds.center.y) / 2;
        Vector2 mu = (Quaternion.Euler(Vector2.down) * m_Collision.transform.localRotation).eulerAngles;
        RaycastHit2D c = Physics2D.Raycast(foot2, mu, 0.0001f, Branch);

        Vector2 mae;
        Vector2 kaku;
        if (m_Collision.transform.localScale.x < 0.0f)
        {
            mae.x = m_Collision.bounds.max.x;
            mae.y = m_Collision.bounds.center.y;
            //kaku = (Quaternion.Euler(Vector2.right) * m_Collision.transform.localRotation).eulerAngles;
            kaku = Vector2.right;
        }
        else
        {
            mae.x = m_Collision.bounds.min.x;
            mae.y = m_Collision.bounds.center.y;
            //kaku = (Quaternion.Euler(Vector2.left) * m_Collision.transform.localRotation).eulerAngles;
            kaku = Vector2.left;
        }

        RaycastHit2D atari = Physics2D.Raycast(mae, kaku, 0.0001f);

        foreach (GameObject obj in colBranch)
        {
            //*******************************足場がないとき*****************************************
            if (!mb_Death /*&& (c.collider.tag == obj.tag)*/&& c == true)
            {
                Vector2 ata;
                //SetR(c);
                kaitenn();
                m_Speed = devSP;
                m_Mode = 0;
                ata.x = leftFoot.bounds.center.x;
                ata.y = leftFoot.bounds.min.y;

                // if (Physics2D.Raycast(ata, mu, 0.001f).collider.tag != "Branch" && Physics2D.Raycast(ata, mu, 0.001f).collider.tag != "RedBranch")
                if (!Physics2D.Raycast(ata, mu, 0.001f, Branch))
                {
                    kiasiba = true; ;
                    this.SHit();
                    //print("ζ*'ヮ')ζ＜足場がない！！");
                }
                m_Animator.SetBool("jimen", true);
            }
            else
            {
                m_Speed = devfallspeed;
                if (m_Mode != 1)
                {
                    kijimenn = true;
                    m_Mode = 2;
                    m_Animator.SetBool("jimen", false);
                }
            }
        }

        //****************************************************************************************
        Vector2 lefoo2;
        lefoo2.x = leftFoot.bounds.center.x;
        lefoo2.y = leftFoot.bounds.min.y;
        Vector2 mu2 = (Quaternion.Euler(Vector2.down) * m_Collision.transform.localRotation).eulerAngles;
        //Vector2 mu2 = Vector2.down;
        RaycastHit2D ci2 = Physics2D.Raycast(lefoo2, mu2, 0.001f);
        Vector2 rifoo3;
        rifoo3.x = rightFoot.bounds.center.x;
        rifoo3.y = rightFoot.bounds.min.y;
        Vector2 mu3 = (Quaternion.Euler(Vector2.down) * m_Collision.transform.localRotation).eulerAngles;
        //Vector2 mu3 = Vector2.down;
        RaycastHit2D ci3 = Physics2D.Raycast(rifoo3, mu3, 0.001f);

        if (ci2.collider.tag == "Branch" && ci3.collider.tag == "Branch")
        {
            Vector3 r = ci2.transform.localRotation.eulerAngles;
            Vector3 s = ci3.transform.localRotation.eulerAngles;
            //print(s.z - r.z);
            bool sa;
            if (m_Collision.transform.localScale.x < 0.0f)
            {
                if (Mathf.Abs(r.z - s.z) <= 90) { sa = true; } else { sa = false; }
            }
            else
            {
                if (Mathf.Abs(s.z - r.z) <= 90) { sa = true; } else { sa = false; }
            }

            if (sa)
            {
                //*********************************目の前が木のとき***************************************

                Vector2 ata;
                ata.x = head.bounds.center.x;
                ata.y = head.bounds.center.y;

                if (Physics2D.Raycast(ata, kaku, 0.001f, Branch))
                {
                    this.SHit(); //print("ζ*'ヮ')ζ＜きだ！！");
                }
                Debug.DrawRay(ata, kaku);
                //************************************************************************
            }
            else
            {
                kaitenn();
            }
        }

        //************************************目の前が敵なとき************************************
        GameObject[] colEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        atari = Physics2D.Raycast(mae, kaku, 0.0001f);

        foreach (GameObject obj in colEnemy)
        {
            if (!mb_Death && (atari.collider.tag == obj.tag) && (obj.name != this.name) && (atari.collider.name != this.name))
            {
                Vector2 ata;
                m_Speed = devSP;
                m_Mode = 0;
                ata.x = head.bounds.center.x;
                ata.y = head.bounds.min.y;

                if (Physics2D.Raycast(ata, kaku, 0.001f).collider.tag == "Enemy")
                {
                    this.SHit();
                }
                Debug.DrawRay(ata, kaku);
            }
            else
            {
            }
        }

        //************************************************************************
        //************************************目の前が敵なとき************************************
        GameObject[] colLeaf = GameObject.FindGameObjectsWithTag("Leaf");
        atari = Physics2D.Raycast(mae, kaku, 0.0001f);

        foreach (GameObject obj in colLeaf)
        {
            if (!mb_Death && (atari.collider.tag == obj.tag))
            {
                // print("ζ*'ヮ')ζ＜てき！?");
                // print(atari.collider.name);
                // print(obj.name);
                Vector2 ata;
                m_Speed = devSP;
                m_Mode = 0;
                ata.x = head.bounds.center.x;
                ata.y = head.bounds.min.y;

                if (Physics2D.Raycast(ata, kaku, 0.001f).collider.tag == "Leaf")
                {
                    this.SHit(); //print("ζ*'ヮ')ζ＜てきだ！！"); print(atari.collider.name); print(obj.name);
                }
                Debug.DrawRay(ata, kaku);
            }
            else
            {
            }
        }

        //************************************************************************
        //何か振り向くことがあったらhitにtrue
        //if (Input.GetMouseButtonDown(0)) { SetHit(); }//仮でクリックした時
        //0526時点での振り向く時
        //・画面端
        //・ステージオブジェクトの端
        //・足元の地形と角度が鋭角（直角も？）なステージオブジェクト
        //・フィルムの地形（ただし足元は無効）ステージオブジェクトの端と二回とらないよう注意
        //・他の敵（スパイダービッドの弾は何も起こらないらしい）
        if (m_HitTime >= 0.0f)
        { m_HitTime -= 1.0f; }
        base.SetHit(hit);
        this.EnemyUpdate();
        //if (Input.GetButtonDown("Jump"))
        if (m_Mode == 1)
        {
            m_Speed = devfallspeed;
            //base.SetMode(1);
            mb_Death = true;
            mat.SetTexture("_MainTex", devilmushi_dead);
        }
        m_Animator.SetBool("deathflag", mb_Death);

        if (Input.GetAxis("Horizontal") != 0) { resetpos(); }
    }

    public void SHit()
    {
        if (m_HitTime <= 0.0f)
        {
            hit = true;
            m_HitTime = 30.0f;
        }
    }

    public void resetpos()
    {
        m_Speed = devSP;
        mat.SetTexture("_MainTex", devilmushi_normal);
        mb_Death = false;
        base.resetpos();
    }

    private void SetR(Vector3 c)
    {
        Vector3 setr;
        setr = c;
        setr.y = 180;
        setr *= -1;
        base.SetRotation(setr);
    }

    private void kaitenn()
    {
        Vector2 foo;
        foo.x = m_Collision.bounds.center.x;
        foo.y = m_Collision.bounds.min.y;
        Vector2 mu = (Quaternion.Euler(Vector2.down) * m_Collision.transform.localRotation).eulerAngles;
        //Vector2 mu = Vector2.down;
        RaycastHit2D ci = Physics2D.Raycast(foo, mu, 0.001f);
        Vector2 foo2;
        foo2.x = leftFoot.bounds.center.x;
        foo2.y = leftFoot.bounds.min.y;
        Vector2 mu2 = (Quaternion.Euler(Vector2.down) * m_Collision.transform.localRotation).eulerAngles;
        //Vector2 mu2 = Vector2.down;
        RaycastHit2D ci2 = Physics2D.Raycast(foo2, mu2, 0.001f);
        Vector2 foo3;
        foo3.x = rightFoot.bounds.center.x;
        foo3.y = rightFoot.bounds.min.y;
        Vector2 mu3 = (Quaternion.Euler(Vector2.down) * m_Collision.transform.localRotation).eulerAngles;
        //Vector2 mu3 = Vector2.down;
        RaycastHit2D ci3 = Physics2D.Raycast(foo3, mu3, 0.001f);

        if (ci2.collider.tag == "Branch" && ci3.collider.tag == "Branch")
        {
            Vector3 r = ci2.transform.localRotation.eulerAngles;
            Vector3 s = ci3.transform.localRotation.eulerAngles;
            if (r.z != s.z)         //角度違う坂に乗った時角度変更したいけどできてない←できそう
            {
                float O = (s.z - r.z) * Mathf.PI / 180;
                float R = Mathf.Sqrt(((rightFoot.transform.position.x - leftFoot.transform.position.x) * (rightFoot.transform.position.x - leftFoot.transform.position.x)) - ((rightFoot.transform.position.y - leftFoot.transform.position.y) * (rightFoot.transform.position.y - leftFoot.transform.position.y)));
                float p_distance = 1.0f;//交点-leftFoot.transform.position.x
                float k = Mathf.Acos((((-2 * Mathf.Tan(O) * Mathf.Tan(O) * -p_distance) + Mathf.Sqrt((4 * Mathf.Pow(Mathf.Tan(O), 4.0f) * (p_distance * p_distance)) - (4 * Mathf.Tan(O) + 4) * ((p_distance * p_distance * Mathf.Tan(O) * Mathf.Tan(O)) - (R * R))))
                    / (2 * Mathf.Tan(O) + 2))
                    / R);
            }
        }
        else if (ci.collider.tag == "Branch") { SetR(ci.transform.localRotation.eulerAngles); }

        //  Debug.DrawRay(foo, mu, Color.blue, 0, false);
        // Debug.DrawRay(foo2, mu2, Color.green, 0, false);
        // Debug.DrawRay(foo3, mu3, Color.red, 0, false);
    }

    private bool CalcIntersectionPoint(Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 pointD, Vector2 pointIntersection)//交点求めれるっぽい？
    {
        double dBunbo = (pointB.x - pointA.x)
                        * (pointD.y - pointC.y)
                        - (pointB.y - pointA.y)
                        * (pointD.x - pointC.x);
        if (0 == dBunbo)
        {   // 平行
            return false;
        }

        Vector2 vectorAC = pointC - pointA;
        double dR, dS;
        dR = ((pointD.y - pointC.y) * vectorAC.x
             - (pointD.x - pointC.x) * vectorAC.y) / dBunbo;
        dS = ((pointB.y - pointA.y) * vectorAC.x
             - (pointB.x - pointA.x) * vectorAC.y) / dBunbo;

        pointIntersection.x = pointA.x + (float)dR * (pointB.x - pointA.x);
        pointIntersection.y = pointA.y + (float)dR * (pointB.y - pointA.y);

        return true;
    }
}