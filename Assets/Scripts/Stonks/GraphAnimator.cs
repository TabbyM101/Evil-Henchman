using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GraphAnimator : MonoBehaviour
{
    public UILineRenderer[] lines;

    public float time = 1f;

    private void OnEnable()
    {
        AnimateLines();
    }

    public void AnimateLines()
    {
        foreach (UILineRenderer line in lines)
        {
            AnimateLine(line);
        }
    }

    void AnimateLine(UILineRenderer line)
    {
        //List<Vector2> points = line.points.Clone();
        
        //Animate(line, points);
    }

    public void Animate(UILineRenderer line, List<Vector2> points)
    {
        line.points = new List<Vector2>();
        for (int i=0; i < points.Count; i++)
        {
            int index = i;
            AnimatePoint(line, index, new Vector2(0,4), points[index]);
        }
        
    }

    private void AnimatePoint(UILineRenderer line, int index, Vector2 start, Vector2 end)
    {
        
    }
}
