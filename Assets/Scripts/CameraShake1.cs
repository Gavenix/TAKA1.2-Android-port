using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private CinemachineCamera cinemachineCamera; // Используем Cinemachine 3.0+
    private CinemachineBasicMultiChannelPerlin noise;

    private Coroutine shakeCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        cinemachineCamera = GetComponent<CinemachineCamera>();

        if (cinemachineCamera == null)
        {
            Debug.LogError("CinemachineCamera не найден! Убедись, что скрипт CameraShake прикреплён к камере.");
            return;
        }

        // Получаем компонент шума
        noise = cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();

        if (noise == null)
        {
            Debug.LogError("CinemachineBasicMultiChannelPerlin не найден! Добавь его в камеру в Inspector.");
        }
    }

    public void Shake(float intensity, float duration)
    {
        if (noise == null) return;

        // Если уже идёт другая тряска – останавливаем её
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        shakeCoroutine = StartCoroutine(ShakeCoroutine(intensity, duration));
    }

    private IEnumerator ShakeCoroutine(float intensity, float duration)
    {
        noise.AmplitudeGain = intensity;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Плавное затухание тряски
        float fadeTime = 0.2f; // Время затухания
        float startIntensity = intensity;

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            noise.AmplitudeGain = Mathf.Lerp(startIntensity, 0, t / fadeTime);
            yield return null;
        }

        noise.AmplitudeGain = 0; // Полностью убираем тряску
        shakeCoroutine = null;
    }
}