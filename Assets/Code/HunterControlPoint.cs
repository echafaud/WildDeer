using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterControlPoint : MonoBehaviour
{
    private GameObject hunter;
    private GameObject hunterPoint;
    public bool isStayAtPoint;
    public bool isAlreadyWorked = false;
    // Start is called before the first frame update
    void Start()
    {
        hunter = transform.parent.parent.Find("Hunter").gameObject;
        hunterPoint = transform.parent.Find("HunterPoint").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player") && !isAlreadyWorked)
        {
            isAlreadyWorked = true;
            if (isStayAtPoint)
            {
                hunter.GetComponent<Hunter>().StayAtPoint(hunterPoint.transform);
            }
            else
            {
                hunter.GetComponent<Hunter>().HuntDeerAtPoint(hunterPoint.transform);
            }
        }
    }

    public void DoSame()
    {
        if (isStayAtPoint)
        {
            hunter.GetComponent<Hunter>().StayAtPoint(hunterPoint.transform);
        }
        else
        {
            hunter.GetComponent<Hunter>().HuntDeerAtPoint(hunterPoint.transform);
        }
    }
}
