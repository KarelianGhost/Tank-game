using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameInit : MonoBehaviour
{
    public static Map map;
    public static EventManager events;
    public static int enemiesAlive = 0;
    public static int money = 0;
    public List<Wave> waves;
    private int currentWave = 0;
    private int currentGroup = 0;
    private int mobsSpawned = 0;
    private bool isWaveComplete = true;
    private int width = 30;
    private int height = 30;
    List<MapObject> spawnPoints = new List<MapObject>();
    MapLocation headquartersPosition = new MapLocation(8, 8);
    [SerializeField] private GameObject moneyTextPrefab;
    [SerializeField] private GameObject moneyCounter;
    [SerializeField] private Canvas ui;
    private RectTransform canvasRect;
    public static HealthBar playerHealthBar;
    public static HealthBar enemyHealthBar;
    private GameObject enemyHealthBarObject;
    private GameObject lastDamagedEnemy;

    public GameInit()
    {
        spawnPoints.Add(new MapObject(new MapLocation(10, 10), MapObject.Direction.left));
        spawnPoints.Add(new MapObject(new MapLocation(10, 0), MapObject.Direction.top));
        map = new Map(width, height, headquartersPosition);
        events = new EventManager();
    }
    // Start is called before the first frame update
    void Start()
    {
        canvasRect = ui.GetComponent<RectTransform>();
        playerHealthBar = GameObject.Find("PlayerHealthBar").GetComponent<HealthBar>();
        enemyHealthBarObject = GameObject.Find("EnemyHealthBar");
        enemyHealthBar = enemyHealthBarObject.GetComponent<HealthBar>();
        StartCoroutine(EnemySpawnLoop());
    }

    // Update is called once per frame
    void Update()
    {
        SetEnemyHealthBarPosition();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 mousePos = Input.mousePosition;
            Debug.Log(mousePos.x + "|" + mousePos.y + "|" + mousePos.z);

        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (currentWave < waves.Count && isWaveComplete)
            {
                StartNextWave();
            }
        }
    }
    IEnumerator EnemySpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            DoSpawnAttempt();
        }
    }
    private void DoSpawnAttempt()
    {
        //Debug.Log("Current Wave Info:");
        //Debug.Log("CurrentWave: " + currentWave);
        //Debug.Log("CurrentGroup: " + currentGroup);
        //Debug.Log("CurrentEnemiesAlive: " + enemiesAlive);
        //Debug.Log("CurrentMobsSpawned: " + mobsSpawned);
        //Debug.Log("IsWaveComplete: " + isWaveComplete);
        if (!isWaveComplete)
        {
            if (enemiesAlive < waves[currentWave - 1].maxActive)
            {
                MapObject currentSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
                if (waves[currentWave - 1].wave[currentGroup].amount == mobsSpawned)
                {
                    currentGroup++;
                    mobsSpawned = 0;
                }
                if (currentGroup == waves[currentWave - 1].wave.Count)
                {
                    isWaveComplete = true;
                    return;
                }
                if (GameInit.map.GetValue(currentSpawnPoint.pos) == 0)
                {
                    GameObject mob = waves[currentWave - 1].wave[currentGroup].mob;
                    mob.GetComponent<EnemyController>().SetPosition(currentSpawnPoint.pos);
                    mob.GetComponent<EnemyController>().SetRotation(currentSpawnPoint.dir);
                    Instantiate(mob,
                        GameInit.map.GetCellCenterPosition(currentSpawnPoint.pos),
                        mob.transform.rotation);
                    mobsSpawned++;
                }
            }
        }
    }
    private void StartNextWave()
    {
        isWaveComplete = false;
        currentWave++;
        currentGroup = 0;
    }

    private void OnEnable()
    {
        EventManager.onDamageTaken += UpdateHealthBars;
        EventManager.onTankDestroy += MoneyReward;
    }

    private void OnDisable()
    {
        EventManager.onDamageTaken -= UpdateHealthBars;
        EventManager.onTankDestroy -= MoneyReward;
    }

    private void SetEnemyHealthBarPosition()
    {
        if (lastDamagedEnemy == null)
        {
            enemyHealthBarObject.SetActive(false);
        }
        else
        {
            enemyHealthBarObject.SetActive(true);
            Vector3 modelPos = lastDamagedEnemy.transform.position;
            modelPos.y += 2.0f;
            enemyHealthBarObject.transform.localPosition = CalculateCanvasPostion(modelPos);
        }
    }
    private void UpdateHealthBars(GameObject damageTaker, float health, float maxHealth, DestructableObject.Faction faction, bool isPlayer)
    {
        if (isPlayer)
        {
            GameInit.playerHealthBar.SetHealth(health);
        } else if (faction == DestructableObject.Faction.enemy)
        {
            lastDamagedEnemy = damageTaker;
            GameInit.enemyHealthBar.SetMaxHealth(maxHealth);
            GameInit.enemyHealthBar.SetHealth(health);
        }
    }

    private void MoneyReward(Tank tank, Vector3 modelPos)
    {
        if (tank.moneyReward > 0) {
            Debug.Log("You got " + tank.moneyReward + " money");
            GameObject uiElement = Instantiate(moneyTextPrefab);
            uiElement.transform.SetParent(ui.transform);
            uiElement.transform.localPosition = CalculateCanvasPostion(modelPos);
            string rewardString = "+" + tank.moneyReward + "$";
            uiElement.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = rewardString;
            changeMoney(tank.moneyReward);
        }
    }

    private void changeMoney(int value)
    {
        money += value;
        string moneyString = "$" + money;
        moneyCounter.GetComponent<TextMeshProUGUI>().text = moneyString;
    }

    private Vector3 CalculateCanvasPostion(Vector3 startPos)
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(startPos);
        Vector3 centerBasedViewPortPosition = viewportPosition - new Vector3(0.5f, 0.5f, 0);
        var scale = canvasRect.sizeDelta;
        return Vector3.Scale(centerBasedViewPortPosition, scale);
    }

}
