using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePlatform : MonoBehaviour
{
    [SerializeField]
    Transform center;

    [SerializeField]
    float radius = 2f, angularSpeed = 2f;
    float positionX, positionY, angle = 0f;
    public int direction;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (direction == 1)
        {
            positionX = center.position.x + Mathf.Cos(90 - angle) * radius;
            positionY = center.position.y + Mathf.Sin(90 - angle) * radius;
        }
        else
        {
            positionX = center.position.x + Mathf.Cos(angle) * radius;
            positionY = center.position.y + Mathf.Sin(angle) * radius;
        }
        transform.Find("Platform").gameObject.transform.position = new Vector3(positionX, positionY, transform.position.z);
        angle = angle + Time.deltaTime * angularSpeed;
        if (angle >= 360)
        {
            angle = 0f;
        }
    }
}
