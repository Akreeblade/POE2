
using System.Collections;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnScript : MonoBehaviour
{
    [SerializeField] GameObject enemyprefab;
    GameObject manager;
    PathGenerator pathgenerator;
    GameObject[] spawnpoints = new GameObject[3];
    [SerializeField] int EnemiesToSpawn = 20;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = GameObject.Find("GameManager");

        
        pathgenerator = manager.GetComponent<PathGenerator>();

        

        //Debug.Log("EnemySpawnScript successfully found PathGenerator.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void GetSpawnPoints()
    {
        spawnpoints=null;
        spawnpoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }


    public IEnumerator  SpawnEnemiesOverTime(float spawnInterval)
    {
        for (int i = 0; i < EnemiesToSpawn; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    public void SpawnEnemy()
    {
        
              int path = Random.Range(0, 3);
              GameObject spawnPoint = spawnpoints[path];
              GameObject enemy = Instantiate(enemyprefab, spawnPoint.transform.position, Quaternion.identity);
                Debug.Log("enemy spawned");
            Enemy_walking walking = enemy.GetComponent<Enemy_walking>();

            walking.SetPath(pathgenerator.generatedPaths[path].waypoints);
        

    }

}
