using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SensitivitySliderBinder : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text valueText;

    [Header("Camera")]
    [SerializeField] private TMPro.Examples.CameraController cameraController;

    private void Start()
    {
        if (slider == null)
        {
            Debug.LogError("SensitivitySliderBinder: Slider reference missing.");
            enabled = false;
            return;
        }

        // Initialize slider to current camera sensitivity (already loaded from PlayerPrefs in Awake)
        if (cameraController != null)
        {
            slider.value = cameraController.MoveSensitivity;
            UpdateValueText(slider.value);
        }
        else
        {
            UpdateValueText(slider.value);
        }

        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnDestroy()
    {
        if (slider != null)
            slider.onValueChanged.RemoveListener(OnSliderChanged);
    }

    private void OnSliderChanged(float value)
    {
        if (cameraController != null)
            cameraController.SetSensitivity(value);

        UpdateValueText(value);
    }

    private void UpdateValueText(float value)
    {
        if (valueText != null)
            valueText.text = value.ToString("0.0");
    }
}