using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReindeerBig : MonoBehaviour //большой олень. Пока полностью совпадает с призрачным, кроме сил передвижения. Подробно все описано в призрачном
{
    public float CurrentHorizontalVelocity { get; private set; } = 0;
    public float CurrentVerticalVelocity { get; private set; } = 0;
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    public float horizontalForceRatio = 1;
    private int shiftRatio = 1;
    public bool isRunning = false;
    private GameObject deerUnity;
    private bool isTrapped = false;
    private int countJumpsToEscape = 0;
    private float normalGravityScale = 0;
    public int direction = 1;
    //public bool isGrounded = true;
    private bool isInWind = false;
    private float windHorizontal = 0;
    private float windVertical = 0;
    private float windForceRatio = 0;
    private int directionOfStack = 0;
    private GameObject tilemap1;
    private bool isStacked = false;
    private int previousDirection = 1;
    private List<GameObject> allAnotherPlatforms = new List<GameObject>();
    public int lungeImpulse;
    public static bool isLunge;
    private GameObject trapTriggerLeft;
    private GameObject trapTriggerRight;
    public GameObject CurrentActiveTrapTrigger;

    //public RuntimeAnimatorController stayAnimation;
    //public RuntimeAnimatorController walkAnimation;


    //private bool isStayAni = true;
    //private bool isWalkAni = false;
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
        lungeImpulse = 1000;
        isLunge = false;

        trapTriggerLeft = transform.Find("TrapTriggerLeft").gameObject;
        trapTriggerRight = transform.Find("TrapTriggerRight").gameObject;
        CurrentActiveTrapTrigger = trapTriggerLeft;
    }

    void FixedUpdate()
    {
        if (isInWind)
        {
            rigidbody.AddForce(new Vector2(0, (windForceRatio * windVertical) / 25));
        }
    }

    void Update()
    {
        LungeToDestroid();
        CheckIsStucked();
        MakeAction();
        FlipPlayer();

        /*if (isStayAni && horizontalForceRatio != 0 && CurrentHorizontalVelocity != 0 && DeerUnity.IsGrounded)
        {
            isStayAni = false;
            isWalkAni = true;
            GetComponent<Animator>().runtimeAnimatorController = walkAnimation;
        }
        else if (isWalkAni && (horizontalForceRatio == 0 || CurrentHorizontalVelocity == 0 || !DeerUnity.IsGrounded))
        {
            isStayAni = true;
            isWalkAni = false;
            GetComponent<Animator>().runtimeAnimatorController = stayAnimation;
        }*/
    }

    private void CheckIsStucked()
    {
        if (!isStacked && !DeerUnity.IsGrounded
            && (GetComponent<BoxCollider2D>().IsTouching(tilemap1.GetComponent<CompositeCollider2D>()) || isTouchingAnythingElse()))
        {
            directionOfStack = direction;
            isStacked = true;
        }
        else if (isStacked
            && (DeerUnity.IsGrounded
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
        if (Input.GetKeyDown(KeyCode.Space) && DeerUnity.IsGrounded)
        {
            if (!isTrapped)
            {
                rigidbody.AddForce(new Vector2(0, 240));
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTrapped && countJumpsToEscape > 0)
            {
                countJumpsToEscape--;
                deerUnity.GetComponent<DeerUnity>().TakeDamage(10);
                if (countJumpsToEscape <= 0)
                {
                    EscapedTrap();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            CurrentHorizontalVelocity += 4;
            //horizontalForceRatio = 0;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            CurrentHorizontalVelocity += -4;
            //horizontalForceRatio = 0;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            CurrentHorizontalVelocity += -4;
            //horizontalForceRatio = 0;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            CurrentHorizontalVelocity += 4;
            //horizontalForceRatio = 0;
        }
        if ((Input.GetKeyDown(KeyCode.LeftShift) || isRunning) && deerUnity.GetComponent<DeerUnity>().currentStamina > 0 && DeerUnity.IsGrounded)
        {
            shiftRatio = 2;
            isRunning = true;
            deerUnity.GetComponent<DeerUnity>().isRunning = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || !isRunning || (isRunning && deerUnity.GetComponent<DeerUnity>().currentStamina <= 0))
        {
            shiftRatio = 1;
            isRunning = false;
            deerUnity.GetComponent<DeerUnity>().isRunning = false;
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

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && CurrentHorizontalVelocity != 0 )
        {
            CurrentHorizontalVelocity = 0;
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
        
        if (!isTrapped)
        {
            if (!((directionOfStack == -1 && CurrentHorizontalVelocity < 0) || (directionOfStack == 1 && CurrentHorizontalVelocity > 0)))
            {
                if (CurrentHorizontalVelocity > -0.01f && CurrentHorizontalVelocity < 0.01f)
                {
                    horizontalForceRatio = 0;
                }
                var velocity = new Vector2(4 * direction * horizontalForceRatio * shiftRatio, rigidbody.velocity.y);
                if (isInWind)
                {
                    velocity += new Vector2((windForceRatio * windHorizontal) / 5, 0);
                }
                rigidbody.velocity = velocity;
            }
           /* if (isInWind)
            {
                rigidbody.AddForce(new Vector2(0, (windForceRatio * windVertical) / 25));
            }*/

            CurrentVerticalVelocity = rigidbody.velocity.y;
        }
        else if (isTrapped)
        {
            rigidbody.velocity = new Vector2(0, 0);
        }
    }

    public void LungeToDestroid()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            rigidbody.velocity = new Vector2(0, 0);
            isLunge = true;
            Invoke("ChangeLunge", 3f);
            if (direction < 0)
            {
                rigidbody.AddForce(Vector2.left * lungeImpulse, 0);
            }
            else
            {
                rigidbody.AddForce(Vector2.right * lungeImpulse, 0);
            }
        }
    }

    void ChangeLunge()
    {
        isLunge = false;
    }
    public void FlipPlayer()
    {
        if (direction < 0 && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
            CurrentActiveTrapTrigger = trapTriggerRight;
        }
        if (direction > 0 && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
            CurrentActiveTrapTrigger = trapTriggerLeft;
        }
    }

    public void SetVerticalVelocity(float velocity)
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, velocity);
    }

    /*public static void SetGorizontalForce()
    {

    }*/

    public void SetHorizontalVelocity(float velocity)
    {
        CurrentHorizontalVelocity = velocity;
    }

    public void Trapped()
    {
        isTrapped = true;
        countJumpsToEscape = 5;
        normalGravityScale = rigidbody.gravityScale;
        rigidbody.gravityScale = 0;
        rigidbody.velocity = new Vector2(0, 0);
    }

    public void EscapedTrap()
    {
        countJumpsToEscape = 0;
        isTrapped = false;
        rigidbody.gravityScale = normalGravityScale;
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
}
