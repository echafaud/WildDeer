using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DeerUnity : MonoBehaviour //класс, объедин€ющий всех оленей и отвечающий за переключение между ними. ≈го координата "x" всегда совпадает с координатой "x" у текущего олен€
{// он €вл€етс€ просто св€зующим звеном, его не видно на камере и у него нет физики
    private GameObject reindeerGhost;
    private GameObject reindeerSmall;
    private GameObject reindeerBig;
    private GameObject currentActiveDeer;
    private GameObject stamina;
    private float unityGhostDeltaY;//дельта нужна, чтобы зафиксировать оленей на одной высоте при переключении между ними
    private float unitySmallDeltaY;//т.к. олень, на которого переключились, по€вл€етс€ на месте объекта Unity, то переключившись с маленького на большого,
    private float unityBigDeltaY;//большой может застр€ть в текстурах, из-за того, что он больше
    private CompositeCollider2D tileMapCollider1;
    private CompositeCollider2D tileMapCollider2;
    public GameObject spawn;
    private bool isCanSwitch = true;
    public bool isOnPlatform = false;
    public bool isOnGhostPlatform = false;
    public bool isOnMovePlatform = false;
    public GameObject ghostCheckpoint;
    public static bool isCameraTiedGhost;
    public bool isRunning;
    private float previousX;
    private bool isDead = false;

    public bool isFirstDeerAvailable;
    public bool isSecondDeerAvailable;
    public bool isThirdDeerAvailable;

    public static float VolumeRatio = 0.5f;

    public bool isBushed { get; private set; } = false;
    public static int CurrentActive { get; private set; }
    public static bool IsGrounded { get; private set; } = true;

    public AudioClip mainTheme;
    public AudioClip hunterTheme;

    private bool isDamage;

    [SerializeField] private Slider sliderHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private float minHealth;

    [Space(10)]
    [SerializeField] private Slider sliderStamina;
    [SerializeField] public float currentStamina;
    [SerializeField] private float maxStamina;
    [SerializeField] private float minStamina;

    [Space(10)]
    [SerializeField] private Slider sliderCooling;
    [SerializeField] public float currentCooling;
    [SerializeField] private float maxCooling;
    [SerializeField] private float minCooling;

    public bool isActivateCooling;

    public int countOfFoundTracks;
    public static int countOfFoundLemmings;

    public GameObject inGameMenu;
    private float maxFallVelocity = 0;

    public GameObject firstAbility;
    public GameObject secondAbility;

    public Sprite smallFirstAbil;
    public Sprite smallFirstAbilActive;
    public Sprite smallSecondAbil;
    public Sprite smallSecondAbilActive;

    public Sprite ghostFirstAbil;
    public Sprite ghostFirstAbilActive;
    public Sprite ghostSecondAbil;
    public Sprite ghostSecondAbilActive;

    public Sprite bigFirstAbil;
    public Sprite bigFirstAbilActive;
    public Sprite bigSecondAbil;
    public Sprite bigSecondAbilActive;

    public GameObject TaskMenuParent;
    public GameObject TaskMenu;
    private HashSet<int> indexOfCurrentTasks = new HashSet<int>();
    private string[] Tasks = new string[] { "«адани€\n",
        "—обрать все следы (0/4)\n",
        "ѕробратьс€ мимо охотника\n",
        "”бежать от охотника\n",
        "—ломать проход пулей охотника\n",
        "ѕереключитьс€ за духа и преодолеть пропасть\n",
        "»спользу€ способность духа \"материализаци€\" пройти дальше\n",
        "»спользу€ способность духа \"парение\" спуститьс€ вниз\n"};
    public GameObject Head;

    private float defaultSize;
    public GameObject completeWindow;

    public GameObject backgroundChecker;

    public GameObject smokeFront1;
    public GameObject smokeFront2;
    public GameObject smokeFront3;
    private GameObject[] smokesFront;

    public GameObject smokeMid1;
    public GameObject smokeMid2;
    public GameObject smokeMid3;
    private GameObject[] smokesMid;

    public GameObject smokeBack1;
    public GameObject smokeBack2;
    public GameObject smokeBack3;
    private GameObject[] smokesBack;

    public GameObject midFront1;
    public GameObject midFront2;
    public GameObject midFront3;
    private GameObject[] midesFront;

    public GameObject midMid1;
    public GameObject midMid2;
    public GameObject midMid3;
    private GameObject[] midesMid;

    public GameObject midBack1;
    public GameObject midBack2;
    public GameObject midBack3;
    private GameObject[] midesBack;

    public GameObject back1;
    public GameObject back2;
    public GameObject back3;
    private GameObject[] backs;

    private float xDelSmokeFront;
    private float xDelSmokeMid;
    private float xDelSmokeBack;
    private float xDelMidFront;
    private float xDelMidMid;
    private float xDelMidBack;
    private float xDelBack;

    public GameObject backgroundOfTimer;
    public GameObject textTimer;
    private bool isGhostOn;
    private float time;
    private float lastTime;
    public float ghostActiveTime;
    public float ghostInactiveTime;
    private bool isCanSwitchOnGhost;

    // Start is called before the first frame update
    void Start()
    {
        lastTime = ghostActiveTime;
        isCanSwitchOnGhost = true;
        textTimer.GetComponent<Text>().text = "";
        backgroundOfTimer.GetComponent<Image>().fillAmount = 0;
        //defaultSize = TaskMenuParent.GetComponent<RectTransform>().sizeDelta.y;
        defaultSize = 65;
        //TaskMenuParent.transform.Find("TextBackground").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        //TaskMenu.GetComponent<Text>().color = new Color(0, 0, 0, 0);
        reindeerGhost = GameObject.Find("ReindeerGhost");
        reindeerSmall = GameObject.Find("ReindeerSmall");//тут находим трех оленей на сцене
        reindeerBig = GameObject.Find("ReindeerBig");
        ghostCheckpoint = GameObject.Find("Ghost");
        stamina = GameObject.Find("Stamina");
        isCameraTiedGhost = false;
        unityGhostDeltaY = transform.position.y - reindeerGhost.transform.position.y;
        unitySmallDeltaY = transform.position.y - reindeerSmall.transform.position.y;//тут находим разницу по "у" между оленем и объектом unity (важно,
        unityBigDeltaY = transform.position.y - reindeerBig.transform.position.y;//чтобы нижн€€ граница коллидера всех оленей была на одном уровне, иначе дельты будут не правильными)
        reindeerGhost.SetActive(false);
        reindeerBig.SetActive(false);
        currentActiveDeer = reindeerSmall;
        CurrentActive = 1;
        transform.position = reindeerSmall.transform.position;

        sliderStamina.maxValue = maxStamina;
        sliderStamina.minValue = minStamina;
        currentStamina = maxStamina;

        sliderHealth.maxValue = maxHealth;
        sliderHealth.minValue = minHealth;
        currentHealth = maxHealth;

        sliderCooling.maxValue = maxCooling;
        sliderCooling.minValue = minCooling;
        currentCooling = maxCooling;
        isDamage = true;
        countOfFoundTracks = 0;
        countOfFoundLemmings = 0;
        //isFirstDeerAvailable = true;
        //isSecondDeerAvailable = true;
        //isThirdDeerAvailable = true;

        //isActivateCooling = true;
        tileMapCollider1 = GameObject.Find("Tilemap1").GetComponent<CompositeCollider2D>();
        tileMapCollider2 = GameObject.Find("Tilemap2").GetComponent<CompositeCollider2D>();

        spawn = GameObject.Find("Spawn");
        MoveAllDeersToSpawn();
        ActivateCooling();

        if (SaveManager.LastCheckPointName != null)
        {
            GameObject.Find(SaveManager.LastCheckPointName).GetComponent<CheckPoint>().isReached = true;
        }

        previousX = transform.position.x;
        smokesFront = new GameObject[] { smokeFront1, smokeFront2, smokeFront3 };
        smokesMid = new GameObject[] { smokeMid1, smokeMid2, smokeMid3 };
        smokesBack = new GameObject[] { smokeBack1, smokeBack2, smokeBack3 };

        midesFront = new GameObject[] { midFront1, midFront2, midFront3 };
        midesMid = new GameObject[] { midMid1, midMid2, midMid3 };
        midesBack = new GameObject[] { midBack1, midBack2, midBack3 };

        backs = new GameObject[] { back1, back2, back3 };

        xDelSmokeFront = smokesFront[1].transform.localPosition.x - smokesFront[2].transform.localPosition.x;
        xDelSmokeMid = smokesMid[1].transform.localPosition.x - smokesMid[2].transform.localPosition.x;
        xDelSmokeBack = smokesBack[1].transform.localPosition.x - smokesBack[2].transform.localPosition.x;
        xDelMidFront = midesFront[1].transform.localPosition.x - midesFront[2].transform.localPosition.x;
        xDelMidMid = midesMid[1].transform.localPosition.x - midesMid[2].transform.localPosition.x;
        xDelMidBack = midesBack[1].transform.localPosition.x - midesBack[2].transform.localPosition.x;
        xDelBack = backs[1].transform.localPosition.x - backs[2].transform.localPosition.x;

        TaskMenuParent.transform.Find("TextBackground").GetComponent<Image>().color = new Color(0, 0, 0, 0);
        Head.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        TaskMenu.GetComponent<Text>().text = "";
    }

    // Update is called once per frame
    void Update()
    {
        HealthChecked();
        StaminaChecked();
        StaminaKeys();
        PositionDeerUnity();
        ActivateDeer();
        UpdateIsOnGround();
        if (isActivateCooling)
        {
            Shadow();
        }
        //HealHealth();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inGameMenu.activeSelf)
            {
                inGameMenu.SetActive(false);
            }
            else
            {
                inGameMenu.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            OnE();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            OnAlt();
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            OffE();
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            OffAlt();
        }

        if (Math.Abs(transform.position.x - previousX) < 5)
        {
            smokesFront[0].transform.parent.gameObject.transform.localPosition -= new Vector3(2 * (transform.position.x - previousX), 0, 0);
            smokesMid[0].transform.parent.gameObject.transform.localPosition -= new Vector3(1.5f * (transform.position.x - previousX), 0, 0);
            smokesBack[0].transform.parent.gameObject.transform.localPosition -= new Vector3(1 * (transform.position.x - previousX), 0, 0);

            midesFront[0].transform.parent.gameObject.transform.localPosition -= new Vector3(0.5f * (transform.position.x - previousX), 0, 0);
            midesMid[0].transform.parent.gameObject.transform.localPosition -= new Vector3(0.33f * (transform.position.x - previousX), 0, 0);
            midesBack[0].transform.parent.gameObject.transform.localPosition -= new Vector3(0.25f * (transform.position.x - previousX), 0, 0);

            backs[0].transform.parent.gameObject.transform.localPosition -= new Vector3(0.1f * (transform.position.x - previousX), 0, 0);
        }


        previousX = transform.position.x;

        /////////////////////////////
        if (backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(smokesFront[0].GetComponent<BoxCollider2D>())
            && !backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(smokesFront[1].GetComponent<BoxCollider2D>()))
        {
            smokesFront[2].transform.localPosition += new Vector3(3 * xDelSmokeFront, 0, 0);
            var newSmokes = new GameObject[] { smokesFront[2], smokesFront[0], smokesFront[1] };
            smokesFront = newSmokes;
        }

        if (backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(smokesFront[2].GetComponent<BoxCollider2D>())
            && !backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(smokesFront[1].GetComponent<BoxCollider2D>()))
        {
            smokesFront[0].transform.localPosition += new Vector3(-3 * xDelSmokeFront, 0, 0);
            var newSmokes = new GameObject[] { smokesFront[1], smokesFront[2], smokesFront[0] };
            smokesFront = newSmokes;
        }
        ////////////////////////////////////////

        if (backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(smokesMid[0].GetComponent<BoxCollider2D>())
            && !backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(smokesMid[1].GetComponent<BoxCollider2D>()))
        {
            smokesMid[2].transform.localPosition += new Vector3(3 * xDelSmokeMid, 0, 0);
            var newSmokes = new GameObject[] { smokesMid[2], smokesMid[0], smokesMid[1] };
            smokesMid = newSmokes;
        }

        if (backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(smokesMid[2].GetComponent<BoxCollider2D>())
            && !backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(smokesMid[1].GetComponent<BoxCollider2D>()))
        {
            smokesMid[0].transform.localPosition += new Vector3(-3 * xDelSmokeMid, 0, 0);
            var newSmokes = new GameObject[] { smokesMid[1], smokesMid[2], smokesMid[0] };
            smokesMid = newSmokes;
        }
        //////////////////////////////////////

        if (backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(smokesBack[0].GetComponent<BoxCollider2D>())
            && !backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(smokesBack[1].GetComponent<BoxCollider2D>()))
        {
            smokesBack[2].transform.localPosition += new Vector3(3 * xDelSmokeBack, 0, 0);
            var newSmokes = new GameObject[] { smokesBack[2], smokesBack[0], smokesBack[1] };
            smokesBack = newSmokes;
        }

        if (backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(smokesBack[2].GetComponent<BoxCollider2D>())
            && !backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(smokesBack[1].GetComponent<BoxCollider2D>()))
        {
            smokesBack[0].transform.localPosition += new Vector3(-3 * xDelSmokeBack, 0, 0);
            var newSmokes = new GameObject[] { smokesBack[1], smokesBack[2], smokesBack[0] };
            smokesBack = newSmokes;
        }
        ////////////////////////////////////
        if (backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(midesFront[0].GetComponent<BoxCollider2D>())
            && !backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(midesFront[1].GetComponent<BoxCollider2D>()))
        {
            midesFront[2].transform.localPosition += new Vector3(3 * xDelMidFront, 0, 0);
            var newMides = new GameObject[] { midesFront[2], midesFront[0], midesFront[1] };
            midesFront = newMides;
        }

        if (backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(midesFront[2].GetComponent<BoxCollider2D>())
            && !backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(midesFront[1].GetComponent<BoxCollider2D>()))
        {
            midesFront[0].transform.localPosition += new Vector3(-3 * xDelMidFront, 0, 0);
            var newMides = new GameObject[] { midesFront[1], midesFront[2], midesFront[0] };
            midesFront = newMides;
        }
        ////////////////////////////////////////

        if (backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(midesMid[0].GetComponent<BoxCollider2D>())
            && !backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(midesMid[1].GetComponent<BoxCollider2D>()))
        {
            midesMid[2].transform.localPosition += new Vector3(3 * xDelMidMid, 0, 0);
            var newMides = new GameObject[] { midesMid[2], midesMid[0], midesMid[1] };
            midesMid = newMides;
        }

        if (backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(midesMid[2].GetComponent<BoxCollider2D>())
            && !backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(midesMid[1].GetComponent<BoxCollider2D>()))
        {
            midesMid[0].transform.localPosition += new Vector3(-3 * xDelMidMid, 0, 0);
            var newMides = new GameObject[] { midesMid[1], midesMid[2], midesMid[0] };
            midesMid = newMides;
        }
        //////////////////////////////////////

        if (backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(midesBack[0].GetComponent<BoxCollider2D>())
            && !backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(midesBack[1].GetComponent<BoxCollider2D>()))
        {
            midesBack[2].transform.localPosition += new Vector3(3 * xDelMidBack, 0, 0);
            var newMides = new GameObject[] { midesBack[2], midesBack[0], midesBack[1] };
            midesBack = newMides;
        }

        if (backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(midesBack[2].GetComponent<BoxCollider2D>())
            && !backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(midesBack[1].GetComponent<BoxCollider2D>()))
        {
            midesBack[0].transform.localPosition += new Vector3(-3 * xDelMidBack, 0, 0);
            var newMides = new GameObject[] { midesBack[1], midesBack[2], midesBack[0] };
            midesBack = newMides;
        }
        ////////////////////////////////////
        if (backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(backs[0].GetComponent<BoxCollider2D>())
           && !backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(backs[1].GetComponent<BoxCollider2D>()))
        {
            backs[2].transform.localPosition += new Vector3(3 * xDelBack, 0, 0);
            var newBacks = new GameObject[] { backs[2], backs[0], backs[1] };
            backs = newBacks;
        }

        if (backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(backs[2].GetComponent<BoxCollider2D>())
            && !backgroundChecker.GetComponent<BoxCollider2D>().IsTouching(backs[1].GetComponent<BoxCollider2D>()))
        {
            backs[0].transform.localPosition += new Vector3(-3 * xDelBack, 0, 0);
            var newBacks = new GameObject[] { backs[1], backs[2], backs[0] };
            backs = newBacks;
        }
        ////////////////////////////////
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        if (isGhostOn)
        {
            textTimer.GetComponent<Text>().text = string.Format("{0:0.#}", lastTime);
            lastTime -= Time.deltaTime;
            if (lastTime < 0)
            {
                isGhostOn = false;
                SwitchOnFirst();
                lastTime = ghostInactiveTime;
                textTimer.GetComponent<Text>().text = "";
                isCanSwitchOnGhost = false;
                backgroundOfTimer.GetComponent<Image>().fillAmount = 1;
            }
        }
        if (!isGhostOn && !isCanSwitchOnGhost)
        {
            textTimer.GetComponent<Text>().text = string.Format("{0:0.#}", lastTime);
            lastTime -= Time.deltaTime;
            backgroundOfTimer.GetComponent<Image>().fillAmount = lastTime / ghostInactiveTime;
            if (lastTime < 0)
            {
                backgroundOfTimer.GetComponent<Image>().fillAmount = 0;
                lastTime = ghostActiveTime;
                textTimer.GetComponent<Text>().text = "";
                isCanSwitchOnGhost = true;
            }
        }
    }
    public void SetTask(int numberOfTask)
    {
        if (indexOfCurrentTasks.Count == 0)
        {
            TaskMenuParent.transform.Find("TextBackground").GetComponent<Image>().color = new Color(0, 0, 0, 0.8f);
            Head.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        if (!indexOfCurrentTasks.Contains(numberOfTask))
        {
            indexOfCurrentTasks.Add(numberOfTask);
        }
        else
        {
            var task = Tasks[numberOfTask].Split("/");
            if (task.Length == 1)
            {
                TaskComplete(numberOfTask);
            }
            else
            {
                var cur = int.Parse(task[0].Substring(task[0].Length - 1)) + 1;
                var need = int.Parse(task[1].Substring(0, 1));
                if (cur == need)
                {
                    TaskComplete(numberOfTask);
                }
                else
                {
                    var newTask = "";
                    newTask += task[0].Substring(0, task[0].Length - 1) + cur.ToString() + "/" + task[1];
                    Tasks[numberOfTask] = newTask;
                }
            }
        }
        //TaskMenuParent.transform.Find("TextBackground").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        //TaskMenu.GetComponent<Text>().color = new Color(0, 0, 0, 0);
        var s = Tasks[0];
        foreach (var index in indexOfCurrentTasks)
        {
            s += Tasks[index];
        }
        s = s.Remove(s.Length - 1);
        if (s.Equals(Tasks[0].Remove(Tasks[0].Length - 1)))
        {
            TaskMenuParent.transform.Find("TextBackground").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            Head.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            TaskMenu.GetComponent<Text>().color = new Color(0, 0, 0, 0);
        }
        else
        {
            TaskMenuParent.transform.Find("TextBackground").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0.8f);
            Head.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            TaskMenu.GetComponent<Text>().color = new Color(1, 1, 1, 1);
        }
        TaskMenuParent.GetComponent<Text>().text = s;
        TaskMenu.GetComponent<Text>().text = s;
        Invoke("SetNewSize", 0.01f);
    }

    private void TaskComplete(int numberOfTask)
    {
        Tasks[numberOfTask] = "";
        completeWindow.GetComponent<CompleteWindow>().isMissionCompleted = true;
    }
    private void SetNewSize()
    {
        var nextDefaultSize = TaskMenuParent.GetComponent<RectTransform>().sizeDelta.y;
        TaskMenuParent.transform.localPosition -= new Vector3(0, (nextDefaultSize - defaultSize) / 2, 0);
        TaskMenu.transform.localPosition = TaskMenuParent.transform.localPosition;
        defaultSize = nextDefaultSize;
        //TaskMenuParent.transform.Find("TextBackground").gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        //TaskMenu.GetComponent<Text>().color = new Color(1, 1, 1, 1);
    }

    private void OffAlt()
    {
        if (CurrentActive == 1)
        {
            secondAbility.GetComponent<SpriteRenderer>().sprite = smallSecondAbil;
        }
        else if (CurrentActive == 2)
        {
            secondAbility.GetComponent<SpriteRenderer>().sprite = ghostSecondAbil;
        }
        else
        {
            secondAbility.GetComponent<SpriteRenderer>().sprite = bigSecondAbil;
        }
    }

    private void OffE()
    {
        if (CurrentActive == 1)
        {
            firstAbility.GetComponent<SpriteRenderer>().sprite = smallFirstAbil;
        }
        else if (CurrentActive == 2)
        {
            firstAbility.GetComponent<SpriteRenderer>().sprite = ghostFirstAbil;
        }
        else
        {
            firstAbility.GetComponent<SpriteRenderer>().sprite = bigFirstAbil;
        }
    }

    private void OnAlt()
    {
        if (CurrentActive == 1)
        {
            secondAbility.GetComponent<SpriteRenderer>().sprite = smallSecondAbilActive;
        }
        else if (CurrentActive == 2)
        {
            secondAbility.GetComponent<SpriteRenderer>().sprite = ghostSecondAbilActive;
        }
        else
        {
            secondAbility.GetComponent<SpriteRenderer>().sprite = bigSecondAbilActive;
        }
    }

    private void OnE()
    {
        if (CurrentActive == 1)
        {
            firstAbility.GetComponent<SpriteRenderer>().sprite = smallFirstAbilActive;
        }
        else if (CurrentActive == 2)
        {
            firstAbility.GetComponent<SpriteRenderer>().sprite = ghostFirstAbilActive;
        }
        else
        {
            firstAbility.GetComponent<SpriteRenderer>().sprite = bigFirstAbilActive;
        }
    }

    private void SetAbilIcons()
    {
        if (CurrentActive == 1)
        {
            firstAbility.GetComponent<SpriteRenderer>().sprite = smallFirstAbil;
            secondAbility.GetComponent<SpriteRenderer>().sprite = smallSecondAbil;
        }
        else if (CurrentActive == 2)
        {
            firstAbility.GetComponent<SpriteRenderer>().sprite = ghostFirstAbil;
            secondAbility.GetComponent<SpriteRenderer>().sprite = ghostSecondAbil;
        }
        else
        {
            firstAbility.GetComponent<SpriteRenderer>().sprite = bigFirstAbil;
            secondAbility.GetComponent<SpriteRenderer>().sprite = bigSecondAbil;
        }
    }

    private void StaminaChecked()
    {
        if (currentStamina <= minStamina)
            currentStamina = minStamina;

        if (currentStamina >= maxStamina)
            currentStamina = maxStamina;
        sliderStamina.value = currentStamina;
    }

    private void HealthChecked()
    {
        if (currentHealth <= minHealth)
            currentHealth = minHealth;

        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;
        sliderHealth.value = currentHealth;
    }

    public void HealHealth(float hp)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += hp;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }

    public GameObject GetCurrentActiveTrapTrigger()
    {
        if (CurrentActive == 1)
            return reindeerSmall.GetComponent<ReindeerSmall>().CurrentActiveTrapTrigger;
        return reindeerBig.GetComponent<ReindeerBig>().CurrentActiveTrapTrigger;
    }

    public void ActivateCooling()
    {
        sliderCooling.gameObject.SetActive(isActivateCooling);
    }

    public void Shadow()
    {
        if (reindeerSmall.GetComponent<ReindeerSmall>().isInShadow && currentCooling != 100)
        {
            currentCooling += 10f * Time.deltaTime;
            if (currentCooling > 100)
                currentCooling = 100;
        }
        else if (!reindeerSmall.GetComponent<ReindeerSmall>().isInShadow && currentCooling != 0)
        {
            currentCooling -= 10f * Time.deltaTime;
            if (currentCooling < 0)
                currentCooling = 0;
        }
        if (currentCooling == 0)
        {
            currentHealth -= 1f * Time.deltaTime;
        }
        sliderCooling.value = currentCooling;
    }

    public void TakeDamage(float damage)
    {
        if (isDamage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Respawn();
            }
            //isDamage = false;
        }
        /*else
        {
            currentHealth += Time.deltaTime * 10f;
        }*/
    }

    public void Respawn()
    {

        var allHunterControlPoints = GameObject.FindGameObjectsWithTag("HunterPoint");
        foreach (var point in allHunterControlPoints)
        {
            point.GetComponent<HunterControlPoint>().isAlreadyWorked = false;
        }
        MoveAllDeersToSpawn();
        if (CurrentActive == 1)
        {
            reindeerSmall.GetComponent<ReindeerSmall>().EscapedTrap();
        }
        if (CurrentActive == 2)
        {

        }
        if (CurrentActive == 3)
        {
            reindeerBig.GetComponent<ReindeerBig>().EscapedTrap();
        }
        currentCooling = maxCooling;
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        if (GameObject.Find("HunterKit1").transform.Find("Hunter").gameObject.GetComponent<Hunter>().isEnabled)
        {
            GameObject.Find("EnabledHunterMap1").GetComponent<HunterEnableArea>().MoveHunterAtNearestPoint();
        }
        SwitchOnFirst();
        GetCurrentActiveDeer().GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        maxFallVelocity = 0;

    }

    private void StaminaKeys()
    {
        if ((Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift) && isRunning)
            || (!Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift) && isRunning))
        {
            currentStamina -= Time.deltaTime * 20f;
        }
        else if (currentStamina < maxStamina)
        {
            currentStamina += Time.deltaTime * 10f;
        }
    }

    public void PositionDeerUnity()
    {
        if (CurrentActive == 1)//в зависимости от того, какой олень сейчас активен, перемещаем "unity" на его позицию с учетом дельты
        {
            transform.position = new Vector3(reindeerSmall.transform.position.x, reindeerSmall.transform.position.y + unitySmallDeltaY);
        }
        else if (CurrentActive == 2)
        {
            transform.position = new Vector3(reindeerGhost.transform.position.x, reindeerGhost.transform.position.y + unityGhostDeltaY);
        }
        else
        {
            transform.position = new Vector3(reindeerBig.transform.position.x, reindeerBig.transform.position.y + unityBigDeltaY);
        }
        if (isCameraTiedGhost)
        {
            transform.position = new Vector3(ghostCheckpoint.transform.position.x, ghostCheckpoint.transform.position.y);
        }
        else if (GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize > 4)
        {
            GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize = 4;
        }
    }
    public void ActivateDeer()
    {
        if (isFirstDeerAvailable && Input.GetKeyDown(KeyCode.Alpha1) && CurrentActive != 1 && isCanSwitch)//в зависимости от того, на какую цифру нажали, активируютс€ и дизактивируютс€ нужные олени
        {
            if (CurrentActive == 2)
            {
                isCanSwitchOnGhost = false;
                lastTime = ghostInactiveTime;
                isGhostOn = false;
            }
            SwitchOnFirst();
        }
        if (isSecondDeerAvailable && Input.GetKeyDown(KeyCode.Alpha2) && CurrentActive != 2 && isCanSwitch && isCanSwitchOnGhost)
        {
            SwitchOnSecond();
            reindeerGhost.GetComponent<ReindeerGhost>().isCanMater = true;
            isGhostOn = true;
        }
        if (isThirdDeerAvailable && Input.GetKeyDown(KeyCode.Alpha3) && CurrentActive != 3 && isCanSwitch)
        {
            if (CurrentActive == 2)
            {
                isCanSwitchOnGhost = false;
                lastTime = ghostInactiveTime;
                isGhostOn = false;
            }
            SwitchOnThird();
        }
    }

    public void SwitchOnFirst()
    {

        if (isOnMovePlatform)
        {
            var collisionTransform = GetCurrentActiveDeer().transform.parent;
            GetCurrentActiveDeer().transform.parent = null;
            reindeerGhost.SetActive(false);
            reindeerBig.SetActive(false);
            reindeerSmall.SetActive(true);

            currentActiveDeer = reindeerSmall;
            GetCurrentActiveDeer().transform.parent = collisionTransform;
        }
        else
        {
            reindeerGhost.SetActive(false);
            reindeerBig.SetActive(false);
            reindeerSmall.SetActive(true);
            currentActiveDeer = reindeerSmall;
        }

        reindeerSmall.transform.position = new Vector3(transform.position.x, transform.position.y - unitySmallDeltaY);

        var previousHorizontalVelocity = 0f;
        var previousVerticalVelocity = 0f;
        var previousIsRunning = false;
        var previousHorizontalForceRatio = 0f;
        var previousDirection = 0;
        if (CurrentActive == 2)
        {
            previousHorizontalVelocity = reindeerGhost.GetComponent<ReindeerGhost>().CurrentHorizontalVelocity;
            previousVerticalVelocity = reindeerGhost.GetComponent<ReindeerGhost>().CurrentVerticalVelocity;
            previousIsRunning = reindeerGhost.GetComponent<ReindeerGhost>().isRunning;
            previousHorizontalForceRatio = reindeerGhost.GetComponent<ReindeerGhost>().horizontalForceRatio;
            previousDirection = reindeerGhost.GetComponent<ReindeerGhost>().direction;
        }
        else if (CurrentActive == 3)
        {
            previousHorizontalVelocity = reindeerBig.GetComponent<ReindeerBig>().CurrentHorizontalVelocity;
            previousVerticalVelocity = reindeerBig.GetComponent<ReindeerBig>().CurrentVerticalVelocity;
            previousIsRunning = reindeerBig.GetComponent<ReindeerBig>().isRunning;
            previousHorizontalForceRatio = reindeerBig.GetComponent<ReindeerBig>().horizontalForceRatio;
            previousDirection = reindeerBig.GetComponent<ReindeerBig>().direction;
        }
        reindeerSmall.GetComponent<ReindeerSmall>().SetHorizontalVelocity(previousHorizontalVelocity);
        reindeerSmall.GetComponent<ReindeerSmall>().isRunning = previousIsRunning;
        reindeerSmall.GetComponent<ReindeerSmall>().horizontalForceRatio = previousHorizontalForceRatio;
        reindeerSmall.GetComponent<ReindeerSmall>().direction = previousDirection;
        reindeerSmall.GetComponent<Rigidbody2D>().velocity = new Vector2(previousHorizontalVelocity, previousVerticalVelocity);

        CurrentActive = 1;
        SetAbilIcons();
    }

    public void SwitchOnSecond()
    {

        if (isOnMovePlatform)
        {
            var collisionTransform = GetCurrentActiveDeer().transform.parent;
            GetCurrentActiveDeer().transform.parent = null;
            reindeerSmall.SetActive(false);
            reindeerBig.SetActive(false);
            reindeerGhost.SetActive(true);
            currentActiveDeer = reindeerGhost;
            GetCurrentActiveDeer().transform.parent = collisionTransform;
        }
        else
        {
            reindeerSmall.SetActive(false);
            reindeerBig.SetActive(false);
            reindeerGhost.SetActive(true);
            currentActiveDeer = reindeerGhost;
        }
        reindeerGhost.transform.position = new Vector3(transform.position.x, transform.position.y - unityGhostDeltaY);

        var previousHorizontalVelocity = 0f;
        var previousVerticalVelocity = 0f;
        var previousIsRunning = false;
        var previousHorizontalForceRatio = 0f;
        var previousDirection = 0;
        if (CurrentActive == 1)
        {
            previousHorizontalVelocity = reindeerSmall.GetComponent<ReindeerSmall>().CurrentHorizontalVelocity;
            previousVerticalVelocity = reindeerSmall.GetComponent<ReindeerSmall>().CurrentVerticalVelocity;
            previousIsRunning = reindeerSmall.GetComponent<ReindeerSmall>().isRunning;
            previousHorizontalForceRatio = reindeerSmall.GetComponent<ReindeerSmall>().horizontalForceRatio;
            previousDirection = reindeerSmall.GetComponent<ReindeerSmall>().direction;
        }
        else if (CurrentActive == 3)
        {
            previousHorizontalVelocity = reindeerBig.GetComponent<ReindeerBig>().CurrentHorizontalVelocity;
            previousVerticalVelocity = reindeerBig.GetComponent<ReindeerBig>().CurrentVerticalVelocity;
            previousIsRunning = reindeerBig.GetComponent<ReindeerBig>().isRunning;
            previousHorizontalForceRatio = reindeerBig.GetComponent<ReindeerBig>().horizontalForceRatio;
            previousDirection = reindeerBig.GetComponent<ReindeerBig>().direction;
        }
        reindeerGhost.GetComponent<ReindeerGhost>().SetHorizontalVelocity(previousHorizontalVelocity);
        reindeerGhost.GetComponent<ReindeerGhost>().isRunning = previousIsRunning;
        reindeerGhost.GetComponent<ReindeerGhost>().horizontalForceRatio = previousHorizontalForceRatio;
        reindeerGhost.GetComponent<ReindeerGhost>().direction = previousDirection;
        reindeerGhost.GetComponent<Rigidbody2D>().velocity = new Vector2(previousHorizontalVelocity, previousVerticalVelocity);

        CurrentActive = 2;
        SetAbilIcons();
    }

    public void SwitchOnThird()
    {

        if (isOnMovePlatform)
        {
            var collisionTransform = GetCurrentActiveDeer().transform.parent;
            GetCurrentActiveDeer().transform.parent = null;
            reindeerGhost.SetActive(false);
            reindeerSmall.SetActive(false);
            reindeerBig.SetActive(true);
            currentActiveDeer = reindeerBig;
            GetCurrentActiveDeer().transform.parent = collisionTransform;
        }
        else
        {
            reindeerGhost.SetActive(false);
            reindeerSmall.SetActive(false);
            reindeerBig.SetActive(true);
            currentActiveDeer = reindeerBig;
        }
        reindeerBig.transform.position = new Vector3(transform.position.x, transform.position.y - unityBigDeltaY);

        var previousHorizontalVelocity = 0f;
        var previousVerticalVelocity = 0f;
        var previousIsRunning = false;
        var previousHorizontalForceRatio = 0f;
        var previousDirection = 0;
        if (CurrentActive == 1)
        {
            previousHorizontalVelocity = reindeerSmall.GetComponent<ReindeerSmall>().CurrentHorizontalVelocity;
            previousVerticalVelocity = reindeerSmall.GetComponent<ReindeerSmall>().CurrentVerticalVelocity;
            previousIsRunning = reindeerSmall.GetComponent<ReindeerSmall>().isRunning;
            previousHorizontalForceRatio = reindeerSmall.GetComponent<ReindeerSmall>().horizontalForceRatio;
            previousDirection = reindeerSmall.GetComponent<ReindeerSmall>().direction;
        }
        else if (CurrentActive == 2)
        {
            previousHorizontalVelocity = reindeerGhost.GetComponent<ReindeerGhost>().CurrentHorizontalVelocity;
            previousVerticalVelocity = reindeerGhost.GetComponent<ReindeerGhost>().CurrentVerticalVelocity;
            previousIsRunning = reindeerGhost.GetComponent<ReindeerGhost>().isRunning;
            previousHorizontalForceRatio = reindeerGhost.GetComponent<ReindeerGhost>().horizontalForceRatio;
            previousDirection = reindeerGhost.GetComponent<ReindeerGhost>().direction;
        }
        reindeerBig.GetComponent<ReindeerBig>().SetHorizontalVelocity(previousHorizontalVelocity);
        reindeerBig.GetComponent<ReindeerBig>().isRunning = previousIsRunning;
        reindeerBig.GetComponent<ReindeerBig>().horizontalForceRatio = previousHorizontalForceRatio;
        reindeerBig.GetComponent<ReindeerBig>().direction = previousDirection;
        reindeerBig.GetComponent<Rigidbody2D>().velocity = new Vector2(previousHorizontalVelocity, previousVerticalVelocity);

        CurrentActive = 3;
        SetAbilIcons();
    }

    private void UpdateIsOnGround()
    {
        if (maxFallVelocity < (currentActiveDeer.GetComponent<Rigidbody2D>().velocity.y * -1))
        {
            maxFallVelocity = (currentActiveDeer.GetComponent<Rigidbody2D>().velocity.y * -1);
        }
        //IsGrounded = GetComponent<BoxCollider2D>().IsTouching(tileMapCollider);
        //GameObject.Find("Info").GetComponent<Text>().text = IsGrounded.ToString();
        var nextIsGrounded = false;
        if (currentActiveDeer != null && isOnPlatform)
        {
            nextIsGrounded = true;
        }
        else if (currentActiveDeer != null && !isOnPlatform)
        {
            nextIsGrounded = currentActiveDeer.transform.Find("Ground").GetComponent<BoxCollider2D>().IsTouching(tileMapCollider1)
                || currentActiveDeer.transform.Find("Ground").GetComponent<BoxCollider2D>().IsTouching(tileMapCollider2);
        }
        if (nextIsGrounded && !IsGrounded)
        {
            if (maxFallVelocity > 15)
            {
                TakeDamage((int)((maxFallVelocity - 15) * 100 / 15));
            }

            maxFallVelocity = 0;
        }
        IsGrounded = nextIsGrounded;
    }



    public void Trapped()
    {

        switch (CurrentActive)
        {
            case 1:
                reindeerSmall.GetComponent<ReindeerSmall>().Trapped();
                break;
            /*case 2:

                break;*/
            case 3:
                reindeerBig.GetComponent<ReindeerBig>().Trapped();
                break;
        }
        TakeDamage(25);
    }

    public void Bushed(GameObject bush)
    {
        isBushed = true;
        isCanSwitch = false;
        reindeerSmall.GetComponent<ReindeerSmall>().StopMoving();
        reindeerSmall.transform.position = new Vector3(bush.transform.position.x, reindeerSmall.transform.position.y, bush.transform.position.z + 0.5f);
    }

    public void UnBushed(GameObject bush)
    {
        isBushed = false;
        isCanSwitch = true;
        reindeerSmall.transform.position = new Vector3(bush.transform.position.x, reindeerSmall.transform.position.y, bush.transform.position.z - 0.5f);
        reindeerSmall.GetComponent<ReindeerSmall>().StartMoving();
    }

    public GameObject GetCurrentActiveDeer()
    {
        return currentActiveDeer;
    }

    private void MoveAllDeersToSpawn()
    {
        reindeerSmall.transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y, reindeerSmall.transform.position.z);
        reindeerGhost.transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y, reindeerSmall.transform.position.z);
        reindeerBig.transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y, reindeerSmall.transform.position.z);
        transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y, reindeerSmall.transform.position.z);
        GetCurrentActiveDeer().GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        maxFallVelocity = 0;
    }

    public void StartBlowing(float windHorizontalVelocity, float windVerticalForce)
    {
        reindeerSmall.GetComponent<ReindeerSmall>().InWind(windHorizontalVelocity, windVerticalForce);
        reindeerGhost.GetComponent<ReindeerGhost>().InWind(windHorizontalVelocity, windVerticalForce);
        reindeerBig.GetComponent<ReindeerBig>().InWind(windHorizontalVelocity, windVerticalForce);
    }

    public void StopBlowing()
    {
        reindeerSmall.GetComponent<ReindeerSmall>().WindOut();
        reindeerGhost.GetComponent<ReindeerGhost>().WindOut();
        reindeerBig.GetComponent<ReindeerBig>().WindOut();
    }

    public void PlayHunterTheme()
    {
        var audio = GetComponent<AudioSource>();
        audio.clip = hunterTheme;
        audio.Play();
    }

    public void PlayMainTheme()
    {
        var audio = GetComponent<AudioSource>();
        audio.clip = mainTheme;
        audio.Play();
    }

    /*public void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag.Equals("Platform"))
        {
            SetIsGrounded(false);
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Platform"))
        {
            SetIsGrounded(true);
        }
    }*/

    public void SetIsGrounded(bool value) { IsGrounded = value; }
}
