using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class WaveManager : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private PlayerInputReader playerInputReader;
    [SerializeField] private List<WaveData> waves;
    [SerializeField] private UpgradeUI upgradeUI;
    [SerializeField] private PlayerController player;


    private int CurrentWave = 0;
    private int checkWaveCount = 0;
    private float CurrentTime = -999;
    private bool isWaveRunning = false;
    private bool isWaveEnd;

    private int aliveEnemyCount = 0;
    private float spawnDelayTime;
    private GameObject spawnEnemy;


    public static WaveManager Instance { get; private set; }

    public event Action<int, int> OnwaveChanged;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Start();
            Destroy(gameObject, 0.01f);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        CurrentWave = 0;
        isWaveRunning = false;
        isWaveEnd = false;
    }

    private void Update()
    {
        if(isWaveRunning == true && isWaveEnd == false)
        {
            if (Time.time > CurrentTime + spawnDelayTime)
            {
                CurrentTime = Time.time;
                Instantiate(spawnEnemy, spawnPosition.position, Quaternion.identity);

                checkWaveCount--;
                if (checkWaveCount == 0)
                {
                    isWaveEnd = true;
                }
            }
        }
    }

    private void OnEnable()
    {
        if (playerInputReader != null)
        {
            playerInputReader.OnspawnAction += StartWave;
        }
    }


    private void OnDisable()
    {
        if (playerInputReader != null)
        {
            playerInputReader.OnspawnAction -= StartWave;
        }
    }

    private void StartWave()
    {
        if (isWaveRunning)
        {
            return;
        }

        if (CurrentWave >= waves.Count)
        {
            return;
        }

        SpawnWave(CurrentWave);
    }

    private void SpawnWave(int wave)
    {
        aliveEnemyCount = waves[wave].enemyCount;
        spawnDelayTime = waves[wave].spawnDelay;
        spawnEnemy = waves[wave].enemyPrefab;

        checkWaveCount = aliveEnemyCount;

        isWaveRunning = true;
        isWaveEnd = false;

        OnwaveChanged?.Invoke(CurrentWave, aliveEnemyCount);
    }

    public void OnEnemyDead()
    {
        aliveEnemyCount--;

        OnwaveChanged?.Invoke(CurrentWave, aliveEnemyCount);

        if (aliveEnemyCount <= 0)
        {
            WaveClear();
        }
    }

    private void WaveClear()
    {
        isWaveRunning = false;

        CurrentWave++;

        if (waves.Count == CurrentWave)
        {
            CurrentWave = 0; // wave reset
            GameSceneManager.Instance.LoadSceneByName("GameClear");
            return;
        }

        Time.timeScale = 0f;

        upgradeUI.Open(player, OnUpgradeSelected);
    }

    private void OnUpgradeSelected()
    {
        Time.timeScale = 1f;
    }

}
