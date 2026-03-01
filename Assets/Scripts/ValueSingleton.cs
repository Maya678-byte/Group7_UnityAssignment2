using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ValueSingleton : MonoBehaviour
{
    public static ValueSingleton Instance { get; private set; }
    
    public int wave;
    public int ballsOut;
    public int health;

    public bool isSlow;
    public bool isBoost;
    public Color color;
    public String timerText;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject); // Destroy duplicate instances
            return;
        }

        DontDestroyOnLoad(gameObject);

        Instance = this; // Set the instance to this object
        color = Color.white;    
    }


    private void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0) return;
        
        if (ballsOut == wave)
        {
            ballsOut = 0;
            wave += 1;
            WaveAnnoucement.Instance.GetComponent<Animator>().Play(0);
            EnemySpawner.Instance.OnSpawn();
        }

        if (health <= 0)
        {
            GameUI.Instance.GameOver.SetActive(true);
            AudioSingleton.Instance.PlayGameOver();
            GameUI.Instance.InGame.SetActive(false);
        }
    }

    public void SlowTime()
    {
        StopCoroutine(ISlowTime());
        StartCoroutine(ISlowTime());
    }

    IEnumerator ISlowTime()
    {
        isSlow = true;
        yield return new WaitForSeconds(10f);
        isSlow = false;
    }
    
    public void BoostTime()
    {
        StopCoroutine(IBoostTime());
        StartCoroutine(IBoostTime());
    }

    IEnumerator IBoostTime()
    {
        isBoost = true;
        yield return new WaitForSeconds(10f);
        isBoost = false;
    }
    
}
