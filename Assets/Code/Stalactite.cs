using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalactite : MonoBehaviour
{
    private bool isCanDamage = true;
    private GameObject disableArea;
    // Start is called before the first frame update
    void Start()
    {
        disableArea = transform.parent.Find("DisableArea").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCanDamage && GetComponent<PolygonCollider2D>().IsTouching(disableArea.GetComponent<BoxCollider2D>()))
        {
            isCanDamage = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && isCanDamage)
        {
            GameObject.Find("DeerUnity").GetComponent<DeerUnity>().TakeDamage(100f);
        }
    }
}
