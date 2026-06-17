using UnityEngine;
using TMPro;
using Unity.Netcode;

public class GameplayUI : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text timerText;
    public TMP_Text yourScoreText;
    public TMP_Text enemyScoreText;

    private GameManager gameManager;
    private PlayerController localPlayer;
    private PlayerController enemyPlayer;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        InvokeRepeating(nameof(FindPlayers), 1f, 1f);
    }

    private void Update()
    {
        UpdateTimer();
        UpdateScores();
    }

    private void FindPlayers()
    {
        PlayerController[] players =
        FindObjectsByType<PlayerController>(
            FindObjectsSortMode.None
        );

        ulong localClientId =
            NetworkManager.Singleton.LocalClientId;

        foreach (PlayerController player in players)
        {
            if (player.OwnerClientId == localClientId)
            {
                localPlayer = player;
            }
            else
            {
                enemyPlayer = player;
            }
        }
    }

    private void UpdateTimer()
    {
        if (gameManager == null) return;

        float time =
            gameManager.timeRemaining.Value;

        int minutes =
            Mathf.FloorToInt(time / 60);

        int seconds =
            Mathf.FloorToInt(time % 60);

        timerText.text =
            $"Tiempo: {minutes:00}:{seconds:00}";
    }

    private void UpdateScores()
    {
        if (localPlayer != null)
        {
            yourScoreText.text =
                $"Tú: {localPlayer.score.Value}";
        }

        if (enemyPlayer != null)
        {
            enemyScoreText.text =
                $"Rival: {enemyPlayer.score.Value}";
        }
        else
        {
            enemyScoreText.text =
                "Rival: esperando...";
        }
    }
}
