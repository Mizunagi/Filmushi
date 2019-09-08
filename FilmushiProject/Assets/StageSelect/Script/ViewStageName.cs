using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewStageName : MonoBehaviour
{
    public Sprite[] StageLavel;
    public Sprite[] StageName;

    private enum LabelNum
    {
        LABEL_BLUE,
        LABEL_GREEN,
        LABEL_YELLOW,
        LABEL_ORANGE,
        LABEL_RED,
        LABEL_MAX
    }

    private enum NameNum
    {
        NAME_TUTORIAL1,
        NAME_TUTORIAL2,
        NAME_TUTORIAL3,
        NAME_STAGE1,
        NAME_STAGE2,
        NAME_STAGE3,
        NAME_STAGE4,
        NAME_STAGE5,
        NAME_STAGE6,
        NAME_STAGE7,
        NAME_STAGE8,
        NAME_STAGE9,
        NAME_STAGE10,
        NAME_STAGE11,
        NAME_STAGE12,
        NAME_EXTRA1,
        NAME_EXTRA2,
        NAME_EXTRA3,
        NAME_MAX
    }

    private GameObject ChildStageName;
    private SpriteRenderer Label;

    // Use this for initialization
    private void Start()
    {
        this.ChildStageName = this.transform.Find("StageNeme").gameObject;
        this.Label = this.GetComponent<SpriteRenderer>();
    }

    public void View(StageSelectManager.SelectStage selectstage)
    {
        switch (selectstage)
        {
            case StageSelectManager.SelectStage.TUTORIAL1:
                Label.sprite = this.StageLavel[(int)LabelNum.LABEL_BLUE];
                ChildStageName.GetComponent<SpriteRenderer>().sprite = this.StageName[(int)NameNum.NAME_TUTORIAL1];

                break;

            case StageSelectManager.SelectStage.TUTORIAL2:
                Label.sprite = this.StageLavel[(int)LabelNum.LABEL_BLUE];
                ChildStageName.GetComponent<SpriteRenderer>().sprite = this.StageName[(int)NameNum.NAME_TUTORIAL2];

                break;

            case StageSelectManager.SelectStage.TUTORIAL3:
                Label.sprite = this.StageLavel[(int)LabelNum.LABEL_BLUE];
                ChildStageName.GetComponent<SpriteRenderer>().sprite = this.StageName[(int)NameNum.NAME_TUTORIAL3];
                break;

            case StageSelectManager.SelectStage.STAGE1:
                Label.sprite = this.StageLavel[(int)LabelNum.LABEL_GREEN];
                ChildStageName.GetComponent<SpriteRenderer>().sprite = this.StageName[(int)NameNum.NAME_STAGE1];
                break;

            case StageSelectManager.SelectStage.STAGE2:
                Label.sprite = this.StageLavel[(int)LabelNum.LABEL_GREEN];
                ChildStageName.GetComponent<SpriteRenderer>().sprite = this.StageName[(int)NameNum.NAME_STAGE2];
                break;

            case StageSelectManager.SelectStage.STAGE3:
                Label.sprite = this.StageLavel[(int)LabelNum.LABEL_GREEN];
                ChildStageName.GetComponent<SpriteRenderer>().sprite = this.StageName[(int)NameNum.NAME_STAGE3];
                break;

            case StageSelectManager.SelectStage.STAGE4:
                Label.sprite = this.StageLavel[(int)LabelNum.LABEL_GREEN];
                ChildStageName.GetComponent<SpriteRenderer>().sprite = this.StageName[(int)NameNum.NAME_STAGE4];
                break;

            case StageSelectManager.SelectStage.STAGE5:
                Label.sprite = this.StageLavel[(int)LabelNum.LABEL_YELLOW];
                ChildStageName.GetComponent<SpriteRenderer>().sprite = this.StageName[(int)NameNum.NAME_STAGE5];
                break;

            case StageSelectManager.SelectStage.STAGE6:
                Label.sprite = this.StageLavel[(int)LabelNum.LABEL_YELLOW];
                ChildStageName.GetComponent<SpriteRenderer>().sprite = this.StageName[(int)NameNum.NAME_STAGE6];
                break;

            case StageSelectManager.SelectStage.STAGE7:
                Label.sprite = this.StageLavel[(int)LabelNum.LABEL_YELLOW];
                ChildStageName.GetComponent<SpriteRenderer>().sprite = this.StageName[(int)NameNum.NAME_STAGE7];
                break;

            case StageSelectManager.SelectStage.STAGE8:
                Label.sprite = this.StageLavel[(int)LabelNum.LABEL_YELLOW];
                ChildStageName.GetComponent<SpriteRenderer>().sprite = this.StageName[(int)NameNum.NAME_STAGE8];
                break;

            case StageSelectManager.SelectStage.STAGE9:
                Label.sprite = this.StageLavel[(int)LabelNum.LABEL_ORANGE];
                ChildStageName.GetComponent<SpriteRenderer>().sprite = this.StageName[(int)NameNum.NAME_STAGE9];
                break;

            case StageSelectManager.SelectStage.STAGE10:
                Label.sprite = this.StageLavel[(int)LabelNum.LABEL_ORANGE];
                ChildStageName.GetComponent<SpriteRenderer>().sprite = this.StageName[(int)NameNum.NAME_STAGE10];
                break;

            case StageSelectManager.SelectStage.STAGE11:
                Label.sprite = this.StageLavel[(int)LabelNum.LABEL_ORANGE];
                ChildStageName.GetComponent<SpriteRenderer>().sprite = this.StageName[(int)NameNum.NAME_STAGE11];
                break;

            case StageSelectManager.SelectStage.STAGE12:
                Label.sprite = this.StageLavel[(int)LabelNum.LABEL_ORANGE];
                ChildStageName.GetComponent<SpriteRenderer>().sprite = this.StageName[(int)NameNum.NAME_STAGE12];
                break;

            case StageSelectManager.SelectStage.EXTRA1:
                Label.sprite = this.StageLavel[(int)LabelNum.LABEL_RED];
                ChildStageName.GetComponent<SpriteRenderer>().sprite = this.StageName[(int)NameNum.NAME_EXTRA1];
                break;

            case StageSelectManager.SelectStage.EXTRA2:
                Label.sprite = this.StageLavel[(int)LabelNum.LABEL_RED];
                ChildStageName.GetComponent<SpriteRenderer>().sprite = this.StageName[(int)NameNum.NAME_EXTRA2];
                break;

            case StageSelectManager.SelectStage.EXTRA3:
                Label.sprite = this.StageLavel[(int)LabelNum.LABEL_RED];
                ChildStageName.GetComponent<SpriteRenderer>().sprite = this.StageName[(int)NameNum.NAME_EXTRA3];
                break;

            default:
                break;
        }
    }
}