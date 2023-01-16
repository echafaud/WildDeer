using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformChecker : MonoBehaviour
{
    private GameObject deerUnity;
    private Collider2D collision;
    private Queue<Collider2D> queueOn = new Queue<Collider2D>();
    private Queue<Collider2D> queueOff = new Queue<Collider2D>();
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Platform" || collision.tag == "CircleGhostPlatform")
        {
            deerUnity.GetComponent<DeerUnity>().isOnPlatform = true;
            if (!collision.gameObject.name.Equals("RotateBox"))
            {
                deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().transform.parent = collision.transform;
                deerUnity.GetComponent<DeerUnity>().isOnMovePlatform = true;
            }
        }
        else if(collision.tag == "MaterialisedPlatform")
        {
            deerUnity.GetComponent<DeerUnity>().isOnPlatform = true;
        }
        else if (collision.tag == "MoveObject")
        {
            deerUnity.GetComponent<DeerUnity>().isOnPlatform = true;
        }
        else if (collision.tag == "CollapsingPlat")
        {
            deerUnity.GetComponent<DeerUnity>().isOnPlatform = true;
            if (DeerUnity.CurrentActive != 2)
            {
                /*if (!(deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().transform.Find("RightWallChecker").gameObject.GetComponent<BoxCollider2D>().IsTouching(collision)
                    || deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().transform.Find("LeftWallChecker").gameObject.GetComponent<BoxCollider2D>().IsTouching(collision)))
                {
                    queueOff.Enqueue(collision);
                    queueOn.Enqueue(collision);
                    Invoke("TurnOffPlatform", 1f);
                }*/
                queueOff.Enqueue(collision);
                queueOn.Enqueue(collision);
                Invoke("TurnOffPlatform", 1f);
            }
        }
        else if (collision.tag == "throns")
        {
            deerUnity.GetComponent<DeerUnity>().TakeDamage(100f);
        }
        else if (collision.tag == "GhostPlatform")
        {
            deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerGhost>().currendGhostPlatform = collision.gameObject;
            deerUnity.GetComponent<DeerUnity>().isOnPlatform = true;
        }
        else if (collision.tag == "Circle")
        {
            deerUnity.GetComponent<DeerUnity>().TakeDamage(100f);
        }
        if (collision.tag == "CollectionArea")
        {
            deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerSmall>().currentLemmingArea = collision.gameObject;
        }
        
    }

    void TurnOffPlatform()
    {
        queueOff.Dequeue().gameObject.SetActive(false);
        Invoke("TurnOnPlatform", 5f);
    }
    void TurnOnPlatform()
    {
        queueOn.Dequeue().gameObject.SetActive(true);
    }
    
    //void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.name.Equals("RotateBox"))
    //        deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().
    //}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Platform" || collision.tag == "GhostPlatform" || collision.tag == "MoveObject" || collision.tag == "CircleGhostPlatform" || collision.tag == "MaterialisedPlatform")
        {
            deerUnity.GetComponent<DeerUnity>().isOnPlatform = false;
            deerUnity.GetComponent<DeerUnity>().isOnMovePlatform = false;
            deerUnity.GetComponent<DeerUnity>().isOnGhostPlatform = false;
            deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().transform.parent = null;
            //deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerGhost>().currendGhostPlatform = null;
        }
        else if(collision.tag == "CollapsingPlat")
        {
            deerUnity.GetComponent<DeerUnity>().isOnPlatform = false;
        }
    }

    void Update()
    {
        
    }
}
