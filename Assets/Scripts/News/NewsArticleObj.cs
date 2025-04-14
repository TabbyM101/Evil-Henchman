using UnityEngine;

[CreateAssetMenu(fileName = "NewsArticleObj", menuName = "ScriptableObjects/NewsArticleObj")]
public class NewsArticleObj : ScriptableObject
{
    public string Headline;
    [TextArea]
    public string Content;
    public string Author;
    public bool SuspicionReq;
    public int MinimumSuspicionReq;
    public int MaximumSuspicionReq;
}
