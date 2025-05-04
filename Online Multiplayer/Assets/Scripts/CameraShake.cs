using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPos;
    public float shakeAmount = 0.1f;  // How much shake to apply
    public float shakeDuration = 0.5f;  // How long the shake lasts

    private void Start()
    {
        originalPos = transform.localPosition;
    }

    public void TriggerShake(float amount, float duration)
    {
        shakeAmount = amount;
        shakeDuration = duration;
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-shakeAmount, shakeAmount);
            float y = Random.Range(-shakeAmount, shakeAmount);
            float z = originalPos.z;

            transform.localPosition = new Vector3(x, y, z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;  // Return to original position after shaking
    }
}
