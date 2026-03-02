using System;
using EasyTransition;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject CustomMenu;
    public GameObject htpMenu;

    public TransitionSettings transition;
    public float startDelay;


    public GameObject camera;
    public GameObject back;

    void Start()
    {
        Main();
        //AudioSingleton.Instance.PlayMenu();
        Screen.SetResolution(1920, 1080, true);
    }
    

    public void Play()
    {
        //AudioSingleton.Instance.PlayButton();
        TransitionManager.Instance().Transition(SceneManager.GetActiveScene().buildIndex+1,transition, startDelay);
    }


    public void Main()
    {
        camera.GetComponent<MenuCamera>().GoToMenu();
        if (CustomMenu.activeSelf || settingsMenu.activeSelf || htpMenu.activeSelf)
        {
            //AudioSingleton.Instance.PlayButton();
        }
        back.SetActive(false);
        mainMenu.SetActive(true);
        htpMenu.SetActive(false);
        settingsMenu.SetActive(false);
        CustomMenu.SetActive(false);
    }

    public void Settings()
    {
       // AudioSingleton.Instance.PlayButton();
        
        htpMenu.SetActive(false);
        settingsMenu.SetActive(true);
        CustomMenu.SetActive(false);
    }
    
    public void HTP()
    {
        //AudioSingleton.Instance.PlayButton();
        
        htpMenu.SetActive(true);
        settingsMenu.SetActive(false);
        CustomMenu.SetActive(false);
    }

    public void Customization()
    {
        camera.GetComponent<MenuCamera>().GoToCustom();
        back.SetActive(true);

        
        //AudioSingleton.Instance.PlayButton();

        mainMenu.SetActive(false);
        htpMenu.SetActive(false);
        settingsMenu.SetActive(false);
        CustomMenu.SetActive(true);
    }
    
    public void ChangeScreenMode(int index)
    {
        switch (index)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;

            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;

            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}