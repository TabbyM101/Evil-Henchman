using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    public int CharactersPerLine = 23;
    [SerializeField] private GameObject textLine;
    [SerializeField] private Image reactionImage;
    [SerializeField] private Image profilePicture;
    [SerializeField] private RectTransform textSpawn;
    [SerializeField] private RectTransform reactionSpawn;
    [SerializeField] private RectTransform messageBackground;
    [SerializeField] private RectTransform outerObject;

    public void PopulateMessage(string messageText, Sprite pfp) {
        string line = "";
        int textLines = 1;
        GameObject textObject = Instantiate(textLine, textSpawn);
        TextMeshProUGUI textObjectTextChild = textObject.GetComponentInChildren<TextMeshProUGUI>();
        textObjectTextChild.text = line;
        textObject.gameObject.SetActive(true);
        for (int i = 0; i < messageText.Length; i++) {
            if (messageText[i] == ' ') {
                if (line.Length >= 23) {
                    textLines++;
                    textObjectTextChild.text = line;
                    textObject = Instantiate(textLine, textSpawn);
                    textObjectTextChild = textObject.GetComponentInChildren<TextMeshProUGUI>();
                    textObject.gameObject.SetActive(true);
                    line = "";
                }
                else {
                    line += messageText[i];
                }
            }
            else {
                line += messageText[i];
            }
        }
        textObjectTextChild.text = line;

        // textObjectTextChild.text = line;
        // int startIndex = 0;
        // bool help = textObjectTextChild.isTextOverflowing;
        // Debug.Log(help);
        // while (textObjectTextChild.isTextOverflowing) {
        //     Debug.Log("new line!!");
        //     int endIndex = textObjectTextChild.firstOverflowCharacterIndex;
        //     Debug.Log("start index " + startIndex + "end index " + endIndex);
        //     textLines++;
        //     textObjectTextChild.text = line.Substring(startIndex, endIndex);
        //     Debug.Log(textObjectTextChild.text);
        //     line = line.Substring(endIndex);
        //     Debug.Log(line);
        //     startIndex = endIndex;
    
        //     textObject = Instantiate(textLine, textSpawn);
        //     textObjectTextChild = textObject.GetComponentInChildren<TextMeshProUGUI>();
        //     textObject.gameObject.SetActive(true);
        //     Debug.Log("new objects made");
        // }

        Debug.Log("lkines: " + textLines);

        //format stuff
        profilePicture.sprite = pfp;
        float textLineHeight = textObject.GetComponent<RectTransform>().rect.height;
        Vector2 updatedSizeDelta = messageBackground.sizeDelta;
        updatedSizeDelta.y = Mathf.Max(textLineHeight * textLines, profilePicture.GetComponent<RectTransform>().rect.height);
        messageBackground.sizeDelta = updatedSizeDelta;

        updatedSizeDelta = outerObject.sizeDelta;
        updatedSizeDelta.y = messageBackground.rect.height;
        outerObject.sizeDelta = updatedSizeDelta;
        reactionSpawn.SetAsLastSibling();
    }

    public void SpawnReactions(Sprite[] reactions) {
        reactionSpawn.gameObject.SetActive(true);
        foreach(Sprite reaction in reactions) {
            Image obj = Instantiate(reactionImage, reactionSpawn);
            obj.sprite = reaction;
            obj.gameObject.SetActive(true);
        }
    }
}
