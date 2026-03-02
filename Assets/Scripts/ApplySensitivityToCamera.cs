using UnityEngine;
using Cinemachine;

public class ApplySensitivityToCamera : MonoBehaviour
{
    private const string SensKey = "camera_sensitivity";
    private float defaultSensitivity = 1f;

    private CinemachineFreeLook freeLook;

    private float baseXSpeed;
    private float baseYSpeed;

    void Awake()
    {
        freeLook = GetComponent<CinemachineFreeLook>();

        baseXSpeed = freeLook.m_XAxis.m_MaxSpeed;
        baseYSpeed = freeLook.m_YAxis.m_MaxSpeed;

        float sens = PlayerPrefs.GetFloat(SensKey, defaultSensitivity);

        freeLook.m_XAxis.m_MaxSpeed = baseXSpeed * sens;
        freeLook.m_YAxis.m_MaxSpeed = baseYSpeed * sens;
    }
}