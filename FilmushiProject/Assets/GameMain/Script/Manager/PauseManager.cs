using System.Collections.Generic;
using UnityEngine;

//ポーズオブジェクト構造体
public struct PauseObject
{
    public GameObject pauseObject;
    public int pauseComponent;

    public PauseObject(GameObject obj)
    {
        pauseObject = obj;
        pauseComponent = PauseManager.ALL;
    }

    public PauseObject(GameObject obj, int cmp)
    {
        pauseObject = obj;
        pauseComponent = cmp;
    }
}

//ポーズリスト構造体
public struct PauseList
{
    public string listName;
    public List<PauseObject> pauseObjectList;

    public PauseList(string name)
    {
        listName = name;
        pauseObjectList = new List<PauseObject>();
    }
}

public class PauseManager : MonoBehaviour
{
    /*
    public enum PauseComponent
    {
        MONO,
        RIGID,
        ALL
    }*/
    public const int MONO = 0x01;
    public const int RIGID = 0x02;
    public const int ANIME = 0x04;
    public const int COL = 0x08;
    public const int PARTICLE = 0x10;
    public const int ALL = MONO | RIGID | ANIME | COL | PARTICLE;

    private List<PauseList> pauseListList;    //ポーズリストのリスト

    private void Awake()
    {
        this.pauseListList = new List<PauseList>();
    }

    //新しいポーズリストを作ってインデックスを返す
    public int CreatePauseObjectList(string listName)
    {
        //既に指定した名前のリストが存在する場合はそのインデックスを返す
        for (int i = 0; i < pauseListList.Count; i++)
        {
            if (pauseListList[i].listName == listName)
            {
                return i;
            }
        }

        PauseList pauseList = new PauseList(listName);
        pauseListList.Add(pauseList);
        return pauseListList.Count - 1;
    }

    //指定したインデックスのリストにオブジェクトを追加する
    public void AddPauseObject(GameObject obj, int index)
    {
        PauseObject pauseObject = new PauseObject(obj);
        pauseListList[index].pauseObjectList.Add(pauseObject);
    }

    //指定したインデックスのリストに停止するコンポーネントを指定してオブジェクトを追加する
    public void AddPauseObject(GameObject obj, int index, int cmp)
    {
        PauseObject pauseObject = new PauseObject(obj, cmp);
        pauseListList[index].pauseObjectList.Add(pauseObject);
    }

    //指定した名前のリストにオブジェクトを追加する
    public void AddPauseObject(GameObject obj, string name)
    {
        foreach (var list in pauseListList)
        {
            if (list.listName == name)
            {
                PauseObject pauseObject = new PauseObject(obj);
                list.pauseObjectList.Add(pauseObject);
                break;
            }
        }
    }

    //指定した名前のリストに停止するコンポーネントを指定してオブジェクトを追加する
    public void AddPauseObject(GameObject obj, string name, int cmp)
    {
        foreach (var list in pauseListList)
        {
            if (list.listName == name)
            {
                PauseObject pauseObject = new PauseObject(obj, cmp);
                list.pauseObjectList.Add(pauseObject);
                break;
            }
        }
    }

    //指定した名前のリストから指定のオブジェクトを削除する
    public void RemovePauseObject(GameObject obj, string name)
    {
        foreach (var list in pauseListList)
        {
            if (list.listName == name)
            {
                foreach (var listObj in list.pauseObjectList)
                {
                    if (listObj.pauseObject == obj)
                    {
                        print(listObj.pauseObject);
                        var result = list.pauseObjectList.Remove(listObj);
                        print(result);
                        return;
                    }
                }
            }
        }
    }

