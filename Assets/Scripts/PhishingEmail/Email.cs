using UnityEngine;
using System.Collections;

public class Email : MonoBehaviour
{
    [SerializeField] public float swimmingSpeed;
    [SerializeField] public float swimmingHeight;
    public bool isGood;
    public RectTransform canvasTransform;
    private RectTransform rectTransform;
    private float startingHeight;
    private bool caught = false;

    private void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        startingHeight = rectTransform.anchoredPosition.y;
        StartCoroutine(MoveEmail());
    }

    IEnumerator MoveEmail()
    {
        while(!caught)
        {
            Vector2 newPosition = rectTransform.anchoredPosition;
            newPosition.x -= swimmingSpeed;
            if (newPosition.x < canvasTransform.rect.min.x)
            {
                Destroy(gameObject);
            }
            newPosition.y = (Mathf.Cos(swimmingSpeed * Time.time) * swimmingHeight) + startingHeight;
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, newPosition, Time.deltaTime * swimmingSpeed);
            yield return null;
        }
    }

    public int getScore() {
        if (isGood)
        {
            return 1;
        }
        return -1;
    }

    private void OnTriggerEnter(Collider other) {
        FishingLine hook = other.gameObject.GetComponentInParent<FishingLine>();
        if (!hook.HasFish)
        {
            caught = true;
            hook.caughtFish = this.gameObject;
            this.gameObject.transform.SetParent(other.gameObject.transform);
        }
    }
}