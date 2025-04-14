using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RuiningEmail : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI emailContent;

    public void SetEmailContents(string contents) {
        emailContent.text = contents;
    }
}