    //ポーズオブジェクトの一時停止処理
    private void Pause(PauseObject obj)
    {
        if ((obj.pauseComponent & MONO) != 0)
        {
            var monoList = obj.pauseObject.GetComponents<MonoBehaviour>();
            if (monoList != null)
            {
                foreach (var mono in monoList)
                {
                    mono.Pause();
                }
            }
        }
        if ((obj.pauseComponent & RIGID) != 0)
        {
            var rigid = obj.pauseObject.GetComponent<Rigidbody2D>();
            if (rigid != null)
            {
                rigid.Pause();
            }
        }
        if ((obj.pauseComponent & ANIME) != 0)
        {
            var anime = obj.pauseObject.GetComponent<Animator>();
            if (anime != null)
            {
                anime.Pause();
            }
        }
        if ((obj.pauseComponent & COL) != 0)
        {
            var col = obj.pauseObject.GetComponent<Collider2D>();
            if (col != null)
            {
                col.Pause();
            }
        }
        if ((obj.pauseComponent & PARTICLE) != 0)
        {
            var particle = obj.pauseObject.GetComponent<ParticleSystem>();
            if (particle != null)
            {
                particle.Pause();
            }
        }
    }

    //ポーズオブジェクトの一時停止解除処理
    private void Resume(PauseObject obj)
    {
        if ((obj.pauseComponent & MONO) != 0)
        {
            var monoList = obj.pauseObject.GetComponents<MonoBehaviour>();
            if (monoList != null)
            {
                foreach (var mono in monoList)
                {
                    mono.Resume();
                }
            }
        }
        if ((obj.pauseComponent & RIGID) != 0)
        {
            var rigid = obj.pauseObject.GetComponent<Rigidbody2D>();
            if (rigid != null)
            {
                rigid.Resume();
            }
        }
        if ((obj.pauseComponent & ANIME) != 0)
        {
            var anime = obj.pauseObject.GetComponent<Animator>();
            if (anime != null)
            {
                anime.Resume();
            }
        }
        if ((obj.pauseComponent & COL) != 0)
        {
            var col = obj.pauseObject.GetComponent<Collider2D>();
            if (col != null)
            {
                col.Resume();
            }
        }
        if ((obj.pauseComponent & PARTICLE) != 0)
        {
            var particle = obj.pauseObject.GetComponent<ParticleSystem>();
            if (particle != null)
            {
                particle.Play();
            }
        }
    }

    //ゲームオブジェクトの一時停止
    public void Pause(GameObject obj)
    {
        var monoList = obj.GetComponents<MonoBehaviour>();
        if (monoList != null)
        {
            foreach (var mono in monoList)
            {
                mono.Pause();
            }
        }
        var rigid = obj.GetComponent<Rigidbody2D>();
        if (rigid != null)
        {
            rigid.Pause();
        }
        var anime = obj.GetComponent<Animator>();
        if (anime != null)
        {
            anime.Pause();
        }
        var col = obj.GetComponent<Collider2D>();
        if (col != null)
        {
            col.Pause();
        }
        var particle = obj.GetComponent<ParticleSystem>();
        if (particle != null)
        {
            particle.Pause();
        }
    }

    //ゲームオブジェクトのScriptかRigidbody2dを選択して一時停止する
    public void Pause(GameObject obj, int cmp)
    {
        if ((cmp & MONO) != 0)
        {
            var monoList = obj.GetComponents<MonoBehaviour>();
            if (monoList != null)
            {
                foreach (var mono in monoList)
                {
                    mono.Pause();
                }
            }
        }
        if ((cmp & RIGID) != 0)
        {
            var rigid = obj.GetComponent<Rigidbody2D>();
            if (rigid != null)
            {
                rigid.Pause();
            }
        }
        if ((cmp & ANIME) != 0)
        {
            var anime = obj.GetComponent<Animator>();
            if (anime != null)
            {
                anime.Pause();
            }
        }
        if ((cmp & COL) != 0)
        {
            var col = obj.GetComponent<Collider2D>();
            if (col != null)
            {
                col.Pause();
            }
        }
        if ((cmp & PARTICLE) != 0)
        {
            var particle = obj.GetComponent<ParticleSystem>();
            if (particle != null)
            {
                particle.Pause();
            }
        }
    }

