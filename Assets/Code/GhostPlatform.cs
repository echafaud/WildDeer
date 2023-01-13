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
        if (DeerUnity.CurrentActive == 2 && (platform.tag == "GhostPlatform" || platform.tag == "CircleGhostPlatform"))
        {
            platform.SetActive(true);
        }
        else if(platform.tag == "GhostPlatform" || platform.tag == "CircleGhostPlatform")
        {
            platform.SetActive(false);
        }
    }
}
