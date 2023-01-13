using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    private GameObject deerUnity;
    private bool isTriggeredByDeer = false;
    private bool isTriggeredByHunter = false;
    private float windHorizontalVelocity = 0;
    private float windVerticalForce = 0;
    public float totalForce = 10;
    private float previousTotalForce;
    public bool isWorking = true;
    private GameObject hunter;
    public bool isPeriodically = true;
    private float alphaRatio = 0;
    public float periodForTick;
    public AudioClip windSound;
    private AudioSource audio;
    private bool isPlayingSound = false;
    public bool isWithSound = false;
    // Start is called before the first frame update
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
        hunter = GameObject.Find("Hunter");
        previousTotalForce = totalForce;
        gameObject.AddComponent<Timer>();
        GetComponent<Timer>().SetPeriodForTick(periodForTick);
        GetComponent<Timer>().StartTimer();
        if (isWithSound)
        {
            audio = GetComponent<AudioSource>();
            audio.volume = 1f;
            audio.maxDistance = 25;
            audio.spatialBlend = 1;
            audio.rolloffMode = AudioRolloffMode.Linear;
        }
        CalculateForces();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPeriodically && GetComponent<Timer>().IsTicked())
        {
            isWorking = !isWorking;
        }

        if (isWorking && alphaRatio != 1)
        {
            alphaRatio += Time.deltaTime;
            if (alphaRatio > 1)
            {
                alphaRatio = 1;
            }
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, alphaRatio);

            if (isWithSound && isWorking && alphaRatio > 0.5f && !isPlayingSound)
            {
                isPlayingSound = true;
                if (DeerUnity.VolumeRatio == 0)
                {
                    audio.volume = 0;
                }
                else
                {
                    audio.volume = 1;
                }
                audio.PlayOneShot(windSound);
            }
        }

        if (!isWorking)
        {
            isPlayingSound = false;
        }

        if (!isWorking && alphaRatio != 0)
        {
            alphaRatio -= Time.deltaTime;
            if (alphaRatio < 0)
            {
                alphaRatio = 0;
            }
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, alphaRatio);
        }

        if (isWorking && !isTriggeredByDeer && GetComponent<BoxCollider2D>().IsTouching(deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<BoxCollider2D>()))
        {
            isTriggeredByDeer = true;
            deerUnity.GetComponent<DeerUnity>().StartBlowing(windHorizontalVelocity, windVerticalForce);
        }
        else if (isTriggeredByDeer && !GetComponent<BoxCollider2D>().IsTouching(deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<BoxCollider2D>()))
        {
            isTriggeredByDeer = false;
            deerUnity.GetComponent<DeerUnity>().StopBlowing();
        }
        else if (!isWorking && isTriggeredByDeer)
        {
            isTriggeredByDeer = false;
            deerUnity.GetComponent<DeerUnity>().StopBlowing();
        }
        if (previousTotalForce != totalForce)
        {
            previousTotalForce = totalForce;
            CalculateForces();
        }

        if (isWorking && !isTriggeredByHunter && GetComponent<BoxCollider2D>().IsTouching(hunter.GetComponent<BoxCollider2D>()))
        {
            isTriggeredByHunter = true;
            hunter.GetComponent<Hunter>().InWind(windHorizontalVelocity, windVerticalForce);
        }
        else if (isTriggeredByHunter && !GetComponent<BoxCollider2D>().IsTouching(hunter.GetComponent<BoxCollider2D>()))
        {
            isTriggeredByHunter = false;
            hunter.GetComponent<Hunter>().WindOut();
        }
        else if (!isWorking && isTriggeredByHunter)
        {
            isTriggeredByHunter = false;
            hunter.GetComponent<Hunter>().WindOut();
        }
    }

    private void CalculateForces()
    {
        var rotation = transform.localRotation.eulerAngles.z;
        rotation %= 360;

        if (rotation > 180)
            rotation -= 360;
        if (rotation < -180)
            rotation += 360;

        var vKatet = 0f;
        var hKatet = 0f;

        if (rotation >= 0 && rotation < 90)
        {
            vKatet = totalForce * Mathf.Sin((rotation * Mathf.PI) / 180);
            hKatet = Mathf.Sqrt(totalForce * totalForce - vKatet * vKatet);
        }
        else if (rotation >= 90 && rotation <= 180)
        {
            vKatet = totalForce * Mathf.Sin((rotation * Mathf.PI) / 180);
            hKatet = -Mathf.Sqrt(totalForce * totalForce - vKatet * vKatet);
        }
        else if (rotation >= -90 && rotation < 0)
        {
            vKatet = -totalForce * Mathf.Sin((Mathf.Abs(rotation) * Mathf.PI) / 180);
            hKatet = Mathf.Sqrt(totalForce * totalForce - vKatet * vKatet);
        }
        else if (rotation >= -180 && rotation < -90)
        {
            vKatet = -totalForce * Mathf.Sin((Mathf.Abs(rotation) * Mathf.PI) / 180);
            hKatet = -Mathf.Sqrt(totalForce * totalForce - vKatet * vKatet);
        }
        windVerticalForce = vKatet;
        windHorizontalVelocity = hKatet;
    }
}