    //ゲームオブジェクトの一時停止解除
    public void Resume(GameObject obj)
    {
        var monoList = obj.GetComponents<MonoBehaviour>();
        if (monoList != null)
        {
            foreach (var mono in monoList)
            {
                mono.Resume();
            }
        }
        var rigid = obj.GetComponent<Rigidbody2D>();
        if (rigid != null)
        {
            rigid.Resume();
        }
        var anime = obj.GetComponent<Animator>();
        if (anime != null)
        {
            anime.Resume();
        }
        var col = obj.GetComponent<Collider2D>();
        if (col != null)
        {
            col.Resume();
        }
        var particle = obj.GetComponent<ParticleSystem>();
        if (particle != null)
        {
            particle.Play();
        }
    }

    //ゲームオブジェクトのScriptかRigidbody2Dを選択して一時停止解除
    public void Resume(GameObject obj, int cmp)
    {
        if ((cmp & MONO) != 0)
        {
            var monoList = obj.GetComponents<MonoBehaviour>();
            if (monoList != null)
            {
                foreach (var mono in monoList)
                {
                    mono.Resume();
                }
            }
        }
        if ((cmp & RIGID) != 0)
        {
            var rigid = obj.GetComponent<Rigidbody2D>();
            if (rigid != null)
            {
                rigid.Resume();
            }
        }
        if ((cmp & ANIME) != 0)
        {
            var anime = obj.GetComponent<Animator>();
            if (anime != null)
            {
                anime.Resume();
            }
        }
        if ((cmp & COL) != 0)
        {
            var col = obj.GetComponent<Collider2D>();
            if (col != null)
            {
                col.Resume();
            }
        }
        if ((cmp & PARTICLE) != 0)
        {
            var particle = obj.GetComponent<ParticleSystem>();
            if (particle != null)
            {
                particle.Play();
            }
        }
    }

    //指定したインデックスのリストの一時停止
    public void PauseListObject(int index)
    {
        foreach (var obj in pauseListList[index].pauseObjectList)
        {
            Pause(obj);
        }
    }

    //指定した名前のリストの一時停止
    public void PauseListObject(string name)
    {
        foreach (var list in pauseListList)
        {
            if (list.listName == name)
            {
                foreach (var obj in list.pauseObjectList)
                {
                    Pause(obj);
                }
            }
        }
    }

    //指定したインデックスのリストの一時停止解除
    public void ResumeListObject(int index)
    {
        foreach (var obj in pauseListList[index].pauseObjectList)
        {
            Resume(obj);
        }
    }

    //指定した名前のリストの一時停止解除
    public void ResumeListObject(string name)
    {
        foreach (var list in pauseListList)
        {
            if (list.listName == name)
            {
                foreach (var obj in list.pauseObjectList)
                {
                    Resume(obj);
                }
            }
        }
    }

    //指定したタグのオブジェクトを全て一時停止する
    public void PauseTagObject(string tag)
    {
        var pauseList = GameObject.FindGameObjectsWithTag(tag);
        foreach (var obj in pauseList)
        {
            Pause(obj);
            /*
            var monoList = obj.GetComponents<MonoBehaviour>();
            if (monoList != null)
            {
                foreach(var mono in monoList)
                {
                    mono.Pause();
                }
            }
            var rigid = obj.GetComponent<Rigidbody2D>();
            if (rigid != null)
            {
                rigid.Pause();
            }*/
        }
    }

    //指定したタグのオブジェクトを全て一時停止解除する
    public void ResumeTagObject(string tag)
    {
        var resumeList = GameObject.FindGameObjectsWithTag(tag);
        foreach (var obj in resumeList)
        {
            Resume(obj);
            /*
            var monoList = obj.GetComponents<MonoBehaviour>();
            if (monoList != null)
            {
                foreach (var mono in monoList)
                {
                    mono.Resume();
                }
            }
            var rigid = obj.GetComponent<Rigidbody2D>();
            if (rigid != null)
            {
                rigid.Resume();
            }*/
        }
    }

