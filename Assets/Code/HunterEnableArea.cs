using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterEnableArea : MonoBehaviour
{
    private List<GameObject> hunterPoints = new List<GameObject>();
    private bool isAlreadyMoved = false;
    // Start is called before the first frame update
    void Start()
    {
        hunterPoints.AddRange(GameObject.FindGameObjectsWithTag("HunterPoint"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player") && !isAlreadyMoved)
        {
            MoveHunterAtNearestPoint();
            isAlreadyMoved = true;
        }
    }

    public void MoveHunterAtNearestPoint()
    {
        GameObject.Find("Hunter").GetComponent<Hunter>().isEnabled = true;

        GameObject min = null;
        var mind = float.MaxValue;
        foreach (var e in hunterPoints)
        {
            if (Math.Abs(e.transform.position.x - GameObject.Find("DeerUnity").GetComponent<DeerUnity>().GetCurrentActiveDeer().transform.position.x) < mind)
            {
                mind = Math.Abs(e.transform.position.x - GameObject.Find("DeerUnity").GetComponent<DeerUnity>().GetCurrentActiveDeer().transform.position.x);
                min = e;
            }
        }
        min.GetComponent<HunterControlPoint>().DoSame();
    }

    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            GameObject.Find("Hunter").GetComponent<Hunter>().isEnabled = true;
        }
    }*/

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            GameObject.Find("Hunter").GetComponent<Hunter>().isEnabled = false;
        }
    }
}
