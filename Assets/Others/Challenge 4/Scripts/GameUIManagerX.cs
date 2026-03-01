using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUIManagerX : MonoBehaviour
{
    public TextMeshProUGUI goalsMadeText;
    public TextMeshProUGUI goalsConcededText;
    public TextMeshProUGUI waveText;

    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;

    private SpawnManagerX spawnManager;

    void Start()
    {
        spawnManager = GameObject.Find("Spawn Manager")
                                 .GetComponent<SpawnManagerX>();

        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        goalsMadeText.text = "Goals Made: " + spawnManager.goalsMade;
        goalsConcededText.text = "Goals Conceded: " + spawnManager.goalsConceded;
        waveText.text = "Wave: " + spawnManager.waveNumber;

        CheckGameOver();
    }

    void CheckGameOver()
    {
        if (spawnManager.goalsMade >= spawnManager.maxGoals)
        {
            gameOverPanel.SetActive(true);
            gameOverText.text = "You Win!";
            Time.timeScale = 0f;
        }
        else if (spawnManager.goalsConceded >= spawnManager.maxGoals)
        {
            gameOverPanel.SetActive(true);
            gameOverText.text = "You Lose!";
            Time.timeScale = 0f;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}