using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewsManager : MonoBehaviour
{
    public static NewsManager Current;
    [SerializeField] private GameObject newsIcon;
    [SerializeField] private GameObject newsPanel;
    [SerializeField] private NewsArticle newsPrefab;
    [SerializeField] private RectTransform articleSpawn;
    [SerializeField] private TextMeshProUGUI dayIndicator;
    private List<NewsArticleObj> articles = new List<NewsArticleObj>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Current = this;
        Debug.Log(DayManager.Current.CurrentDayObj.name);
        articles = DayManager.Current.CurrentDayObj.Articles;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void DisplayNews() {
        newsIcon.SetActive(true);
        newsPanel.SetActive(true);
        bool firstArticle = true;
        dayIndicator.text = "Day " + DayManager.Current.dayNumber;
        foreach (NewsArticleObj article in articles) {
            if (!article.SuspicionReq || (DayManager.Current.Standing <= article.MinimumSuspicionReq && DayManager.Current.Standing >= article.MaximumSuspicionReq)) {
                NewsArticle obj = Instantiate(newsPrefab, articleSpawn);
                obj.gameObject.SetActive(true);
                obj.SetArticleContent(article, !firstArticle);
                firstArticle = false;
                LayoutRebuilder.MarkLayoutForRebuild(articleSpawn);
            }
        }
    }
}
