using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Materialization : MonoBehaviour
{
    public Sprite GhostSprite;
    public Sprite OrdinarySprite;


    // Start is called before the first frame update
    public void makeMaterialisation()
    {
        gameObject.transform.Find("GPlatform").GetComponent<SpriteRenderer>().sprite = OrdinarySprite;
        gameObject.tag = "MaterialisedPlatform";
        //Invoke("TurnOffOrdinary", 5f);
    }

    private void TurnOffOrdinary()
    {
        gameObject.transform.Find("GPlatform").GetComponent<SpriteRenderer>().sprite = GhostSprite;
        gameObject.tag = "GhostPlatform";
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
