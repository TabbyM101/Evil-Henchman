using TMPro;
using UnityEngine;

public class AlphaLerp : MonoBehaviour
{
    public GameObject obj;
    public Color minColor;
    public Color maxColor;
    public float lerpRate = 1f;
    private float t;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        Color lerped =  Color.Lerp(minColor, maxColor, Mathf.Abs(Mathf.Sin(t * lerpRate)));
        obj.GetComponent<TextMeshProUGUI>().color = lerped;
    }
}
