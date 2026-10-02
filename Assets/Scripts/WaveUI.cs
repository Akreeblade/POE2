using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WaveManager waveManager;

    [Header("UI")]
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text enemyCountText;
    [SerializeField] private Button startWaveButton;

    [Header("Preparation UI")]
    [SerializeField] private GameObject preparationPanel;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text gameOverText;

    private void Start()
    {
        if (waveManager == null)
        {
            waveManager =
                FindFirstObjectByType<WaveManager>();
        }

        if (waveManager == null)
        {
            Debug.LogError(
                "WaveUI: WaveManager could not be found!",
                this
            );

            return;
        }

        if (startWaveButton != null)
        {
            startWaveButton.onClick.AddListener(
                StartWave
            );
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        UpdateUI();
    }

    private void Update()
    {
        if (waveManager == null)
        {
            return;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        UpdateWaveText();
        UpdateTimerText();
        UpdateEnemyCountText();
        UpdateStartButton();
        UpdatePreparationPanel();
        UpdateGameOverUI();
    }

    private void UpdateWaveText()
    {
        if (waveText == null)
        {
            return;
        }

        waveText.text =
            $"Wave {waveManager.CurrentWave}";
    }

    private void UpdateTimerText()
    {
        if (timerText == null)
        {
            return;
        }

        if (waveManager.GameOver)
        {
            timerText.text = "";
        }
        else if (waveManager.PreparationActive)
        {
            timerText.text =
                $"Next wave in {Mathf.CeilToInt( waveManager.PreparationTimer )}";
        }
        else if (waveManager.WaveActive)
        {
            timerText.text =
                $"Time: {Mathf.CeilToInt( waveManager.WaveTimer )}";
        }
        else
        {
            timerText.text = "";
        }
    }

    private void UpdateEnemyCountText()
    {
        if (enemyCountText == null)
        {
            return;
        }

        if (
            waveManager.WaveActive &&
            !waveManager.GameOver
        )
        {
            enemyCountText.text =
                $"{waveManager.EnemiesSpawned} / " +
                $"{waveManager.EnemiesToSpawn}";
        }
        else
        {
            enemyCountText.text = "";
        }
    }

    private void UpdateStartButton()
    {
        if (startWaveButton == null)
        {
            return;
        }

        if (waveManager.GameOver)
        {
            startWaveButton.gameObject.SetActive(false);
            return;
        }

        startWaveButton.gameObject.SetActive(
            waveManager.PreparationActive
        );

        startWaveButton.interactable =
            waveManager.PreparationActive;
    }

    private void UpdatePreparationPanel()
    {
        if (preparationPanel == null)
        {
            return;
        }

        if (waveManager.GameOver)
        {
            preparationPanel.SetActive(false);
            return;
        }

        preparationPanel.SetActive(
            waveManager.PreparationActive
        );
    }

    private void UpdateGameOverUI()
    {
        if (gameOverPanel == null)
        {
            return;
        }

        gameOverPanel.SetActive(
            waveManager.GameOver
        );

        if (
            waveManager.GameOver &&
            gameOverText != null
        )
        {
            gameOverText.text =
                $"GAME OVER\n\n" +
                $"You reached Wave " +
                $"{waveManager.CurrentWave}";
        }
    }

    private void StartWave()
    {
        if (waveManager == null)
        {
            return;
        }

        waveManager.StartWave();
    }

    private void OnDestroy()
    {
        if (startWaveButton != null)
        {
            startWaveButton.onClick.RemoveListener(
                StartWave
            );
        }
    }
}