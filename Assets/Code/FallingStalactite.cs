using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingStalactite : MonoBehaviour
{
    
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
        if (collision.tag.Equals("Player"))
        {
            transform.Find("Stalactite").GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            transform.Find("Stalactite").GetComponent<Rigidbody2D>().mass = 1000;
            Invoke("TurnOffStalactite", 5f);
        }
    }

    void TurnOffStalactite()
    {
        transform.Find("Stalactite").gameObject.SetActive(false);
    }
}