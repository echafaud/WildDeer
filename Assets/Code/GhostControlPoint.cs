using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostControlPoint : MonoBehaviour
{
    private GameObject ghost;
    private GameObject ghostPoint;
    private bool isStartMoving = false;
    private bool isEnd = false;
    private Vector3 ghostStartPoint;
    private float t = 0;
    private bool isActivateCameraTiedBefore = false;
    private static GameObject active;
    // Start is called before the first frame update
    void Start()
    {
        ghost = transform.parent.parent.Find("Ghost").gameObject;
        ghostPoint = transform.parent.Find("GhostPoint").gameObject;
        ghostStartPoint = ghost.transform.position;
        if (active == null)
        {
            active = GameObject.Find("ActivatedSectionMap");
            Invoke("Disable", 3f);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isStartMoving && !isEnd)
        {
            t += Time.deltaTime / 7;
            ghost.transform.position = Vector3.Lerp(ghostStartPoint, ghostPoint.transform.position, t);
            if (ghost.transform.position.Equals(ghostPoint.transform.position))
            {
                GameObject.Find("DeerUnity").GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerSmall>().StartMoving();
                isEnd = true;
                Invoke("TurnOffCameraTiedGhost", 1f);
            }
        }
        
    }

    private void Disable()
    {
        active.SetActive(false);
    }

    private void TurnOffCameraTiedGhost()
    {
        DeerUnity.isCameraTiedGhost = false;
    }

    private void TurnOnCameraTiedGhost()
    {
        active.SetActive(true);
        DeerUnity.isCameraTiedGhost = true;
        //GameObject.Find("DeerUnity").GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerSmall>().isNeedToUpdatePlatformsList = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (!isStartMoving && collision.tag.Equals("Player"))
        {
            isStartMoving = true;
            ghostStartPoint = ghost.transform.position;
            var delta = ghostPoint.transform.position.x - ghostStartPoint.x;
            if (delta > 0)
            {
                ghost.GetComponent<SpriteRenderer>().flipX = false;
            }
            if (delta < 0)
            {
                ghost.GetComponent<SpriteRenderer>().flipX = true;
            }
            if (gameObject.transform.tag == "moveCamera" && !isActivateCameraTiedBefore)
            {
                GameObject.Find("DeerUnity").GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<ReindeerSmall>().StopMoving();
                isActivateCameraTiedBefore = true;
                TurnOnCameraTiedGhost();
                GameObject.Find("FirstGroupHints").SetActive(false);
                GameObject.Find("MovingPlatforms").SetActive(false);  
            }
        }
    }
}
