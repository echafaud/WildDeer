using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private GameObject deerUnity;
    public bool isTriggered = false;
    public AudioClip triggerSound;
    // Start is called before the first frame update
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
        if (isTriggered)
        {
            Trigger();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTriggered && DeerUnity.CurrentActive != 2 && GetComponent<BoxCollider2D>().IsTouching(deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<BoxCollider2D>()))
        {
            Trigger();
            deerUnity.GetComponent<DeerUnity>().Trapped();
            var audio = gameObject.AddComponent<AudioSource>();
            audio.volume = 1f;
            audio.PlayOneShot(triggerSound);
        }
    }

    private void Trigger()
    {
        isTriggered = true;
        GetComponent<SpriteRenderer>().color = Color.blue;
    }
}
