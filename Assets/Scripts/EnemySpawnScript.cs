using System.Collections;
using UnityEngine;

public class EnemySpawnScript : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Tooltip(
        "Relative spawn chance for each enemy prefab. " +
        "Must match the prefab array."
    )]
    [SerializeField] private float[] enemyBaseWeights;

    [Header("Enemy Difficulty")]
    [Tooltip(
        "Controls how strongly each enemy type responds to difficulty. " +
        "Negative = becomes less common at higher difficulty. " +
        "0 = unaffected. Positive = becomes more common."
    )]
    [SerializeField] private float[] enemyDifficultyInfluence =
    {
        -1f,
        0f,
        1f
    };

    [Header("Spawning")]
    [SerializeField] private int enemiesToSpawn = 20;

    private GameObject manager;
    private PathGenerator pathGenerator;
    private GameObject[] spawnPoints = new GameObject[0];

    private void Start()
    {
        manager =
            GameObject.Find("GameManager");

        if (manager == null)
        {
            Debug.LogError(
                "EnemySpawnScript: GameManager not found!",
                this
            );

            return;
        }

        pathGenerator =
            manager.GetComponent<PathGenerator>();

        if (pathGenerator == null)
        {
            Debug.LogError(
                "EnemySpawnScript: PathGenerator not found!",
                this
            );

            return;
        }

        GetSpawnPoints();
    }

    public void GetSpawnPoints()
    {
        spawnPoints =
            GameObject.FindGameObjectsWithTag(
                "SpawnPoint"
            );

        if (spawnPoints.Length == 0)
        {
            Debug.LogError(
                "EnemySpawnScript: No objects tagged SpawnPoint found!",
                this
            );
        }
    }

    /// <summary>
    /// Called by WaveManager to spawn one enemy.
    /// </summary>
    public GameObject SpawnEnemy(
        float[] difficultyMultipliers = null
    )
    {
        if (
            pathGenerator == null ||
            enemyPrefabs == null ||
            enemyPrefabs.Length == 0
        )
        {
            Debug.LogError(
                "EnemySpawnScript: Missing path generator or enemy prefabs!",
                this
            );

            return null;
        }

        if (
            spawnPoints == null ||
            spawnPoints.Length == 0
        )
        {
            GetSpawnPoints();
        }

        if (spawnPoints.Length == 0)
        {
            return null;
        }

        if (
            pathGenerator.generatedPaths == null ||
            pathGenerator.generatedPaths.Count == 0
        )
        {
            Debug.LogError(
                "EnemySpawnScript: No generated paths available!",
                this
            );

            return null;
        }

        int availableCount =
            Mathf.Min(
                spawnPoints.Length,
                pathGenerator.generatedPaths.Count
            );

        if (availableCount == 0)
        {
            return null;
        }

        int pathIndex =
            Random.Range(
                0,
                availableCount
            );

        GameObject spawnPoint =
            spawnPoints[pathIndex];

        if (spawnPoint == null)
        {
            return null;
        }

        GameObject prefab =
            ChooseEnemyPrefab(
                difficultyMultipliers
            );

        if (prefab == null)
        {
            Debug.LogError(
                "EnemySpawnScript: No valid enemy prefab or weights!",
                this
            );

            return null;
        }

        GameObject enemy =
            Instantiate(
                prefab,
                spawnPoint.transform.position,
                spawnPoint.transform.rotation
            );

        Enemy_walking walking =
            enemy.GetComponent<Enemy_walking>();

        if (walking == null)
        {
            Debug.LogError(
                $"Enemy prefab '{prefab.name}' " +
                "has no Enemy_walking component!",
                enemy
            );

            Destroy(enemy);

            return null;
        }

        walking.SetPath(
            pathGenerator
                .generatedPaths[pathIndex]
                .waypoints
        );

        // Register the enemy with the active wave.
        WaveManager waveManager =
            FindFirstObjectByType<WaveManager>();

        if (waveManager != null)
        {
            waveManager.RegisterEnemy(enemy);
        }
        else
        {
            Debug.LogWarning(
                "EnemySpawnScript: WaveManager could not be found " +
                "when registering enemy."
            );
        }

        Debug.Log(
            $"Spawned {prefab.name} " +
            $"from spawn point {pathIndex + 1}"
        );

        return enemy;
    }

    private GameObject ChooseEnemyPrefab(
        float[] difficultyMultipliers
    )
    {
        float totalWeight = 0f;

        for (
            int i = 0;
            i < enemyPrefabs.Length;
            i++
        )
        {
            if (enemyPrefabs[i] == null)
            {
                continue;
            }

            float baseWeight =
                GetBaseWeight(i);

            float multiplier =
                GetMultiplier(
                    i,
                    difficultyMultipliers
                );

            totalWeight +=
                baseWeight *
                multiplier;
        }

        if (totalWeight <= 0f)
        {
            return null;
        }

        float roll =
            Random.value *
            totalWeight;

        float cumulativeWeight = 0f;

        for (
            int i = 0;
            i < enemyPrefabs.Length;
            i++
        )
        {
            if (enemyPrefabs[i] == null)
            {
                continue;
            }

            cumulativeWeight +=
                GetBaseWeight(i) *
                GetMultiplier(
                    i,
                    difficultyMultipliers
                );

            if (roll <= cumulativeWeight)
            {
                return enemyPrefabs[i];
            }
        }

        return null;
    }

    private float GetBaseWeight(int index)
    {
        if (
            enemyBaseWeights == null ||
            index >= enemyBaseWeights.Length
        )
        {
            return 1f;
        }

        return Mathf.Max(
            0f,
            enemyBaseWeights[index]
        );
    }

    private float GetMultiplier(
        int index,
        float[] multipliers
    )
    {
        if (
            multipliers == null ||
            index >= multipliers.Length
        )
        {
            return 1f;
        }

        return Mathf.Max(
            0f,
            multipliers[index]
        );
    }

    public float[] GetDifficultyMultipliers(
        float difficulty
    )
    {
        float[] multipliers =
            new float[enemyPrefabs.Length];

        for (
            int i = 0;
            i < multipliers.Length;
            i++
        )
        {
            float influence =
                GetDifficultyInfluence(i);

            multipliers[i] =
                Mathf.Pow(
                    difficulty,
                    influence
                );
        }

        return multipliers;
    }

    private float GetDifficultyInfluence(
        int index
    )
    {
        if (
            enemyDifficultyInfluence == null ||
            index >= enemyDifficultyInfluence.Length
        )
        {
            return 0f;
        }

        return enemyDifficultyInfluence[index];
    }

    // Kept for compatibility with the old spawning system.
    // WaveManager controls normal gameplay spawning.
    public IEnumerator SpawnEnemiesOverTime(
        float spawnInterval
    )
    {
        for (
            int i = 0;
            i < enemiesToSpawn;
            i++
        )
        {
            SpawnEnemy();

            yield return new WaitForSeconds(
                spawnInterval
            );
        }
    }
}