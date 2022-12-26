using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingThorns : MonoBehaviour
{
    public int isFirstActivate;
    public GameObject FirstThorns;
    public GameObject SecondThorns;

    void Start()
    {
        InvokeRepeating("ChsngeThorns", 0f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChsngeThorns()
    {
        if (isFirstActivate == 1)
        {
            isFirstActivate = 0;
            SecondThorns.SetActive(true);
            FirstThorns.SetActive(false);
        }
        else
        {
            isFirstActivate = 1;
            SecondThorns.SetActive(false);
            FirstThorns.SetActive(true);
        }
    }
}
