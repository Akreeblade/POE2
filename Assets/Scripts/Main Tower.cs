using UnityEngine;
using UnityEngine.Events;

public class MainTower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;

    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;

    [Header("Events")]
    public UnityEvent onHealthChanged;
    public UnityEvent onTowerDestroyed;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    private bool isDestroyed;

    private void Start()
    {
        if (gameManager == null)
        {
            GameObject managerObject = GameObject.Find("GameManager");

            if (managerObject != null)
                gameManager = managerObject.GetComponent<GameManager>();
        }

        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        isDestroyed = currentHealth <= 0f;
    }

    public void TakeDamage(float damage)
    {
        if (isDestroyed || damage <= 0f)
            return;

        currentHealth = Mathf.Max(0f, currentHealth - damage);

        onHealthChanged?.Invoke();

        Debug.Log($"Main Tower Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0f)
        {
            isDestroyed = true;
            Debug.Log("Main Tower destroyed! Game Over.");

            onTowerDestroyed?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        if (isDestroyed || amount <= 0f)
            return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        onHealthChanged?.Invoke();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDestroyed = false;
        onHealthChanged?.Invoke();
    }
}