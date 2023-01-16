using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReindeerGhost : MonoBehaviour //Призрачный олень. 
{
    public float CurrentHorizontalVelocity { get; private set; } = 0;
    public float CurrentVerticalVelocity { get; private set; } = 0;
    private Rigidbody2D rigidbody;//его "физика", его "тело"
    private SpriteRenderer spriteRenderer;//отрисовщик спрайта, пока нужен только для отзеркаливания спрайта
    public float horizontalForceRatio = 1;
    private float shiftRatio = 1;
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
    private int previousDirection = 1;
    private List<GameObject> allAnotherPlatforms = new List<GameObject>();
    public GameObject currendGhostPlatform;
    private bool isFlying;
    public bool isCanMater = true;

    //public RuntimeAnimatorController stayAnimation;
    //public RuntimeAnimatorController walkAnimation;


    //private bool isStayAni = true;
    //private bool isWalkAni = false;

    //public bool isGrounded = true;
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
        allAnotherPlatforms.AddRange(GameObject.FindGameObjectsWithTag("GhostPlatform"));
    }

    void FixedUpdate()
    {
        if (isInWind)
        {
            rigidbody.AddForce(new Vector2(0, (windForceRatio * windVertical) / 150));
        }
    }

    void Update()
    {
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
            rigidbody.AddForce(new Vector2(0, 45));
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            isFlying = true;
        }
        if (isFlying && rigidbody.velocity.y <= 0)
        {
            rigidbody.gravityScale = 0.1f;
        }
        if (isFlying && rigidbody.velocity.y > 0)
        {
            rigidbody.gravityScale = 1f;
        }
        if (!isFlying)
        {
            rigidbody.gravityScale = 1f;
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            rigidbody.gravityScale = 1f;
            isFlying = false;
        }

        if (Input.GetKeyDown(KeyCode.D))//если нажали вправо, прибавить горизонтальную скорость вправо
        {
            CurrentHorizontalVelocity += 4;
            //horizontalForceRatio = 0;
        }
        if (Input.GetKeyUp(KeyCode.D))//если отпустили вправо, прибавить горизонтальную скорость влео
        {
            CurrentHorizontalVelocity += -4;
            //horizontalForceRatio = 0;
        }
        if (Input.GetKeyDown(KeyCode.A))//если нажали влево, прибавить горизонтальную скорость влево
        {
            CurrentHorizontalVelocity += -4;
            //horizontalForceRatio = 0;
        }
        if (Input.GetKeyDown(KeyCode.E) && currendGhostPlatform != null && isCanMater)//если нажали влево, прибавить горизонтальную скорость влево
        {
            currendGhostPlatform.GetComponent<Materialization>().makeMaterialisation();
            isCanMater = false;
        }
        if (Input.GetKeyUp(KeyCode.A))//если отпустили влево, прибавить горизонтальную скорость вправо
        {
            CurrentHorizontalVelocity += 4;
            //horizontalForceRatio = 0;
        }
        if ((Input.GetKeyDown(KeyCode.LeftShift) || isRunning) && deerUnity.GetComponent<DeerUnity>().currentStamina > 0 && DeerUnity.IsGrounded)
        {
            shiftRatio = 1.5f;
            isRunning = true;
            deerUnity.GetComponent<DeerUnity>().isRunning = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || !isRunning || (isRunning && deerUnity.GetComponent<DeerUnity>().currentStamina <= 0))
        {
            shiftRatio = 1;
            isRunning = false;
            deerUnity.GetComponent<DeerUnity>().isRunning = false;
        }
        if (DeerUnity.IsGrounded)
        {
            rigidbody.gravityScale = 1f;
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

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && CurrentHorizontalVelocity != 0)
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
        if (!((directionOfStack == -1 && CurrentHorizontalVelocity < 0) || (directionOfStack == 1 && CurrentHorizontalVelocity > 0)))
        {
            if (CurrentHorizontalVelocity > -0.01f && CurrentHorizontalVelocity < 0.01f)
            {
                horizontalForceRatio = 0;
            }
            var velocity = new Vector2(4 * direction * horizontalForceRatio * shiftRatio, rigidbody.velocity.y);
            if (isInWind)
            {
                velocity += new Vector2((windForceRatio * windHorizontal) / 1, 0);
            }
            rigidbody.velocity = velocity;
        }
        /*if (isInWind)
        {
            rigidbody.AddForce(new Vector2(0, (windForceRatio * windVertical) / 150));
        }*/

        CurrentVerticalVelocity = rigidbody.velocity.y;
        //в итоге получается результирующая горизонтальная скорость, ее мы и присваиваем, оставляя вертикальную скорость той же
        /*Не знаю как и почему, но присваивать скорость удобнее, чем добавлять силу, если речь идет о горизонтальном движении.
         * Присвоить скорость можно один раз и объект будет с этой скорстью двигаться, а вот силу нужно прикладывать и постоянно с каждый вызовом метода update(), т.к. сила затухает при одноразовом приложении.
         * Можешь попробовать сделать через приложении силы, а не через присвоение скорости, просто я так не умею.
         */
    }

    public void FlipPlayer()
    {
        if (direction < 0 && !spriteRenderer.flipX)//тут в зависимости от того, в какую сторону движется олень, зеркалится спрайт
        {
            spriteRenderer.flipX = true;//тут олень идет влево
        }
        if (direction > 0 && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;//тут вправо
        }
    }

    public void SetHorizontalVelocity(float velocity)
    {
        CurrentHorizontalVelocity = velocity;
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
