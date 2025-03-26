using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] float minIntensity;
    [SerializeField] float maxIntensity;
    [SerializeField] float flickerStrength;
    [SerializeField] float flickerRate;
    private Light light;

    void Start()
    {
        light = GetComponent<Light>();
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            light.intensity = Mathf.Lerp(minIntensity, maxIntensity, flickerStrength * Time.deltaTime);
            yield return new WaitForSeconds(flickerRate);
        }
    }
}
