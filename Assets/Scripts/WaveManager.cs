using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private float preparationTime = 10f;
    [SerializeField] private float waveDuration = 60f;

    [Header("Enemy Count")]
    [SerializeField] private int startingEnemyCount = 15;
    [SerializeField] private int additionalEnemiesPerWave = 3;

    [Header("Spawning")]
    [SerializeField] private float spawnInterval = 3f;

    [Header("Difficulty")]
    [SerializeField] private DifficultyManager difficultyManager;

    [Header("References")]
    [SerializeField] private EnemySpawnScript enemySpawner;

    private MainTower mainTower;

    private Coroutine waveCoroutine;

    private bool waveActive;
    private bool preparationActive;
    private bool gameOver;

    private int currentWave = 0;

    private int enemiesToSpawn;
    private int enemiesSpawned;
    private int enemiesAlive;

    private float preparationTimer;
    private float waveTimer;

    private float currentDifficulty;

    public int CurrentWave => currentWave;

    public bool WaveActive => waveActive;
    public bool PreparationActive => preparationActive;
    public bool GameOver => gameOver;

    public float PreparationTimer => preparationTimer;
    public float WaveTimer => waveTimer;

    public int EnemiesToSpawn => enemiesToSpawn;
    public int EnemiesSpawned => enemiesSpawned;
    public int EnemiesAlive => enemiesAlive;

    private void Start()
    {
        if (enemySpawner == null)
        {
            enemySpawner = GetComponent<EnemySpawnScript>();
        }

        if (difficultyManager == null)
        {
            difficultyManager = GetComponent<DifficultyManager>();
        }

        if (enemySpawner == null)
        {
            Debug.LogError(
                "WaveManager: EnemySpawnScript could not be found!",
                this
            );

            return;
        }

        if (difficultyManager == null)
        {
            Debug.LogError(
                "WaveManager: DifficultyManager could not be found!",
                this
            );

            return;
        }

        Debug.Log(
            "WaveManager initialized. " +
            "Waiting for Main Tower to be placed..."
        );
    }

    private void Update()
    {
        if (gameOver)
        {
            return;
        }

        // The Main Tower is placed by the player,
        // so keep looking for it until it exists.
        if (mainTower == null)
        {
            FindMainTower();

            return;
        }

        // Backup check in case the tower's destruction
        // event is not triggered for some reason.
        if (mainTower.CurrentHealth <= 0f)
        {
            HandleTowerDestroyed();
        }
    }

    /// <summary>
    /// Looks for the MainTower until the player places it.
    /// </summary>
    private void FindMainTower()
    {
        MainTower foundTower =
            FindFirstObjectByType<MainTower>();

        if (foundTower == null)
        {
            return;
        }

        mainTower = foundTower;

        mainTower.onTowerDestroyed.AddListener(
            HandleTowerDestroyed
        );

        Debug.Log(
            $"WaveManager found Main Tower: " +
            $"{mainTower.gameObject.name}"
        );

        Debug.Log(
            "WaveManager is now ready to begin waves."
        );

        StartPreparation();
    }

    /// <summary>
    /// Registers an enemy as being alive in the current wave.
    /// Called by EnemySpawnScript.
    /// </summary>
    public void RegisterEnemy(GameObject enemy)
    {
        if (enemy == null)
        {
            return;
        }

        if (!waveActive)
        {
            return;
        }

        enemiesAlive++;

        Debug.Log(
            $"Wave {currentWave}: " +
            $"Enemy registered. " +
            $"Alive: {enemiesAlive}"
        );
    }

    /// <summary>
    /// Called when an enemy is destroyed.
    /// </summary>
    public void EnemyDefeated(GameObject enemy)
    {
        if (enemy == null)
        {
            return;
        }

        // Prevent the count from going negative.
        if (enemiesAlive > 0)
        {
            enemiesAlive--;
        }

        Debug.Log(
            $"Wave {currentWave}: " +
            $"Enemy defeated. " +
            $"Alive: {enemiesAlive}"
        );
    }

    public void StartPreparation()
    {
        if (gameOver)
        {
            Debug.LogWarning(
                "WaveManager: Cannot start preparation because the game is over."
            );

            return;
        }

        if (mainTower == null)
        {
            Debug.LogWarning(
                "WaveManager: Cannot start preparation because " +
                "the Main Tower has not been placed yet."
            );

            return;
        }

        if (waveActive)
        {
            Debug.LogWarning(
                "WaveManager: Cannot start preparation while a wave is active."
            );

            return;
        }

        if (preparationActive)
        {
            Debug.LogWarning(
                "WaveManager: Preparation is already active."
            );

            return;
        }

        preparationActive = true;

        preparationTimer =
            preparationTime;

        Debug.Log(
            $"Wave {currentWave + 1} preparation started. " +
            $"You have {preparationTime} seconds."
        );

        if (waveCoroutine != null)
        {
            StopCoroutine(waveCoroutine);
        }

        waveCoroutine =
            StartCoroutine(
                PreparationCoroutine()
            );
    }

    public void StartWave()
    {
        if (gameOver)
        {
            Debug.LogWarning(
                "WaveManager: Cannot start a wave because the game is over."
            );

            return;
        }

        if (mainTower == null)
        {
            Debug.LogWarning(
                "WaveManager: Cannot start a wave because " +
                "the Main Tower has not been placed yet."
            );

            return;
        }

        if (waveActive)
        {
            Debug.LogWarning(
                "WaveManager: A wave is already active."
            );

            return;
        }

        if (waveCoroutine != null)
        {
            StopCoroutine(waveCoroutine);

            waveCoroutine = null;
        }

        preparationActive = false;

        BeginWave();
    }

    private void BeginWave()
    {
        if (gameOver || mainTower == null)
        {
            return;
        }

        currentWave++;

        waveActive = true;

        waveTimer =
            waveDuration;

        enemiesSpawned = 0;
        enemiesAlive = 0;

        currentDifficulty =
            difficultyManager.GetDifficultyMultiplier();

        enemiesToSpawn =
            Mathf.RoundToInt(
                (
                    startingEnemyCount +
                    (
                        (currentWave - 1) *
                        additionalEnemiesPerWave
                    )
                ) *
                currentDifficulty
            );

        float currentSpawnInterval =
            spawnInterval /
            currentDifficulty;

        Debug.Log(
            $"===== WAVE {currentWave} STARTED ====="
        );

        Debug.Log(
            $"Difficulty: " +
            $"{currentDifficulty:F2}"
        );

        Debug.Log(
            $"Enemies to spawn: " +
            $"{enemiesToSpawn}"
        );

        Debug.Log(
            $"Spawn interval: " +
            $"{currentSpawnInterval:F2} seconds"
        );

        difficultyManager.StartWave(
            mainTower.CurrentHealth
        );

        waveCoroutine =
            StartCoroutine(
                WaveCoroutine(
                    currentSpawnInterval
                )
            );
    }

    private IEnumerator PreparationCoroutine()
    {
        while (
            preparationTimer > 0f &&
            !gameOver
        )
        {
            preparationTimer -=
                Time.deltaTime;

            yield return null;
        }

        if (gameOver)
        {
            yield break;
        }

        preparationTimer = 0f;

        preparationActive = false;

        BeginWave();
    }

    private IEnumerator WaveCoroutine(
        float currentSpawnInterval
    )
    {
        float spawnTimer = 0f;

        // -----------------------------------------
        // PHASE 1
        // Spawn enemies until either:
        //
        // - all enemies have spawned
        // OR
        // - the wave timer reaches zero.
        // -----------------------------------------

        while (
            waveTimer > 0f &&
            enemiesSpawned < enemiesToSpawn &&
            !gameOver
        )
        {
            waveTimer -=
                Time.deltaTime;

            spawnTimer -=
                Time.deltaTime;

            if (spawnTimer <= 0f)
            {
                SpawnEnemy();

                spawnTimer =
                    currentSpawnInterval;
            }

            yield return null;
        }

        if (gameOver)
        {
            yield break;
        }

        waveTimer = 0f;

        Debug.Log(
            $"Wave {currentWave}: " +
            "Finished spawning."
        );

        Debug.Log(
            $"Wave {currentWave}: " +
            $"Waiting for {enemiesAlive} " +
            "remaining enemies to be defeated."
        );

        // -----------------------------------------
        // PHASE 2
        // The wave is NOT finished yet.
        //
        // Wait until every spawned enemy has died.
        // -----------------------------------------

        while (
            enemiesAlive > 0 &&
            !gameOver
        )
        {
            yield return null;
        }

        if (gameOver)
        {
            yield break;
        }

        // -----------------------------------------
        // PHASE 3
        // Battlefield cleared.
        // -----------------------------------------

        EndWave();
    }

    private void SpawnEnemy()
    {
        if (gameOver)
        {
            return;
        }

        if (enemySpawner == null)
        {
            Debug.LogError(
                "WaveManager: EnemySpawnScript reference is missing!",
                this
            );

            return;
        }

        float[] difficultyMultipliers =
            enemySpawner.GetDifficultyMultipliers(
                currentDifficulty
            );

        GameObject spawnedEnemy =
            enemySpawner.SpawnEnemy(
                difficultyMultipliers
            );

        if (spawnedEnemy != null)
        {
            enemiesSpawned++;

            Debug.Log(
                $"Wave {currentWave}: " +
                $"Spawned enemy " +
                $"{enemiesSpawned}/{enemiesToSpawn}"
            );
        }
    }

    private void EndWave()
    {
        if (gameOver)
        {
            return;
        }

        waveActive = false;

        Debug.Log(
            $"===== WAVE {currentWave} COMPLETE ====="
        );

        Debug.Log(
            $"Enemies spawned: " +
            $"{enemiesSpawned}/{enemiesToSpawn}"
        );

        Debug.Log(
            "All enemies have been defeated."
        );

        difficultyManager.EndWave(
            mainTower.CurrentHealth
        );

        waveCoroutine = null;

        StartPreparation();
    }

    /// <summary>
    /// Called when MainTower invokes onTowerDestroyed.
    /// </summary>
    private void HandleTowerDestroyed()
    {
        if (gameOver)
        {
            return;
        }

        gameOver = true;

        waveActive = false;
        preparationActive = false;

        preparationTimer = 0f;
        waveTimer = 0f;

        if (waveCoroutine != null)
        {
            StopCoroutine(
                waveCoroutine
            );

            waveCoroutine = null;
        }

        Debug.Log(
            "========================================"
        );

        Debug.Log(
            "GAME OVER CALLED"
        );

        Debug.Log(
            $"The Main Tower was destroyed on Wave " +
            $"{currentWave}."
        );

        Debug.Log(
            "WaveManager has stopped all wave activity."
        );

        Debug.Log(
            "========================================"
        );
    }

    private void OnDestroy()
    {
        if (mainTower != null)
        {
            mainTower.onTowerDestroyed.RemoveListener(
                HandleTowerDestroyed
            );
        }
    }

    public bool IsWaveActive()
    {
        return waveActive;
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }
}