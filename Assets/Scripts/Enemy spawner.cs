using UnityEditor.UI;
using UnityEngine;

public class Enemyspawner : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        gameManager.enemySpawners.Add(this);
    }
}
