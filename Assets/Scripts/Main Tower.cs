using UnityEngine;

public class MainTower : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
