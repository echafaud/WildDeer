using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteWindow : MonoBehaviour
{
    private Vector3 OnScreenPoint;
    private Vector3 OffScreenPoint;
    private bool isMoving;
    private int condition;
    private float t;
    public bool isMissionCompleted;
    private bool isShowing;
    private bool isWaiting;
    private bool isHidding;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        isMissionCompleted = false;
        OnScreenPoint = new Vector3(transform.localPosition.x, (int)transform.localPosition.y, transform.localPosition.z);
        transform.localPosition += new Vector3(0, GetComponent<RectTransform>().rect.height, 0);
        OffScreenPoint = new Vector3(transform.localPosition.x, (int)transform.localPosition.y, transform.localPosition.z);
        OffScreenPoint += new Vector3(0, 1, 0);
        condition = 0;
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMissionCompleted)
        {
            if (!isMoving)
            {
                isMoving = true;
                isShowing = true;
            }
        }
        if (isShowing)
        {
            Show();
        }
        if (isHidding)
        {
            Hide();
        }
        if (isWaiting)
        {
            Wait();
        }
    }

    private void Show()
    {
        t += Time.deltaTime;
        var lerp = Vector3.Lerp(transform.localPosition, OnScreenPoint, t);
        transform.localPosition = new Vector3(lerp.x, (int)lerp.y, lerp.z);
        if (transform.localPosition.Equals(OnScreenPoint))
        {
            t = 0;
            isShowing = false;
            isWaiting = true;
        }
    }

    private void Hide()
    {
        t += Time.deltaTime;
        var lerp = Vector3.Lerp(transform.localPosition, OffScreenPoint, t);
        transform.localPosition = new Vector3(lerp.x, (int)lerp.y, lerp.z);
        if (transform.localPosition.Equals(OffScreenPoint))
        {
            isMoving = false;
            isHidding = false;
            isMissionCompleted = false;
            t = 0;
        }
    }

    private void Wait()
    {
        time += Time.deltaTime;
        if (time > 3)
        {
            isWaiting = false;
            isHidding = true;
        }
    }
}
