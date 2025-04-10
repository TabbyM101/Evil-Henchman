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

    private bool animating = false;
    private int count = 0;
    private void Start()
    {
        line.gridSize.x = Mathf.RoundToInt(stonks.time);
        Invoke(nameof(Activate), 0.2f);
    }

    private void Activate()
    {
        /*animator.time = 1;
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
        animator.Activate();*/
        
        line.points = new List<UnityEngine.Vector2>();
        for (int i = 0; i <= line.gridSize.x; i++)
        {
            line.points.Add(new UnityEngine.Vector2(i, Mathf.RoundToInt((float)line.gridSize.y/2)));
        }
        animator.time = 1;
        animator.lines = new UILineRenderer[] { line };
        animator.Activate();
        animating = true;
    }

    private float timer = 0f; // declare a timer outside of the Update function so it retains its value

    private void Update()
    {
        if (animating && line.points.Count > count)
        {
            UnityEngine.Vector2 currentPoint = line.points[count];

            float scoreChange = stonks.score; // Get the score change from Stonks

            // Apply scoreChange to y value in real time, mapping 0 score to the middle of the graph at y = line.gridSize.y / 2
            currentPoint.y = Mathf.Clamp(line.gridSize.y / 2 + scoreChange, 0, line.gridSize.y);

            line.points[count] = currentPoint;

            timer += Time.deltaTime;

            // If we've passed 1 second, move to the next point
            if (timer >= 1f)
            {
                timer = 0f; // reset timer
                count++;

                // If number of points equals gridSize.x, stop animating
                if (count >= Mathf.RoundToInt(line.gridSize.x))
                {
                    animating = false;
                }
            }
        }
    }
}
