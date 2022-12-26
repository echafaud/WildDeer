using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangerDeer : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject deerUnity;
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
    }

    // Update is called once per frame
    void Update()
    {
        if (DeerUnity.CurrentActive == 1)
        {
            gameObject.transform.Find("CircleSmall").gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            gameObject.transform.Find("CircleSmall").gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            gameObject.transform.Find("CircleGhost").gameObject.GetComponent<SpriteRenderer>().color = Color.grey;
            gameObject.transform.Find("CircleGhost").gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.7f, 1f);
            gameObject.transform.Find("CircleBig").gameObject.GetComponent<SpriteRenderer>().color = Color.grey;
            gameObject.transform.Find("CircleBig").gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.7f, 1f);
        }
        else if (DeerUnity.CurrentActive == 2)
        {
            gameObject.transform.Find("CircleSmall").gameObject.GetComponent<SpriteRenderer>().color = Color.grey;
            gameObject.transform.Find("CircleSmall").gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.7f, 1f);
            gameObject.transform.Find("CircleGhost").gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            gameObject.transform.Find("CircleGhost").gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            gameObject.transform.Find("CircleBig").gameObject.GetComponent<SpriteRenderer>().color = Color.grey;
            gameObject.transform.Find("CircleBig").gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.7f, 1f);
        }
        else if (DeerUnity.CurrentActive == 3)
        {
            gameObject.transform.Find("CircleSmall").gameObject.GetComponent<SpriteRenderer>().color = Color.grey;
            gameObject.transform.Find("CircleSmall").gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.7f, 1f);
            gameObject.transform.Find("CircleGhost").gameObject.GetComponent<SpriteRenderer>().color = Color.grey;
            gameObject.transform.Find("CircleGhost").gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.7f, 1f);
            gameObject.transform.Find("CircleBig").gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            gameObject.transform.Find("CircleBig").gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
