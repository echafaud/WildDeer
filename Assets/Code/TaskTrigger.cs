using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTrigger : MonoBehaviour
{
    public string whatTaskTrigger;
    private GameObject deerUnity;
    private bool isTriggered;
    // Start is called before the first frame update
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player") && !isTriggered)
        {
            isTriggered = true;
            var taskIndexes = whatTaskTrigger.Split(" ");
            foreach(var taskIndex in taskIndexes)
            {
                deerUnity.GetComponent<DeerUnity>().SetTask(int.Parse(taskIndex));
            }
        }
    }
}
