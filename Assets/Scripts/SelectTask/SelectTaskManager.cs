using UnityEngine;

public class SelectTaskManager : MonoBehaviour
{
    public static SelectTaskManager Current; // Singleton
    [SerializeField] private SelectTaskDisplay display;
    [SerializeField] private GameObject selectTaskWindow;

    /// <summary>
    /// Initialize singleton var.
    /// </summary>
    private void Awake()
    {
        Current = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectTaskWindow.SetActive(false);
    }

    public void TaskSelected(int ticketObjIndex) {
        selectTaskWindow.SetActive(true);
        display.OpenDisplay(DayManager.Current.CurrentDayObj.Minigames, ticketObjIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            selectTaskWindow.SetActive(false);
        }
    }
}
