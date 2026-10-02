using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class EnemyListScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private List<GameObject> enemies = new List<GameObject>();
    
    public void SetEnemies(GameObject enemy)
    {
       enemies.Add(enemy);
    }

    public void RemoveEnemies(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    public bool returnEnemylistifEmpty()
    {
        if (enemies.Count == 0)
        {
            Debug.Log("Enemy list is empty");
            return true;
        }
        else
        {
            Debug.Log("Enemy list is not empty");
            return false;
        }
    }
}
