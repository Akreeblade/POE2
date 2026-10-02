using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyClass : MonoBehaviour
{
    protected int PathSpawnedOn;

    protected List<Vector3> path;
    [SerializeField] protected int currentPoint;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int currentHealth;

    [SerializeField] String Tagtofind;

    [SerializeField] protected int speed;
    [SerializeField] protected int money;

    Transform[] waypointTransforms;

    protected GameObject gameManager;
    protected Moneyholdscript moneyholdscript;
    protected EnemyListScript enemyListScript;
    protected MainTower mainTower;

    protected Collider Tower;
    protected Transform TowerTransform;

    [SerializeField] GameObject bullet;

    [SerializeField] protected float fireRate;
    protected float fireTimer;

    [SerializeField] protected float checkRadius;

    [Header("Health Bar")]
    public Slider healthBar;
    public float healthBarVisibleTime = 2f;

    protected Coroutine hideBarRoutine;
    protected Transform cam;

    private WaveManager waveManager;

    private bool registeredWithWaveManager;
    private bool enemyDestroyed;

    void Start()
    {
        cam =
            Camera.main.transform;

        currentHealth =
            maxHealth;

        gameManager =
            GameObject.Find("GameManager");

        if (gameManager == null)
        {
            Debug.LogError(
                "GAME MANAGER IS NULL!"
            );

            return;
        }

        Debug.Log(
            "Found GameManager: " +
            gameManager.name
        );

        moneyholdscript =
            gameManager.GetComponent<Moneyholdscript>();

        if (moneyholdscript == null)
        {
            Debug.LogError(
                "Moneyholdscript is NULL!"
            );
        }

        enemyListScript =
            gameManager.GetComponent<EnemyListScript>();

        if (enemyListScript == null)
        {
            Debug.LogError(
                "EnemyListScript is NULL!"
            );

            return;
        }

        Debug.Log(
            "EnemyListScript found!"
        );

        enemyListScript.SetEnemies(
            this.gameObject
        );

        if (healthBar != null)
        {
            healthBar.maxValue =
                maxHealth;

            healthBar.value =
                currentHealth;

            healthBar.gameObject.SetActive(
                false
            );
        }

        mainTower =
            gameManager
                .GetComponent<GameManager>()
                .GetComponentInChildren<MainTower>();

        GameObject[] towers =
            GameObject.FindGameObjectsWithTag(
                "Tower"
            );

        if (towers.Length > 0)
        {
            mainTower =
                towers[0].GetComponent<MainTower>();
        }

        if (mainTower == null)
        {
            Debug.LogError(
                "MainTower could not be found!"
            );
        }

        // Find the wave manager.
        waveManager =
            FindFirstObjectByType<WaveManager>();

        if (waveManager != null)
        {
            registeredWithWaveManager = true;
        }
    }

    protected void Update()
    {
        move();

        fireTimer -=
            Time.deltaTime;

        if (Tower == null)
        {
            Tower = null;

            findEnemy();

            return;
        }
        else
        {
            if (fireTimer <= 0f)
            {
                fireATenemy();

                fireTimer =
                    fireRate;
            }
        }

        IfOutOfRaduis();

        if (currentHealth <= 0)
        {
            Destroy(
                this.gameObject
            );
        }
    }

    public void fireATenemy()
    {
        if (Tower == null)
        {
            Debug.LogError(
                "There is no enemy assigned!"
            );

            return;
        }

        GameObject newBullet =
            Instantiate(
                bullet,
                transform.position,
                Quaternion.identity
            );

        bulletmovement bulletScript =
            newBullet.GetComponent<bulletmovement>();

        if (bulletScript == null)
        {
            Debug.LogError(
                "The bullet prefab does not have " +
                "a bulletmovement component!"
            );

            return;
        }

        bulletScript.SetEnemy(
            Tower.gameObject
        );
    }

    public void IfOutOfRaduis()
    {
        if (Tower == null)
        {
            return;
        }

        Vector3 offset =
            TowerTransform.position -
            transform.position;

        float sqrDistance =
            offset.sqrMagnitude;

        float sqrRadius =
            checkRadius *
            checkRadius;

        if (sqrDistance > sqrRadius)
        {
            Tower = null;

            return;
        }

        return;
    }

    public float GetLifeinfloat()
    {
        float lifePercentage =
            (float)currentHealth /
            (float)maxHealth;

        return lifePercentage;
    }

    public void SetPath(
        List<Vector3> newPath
    )
    {
        if (newPath == null)
        {
            Debug.LogError(
                "Enemy_walking received a NULL path!"
            );

            return;
        }

        if (newPath.Count == 0)
        {
            Debug.LogError(
                "Enemy_walking received an EMPTY path!"
            );

            return;
        }

        path =
            newPath;

        currentPoint =
            0;

        Debug.Log(
            "Path successfully assigned. " +
            "Points: " +
            path.Count
        );
    }

    public void move()
    {
        if (
            path == null ||
            path.Count == 0
        )
        {
            return;
        }

        if (
            currentPoint >=
            path.Count
        )
        {
            return;
        }

        Vector3 target =
            path[currentPoint];

        transform.position =
            Vector3.MoveTowards(
                transform.position,
                target,
                Time.deltaTime *
                speed
            );

        Vector3 direction =
            target -
            transform.position;

        if (direction != Vector3.zero)
        {
            transform.rotation =
                Quaternion.LookRotation(
                    direction
                );
        }

        if (
            Vector3.Distance(
                transform.position,
                target
            ) < 0.1f
        )
        {
            currentPoint++;

            if (
                currentPoint >=
                path.Count
            )
            {
                Debug.Log(
                    "Enemy reached the end!"
                );

                if (mainTower != null)
                {
                    mainTower.TakeDamage(
                        10f
                    );
                }

                Destroy(
                    gameObject
                );

                return;
            }
        }
    }

    public void takedamage(
        int damage
    )
    {
        ShowHealthBar();

        currentHealth -=
            damage;

        if (healthBar != null)
        {
            healthBar.value =
                currentHealth;
        }

        if (currentHealth <= 0)
        {
            Destroy(
                gameObject
            );
        }
    }

    void ShowHealthBar()
    {
        if (healthBar == null)
        {
            return;
        }

        healthBar.gameObject.SetActive(
            true
        );

        if (hideBarRoutine != null)
        {
            StopCoroutine(
                hideBarRoutine
            );
        }

        hideBarRoutine =
            StartCoroutine(
                HideHealthBar()
            );
    }

    IEnumerator HideHealthBar()
    {
        yield return new WaitForSeconds(
            healthBarVisibleTime
        );

        healthBar.gameObject.SetActive(
            false
        );
    }

    public void getmoney()
    {
        if (moneyholdscript != null)
        {
            moneyholdscript.addmoney(
                money
            );
        }
    }

    private void OnDestroy()
    {
        // Prevent this from being reported more than once.
        if (enemyDestroyed)
        {
            return;
        }

        enemyDestroyed = true;

        // Existing enemy list system.
        if (enemyListScript != null)
        {
            enemyListScript.RemoveEnemies(
                this.gameObject
            );
        }

        // get money when enemy is destroyed
        getmoney();

        // Notify the wave manager.
        if (waveManager != null &&
            registeredWithWaveManager)
        {
            waveManager.EnemyDefeated(
                this.gameObject
            );
        }
    }

    public bool findEnemy()
    {
        Collider[] colliders =
            Physics.OverlapSphere(
                transform.position,
                checkRadius
            );

        foreach (
            Collider col in colliders
        )
        {
            if (
                col.CompareTag(
                    Tagtofind
                )
            )
            {
                Tower =
                    col;

                TowerTransform =
                    col.transform;

                return true;
            }
            else
            {
                Tower = null;
                TowerTransform = null;

                return false;
            }
        }

        return false;
    }
}