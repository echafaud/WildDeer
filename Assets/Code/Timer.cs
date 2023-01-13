using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float period = 1;
    private float totalTime = 0;
    private float time = 0;
    private bool isTicked = false;
    private bool isStart = false;
    // Start is called before the first frame update
    void Start()
    {
        totalTime = Time.deltaTime;
        totalTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            time += Time.deltaTime;
            if (time > period)
            {
                time -= period;
                isTicked = true;
                totalTime += period;
                //GameObject.Find("Info").GetComponent<Text>().text = totalTime.ToString();
            }
        }
    }

    public void SetPeriodForTick(float period)
    {
        this.period = period;
    }

    public void StartTimer()
    {
        isStart = true;
    }

    public void StopTimer()
    {
        isStart = false;
    }

    public void ClearTimer()
    {
        totalTime = 0;
        time = 0;
        isTicked = false;
    }

    public bool IsTicked()
    {
        var isTickedClone = isTicked;
        isTicked = false;
        return isTickedClone;
    }

    public float GetTime()
    {
        return totalTime;
    }
}
