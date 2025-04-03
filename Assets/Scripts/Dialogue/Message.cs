using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    public int CharactersPerLine = 23;
    [SerializeField] private TextMeshProUGUI textLine;
    [SerializeField] private Image pfpImgage;
    [SerializeField] private Image reactionImage;
    [SerializeField] private RectTransform textSpawn;
    [SerializeField] private RectTransform reactionSpawn;
    [SerializeField] private RectTransform messageBackground;
    [SerializeField] private RectTransform outerObject;

    public void PopulateMessage(string messageText, bool lastMessageReceived, Sprite pfp = null) {
        textLine.text = messageText;
        textLine.gameObject.SetActive(true);
        pfpImgage.gameObject.SetActive(lastMessageReceived);
        if (pfp != null) pfpImgage.sprite = pfp;

        LayoutRebuilder.ForceRebuildLayoutImmediate(textSpawn);

        //format stuff
        float textLineHeight = textSpawn.rect.height + 0.008f;
        Vector2 updatedSizeDelta = messageBackground.sizeDelta;
        updatedSizeDelta.y = Mathf.Max(textLineHeight, 0.008f);
        messageBackground.sizeDelta = updatedSizeDelta;

        updatedSizeDelta = outerObject.sizeDelta;
        float additionalHeight = lastMessageReceived && (messageBackground.rect.height < 0.0565f) ? (0.0565f - messageBackground.rect.height)  : 0;
        Debug.Log(additionalHeight + " " + messageBackground.rect.height);
        updatedSizeDelta.y = messageBackground.rect.height + additionalHeight;
        outerObject.sizeDelta = updatedSizeDelta;
        reactionSpawn.SetAsLastSibling();
    }

    public void SpawnReaction(Sprite reaction) {
        reactionSpawn.gameObject.SetActive(true);
        Image obj = Instantiate(reactionImage, reactionSpawn);
        obj.sprite = reaction;
        obj.gameObject.SetActive(true);
    }
}
