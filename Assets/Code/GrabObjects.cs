using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class GrabObjects : MonoBehaviour
{
    [SerializeField]
    private Transform secondRayPoint;

    [SerializeField]
    private Transform rayPoint;
    [SerializeField]
    private float rayDistance;

    private GameObject grabbedObject;
    private int layerIndex;

    void Start()
    {
        layerIndex = LayerMask.NameToLayer("MoveObject");
    }

    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, transform.right, rayDistance);
        RaycastHit2D secondHitInfo = Physics2D.Raycast(secondRayPoint.position, transform.right, rayDistance);

        if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == layerIndex)
        {
            if (Input.GetKeyDown(KeyCode.X) && grabbedObject == null)
            {
                grabbedObject = hitInfo.collider.gameObject;
                grabbedObject.GetComponent<Rigidbody2D>().mass = 10;
            }
            else if (Input.GetKeyUp(KeyCode.X) && grabbedObject != null)
            {
                grabbedObject.GetComponent<Rigidbody2D>().mass = 1000;
                grabbedObject = null;
            }
        }
        else if (secondHitInfo.collider != null && secondHitInfo.collider.gameObject.layer == layerIndex)
        {
            if (Input.GetKeyDown(KeyCode.X) && grabbedObject == null)
            {
                grabbedObject = secondHitInfo.collider.gameObject;
                grabbedObject.GetComponent<Rigidbody2D>().mass = 10;
            }
            else if (Input.GetKeyUp(KeyCode.X) && grabbedObject != null)
            {
                grabbedObject.GetComponent<Rigidbody2D>().mass = 1000;
                grabbedObject = null;
            }
        }
        //if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == layerIndex)
        //{

        //    if (Input.GetKey(KeyCode.LeftAlt) && grabbedObject == null)
        //    {
        //        grabbedObject = hitInfo.collider.gameObject;
        //        grabbedObject.GetComponent<Rigidbody2D>().isKinematic = true;
        //        grabbedObject.transform.SetParent(transform);
        //    }
        //    else if (Input.GetKey(KeyCode.LeftAlt))
        //    {
        //        grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
        //        grabbedObject.transform.SetParent(null);
        //        grabbedObject = null;
        //    }
        //    Debug.DrawRay(rayPoint.position, transform.right * rayDistance);
        //}
        //else if (secondHitInfo.collider != null && secondHitInfo.collider.gameObject.layer == layerIndex)
        //{
        //    if (Input.GetKey(KeyCode.LeftAlt) && grabbedObject == null)
        //    {
        //        grabbedObject = secondHitInfo.collider.gameObject;
        //        grabbedObject.GetComponent<Rigidbody2D>().isKinematic = true;
        //        grabbedObject.transform.SetParent(transform);
        //    }
        //    else if (Input.GetKey(KeyCode.LeftAlt))
        //    {
        //        grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
        //        grabbedObject.transform.SetParent(null);
        //        grabbedObject = null;
        //    }
        //    Debug.DrawRay(secondRayPoint.position, transform.right * rayDistance);
        //}
        //else
        //{
        //    Debug.DrawRay(rayPoint.position, transform.right * rayDistance);
        //}
    }
}
