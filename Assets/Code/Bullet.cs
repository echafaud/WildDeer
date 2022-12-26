using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject deerUnity;
    private Vector2 velocity;
    private bool isShooted = false;
    private float sumVelocity = 10;
    private GameObject tilemap1;
    private List<GameObject> collapsingPlatforms = new List<GameObject>();
    void Start()
    {
        deerUnity = GameObject.Find("DeerUnity");
        this.gameObject.AddComponent<Timer>();
        GetComponent<Timer>().SetPeriodForTick(5f);

        tilemap1 = GameObject.Find("Tilemap1");

        collapsingPlatforms.AddRange(GameObject.FindGameObjectsWithTag("CollapsingPlat"));
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooted)
        {
            if (velocity.x == 0 && velocity.y == 0)
            {
                UpdateVelocity();
                GetComponent<Timer>().StartTimer();
            }
            GetComponent<Rigidbody2D>().velocity = velocity;
            if (GetComponent<CircleCollider2D>().IsTouching(deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().GetComponent<BoxCollider2D>()))
            {
                deerUnity.GetComponent<DeerUnity>().TakeDamage(25);
                Destroy(this.gameObject);
            }
            if (GetComponent<CircleCollider2D>().IsTouching(tilemap1.GetComponent<CompositeCollider2D>()) || GetComponent<Timer>().IsTicked())
            {
                Destroy(this.gameObject);
            }
            foreach (var e in collapsingPlatforms)
            {
                if (GetComponent<CircleCollider2D>().IsTouching(e.GetComponent<BoxCollider2D>()))
                {
                    e.SetActive(false);
                    Destroy(this.gameObject);
                }
            }
        }
    }

    public void GoToDeer()
    {
        isShooted = true;
    }

    private void UpdateVelocity()
    {
        var deer = deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer();
        var kx = deer.transform.position.x - transform.position.x;
        var ky = deer.transform.position.y - transform.position.y;
        var g = Mathf.Sqrt(kx * kx + ky * ky);
        var ratio = sumVelocity / g;
        velocity = new Vector2(kx * ratio, ky * ratio);
    }
}
