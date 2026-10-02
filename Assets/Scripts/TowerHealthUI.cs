using UnityEngine;
using UnityEngine.UI;

public class TowerHealthUI : MonoBehaviour
{
    [Header("Health Bar")]
    [SerializeField] private Slider healthSlider;

    [Header("Tower Health")]
    [SerializeField] private float maximumHealth = 100f;

    [Header("Behaviour")]
    [SerializeField] private bool hideWhenTowerDestroyed = true;

    private MainTower mainTower;

    private void Start()
    {
        // The tower doesn't exist when the scene starts,
        // so the UI starts hidden.
        HideHealthBar();
    }

    private void Update()
    {
        // If we don't currently have a tower,
        // keep looking for the one the player places.
        if (mainTower == null)
        {
            FindMainTower();

            return;
        }

        UpdateHealthBar();
    }

    private void FindMainTower()
    {
        MainTower foundTower =
            FindFirstObjectByType<MainTower>();

        if (foundTower == null)
        {
            return;
        }

        mainTower = foundTower;

        Debug.Log(
            "TowerHealthUI found Main Tower: " +
            mainTower.gameObject.name
        );

        ShowHealthBar();

        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthSlider == null)
        {
            return;
        }

        float currentHealth =
            mainTower.CurrentHealth;

        healthSlider.maxValue =
            maximumHealth;

        healthSlider.value =
            Mathf.Clamp(
                currentHealth,
                0f,
                maximumHealth
            );

        if (
            hideWhenTowerDestroyed &&
            currentHealth <= 0f
        )
        {
            HideHealthBar();
        }
    }

    private void ShowHealthBar()
    {
        gameObject.SetActive(true);

        if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(true);
        }
    }

    private void HideHealthBar()
    {
        if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(false);
        }
    }
}