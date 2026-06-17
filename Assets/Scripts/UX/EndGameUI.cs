using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject gameplayPanel;
    public GameObject endGamePanel;

    [Header("Texts")]
    public TMP_Text winnerText;
    public TMP_Text finalScoreText;

    [Header("Button")]
    public Button restartButton;

    private void Awake()
    {
        endGamePanel.SetActive(false);

        restartButton.onClick.AddListener(
            RestartGame
        );
    }

    public void ShowResults(
        int yourScore,
        int enemyScore
    )
    {
        gameplayPanel.SetActive(false);
        endGamePanel.SetActive(true);

        // Mostrar mouse
        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;

        if (yourScore > enemyScore)
        {
            winnerText.text =
                " GANASTE";
        }
        else if (yourScore < enemyScore)
        {
            winnerText.text =
                " PERDISTE";
        }
        else
        {
            winnerText.text =
                " EMPATE";
        }

        finalScoreText.text =
            $"Tú: {yourScore}\n" +
            $"Rival: {enemyScore}";
    }

    public void HideResults()
    {
        gameplayPanel.SetActive(true);
        endGamePanel.SetActive(false);

        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;
    }

    private void RestartGame()
    {
        GameManager.Instance.RestartMatch();
    }
}
