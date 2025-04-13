using System;
using Unity.VisualScripting;
using UnityEngine;

public class Plumber : MonoBehaviour
{
    private Pipe _pipe;
    [SerializeField] private int clicksToRemove = 3;
    [SerializeField] private float timeToUnclog = 5;
    private int currentClicks = 0;
    [SerializeField]private float clickTimer = 2f;
    private float? lastClickTime = null;
    [SerializeField] private Animation clickAnimation;

    public void SetPipeToPlumb(Pipe pipeToPlumb)
    {
        _pipe = pipeToPlumb;
        transform.SetParent(_pipe.GetPlumberAnchorSpot().transform, false);
    }

    private void Start()
    {
        Invoke(nameof(UnclogPipe), timeToUnclog);
    }

    private void Update()
    {
        if (lastClickTime.HasValue && Time.time - lastClickTime.Value >= clickTimer)
        {
            currentClicks = 0; // reset click count
            lastClickTime = null; // clear last click time
        }
    }

    public void OnPointerClick()
    {
        clickAnimation.Stop(); // stop current animation
        clickAnimation.Play(); // play animation from start

        currentClicks++;

        if (currentClicks >= clicksToRemove)
        {
            _pipe.Unplumb();
            Destroy(this.gameObject);
        }
        else
        {
            lastClickTime = Time.time; // update last click time
        }
    }

    public void UnclogPipe()
    {
        _pipe.Unplumb();
        _pipe.UnclogPipe();
        Destroy(this.gameObject);
    }
}