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
    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKey(KeyCode.E) || Input.GetKeyUp(KeyCode.E)) && collision.tag == "Track" && DeerUnity.CurrentActive == 1 && ReindeerSmall.isSmell)
        {
            if (!textDict[collision.gameObject.name].Item2)
            {
                deerUnity.GetComponent<DeerUnity>().countOfFoundTracks += 1;
                textDict[collision.gameObject.name] = Tuple.Create(textDict[collision.gameObject.name].Item1, true);
            }
            text.GetComponent<Text>().text = textDict[collision.gameObject.name].Item1;
            text.GetComponent<Timer>().ClearTimer();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
        text = GameObject.Find("TracksText");
        text.GetComponent<Text>().text = "";
        textDict = new Dictionary<string, Tuple<string, bool>> { { "TrapFootprint", Tuple.Create("Сработавшая ловушка, их расставляют эти мерзавцы. Кажется в неё уже кто-то попался. Нужно быть внимательнее.", false) },
            { "WhirlwindFootprint", Tuple.Create("Здесь довольно ветренно. Ветер опасен, но мама говорила, что он может быть моим союзником.", false) },
            { "CasingsFootprint", Tuple.Create("Я не знаю что это такое, но в лесу этого добра много.", false) }};
        text.gameObject.AddComponent<Timer>();
        text.GetComponent<Timer>().SetPeriodForTick(5f);
        text.GetComponent<Timer>().StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (text.GetComponent<Timer>().IsTicked())
        {
            text.GetComponent<Text>().text = "";
        }
    }
}
