using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackChecker : MonoBehaviour
{
    private GameObject deerUnity;
    private GameObject text;
    private Dictionary<string, Tuple<string, bool>> textDict;
    public static bool isChanged = false;
    private Timer timer;
    private bool isInArea = false;
    private Collider2D collision;
    private GameObject parent;
    private GameObject back;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Track")
        {
            this.collision = collision;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Track")
        {
            isInArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Track")
        {
            isInArea = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
        text = GameObject.Find("TracksText");

        parent = text.transform.parent.Find("TracksTextParent").transform.gameObject;
        back = parent.transform.Find("TextBackground").gameObject;
        back.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        parent.GetComponent<Text>().text = "";

        text.GetComponent<Text>().text = "";
        textDict = new Dictionary<string, Tuple<string, bool>> { { "TrapFootprint", Tuple.Create("����������� �������, �� ����������� ��� ��������. \n������� � �� ��� ���-�� �������. \n����� ���� ������������.", false) },
            { "WhirlwindFootprint", Tuple.Create("����� �������� ��������. \n����� ������, �� ���� ��������, \n��� �� ����� ���� ���� ���������.", false) },
            { "CasingsFootprint", Tuple.Create("���� �������� �������� � ��� - ����. \n��� ��� ��� ����� ������. �� ���� ���� ������� � ������� ���������, \n��� ����� ����������� � ���������� ����.", false) },
            { "LittleShadow", Tuple.Create("����� ��� � ����� ������� � ����.\n ���� ��� ���������.", false) },
            { "FallenStalactite", Tuple.Create("�� �������� � ����, \n��� ����� ����� �� ��������.", false) },
            { "SmellOfDog", Tuple.Create("� �������� ���������� ����, a � ���� ������ ��������.\n ����� ������ ������� ������.", false) }};
        timer = text.gameObject.AddComponent<Timer>();
        timer.SetPeriodForTick(5f);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && ReindeerSmall.isSmell && isInArea)
        {
            if (!textDict[collision.gameObject.name].Item2)
            {
                deerUnity.GetComponent<DeerUnity>().countOfFoundTracks += 1;
                deerUnity.GetComponent<DeerUnity>().SetTask(1);
                textDict[collision.gameObject.name] = Tuple.Create(textDict[collision.gameObject.name].Item1, true);
            }
            text.GetComponent<Text>().text = textDict[collision.gameObject.name].Item1;

            parent.GetComponent<Text>().text = textDict[collision.gameObject.name].Item1;
            back.GetComponent<Image>().color = new Color(1, 1, 1, 1);

            isChanged = true;
            timer.ClearTimer();
            timer.SetPeriodForTick(5f);
            timer.StartTimer();
        }

        var tick = timer.IsTicked();
        if (tick && !Hints.isChanged)
        {
            parent.GetComponent<Text>().text = "";
            back.GetComponent<Image>().color = new Color(0, 0, 0, 0);

            text.GetComponent<Text>().text = "";
            timer.ClearTimer();
            timer.SetPeriodForTick(5f);
            timer.StopTimer();
            isChanged = false;
        }
        else if (tick && Hints.isChanged)
        {
            timer.ClearTimer();
            timer.SetPeriodForTick(5f);
            timer.StopTimer();
            isChanged = false;
        }
    }
}
