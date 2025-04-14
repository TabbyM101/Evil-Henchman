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
        //AnimateLines();
    }

    public void Activate()
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

    private void AnimateLine(UILineRenderer line)
    {
        List<Vector2> points = line.Clone();
        
        Animate(line, points);
    }

    public void Animate(UILineRenderer line, List<Vector2> points)
    {
        line.points = new List<Vector2>();
        AnimatePoint(line, 0, points[0], points[0], true);
        for (int i = 1; i < points.Count; i++)
        {
            int index = i;
            AnimatePoint(line, index, new Vector2(0, 4), points[index]);
        }
    }

    private void AnimatePoint(UILineRenderer line, int index, Vector2 start, Vector2 end, bool instant = false)
    {
        if (instant == true)
        {
            LeanTween.delayedCall(0, () =>
            {
                if (index > 0)
                {
                    start = line.points[index - 1];
                    line.points.Add(start);
                }
                else
                {
                    line.points.Add(start);
                }
            
                LeanTween.value(gameObject, (value) =>
                {
                    if (line.points.Count > index)
                    {
                        line.points[index] = value;
                    }
                    else
                    {
                        line.points.Add(value);
                    }

                    line.SetVerticesDirty();
                }, start, end, time);
            });
            return;
        }
        
        LeanTween.delayedCall((time * index) - time, () =>
        {
            if (index > 0)
            {
                start = line.points[index - 1];
                line.points.Add(start);
            }
            else
            {
                line.points.Add(start);
            }
            
            LeanTween.value(gameObject, (value) =>
            {
                line.points[index] = value;
                line.SetVerticesDirty();
            }, start, end, time);
        });
    }
}
