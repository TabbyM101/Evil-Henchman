using System;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class StonkVisual : MonoBehaviour
{
    [SerializeField]private UIGridRenderer grid;

    [SerializeField]private UILineRenderer line;

    [SerializeField]private GraphAnimator animator;

    [SerializeField] private Stonks stonks;
    private void Start()
    {
        Invoke(nameof(Activate), 2);
    }

    private void Activate()
    {
        animator.time = 1;
        line.gridSize.x = Mathf.RoundToInt(stonks.time);
        List<UnityEngine.Vector2> points = new List<UnityEngine.Vector2>();
        //generate all of the necessary points for the line
        int timeCount = 0;
        float runTime = 0;
        points.Add(new UnityEngine.Vector2(timeCount, Mathf.RoundToInt(stonks.fishEndPositions[0])));
        timeCount++;
        
        for (int i = 1; i < stonks.fishMovesCount-1; i++)
        {
            runTime += stonks.fishMoveDelays[i - 1];
            if (runTime >= timeCount)
            {
                points.Add(new UnityEngine.Vector2(timeCount, Mathf.RoundToInt(stonks.fishEndPositions[i])));
                timeCount++;
            }
        }
        
        line.points = points;
        animator.lines = new UILineRenderer[] { line };
        animator.Activate();
    }
}
