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
        textDict = new Dictionary<string, string> { { "ExitFromTheFirstShelter", "Кажется я остался один, мне нужно найти своё стадо.\n" +
                " Хорошо, что я примерно помню часть нашего пути.\n Мне нужно выбраться из леса."},
            { "Smells", "Вокруг меня много запахов. \nПри нажатии на Alt, я чувствую своё стадо и вижу их следы." +
            " \nЯ буду говорить, когда буду чувствовать что-то новое."},
            { "Traces", "Следы привели меня к какой-то вещи. \nНужно попробовать взаиодействовать с ней при помощи клавиши E в режиме нюха. \nМожет она мне чем-то поможет..."},
            { "MovingPlatforms", "Платформы могут двигаться."},
            { "DeerSpirit", "Неужели это дух древнего северного оленя. \nМама говорила, что он помогает нам в трудностях. \nМне нужно следовать за ним!"},
            { "AdditionalPlatforms", "Многие платформы помимо движения могут крутиться и ломаться. \nНужно очень ловко прыгать по ним."},
            { "Boost", "Я могу ускоряться. Для этого необходимо нажимать клавишу Shift. \nНо бег утомляет меня," +
            " поэтому нужно следить за выносливостью - \nбелой полоской в левом верхнем углу."},
            { "HintSpikes", "Охотники расставляют не только капканы, но и ловушки-шипы. \nИз капкана я могу выбраться," +
            " но шипы ранят слишком сильно. \nИх нужно остерегаться!"},
            { "HintBushes", "В случае опасности я могу спрятаться в куст. \nДля этого нужно нажать кнопку взаимодействия."},
            { "Shot", "Снова эти ужасные звуки - выстрелы! \nГде-то здесь охотник, нужно бежать!"},
            { "ScentActivationSign", "Чувствую новые запахи!"},
            { "HintGhostPlatforms", "Теперь я могу вызывать духа оленя, но только на ... секунд.\n" +
            " В отличие от меня, дух видит призрачные платформы и может с ними взаимодействовать."},
            { "HintGhost", "Дух прыгает выше меня. Но он вызывается лишь на время, \nпоэтому иногда нужно использовать его способность на E - материализация платформ." +
            " Когда дух активиреут способность стоя на призрачной платформе, \nона становится материальной."},
            {"Soaring", "Чтобы спуститься безопасно с большой высоты, \nнужно использовать возможность парения у духа на Alt." },
            { "ExitFromTheSecondShelter", "Я вышел из леса. Кажется теперь мне нужно направиться к холму. \nНо куда идти дальше я не помню… Надеюсь я догоню своё стадо. \nНо я уже чувствую новые запахи!"},
            {"HintTemperature", "В тени я охлаждаюсь и чувствую себя лучше. \nНужно стараться вовремя до неё добираться."},
            {"HintBoulders", "Кажется что-то крупное катится впереди! \nНужно быть осторожным!"},
            {"HintWindOnSlope", "Ого, какой поток ветра впереди. \nМне нужны какие-то барьеры, чтобы приодолеть его..."},
            {"HintOnMountain", "Нужно тщательно обыскать территорию горы. \nЯ чувствую новые запахи!" } };
        
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
