using UnityEngine;
using UnityEngine.UI;

using EasyTransition;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    
    public static GameUI Instance { get; private set; }

    public GameObject GameOver;
    public GameObject PauseUI;

    public GameObject InGame;
    public GameObject FreeLook;
    public GameObject Player;
    
    public TransitionSettings transition;
    public float startDelay;
    public Text BestText;
    
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject); // Destroy duplicate instances
            return;
        }
        

        Instance = this; // Set the instance to this object
    }
    
    
    void Start()
    {
        GameOver.SetActive(false);
        InGame.SetActive(true);
        PauseUI.SetActive(false);
    }

    void Update()
    {
        if (GameOver.activeSelf)
        {
            if (Player.GetComponent<PlayerMovement>().enabled == true)
            {
                AudioSingleton.Instance.PlayGameOver();
            }
            FreeLook.SetActive(false);
            Player.GetComponent<PlayerMovement>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            
        } else if (PauseUI.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (PlayerPrefs.GetInt("Wave") < ValueSingleton.Instance.wave)
        {
            PlayerPrefs.SetInt("Wave", ValueSingleton.Instance.wave);
        }

        if (GameOver.activeSelf == false && Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseUI.activeSelf)
            {
                AudioSingleton.Instance.PlayUnPause();
                PauseUI.SetActive(false);
                InGame.SetActive(true);
                Time.timeScale = 1;
            }
            else
            {
                AudioSingleton.Instance.PlayPause();
                PauseUI.SetActive(true);
                InGame.SetActive(false);
                Time.timeScale = 0;

            }
        }
        
        BestText.text = "Highest Wave: " + PlayerPrefs.GetInt("Wave").ToString();
    }

    public void RTM()
    {
        Time.timeScale = 1;

        TransitionManager.Instance().Transition(SceneManager.GetActiveScene().buildIndex-1,transition, startDelay);
    }

    public void Continue()
    {
        AudioSingleton.Instance.PlayUnPause();
        PauseUI.SetActive(false);
        InGame.SetActive(true);
        Time.timeScale = 1;

    }
}
