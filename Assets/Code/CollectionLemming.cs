using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionLemming : MonoBehaviour
{
    // Start is called before the first frame update
    public void assembleLemming()
    {
        gameObject.transform.Find("Lemming").gameObject.SetActive(false);
        DeerUnity.countOfFoundLemmings += 1;
        gameObject.SetActive(false);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
