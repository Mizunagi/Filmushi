using UnityEngine;

public class zuu : enemy
{
    public Material mat;
    public Texture dead_tex;
    public Texture nomal_tex;
    public float zuuSP = 0.1f;
    public float zuufallspeed = 0.1f;
    public LayerMask BackGround;
    public LayerMask Branch;
    private BoxCollider2D head;

    private bool hit;

    // Use this for initialization
    private void Start()
    {
        m_Animator = this.GetComponent<Animator>();
        m_Collision = this.GetComponent<BoxCollider2D>();
        m_Transform = this.GetComponent<Transform>();
        m_PlayerCol = GameObject.Find("Player").GetComponent<BoxCollider2D>();
        head = transform.Find("hidari").GetComponent<BoxCollider2D>();

        m_NowPos = m_InitPos = m_Transform.position;
        syokikaku = m_Transform.localScale;
        m_Mode = 0;

        m_Speed = zuuSP;
        m_Mode = 0;
        mat.SetTexture("_MainTex", nomal_tex);
    }

    // Update is called once per frame
    private void Update()
    {
        hit = false;
        Vector2 foot;

        if (base.transform.localScale.x > 0.0f) { foot.x = m_Collision.bounds.min.x; foot.y = m_Collision.bounds.min.y; }
        else { foot.x = m_Collision.bounds.max.x; foot.y = m_Collision.bounds.min.y; }
        if (!(Physics2D.Raycast(foot, -Vector2.up, 0.001f, BackGround))) { SetHit();/* print("ζ*'ヮ')ζ＜出ちゃダメかなーって");*/ }

        GameObject[] colBranch = GameObject.FindGameObjectsWithTag("Branch");
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
            {
                //*********************************目の前が木のとき***************************************

                Vector2 ata;
                ata.x = head.bounds.center.x;
                ata.y = head.bounds.center.y;
                RaycastHit2D ataki = Physics2D.Raycast(ata, kaku, 0.001f, Branch);
                if (ataki)
                {
                    if (!ataki.collider.isTrigger)
                        this.SetHit(); //print("ζ*'ヮ')ζ＜きだ！！");
                }
                Debug.DrawRay(ata, kaku);
                //************************************************************************
            }
        }

        //************************************目の前が敵なとき************************************
        GameObject[] colEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        atari = Physics2D.Raycast(mae, kaku, 0.0001f);

        foreach (GameObject obj in colEnemy)
        {
            if (!mb_Death && (atari.collider.tag == obj.tag) && (obj != m_Collision.gameObject) && (atari.collider.gameObject != m_Collision.gameObject) && (obj != this.gameObject.transform.Find("hidari").gameObject) && (atari.collider.gameObject != this.gameObject.transform.Find("hidari").gameObject))
            {
                Vector2 ata;
                ata.x = head.bounds.center.x;
                ata.y = head.bounds.center.y;

                if (Physics2D.Raycast(ata, kaku, 0.001f).collider.tag == "Enemy")
                {
                    this.SetHit(); //print("ζ*'ヮ')ζ＜てきだ！！"); print(atari.collider.name); print(obj.name);
                }
                Debug.DrawRay(ata, kaku);
            }
            else
            {
            }
        }

        //************************************************************************
        //************************************目の前が葉なとき************************************
        GameObject[] colLeaf = GameObject.FindGameObjectsWithTag("Leaf");
        atari = Physics2D.Raycast(mae, kaku, 0.0001f);

        foreach (GameObject obj in colLeaf)
        {
            if (!mb_Death && (atari.collider.tag == obj.tag) && (obj.name != this.name) && (atari.collider.name != this.name))
            {
                Vector2 ata;
                ata.x = head.bounds.center.x;
                ata.y = head.bounds.center.y;

                if (Physics2D.Raycast(ata, kaku, 0.001f).collider.tag == "Leaf")
                {
                    this.SetHit(); //print("ζ*'ヮ')ζ＜てきだ！！"); print(atari.collider.name); print(obj.name);
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
        //  if () { }
        //・進行方向と角度が鋭角（直角も？）なステージオブジェクト(鈍角も？)
        //・フィルムの地形（ただし足元は無効）ステージオブジェクトの端と二回とらないよう注意
        //・他の敵（スパイダービッドの弾は何も起こらないらしい）
        if (m_HitTime >= 0.0f)
        { m_HitTime -= 1.0f; }
        base.SetHit(hit);
        this.EnemyUpdate();
        if (m_Mode == 1)
        {
            m_Speed = zuufallspeed;
            //base.SetMode(1);
            mb_Death = true;
            mat.SetTexture("_MainTex", dead_tex);
        }
        m_Animator.SetBool("death", mb_Death);
        if (Input.GetAxis("Horizontal") != 0) { resetpos(); }
    }

    public void SetHit()
    {
        if (m_HitTime <= 0.0f)
        {
            hit = true;
            m_HitTime = 50.0f;
        }
    }

    public void resetpos()
    {
        m_Speed = zuuSP;
        mat.SetTexture("_MainTex", nomal_tex);

        mb_Death = false;
        base.resetpos();
    }
}