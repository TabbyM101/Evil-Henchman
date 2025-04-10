using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewsArticle : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private TextMeshProUGUI authorText;
    [SerializeField] private GameObject dividerObj;

    public void SetArticleContent(NewsArticleObj info, bool divider) {
        headerText.text = info.Headline;
        contentText.text = info.Content;
        authorText.text = "By: " + info.Author;
        dividerObj.SetActive(divider);
        float height = contentText.transform.parent.GetComponent<RectTransform>().rect.height;

        LayoutRebuilder.ForceRebuildLayoutImmediate(contentText.transform.parent.GetComponent<RectTransform>());

        Vector2 sizeDelta = dividerObj.transform.parent.GetComponent<RectTransform>().sizeDelta;
        sizeDelta.y -= height;
        sizeDelta.y += contentText.transform.parent.GetComponent<RectTransform>().rect.height;
        dividerObj.transform.parent.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        //LayoutRebuilder.MarkLayoutForRebuild(headerText.transform.parent.parent.GetComponent<RectTransform>());
    }
}
