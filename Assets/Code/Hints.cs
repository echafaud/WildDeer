using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Hints : MonoBehaviour
{

    private GameObject deerUnity;
    private GameObject text;
    private GameObject footprintTimer;
    private Dictionary<string, string> textDict;
    public static bool isChanged = false;
    private Timer timer;
    private GameObject parent;
    private GameObject back;
    private bool isRead;
    // Start is called before the first frame update
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");

        text = GameObject.Find("TracksText");

        parent = text.transform.parent.Find("TracksTextParent").transform.gameObject;
        back = parent.transform.Find("TextBackground").gameObject;
        back.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        parent.GetComponent<Text>().text = "";

        timer = text.gameObject.AddComponent<Timer>();
        text.GetComponent<Text>().text = "";
        textDict = new Dictionary<string, string> { { "ExitFromTheFirstShelter", "������� � ������� ����, ��� ����� ����� ��� �����.\n" +
                " ������, ��� � �������� ����� ����� ������ ����.\n ��� ����� ��������� �� ����."},
            { "Smells", "������ ���� ����� �������. \n��� ������� �� Alt, � �������� ��� ����� � ���� �� �����." +
            " \n� ���� ��������, ����� ���� ����������� ���-�� �����."},
            { "Traces", "����� ������� ���� � �����-�� ����. \n����� ����������� ���������������� � ��� ��� ������ ������� E � ������ ����. \n����� ��� ��� ���-�� �������..."},
            { "MovingPlatforms", "��������� ����� ���������."},
            { "DeerSpirit", "������� ��� ��� �������� ��������� �����. \n���� ��������, ��� �� �������� ��� � ����������. \n��� ����� ��������� �� ���!"},
            { "AdditionalPlatforms", "������ ��������� ������ �������� ����� ��������� � ��������. \n����� ����� ����� ������� �� ���."},
            { "Boost", "� ���� ����������. ��� ����� ���������� �������� ������� Shift. \n�� ��� �������� ����," +
            " ������� ����� ������� �� ������������� - \n����� �������� � ����� ������� ����."},
            { "HintSpikes", "�������� ����������� �� ������ �������, �� � �������-����. \n�� ������� � ���� ���������," +
            " �� ���� ����� ������� ������. \n�� ����� ������������!"},
            { "HintBushes", "� ������ ��������� � ���� ���������� � ����. \n��� ����� ����� ������ ������ ��������������."},
            { "Shot", "����� ��� ������� ����� - ��������! \n���-�� ����� �������, ����� ������!"},
            { "ScentActivationSign", "�������� ����� ������!"},
            { "HintGhostPlatforms", "������ � ���� �������� ���� �����, �� ������ �� ... ������.\n" +
            " � ������� �� ����, ��� ����� ���������� ��������� � ����� � ���� �����������������."},
            { "HintGhost", "��� ������� ���� ����. �� �� ���������� ���� �� �����, \n������� ������ ����� ������������ ��� ����������� �� E - �������������� ��������." +
            " ����� ��� ���������� ����������� ���� �� ���������� ���������, \n��� ���������� ������������."},
            {"Soaring", "����� ���������� ��������� � ������� ������, \n����� ������������ ����������� ������� � ���� �� Alt." },
            { "ExitFromTheSecondShelter", "� ����� �� ����. ������� ������ ��� ����� ����������� � �����. \n�� ���� ���� ������ � �� ������ ������� � ������ ��� �����. \n�� � ��� �������� ����� ������!"},
            {"HintTemperature", "� ���� � ���������� � �������� ���� �����. \n����� ��������� ������� �� �� ����������."},
            {"HintBoulders", "������� ���-�� ������� ������� �������! \n����� ���� ����������!"},
            {"HintWindOnSlope", "���, ����� ����� ����� �������. \n��� ����� �����-�� �������, ����� ���������� ���..."},
            {"HintOnMountain", "����� ��������� �������� ���������� ����. \n� �������� ����� ������!" } };
        
        timer.SetPeriodForTick(5f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            text.GetComponent<Text>().text = textDict[gameObject.name];
            //if (gameObject.name.Equals("ExitFromTheFirstShelter"))
            //{
            //    GameObject.Find("DeerUnity").GetComponent<DeerUnity>().SetTask(1);
            //}
            if (gameObject.name.Equals("Smells") && !isRead)
            {
                GameObject.Find("DeerUnity").GetComponent<DeerUnity>().SetTask(1);
                isRead = true;
            }
            //if (gameObject.name.Equals("DeerSpirit"))
            //{
            //    GameObject.Find("DeerUnity").GetComponent<DeerUnity>().SetTask(1);
            //}
            parent.GetComponent<Text>().text = textDict[gameObject.name];
            back.GetComponent<Image>().color = new Color(1, 1, 1, 1);

            isChanged = true;
            //TrackChecker.isHintsOn = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //text.GetComponent<Text>().text = "";
            
            timer.ClearTimer();
            timer.SetPeriodForTick(5f);
            timer.StartTimer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        var tick = timer.IsTicked();
        if (tick && !TrackChecker.isChanged)
        {
            parent.GetComponent<Text>().text = "";
            back.GetComponent<Image>().color = new Color(0, 0, 0, 0);

            text.GetComponent<Text>().text = "";
            timer.ClearTimer();
            timer.StopTimer();
            isChanged = false;
        }
        else if(tick && TrackChecker.isChanged)
        {
            timer.ClearTimer();
            timer.StopTimer();
            isChanged = false;
        }
    }
}
