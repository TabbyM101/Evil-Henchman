using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/DayObject")]
public class DayObj : ScriptableObject
{
    public enum SceneType
    {
        Office,
        Room
    }
    // Each day has a scene
    public SceneType sceneType;
    
    // Each day has a list of minigames
    public List<TicketObj> Minigames;

    public Dialogue startDay;
    // TODO if we want to implement endDay dialogue, then it will need to not close the scene until XYZ seconds after dialogue is done.
    // public Dialogue endDay;
}