using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageDisplay : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image pfp;
    public GameObject reactions;
    public Image reactionIcon;
    public RectTransform outerObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void UpdateSize() {
        Vector2 newRect = Vector2.zero;
        newRect.x = outerObject.rect.width;
        newRect.y = text.gameObject.GetComponent<RectTransform>().rect.height;
        newRect.y += reactions.gameObject.GetComponent<RectTransform>().rect.height;
        outerObject.sizeDelta = newRect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
