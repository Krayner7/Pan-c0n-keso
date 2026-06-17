using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    [Header("Tiempo de partida")]
    public float gameDuration = 60f;

    public NetworkVariable<float> timeRemaining =
        new NetworkVariable<float>();

    private bool gameEnded = false;

    [Header("UI")]
    public EndGameUI endGameUI;

    private void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            StartMatch();
        }
    }

    private void Update()
    {
        if (!IsServer) return;
        if (gameEnded) return;

        timeRemaining.Value -= Time.deltaTime;

        if (timeRemaining.Value <= 0)
        {
            timeRemaining.Value = 0;
            gameEnded = true;

            EndGame();
        }
    }

    private void EndGame()
    {
        Debug.Log("FIN DEL JUEGO");

        ShowResultsClientRpc();
    }

    [ClientRpc]
    private void ShowResultsClientRpc()
    {
        Debug.Log("RPC recibido");

        if (endGameUI == null)
        {
            endGameUI =
                FindFirstObjectByType<EndGameUI>();
        }

        if (endGameUI == null)
        {
            Debug.LogError(
                "NO ENCONTRE EndGameUI"
            );
            return;
        }

        PlayerController[] players =
            FindObjectsByType<PlayerController>(
                FindObjectsSortMode.None
            );

        PlayerController localPlayer = null;
        PlayerController enemyPlayer = null;

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

        int yourScore =
            localPlayer != null
            ? localPlayer.score.Value
            : 0;

        int enemyScore =
            enemyPlayer != null
            ? enemyPlayer.score.Value
            : 0;

        endGameUI.ShowResults(
            yourScore,
            enemyScore
        );
    }

    public void RestartMatch()
    {
        PlayerController[] players =
            FindObjectsByType<PlayerController>(
                FindObjectsSortMode.None
            );

        foreach (PlayerController player in players)
        {
            player.score.Value = 0;
            player.isCarrying = false;
            player.carriedValue = 0;

            // mover a spawn
            player.transform.position =
                Vector3.zero;
        }

        StartMatch();

        RestartUIClientRpc();
    }

    [ClientRpc]
    private void RestartUIClientRpc()
    {

        if (endGameUI == null)
        {
            endGameUI =
                FindFirstObjectByType<EndGameUI>();
        }

        if (endGameUI != null)
        {
            endGameUI.HideResults();
        }

        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;
    }

    private void StartMatch()
    {
        gameEnded = false;
        timeRemaining.Value = gameDuration;
        CollectibleSpawner spawner =
    FindFirstObjectByType<
        CollectibleSpawner>();

        if (spawner != null)
        {
            spawner.SpawnCollectibles();
        }
    }
}
