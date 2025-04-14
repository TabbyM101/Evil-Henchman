using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private float minIntensity;
    [SerializeField] private float maxIntensity;
    [SerializeField] private float flickerStrength;
    [SerializeField] private float flickerRate;
    private Light light;

    private void Start()
    {
        light = GetComponent<Light>();
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            light.intensity = Mathf.Lerp(minIntensity, maxIntensity, flickerStrength * Time.deltaTime);
            yield return new WaitForSeconds(flickerRate);
        }
    }
}
