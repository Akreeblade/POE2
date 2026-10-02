using UnityEngine;

public class RoundStart : MonoBehaviour
{
    private int roundNumber = 1;
    private GameManager gameManager;
    private WaveManager waveManager;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
        waveManager = GetComponent<WaveManager>();

        if (waveManager == null)
        {
            Debug.LogError(
                "RoundStart: WaveManager could not be found!",
                this
            );
        }
    }

    public void StartRound()
    {
        roundNumber++;

        if (waveManager != null)
        {
            waveManager.StartWave();
        }
    }
}