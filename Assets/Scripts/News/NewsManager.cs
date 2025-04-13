using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewsManager : MonoBehaviour
{
    public static NewsManager Current;
    [SerializeField] private GameObject newsIcon;
    [SerializeField] private GameObject newsPanel;
    [SerializeField] private NewsArticle newsPrefab;
    [SerializeField] private RectTransform articleSpawn;
    [SerializeField] private TextMeshProUGUI dayIndicator;
    [SerializeField] private GameObject newsNotif;
    [SerializeField] [Tooltip("Overrides any articles from the dayObj")] private List<NewsArticleObj> injectedArticles; // Manually inject news articles for tutorial scene
    private List<NewsArticleObj> articles = new List<NewsArticleObj>();

    void Start()
    {
        Current = this;
        articles = injectedArticles.Any() ? injectedArticles : DayManager.Current.CurrentDayObj.Articles;
    }
    
    public void DisplayNews() {
        newsIcon.SetActive(true);
        newsPanel.SetActive(true);
        bool firstArticle = true;
        dayIndicator.text = "Day " + DayManager.Current.dayNumber;
        foreach (NewsArticleObj article in articles) {
            if (!article.SuspicionReq || (DayManager.Current.Standing > article.MinimumSuspicionReq && DayManager.Current.Standing <= article.MaximumSuspicionReq)) {
                NewsArticle obj = Instantiate(newsPrefab, articleSpawn);
                obj.gameObject.SetActive(true);
                obj.SetArticleContent(article, !firstArticle);
                firstArticle = false;
                LayoutRebuilder.MarkLayoutForRebuild(articleSpawn);
            }
        }
    }

    public System.Collections.IEnumerator SendCompleteNewsNotification() {
        yield return new WaitForSeconds(1 * articleSpawn.childCount); //scales with amount of articles
        AudioManager.Current.PlayClip("generalNotification");
        newsNotif.SetActive(true);
    }

    public void DoneReading() {

        if (DayManager.Current.isLastDay)
        {
            // Last day
            SceneManager.LoadScene("EndGameScreen");
        } else if (DayManager.Current.dayNumber == -1) {
            SceneManager.LoadScene("TrainingVideo");
        }
        else {
            DayManager.Current.StartNewDay();
        }
    }
}
