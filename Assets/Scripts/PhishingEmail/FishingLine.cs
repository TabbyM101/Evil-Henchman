using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FishingLine : MonoBehaviour
{
    [SerializeField] private float lineSpeed;
    [SerializeField] private GameObject canvas;
    private RectTransform rectTransform;
    private Vector2 canvasPos;
    public GameObject caughtFish;
    public bool HasFish => caughtFish != null;

    private void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        RectTransform canvasTransform = canvas.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, canvas.transform.position, Camera.main, out canvasPos);
    }

    private void Update()
    {
        UpdateHeight();

    }

    private void UpdateHeight()
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, Input.mousePosition, Camera.main, out mousePos);
        float newHeight = -mousePos.y;
        if (newHeight > 0 && newHeight <= -canvasPos.y) {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newHeight);
        }
    }
}