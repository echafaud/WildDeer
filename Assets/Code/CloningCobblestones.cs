using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloningCobblestones : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject startPosition;
    private Queue<GameObject> queue;
    private bool isStartCloning;
    void Start()
    {
        this.gameObject.AddComponent<Timer>();
        GetComponent<Timer>().SetPeriodForTick(5f);
        GetComponent<Timer>().StartTimer();
        queue = new Queue<GameObject>();
        isStartCloning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Timer>().IsTicked())
        {
            var clone = GameObject.Instantiate(this.gameObject.transform.Find("Circle").gameObject, startPosition.transform.position, transform.rotation);
            queue.Enqueue(clone);
            Invoke("destroyClone", 9f);
        }
    }

    private void destroyClone()
    {
       Object.Destroy(queue.Dequeue());
    }
}
