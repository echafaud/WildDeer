using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HunterMode
{
    Chasing,
    Searching
}

public class Hunter : MonoBehaviour
{
    public float CurrentHorizontalVelocity { get; private set; } = 0;
    public float CurrentVerticalVelocity { get; private set; } = 0;
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    public float horizontalForceRatio = 1;
    private int shiftRatio = 1;
    public bool isRunning = false;
    private GameObject deerUnity;
    public int direction = 1;
    private bool isInWind = false;
    private float windHorizontal = 0;
    private float windVertical = 0;
    private float windForceRatio = 0;
    private int directionOfStack = 0;
    private GameObject tilemap1;
    private bool isStacked = false;
    private List<GameObject> allAnotherPlatforms = new List<GameObject>();
    private float previousTime;
    private bool isGrounded = true;
    private bool isCanMoving = true;
    private float previousHorizontalVelocity;
    private float shootTime = 0;
    private float standingTime = 0;
    private HunterMode mode;
    private GameObject searchingCentre;
    private bool isAlreadyStanded = false;

    private GameObject rightWallChecker;
    private GameObject leftWallChecker;
    private bool isStayAtPoint = false;
    public bool isEnabled;
    public AudioClip shootSound;
    private HunterMode previousHunterMode = HunterMode.Searching;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.gameObject.AddComponent<Timer>();
        GetComponent<Timer>().SetPeriodForTick(0.1f);
        GetComponent<Timer>().StartTimer();
        deerUnity = GameObject.Find("DeerUnity");

        tilemap1 = GameObject.Find("Tilemap1");

        allAnotherPlatforms.AddRange(GameObject.FindGameObjectsWithTag("CollapsingPlat"));
        allAnotherPlatforms.AddRange(GameObject.FindGameObjectsWithTag("Platform"));

        rightWallChecker = transform.Find("RightWallChecker").gameObject;
        leftWallChecker = transform.Find("LeftWallChecker").gameObject;
        searchingCentre = GameObject.Find("HunterKit1").transform.Find("SearchingCentre").gameObject;

