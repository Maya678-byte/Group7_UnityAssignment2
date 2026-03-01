using UnityEngine;
using UnityEngine.UI;

public class AudioSingleton : MonoBehaviour
{
    
    public static AudioSingleton Instance { get; private set; } 
    public float volume;

    public AudioSource jump;
    public AudioSource pause;
    public AudioSource unpause;
    
    public AudioSource button;
    public AudioSource menu;
    public AudioSource normal;
    
    public AudioSource goal;
    public AudioSource gameover;
    public AudioSource smash;
    
    private void Awake()
    {
        volume = 0.5f;
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject); // Destroy duplicate instances
            return;
        }
        DontDestroyOnLoad(gameObject);

        Instance = this; // Set the instance to this object
    }

    public void ChangeVolume(Slider slider)
    {
        volume = slider.value;
        
        jump.volume = volume;
        pause.volume = volume;
        unpause.volume = volume;
        
        button.volume = volume;
        menu.volume = volume;
        normal.volume = volume;
        
        goal.volume = volume;
        gameover.volume = volume;
        smash.volume = volume;
       
    }

    

    public void PlayJump()
    {
        jump.volume = volume;
        jump.Play();
    }
    

    public void PlayPause()
    {
        pause.volume = volume;
        pause.Play();
    }
    public void PlayUnPause()
    {
        unpause.volume = volume;
        unpause.Play();
    }

    public void PlayButton()
    {
        button.volume = volume;
        button.Play();
    }

    public void PlayMenu()
    {
        PauseMusic();
        menu.volume = volume;
        menu.loop = true;
        menu.Play();
    }

    public void PlayNormal()
    {
        PauseMusic();
        normal.volume = volume;
        normal.loop = true;
        normal.Play();
    }

    public void PlayGoal()
    {
        goal.volume = volume;
        goal.Play();
    }
    
    public void PlayGameOver()
    {
        gameover.volume = volume;
        gameover.Play();
    }

    public void PlaySmash()
    {
        smash.volume = volume;
        smash.Play();
    }


    public void PauseMusic()
    {
        normal.Stop();
        menu.Stop();
    }
}