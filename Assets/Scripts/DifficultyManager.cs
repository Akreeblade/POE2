using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [Header("Difficulty")]
    [SerializeField] private float startingDifficulty = 1f;
    [SerializeField] private float minimumDifficulty = 0.5f;
    [SerializeField] private float maximumDifficulty = 2f;

    [Header("Health Loss Thresholds")]
    [SerializeField] private float noDamageThreshold = 0f;
    [SerializeField] private float lowDamageThreshold = 10f;
    [SerializeField] private float highDamageThreshold = 30f;

    [Header("Difficulty Adjustments")]
    [SerializeField] private float significantIncrease = 0.2f;
    [SerializeField] private float smallIncrease = 0.1f;
    [SerializeField] private float decrease = 0.2f;

    private float currentDifficulty;

    private float startingHealth;

    public float CurrentDifficulty => currentDifficulty;

    private void Awake()
    {
        currentDifficulty = startingDifficulty;
    }

    /// <summary>
    /// Records the tower's health at the beginning of a wave.
    /// </summary>
    public void StartWave(float towerHealth)
    {
        startingHealth = towerHealth;

        Debug.Log(
            $"DifficultyManager: Starting health recorded: {startingHealth}"
        );

        Debug.Log(
            $"DifficultyManager: Current difficulty: {currentDifficulty:F2}"
        );
    }

    /// <summary>
    /// Calculates the percentage of health lost during the wave
    /// and adjusts the difficulty for the next wave.
    /// </summary>
    public void EndWave(float endingHealth)
    {
        if (startingHealth <= 0f)
        {
            Debug.LogWarning(
                "DifficultyManager: Starting health was 0 or less."
            );

            return;
        }

        float healthLost = startingHealth - endingHealth;

        if (healthLost < 0f)
        {
            healthLost = 0f;
        }

        float healthLostPercentage =
            (healthLost / startingHealth) * 100f;

        Debug.Log(
            $"DifficultyManager: Health lost: " +
            $"{healthLostPercentage:F1}%"
        );

        AdjustDifficulty(healthLostPercentage);
    }

    /// <summary>
    /// Adjusts difficulty based on tower damage.
    /// </summary>
    private void AdjustDifficulty(float healthLostPercentage)
    {
        if (healthLostPercentage <= noDamageThreshold)
        {
            currentDifficulty += significantIncrease;

            Debug.Log(
                "DifficultyManager: No health lost. " +
                "Increasing difficulty significantly."
            );
        }
        else if (healthLostPercentage <= lowDamageThreshold)
        {
            currentDifficulty += smallIncrease;

            Debug.Log(
                "DifficultyManager: Low health loss. " +
                "Increasing difficulty slightly."
            );
        }
        else if (healthLostPercentage <= highDamageThreshold)
        {
            Debug.Log(
                "DifficultyManager: Moderate health loss. " +
                "Maintaining difficulty."
            );
        }
        else
        {
            currentDifficulty -= decrease;

            Debug.Log(
                "DifficultyManager: High health loss. " +
                "Reducing difficulty."
            );
        }

        currentDifficulty = Mathf.Clamp(
            currentDifficulty,
            minimumDifficulty,
            maximumDifficulty
        );

        Debug.Log(
            $"DifficultyManager: New difficulty: " +
            $"{currentDifficulty:F2}"
        );
    }

    /// <summary>
    /// Returns a value adjusted by the current difficulty.
    /// Higher difficulty produces a higher value.
    /// </summary>
    public float GetDifficultyMultiplier()
    {
        return currentDifficulty;
    }
}