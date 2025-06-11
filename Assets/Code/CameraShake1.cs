using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public float duration = 0.5f;  // Dauer des Kamerawackelns
    public float amplitude = 1.2f;  // Stärke des Kamerawackelns
    public float frequency = 2.0f;  // Frequenz des Kamerawackelns

    private float elapsed = 0.0f;
    private CinemachineBasicMultiChannelPerlin cinemachinePerlin;
    private float initialAmplitude;
    private float initialFrequency;

    void Start()
    {
        CinemachineVirtualCamera vcam = GetComponent<CinemachineVirtualCamera>();
        if (vcam != null)
        {
            cinemachinePerlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        else
        {
            Debug.LogError("CinemachineVirtualCamera component not found.");
        }
    }

    void Update()
    {
        if (elapsed < duration && cinemachinePerlin != null)
        {
            // Während der Schüttelphase
            cinemachinePerlin.m_AmplitudeGain = amplitude;
            cinemachinePerlin.m_FrequencyGain = frequency;
            elapsed += Time.deltaTime;
        }
        else if (cinemachinePerlin != null)
        {
            // Sanftes Ausklingen
            float remainingTime = duration * 0.5f; // Die Zeitspanne, über die das Ausklingen stattfinden soll
            float t = (elapsed - duration) / remainingTime; // Normalisiertes Intervall für Lerp

            cinemachinePerlin.m_AmplitudeGain = Mathf.Lerp(amplitude, 0, t);
            cinemachinePerlin.m_FrequencyGain = Mathf.Lerp(frequency, 0, t);

            elapsed += Time.deltaTime;

            if (t >= 1.0f)
            {
                // Wenn das Ausklingen abgeschlossen ist, alles zurücksetzen
                cinemachinePerlin.m_AmplitudeGain = 0;
                cinemachinePerlin.m_FrequencyGain = 0;
                enabled = false;
            }
        }
    }

    public void TriggerShake()
    {
        elapsed = 0.0f;
        initialAmplitude = amplitude;
        initialFrequency = frequency;
        enabled = true;  // Aktiviert das Skript zum Wackeln der Kamera
    }
}
