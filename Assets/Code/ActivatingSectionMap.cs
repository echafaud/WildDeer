using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatingSectionMap : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isActivate;
    private bool previousValue;
    void Start()
    {
        isActivate = false;
        previousValue = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
