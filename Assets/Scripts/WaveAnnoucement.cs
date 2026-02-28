using System;
using UnityEngine;
using UnityEngine.UI;

public class WaveAnnoucement : MonoBehaviour
{
    public static WaveAnnoucement Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject); // Destroy duplicate instances
            return;
        }
        
        Instance = this; // Set the instance to this object
    }

    private void Update()
    {
        GetComponent<Text>().text = "Wave " + ValueSingleton.Instance.wave;
    }
}
