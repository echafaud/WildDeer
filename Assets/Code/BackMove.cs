using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMove : MonoBehaviour
{
    private GameObject deerUnity;
    private float xDelta = 0;
    private float yDelta = 0;
    private float xStart = 0;
    private float yStart = 0;
    public float movingRatio;
    // Start is called before the first frame update
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
        xDelta = deerUnity.transform.position.x - transform.position.x;
        yDelta = deerUnity.transform.position.y - transform.position.y;
        xStart = deerUnity.transform.position.x;
        yStart = deerUnity.transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += new Vector3((xStart - deerUnity.transform.position.x) / movingRatio, 0, 0);

        //transform.position = new Vector3(0, deerUnity.transform.position.y - yDelta, 0);
        xStart = deerUnity.transform.position.x;
        yStart = deerUnity.transform.position.y;
    }
}