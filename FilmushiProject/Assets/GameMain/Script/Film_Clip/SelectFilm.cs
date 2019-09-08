using UnityEngine;

public class SelectFilm : MonoBehaviour
{
    private bool selectFlag = false;
    private Transform tf;
    private Vector3 largeScale;
    private Vector3 smallScale;

    // Use this for initialization
    private void Start()
    {
        tf = transform;
    }

    // Update is called once per frame
    private void Update()
    {
        if (selectFlag)
        {
            //Vector3 _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //_mousePos.z = tf.position.z;
            //tf.position = _mousePos;
        }
    }

    public void SelectThisFilm()
    {
        selectFlag = true;
    }

    public void ReleseThisFilm()
    {
        if (selectFlag)
        {
            selectFlag = false;
        }
    }

    //拡大
    public void IncreaseSize()
    {
        tf.localScale = largeScale;
    }

    //縮小
    public void DecreaseSize()
    {
        tf.localScale = smallScale;
    }
}