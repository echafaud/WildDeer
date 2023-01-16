using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangerAbilities : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.Find("Alt").gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
        gameObject.transform.Find("E").gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            gameObject.transform.Find("E").gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            Invoke("TurnOffE", 0.3f);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            gameObject.transform.Find("Alt").gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            Invoke("TurnOffAlt", 3f);
        }
    }

    public void TurnOffAlt()
    {
        gameObject.transform.Find("Alt").gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
    }

    public void TurnOffE()
    {
        gameObject.transform.Find("E").gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
    }
}
