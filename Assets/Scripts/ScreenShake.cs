using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScreenShake : MonoBehaviour
{
    private Vector3 _initialPosition;
    public float shakeTime = 0f;

    private void OnEnable()
    {
        _initialPosition = transform.localPosition;
    }

    private void Update()
    {
        if (shakeTime > 0.0f)
        {
            transform.localPosition = _initialPosition + Random.insideUnitSphere * 0.1f;
            shakeTime -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = _initialPosition;
        }
    }
}