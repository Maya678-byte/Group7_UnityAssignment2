using UnityEngine;
using Cinemachine;

public class ApplyCameraSensitivity : MonoBehaviour
{
    private const string SensKey = "camera_sensitivity";

    [Header("Default if no setting saved")]
    [SerializeField] private float defaultSensitivity = 2.5f;

    [Header("Base speeds (set these to your current values)")]
    [SerializeField] private float baseXSpeed = 300f;
    [SerializeField] private float baseYSpeed = 2f;

    private CinemachineFreeLook freeLook;

    private void Awake()
    {
        freeLook = GetComponent<CinemachineFreeLook>();
        if (freeLook == null)
        {
            Debug.LogError("ApplyCameraSensitivity: No CinemachineFreeLook on this object.");
            enabled = false;
            return;
        }

        float sens = PlayerPrefs.GetFloat(SensKey, defaultSensitivity);

        // Apply scaled speeds
        freeLook.m_XAxis.m_MaxSpeed = baseXSpeed * sens;
        freeLook.m_YAxis.m_MaxSpeed = baseYSpeed * sens;
    }
}