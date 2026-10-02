using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Enemyspawner> enemySpawners;
    public MainTower tower;

    public PathGenerator pathGenerator;

    void Start()
    {
    }

    public void GeneratePaths()
    {
        Debug.Log("===== GAME MANAGER =====");
        Debug.Log("GameManager object: " + gameObject.name);
        Debug.Log("Tower: " + tower);
        Debug.Log("Tower GameObject: " +
                  (tower != null ? tower.gameObject.name : "NULL"));
        Debug.Log("PathGenerator: " + pathGenerator);

        if (pathGenerator == null)
        {
            Debug.LogError("GameManager: PathGenerator is missing!", this);
            return;
        }

        if (tower == null)
        {
            Debug.LogError("GameManager: MainTower is missing!", this);
            return;
        }

        pathGenerator.GeneratePaths(enemySpawners, tower);

        // Enemy spawning is handled by WaveManager.
        // Do NOT start the old spawning coroutine here.
    }
}