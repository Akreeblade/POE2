using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Enemyspawner> enemySpawners;
    public MainTower tower;
    [SerializeField] float spawnInterval = 1f;
    
    public PathGenerator pathGenerator;
    
    void Start()
    {
    }
    
    public void GeneratePaths()
    {
        Debug.Log("===== GAME MANAGER =====");
        Debug.Log("GameManager object: " + gameObject.name);
        Debug.Log("Tower: " + tower);
        Debug.Log("Tower GameObject: " + (tower != null ? tower.gameObject.name : "NULL"));
        Debug.Log("PathGenerator: " + pathGenerator);
        
        pathGenerator.GeneratePaths(enemySpawners, tower);
        
        
        this.gameObject.GetComponent<EnemySpawnScript>().GetSpawnPoints();
        this.gameObject.GetComponent<EnemySpawnScript>().StartCoroutine(this.gameObject.GetComponent<EnemySpawnScript>().SpawnEnemiesOverTime(spawnInterval));
    }
}
