using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private GameObject deerUnity;
    public bool isReached;
    private bool isApplied = false;
    // Start is called before the first frame update
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
       
    }

    // Update is called once per frame
    void Update()
    {
        if (isReached && !isApplied)
        {
            deerUnity.GetComponent<DeerUnity>().spawn.transform.position = transform.position;
            deerUnity.GetComponent<DeerUnity>().Respawn();
            isApplied = true;
            SaveManager.SetLastCheckPointName(name);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player") && !isReached)
        {
            deerUnity.GetComponent<DeerUnity>().spawn.transform.position = transform.position;
            isReached = true;
            isApplied = true;
            deerUnity.GetComponent<DeerUnity>().HealHealth(100);
            SaveManager.SetLastCheckPointName(name);
        }
    }
}
