using UnityEngine;

public interface IInteractable
{
    float maxRange { get; }
    void OnStartHover();
    void OnInteract();
    void OnStopHover();
}
