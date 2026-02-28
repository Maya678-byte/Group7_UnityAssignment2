using UnityEngine;
using UnityEngine.UI;

using EasyTransition;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    
    public static GameUI Instance { get; private set; }

    public GameObject GameOver;

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
    }

    void Update()
    {
        if (GameOver.activeSelf)
        {
            FreeLook.SetActive(false);
            Player.GetComponent<PlayerMovement>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }

        if (PlayerPrefs.GetInt("Wave") < ValueSingleton.Instance.wave)
        {
            PlayerPrefs.SetInt("Wave", ValueSingleton.Instance.wave);
        }
        
        BestText.text = "Highest Wave: " + PlayerPrefs.GetInt("Wave").ToString();
    }

    public void RTM()
    {
        TransitionManager.Instance().Transition(SceneManager.GetActiveScene().buildIndex-1,transition, startDelay);
    }
}
