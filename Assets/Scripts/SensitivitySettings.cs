using UnityEngine;
using UnityEngine.UI;

public class SensitivitySettings : MonoBehaviour
{
    public Slider sensSlider;

    private const string SensKey = "camera_sensitivity";

    void Start()
    {
        float saved = PlayerPrefs.GetFloat(SensKey, 2.5f);

        sensSlider.value = saved;

        sensSlider.onValueChanged.AddListener(OnSensitivityChanged);
    }

    void OnSensitivityChanged(float value)
    {
        PlayerPrefs.SetFloat(SensKey, value);
        PlayerPrefs.Save();
    }
}