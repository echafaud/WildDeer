using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBox : MonoBehaviour
{
    public float defaultSpeed;
    private float rotateSpeed;
    public float direction;
    private float wait = 0;
    private GameObject localWind;
    void Start()
    {
        rotateSpeed = defaultSpeed;
        localWind = transform.parent.Find("Wind").gameObject;
        localWind.GetComponent<Wind>().isWorking = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float angle = (int)transform.eulerAngles.z % 90;
        if (angle == 0)
        {
            wait += Time.deltaTime;
            rotateSpeed = 0;
            if (wait > 2f)
            {
                wait = 0;
                rotateSpeed = defaultSpeed;
            }
        }
        if (angle > 0 && angle < 45)
        {
            localWind.GetComponent<Wind>().isWorking = true;
            if (GameObject.Find("DeerUnity").GetComponent<DeerUnity>().GetCurrentActiveDeer().name.Equals("ReindeerBig"))
            {
                localWind.GetComponent<Wind>().totalForce = 50;
            }
            else if (GameObject.Find("DeerUnity").GetComponent<DeerUnity>().GetCurrentActiveDeer().name.Equals("ReindeerGhost"))
            {
                localWind.GetComponent<Wind>().totalForce = 8;
            }
            else
            {
                localWind.GetComponent<Wind>().totalForce = 10;
            }
        }
        else
        {
            localWind.GetComponent<Wind>().isWorking = false;
        }

        transform.Rotate(0, 0, rotateSpeed * direction * Time.deltaTime);
    }
}