    //指定したタグのオブジェクトのScriptかRigidbody2dを選択して一時停止する
    public void PauseTagObject(string tag, int cmp)
    {
        var pauseList = GameObject.FindGameObjectsWithTag(tag);
        foreach (var obj in pauseList)
        {
            Pause(obj, cmp);
        }
        /*
        switch (cmp)
        {
            case MONO:
                foreach (var obj in pauseList)
                {
                    var monoList = obj.GetComponents<MonoBehaviour>();
                    if (monoList != null)
                    {
                        foreach (var mono in monoList)
                        {
                            mono.Pause();
                        }
                    }
                }
                break;

            case RIGID:
                foreach (var obj in pauseList)
                {
                    var rigid = obj.GetComponent<Rigidbody2D>();
                    if (rigid != null)
                    {
                        rigid.Pause();
                    }
                }
                break;

            case ANIME:
                foreach (var obj in pauseList)
                {
                    var anime = obj.GetComponent<Animator>();
                    if (anime != null)
                    {
                        anime.Pause();
                    }
                }
                break;

            case ALL:
                PauseTagObject(tag);
                break;

            default:
                break;
        }*/
    }

    //指定したタグのオブジェクトのScriptかRigidbody2Dを選択して一時停止解除する
    public void ResumeTagObject(string tag, int cmp)
    {
        var resumeList = GameObject.FindGameObjectsWithTag(tag);

        foreach (var obj in resumeList)
        {
            Pause(obj, cmp);
        }
        /*
        switch (cmp)
        {
            case MONO:
                foreach (var obj in resumeList)
                {
                    var monoList = obj.GetComponents<MonoBehaviour>();
                    if (monoList != null)
                    {
                        foreach (var mono in monoList)
                        {
                            mono.Resume();
                        }
                    }
                }
                break;

            case RIGID:
                foreach (var obj in resumeList)
                {
                    var rigid = obj.GetComponent<Rigidbody2D>();
                    if (rigid != null)
                    {
                        rigid.Resume();
                    }
                }
                break;

            case ANIME:
                foreach (var obj in resumeList)
                {
                    var anime = obj.GetComponent<Animator>();
                    if (anime != null)
                    {
                        anime.Resume();
                    }
                }
                break;

            case ALL:
                ResumeTagObject(tag);
                break;

            default:
                break;
        }*/
    }

    //指定したオブジェクトを親とする親子を全て一時停止
    public void PausePrefab(GameObject obj)
    {
        Pause(obj);
        PauseAllChildren(obj);
    }

    //指定したオブジェクトを親とする親子をScriptかRigidbody2dを選択して全て一時停止
    public void PausePrefab(GameObject obj, int cmp)
    {
        Pause(obj, cmp);
        PauseAllChildren(obj, cmp);
    }

    //指定したオブジェクトを親とする親子を全て一時停止解除
    public void ResumePrefab(GameObject obj)
    {
        Resume(obj);
        ResumeAllChildren(obj);
    }

    //指定したオブジェクトを親とする親子をScriptかRigidbody2dを選択して全て一時停止解除
    public void ResumePrefab(GameObject obj, int cmp)
    {
        Resume(obj, cmp);
        ResumeAllChildren(obj, cmp);
    }

    //指定したオブジェクトの子を全て一時停止
    public void PauseAllChildren(GameObject obj)
    {
        var children = obj.GetAllChildren();
        foreach (var child in children)
        {
            Pause(child);
        }
    }

    //指定したオブジェクトの子をScriptかRigidbody2dを選択して全て一時停止
    public void PauseAllChildren(GameObject obj, int cmp)
    {
        var children = obj.GetAllChildren();
        foreach (var child in children)
        {
            Pause(child, cmp);
        }
    }

    //指定したオブジェクトの子を全て一時停止解除
    public void ResumeAllChildren(GameObject obj)
    {
        var children = obj.GetAllChildren();
        foreach (var child in children)
        {
            Resume(child);
        }
    }

    //指定したオブジェクトの子をScriptかRigidbody2dを選択して全て一時停止解除
    public void ResumeAllChildren(GameObject obj, int cmp)
    {
        var children = obj.GetAllChildren();
        foreach (var child in children)
        {
            Resume(child, cmp);
        }
    }
}