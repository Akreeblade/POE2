using TMPro;
using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    private int maxHealth = 10;
    private int health = 10;
    [SerializeField ]private TextMeshProUGUI end_text;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        end_text = FindAnyObjectByType<TextMeshProUGUI>();
        if (end_text == null)
        {
            Debug.LogError("EndText UI element not found!");
        }
        Debug.Log(end_text);
        end_text.text = "TowerHealth: " + health + " / " + maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            end_text.text = "TowerHealth: " + health +" / " + maxHealth;
            health--;
            Debug.Log("Tower hit! Health: " + health);
            if (health <= 0)
            {
                end_text.text = "Game Over";
                Destroy(gameObject);
                Debug.Log("Tower destroyed!");
            }
        }
    }
}