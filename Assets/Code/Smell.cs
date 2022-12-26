using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smell : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject trace;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DeerUnity.CurrentActive == 1 && ReindeerSmall.isSmell)
        {
            trace.SetActive(true);
        }
        else
        {
            trace.SetActive(false);
        }
    }
}
