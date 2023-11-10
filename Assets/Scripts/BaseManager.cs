using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseManager : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private float baseCaptureRate = 1.0f;
    [SerializeField]
    private int maxEnemyMultiplierLimit = 4;
    [SerializeField]
    private float baseRecoveryRate = 1.0f;

    private int numberOfEnemiesCapturing = 0;

    private void Start()
    {
        slider.value = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            numberOfEnemiesCapturing++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            numberOfEnemiesCapturing--;
        }
    }

    private void Update()
    {
        if(numberOfEnemiesCapturing > 0 && slider.value < slider.maxValue)
        {
            slider.value += baseCaptureRate * Time.deltaTime * Mathf.Clamp(numberOfEnemiesCapturing, numberOfEnemiesCapturing, maxEnemyMultiplierLimit);
        }
        else if(numberOfEnemiesCapturing == 0 && slider.value > slider.minValue)
        {
            slider.value -= baseRecoveryRate * Time.deltaTime;
        }
    }
}
