using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlatform : MonoBehaviour
{
    public GameObject platform;
    // Start is called before the first frame update
    void Start()
    {
        platform.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (DeerUnity.CurrentActive == 2)
        {
            platform.SetActive(true);
        }
        else
        {
            platform.SetActive(false);
        }
    }
}
