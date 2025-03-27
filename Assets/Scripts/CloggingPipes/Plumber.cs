using System;
using Unity.VisualScripting;
using UnityEngine;

public class Plumber : MonoBehaviour
{
    private Pipe _pipe;
    [SerializeField] private int clicksToRemove = 3;
    [SerializeField] private float timeToUnclog = 5;
    private float currentClicks = 0;
    private float clickTimer = 2f;
    
    public void SetPipeToPlumb(Pipe pipeToPlumb)
    {
        _pipe = pipeToPlumb;
        transform.SetParent(_pipe.GetPlumberAnchorSpot().transform, false);
    }

    private void Start()
    {
        Invoke(nameof(UnclogPipe), timeToUnclog);
    }

    public void OnPointerClick()
    {
            if (currentClicks < clicksToRemove - 1)
            {
                currentClicks++;
            }
            else
            {
                _pipe.Unplumb();
                Destroy(this.gameObject);
            }
    }

    public void UnclogPipe()
    {
        _pipe.Unplumb();
        _pipe.UnclogPipe();
        Destroy(this.gameObject);
    }
    
}