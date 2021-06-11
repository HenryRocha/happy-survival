public interface hr_IInteractable
{
    float maxRange { get; }
    void OnStartHover();
    void OnInteract();
    void OnStopHover();
}
