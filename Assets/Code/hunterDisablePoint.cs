using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hunterDisablePoint : MonoBehaviour
{
    private bool isTriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTriggered)
        {
            isTriggered = true;
            transform.parent.Find("Hunter").gameObject.GetComponent<Hunter>().DisableHunter();
        }
    }
}
