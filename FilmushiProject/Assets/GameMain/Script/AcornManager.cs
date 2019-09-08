using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcornManager : MonoBehaviour
{
    public GameObject Acornobj;
    public List<Vector3> Acornposlist;
    private bool[] AcornflgAly;
    private int GetAcornCnt;
    private int AcornMAX;

    // Use this for initialization
    private void Start()
    {
        Vector3 workpos = new Vector3();
        GameObject obj;
        Acorn acorn;
        int i;

        AcornMAX = Acornposlist.Count;
        AcornflgAly = new bool[AcornMAX];

        for (i = 0; i < AcornflgAly.Length; i++)
        {
            AcornflgAly[i] = false;
        }

        for (i = 0; i < AcornMAX; i++)
        {
            workpos.Set(Acornposlist[i].x, Acornposlist[i].y, 1);
            obj = Instantiate(Acornobj, workpos, Quaternion.identity) as GameObject;
            obj.transform.parent = transform;
            acorn = obj.GetComponent<Acorn>();
            acorn.SetAcornID(i);
        }

        GetAcornCnt = 0;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void SendDestroyAcorn(int acornID)
    {
        print("GetacornID" + acornID);
        GetAcornCnt++;
        print("GetAcornCnt" + GetAcornCnt);
        AcornflgAly[acornID] = true;

        for (int i = 0; i < AcornflgAly.Length; i++)
        {
            print("AcornflgAly" + i + AcornflgAly[i]);
        }
    }

    //ドングリのMAX数取得
    public int GetMAXAcorn
    {
        get { return AcornMAX; }
    }

    //ドングリの取得数を取得
    public int GetCntAcorn
    {
        get { return GetAcornCnt; }
    }
}