        mode = HunterMode.Searching;
    }

    void Update()
    {
        CheckIsStucked();
        MakeAction();
        FlipPlayer();
        
        if(previousHunterMode == HunterMode.Searching && mode == HunterMode.Chasing)
        {
            deerUnity.GetComponent<DeerUnity>().PlayHunterTheme();
        }
        previousHunterMode = mode;
        isGrounded = transform.Find("Ground").GetComponent<BoxCollider2D>().IsTouching(tilemap1.GetComponent<CompositeCollider2D>());
    }

    private void CheckIsStucked()
    {
        if (!isStacked && !isGrounded
            && (GetComponent<BoxCollider2D>().IsTouching(tilemap1.GetComponent<CompositeCollider2D>()) || isTouchingAnythingElse()))
        {
            directionOfStack = direction;
            isStacked = true;
        }
        else if (isStacked
            && (isGrounded
            || (!GetComponent<BoxCollider2D>().IsTouching(tilemap1.GetComponent<CompositeCollider2D>()) && !isTouchingAnythingElse())))
        {
            isStacked = false;
            directionOfStack = 0;
        }
    }

    private bool isTouchingAnythingElse()
    {
        foreach (var obj in allAnotherPlatforms)
        {
            var vector = transform.position - obj.transform.position;
            if (vector.x < 10 && vector.x > -10 && vector.y > -10 && vector.y < 10)
            {
                if (GetComponent<BoxCollider2D>().IsTouching(obj.GetComponent<BoxCollider2D>()))
                    return true;
            }
        }
        return false;
    }

    public void MakeAction()
    {
        if (mode == HunterMode.Searching)
        {
            var deltaX = transform.position.x - searchingCentre.transform.position.x;

            //GameObject.Find("Info").GetComponent<Text>().text = (delta).ToString();
            if (deltaX > 15)
            {
                if (standingTime == 0 && !isAlreadyStanded)
                {
                    standingTime = GetComponent<Timer>().GetTime();
                    isAlreadyStanded = true;
                    StopMoving();
                }
                else if (GetComponent<Timer>().GetTime() - standingTime > 2)
                {
                    GoLeft();
                    standingTime = 0;
                }

            }
            else if (deltaX < -15)
            {
                if (standingTime == 0 && !isAlreadyStanded)
                {
                    standingTime = GetComponent<Timer>().GetTime();
                    isAlreadyStanded = true;
                    StopMoving();
                }
                else if (GetComponent<Timer>().GetTime() - standingTime > 2)
                {
                    GoRight();
                    standingTime = 0;
                }

            }
            else if (isAlreadyStanded)
            {
                isAlreadyStanded = false;
            }


            deltaX = deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().transform.position.x - transform.position.x;
            var deltaY = Math.Abs(deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().transform.position.y - transform.position.y);
            if (deltaX > 15 || deltaX < -15)
            {
                Run();
            }
            else
            {
                StopRunning();
            }
            if (deltaY < 10)
            {
                if (deltaX > -5 && deltaX < 5 && !deerUnity.GetComponent<DeerUnity>().isBushed)
                {
                    mode = HunterMode.Chasing;
                    
                }
            }
        }
        else if (mode == HunterMode.Chasing)
        {
            var delta = deerUnity.GetComponent<DeerUnity>().GetCurrentActiveDeer().transform.position.x - transform.position.x;
            if (delta > 0)
            {
                direction = 1;
            }
            else if (delta < 0)
            {
                direction = -1;
            }
            if (delta > 5 && isCanMoving && !isStayAtPoint)
            {
                GoRight();
            }
            else if (delta < -5 && isCanMoving && !isStayAtPoint)
            {
                GoLeft();
            }
            else if (delta > -4 && delta < 4)
            {
                StopMoving();
            }

            if ((delta > 10 || delta < -10) && !isStayAtPoint)
            {
                Run();
            }
            else if (isRunning)
            {
                StopRunning();
            }

            if (delta > -7 && delta < 7)
            {
                if (GetComponent<Timer>().GetTime() - shootTime > 3f)
                {
                    Shoot();
                    shootTime = GetComponent<Timer>().GetTime();
                }
            }
        }
        

        if ((direction > 0 && rightWallChecker.GetComponent<BoxCollider2D>().IsTouching(tilemap1.GetComponent<CompositeCollider2D>()))
            || (direction < 0 && leftWallChecker.GetComponent<BoxCollider2D>().IsTouching(tilemap1.GetComponent<CompositeCollider2D>())))
        {
            if (previousTime == 0)
            {
                isCanMoving = false;
                StopMoving();
                Jump();
                previousTime = GetComponent<Timer>().GetTime();
            }
            if (previousTime != 0 && GetComponent<Timer>().GetTime() - previousTime > 1)
            {
                Jump();
                previousTime = GetComponent<Timer>().GetTime();
            }
        }
        else if (!isCanMoving)
        {
            isCanMoving = true;
            StartMoving();
            previousTime = 0;
        }
        

        var previousDirection = direction;
        if (CurrentHorizontalVelocity > 0)
        {
            direction = 1;
        }
        else if (CurrentHorizontalVelocity < 0)
        {
            direction = -1;
        }
        if (previousDirection != direction)
        {
            horizontalForceRatio = 0;
        }

        if (GetComponent<Timer>().IsTicked())
        {
            if (horizontalForceRatio < 1 && CurrentHorizontalVelocity != 0)
            {
                horizontalForceRatio += 0.2f;
                if (horizontalForceRatio > 1)
                {
                    horizontalForceRatio = 1;
                }
            }
            if (horizontalForceRatio > 0 && CurrentHorizontalVelocity == 0)
            {
                horizontalForceRatio -= 0.33f;
                if (horizontalForceRatio < 0)
                {
                    horizontalForceRatio = 0;
                }
            }

            if (isInWind && windForceRatio != 1)
            {
                windForceRatio += 0.1f;
                if (windForceRatio >= 1)
                {
                    windForceRatio = 1;
                }
            }
            if (!isInWind && windForceRatio > 0)
            {
                windForceRatio -= 0.1f;
                if (windForceRatio <= 0)
                {
                    windForceRatio = 0;
                }
            }
        }

        if (!((directionOfStack == -1 && CurrentHorizontalVelocity < 0) || (directionOfStack == 1 && CurrentHorizontalVelocity > 0)))
        {
            if (CurrentHorizontalVelocity > -0.01f && CurrentHorizontalVelocity < 0.01f)
            {
                horizontalForceRatio = 0;
            }
            var velocity = new Vector2(6 * direction * horizontalForceRatio * shiftRatio, rigidbody.velocity.y);
            if (isInWind)
            {
                velocity += new Vector2((windForceRatio * windHorizontal) / 5, 0);
            }
            rigidbody.velocity = velocity;
        }
        if (isInWind)
        {
            rigidbody.AddForce(new Vector2(0, (windForceRatio * windVertical) / 25));
        }
    }

    public void StayAtPoint(Transform tr)
    {
        mode = HunterMode.Chasing;
        isStayAtPoint = true;
        StopMoving();
        transform.position = tr.position;
    }

    public void HuntDeerAtPoint(Transform tr)
    {
        mode = HunterMode.Chasing;
        isStayAtPoint = false;
        transform.position = tr.position;
    }

    private void GoRight()
    {
        CurrentHorizontalVelocity = 4;
        previousHorizontalVelocity = CurrentHorizontalVelocity;
    }

    private void GoLeft()
    {
        CurrentHorizontalVelocity = -4;
        previousHorizontalVelocity = CurrentHorizontalVelocity;
    }

    private void StopMoving()
    {
        previousHorizontalVelocity = CurrentHorizontalVelocity;
        CurrentHorizontalVelocity = 0;
        //isCanMoving = false;
    }

    private void StartMoving()
    {
        CurrentHorizontalVelocity = previousHorizontalVelocity;
        //isCanMoving = true;
    }

    private void Jump()
    {
        rigidbody.AddForce(new Vector2(0, 250));
    }

    private void Run()
    {
        shiftRatio = 2;
        isRunning = true;
    }

    private void StopRunning()
    {
        shiftRatio = 1;
        isRunning = false;
    }

    private void Shoot()
    {
        var audio = GetComponent<AudioSource>();
        if (DeerUnity.VolumeRatio == 0)
        {
            audio.volume = 0;
        }
        else
        {
            audio.volume = 0.1f;
        }
        audio.PlayOneShot(shootSound);
        var newBullet = GameObject.Instantiate(GameObject.Find("HunterKit1").transform.Find("Bullet").gameObject, transform.position, transform.rotation);
        newBullet.GetComponent<Bullet>().GoToDeer();
    }

    public void FlipPlayer()
    {
        if (direction < 0 && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
        }
        if (direction > 0 && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void InWind(float windHorizontalVelocity, float windVerticalForce)
    {
        isInWind = true;
        windHorizontal = windHorizontalVelocity;
        windVertical = windVerticalForce;
    }

    public void WindOut()
    {
        isInWind = false;
    }

    public void DisableHunter()
    {
        deerUnity.GetComponent<DeerUnity>().PlayMainTheme();
    }
}
