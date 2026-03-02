using UnityEngine;
using UnityEngine.UI;

public class SensitivitySettings : MonoBehaviour
{
    public Slider sensSlider;

    private const string SensKey = "camera_sensitivity";
    private float defaultSensitivity = 1f;

    void Start()
    {
        float saved = PlayerPrefs.GetFloat(SensKey, defaultSensitivity);
        sensSlider.value = saved;

        sensSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnSliderChanged(float value)
    {
        PlayerPrefs.SetFloat(SensKey, value);
        PlayerPrefs.Save();
    }
}