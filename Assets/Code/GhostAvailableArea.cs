using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAvailableArea : MonoBehaviour
{
    private GameObject deerUnity;
    // Start is called before the first frame update
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            deerUnity.GetComponent<DeerUnity>().isSecondDeerAvailable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            deerUnity.GetComponent<DeerUnity>().isSecondDeerAvailable = false;
        }
    }
}
