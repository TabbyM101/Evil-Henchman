using UnityEngine;

/// <summary>
/// Interface for a minigame. Since minigames are widely varied, we just provide an entrypoint and let the concrete class handle the rest.
/// </summary>
public interface IMinigame
{
    void StartMinigame();
}
