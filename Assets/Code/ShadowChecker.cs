using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowChecker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Shadow")
        {
            gameObject.transform.parent.gameObject.GetComponent<ReindeerSmall>().SetShadowChecker(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Shadow")
        {
            gameObject.transform.parent.gameObject.GetComponent<ReindeerSmall>().SetShadowChecker(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
