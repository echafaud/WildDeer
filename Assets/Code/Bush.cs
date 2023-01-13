using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    private GameObject deerUnity;
    private bool isTriggered = false;
    public AudioClip bushInSound;
    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
        audio = GetComponent<AudioSource>();
        audio.volume = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isTriggered)
        {
            if (DeerUnity.VolumeRatio == 0)
            {
                audio.volume = 0;
            }
            else
            {
                audio.volume = 1;
            }
            audio.PlayOneShot(bushInSound);
            isTriggered = false;
            deerUnity.GetComponent<DeerUnity>().UnBushed(gameObject);
            GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.E) && !isTriggered && DeerUnity.CurrentActive == 1
            && GetComponent<PolygonCollider2D>().IsTouching(deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<BoxCollider2D>()))
        {
            if (DeerUnity.VolumeRatio == 0)
            {
                audio.volume = 0;
            }
            else
            {
                audio.volume = 1;
            }
            audio.PlayOneShot(bushInSound);
            isTriggered = true;
            deerUnity.GetComponent<DeerUnity>().Bushed(gameObject);
            GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 0.5f);
        }
    }
}
