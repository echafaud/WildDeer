using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abyss : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D coll;
    private GameObject deerUnity;
    private Color defaultColor = new Color(0, 0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(0, 0, 0, 0);
        coll = GetComponent<BoxCollider2D>();
        deerUnity = GameObject.Find("DeerUnity");
    }

    // Update is called once per frame
    void Update()
    {
        var deer = deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer();
        if (coll.IsTouching(deer.GetComponent<BoxCollider2D>()))
        {
            sr.color = new Color(0, 0, 0, GetAlpha(deer.transform.position.y));
        }
        else
        {
            if (!sr.color.Equals(defaultColor))
            {
                sr.color = defaultColor;
            }
        }
    }

    private float GetAlpha(float deerY)
    {
        var thisHeight = transform.localScale.y;
        var startDarkingY = transform.position.y + (thisHeight / 3);
        var endDarkingY = transform.position.y - (thisHeight / 3);
        if (deerY > startDarkingY)
        {
            return 0;
        }
        var alpha = 1 - (deerY - endDarkingY) / (thisHeight / 3);
        if (deerY < endDarkingY)
        {
            deerUnity.GetComponent<DeerUnity>().TakeDamage(1000);
        }
        if (alpha < 0 || alpha > 1)
            return 0;
        return alpha;
    }
}
