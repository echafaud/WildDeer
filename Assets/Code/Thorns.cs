using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorns : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject deerUnity;
    // Start is called before the first frame update
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
    }

    // Update is called once per frame
    void Update()
    {
        /*if (DeerUnity.CurrentActive != 2 && GetComponent<BoxCollider2D>().IsTouching(deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<BoxCollider2D>()))
        {
            deerUnity.GetComponent<DeerUnity>().TakeDamage(100f);
        }*/

        if (GetComponent<BoxCollider2D>().IsTouching(deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<BoxCollider2D>()))
        {
            deerUnity.GetComponent<DeerUnity>().TakeDamage(100f);
        }
    }
}
