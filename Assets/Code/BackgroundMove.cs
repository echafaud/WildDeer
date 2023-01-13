using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    private GameObject deerUnity;
    private float xDelta = 0;
    private float yDelta = 0;
    // Start is called before the first frame update
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
        xDelta = deerUnity.transform.position.x - transform.position.x;
        yDelta = deerUnity.transform.position.y - transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(deerUnity.transform.position.x - xDelta, deerUnity.transform.position.y - yDelta, transform.position.z);
    }
}
