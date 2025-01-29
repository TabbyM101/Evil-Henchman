using System;
using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    private Action onClick;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown() {
        Debug.Log(gameObject.name);
    }

    public void SetOnClickAction(Action action) {
        onClick = action;
    }
}
