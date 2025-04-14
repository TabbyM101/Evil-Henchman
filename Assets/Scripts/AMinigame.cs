using UnityEngine;

// Convenience class to automatically start minigame on start. Used for Scene-based minigame scripts.
public abstract class AMinigame : MonoBehaviour
{
    private void Start() => StartMinigame();

    protected abstract void StartMinigame();
}
