using UnityEngine;
using UnityEngine.UI;

public class SFXVolumeBar : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat("sfx_volume", 0.5f);
        slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnValueChanged);
    }

    private void OnValueChanged(float value)
    {
        if (AudioSingleton.Instance != null)
            AudioSingleton.Instance.SetSFXVolume(value);
    }
}