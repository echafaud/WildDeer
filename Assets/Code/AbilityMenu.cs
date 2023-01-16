using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMenu : MonoBehaviour
{
    private Vector3 OnScreenPoint;
    private Vector3 OffScreenPoint;
    private bool isMoving;
    private int condition;
    private float t;
    // Start is called before the first frame update
    void Start()
    {
        OnScreenPoint = new Vector3(transform.localPosition.x, (int)transform.localPosition.y, transform.localPosition.z);
        transform.localPosition -= new Vector3(0, GetComponent<RectTransform>().rect.height, 0);
        OffScreenPoint = new Vector3(transform.localPosition.x, (int)transform.localPosition.y, transform.localPosition.z);
        condition = 0;
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (!isMoving)
            {
                isMoving = true;
            }
        }
        if (isMoving)
        {
            if (condition == 0)
            {
                t += Time.deltaTime;
                var lerp =  Vector3.Lerp(transform.localPosition, OnScreenPoint, t);
                transform.localPosition = new Vector3(lerp.x, (int)lerp.y, lerp.z);
                if (transform.localPosition.Equals(OnScreenPoint))
                {
                    isMoving = false;
                    condition = 1;
                    t = 0;
                }
            }
            if (condition == 1)
            {
                t += Time.deltaTime;
                var lerp = Vector3.Lerp(transform.localPosition, OffScreenPoint, t);
                transform.localPosition = new Vector3(lerp.x, (int)lerp.y, lerp.z);
                if (transform.localPosition.Equals(OffScreenPoint))
                {
                    isMoving = false;
                    condition = 0;
                    t = 0;
                }
            }
        }
    }
}
