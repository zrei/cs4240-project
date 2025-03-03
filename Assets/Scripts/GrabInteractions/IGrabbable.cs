using UnityEngine;

/// <summary>
/// Enum for the hand to know whether to change the state to "Grabbed"
/// </summary>
public enum GrabState
{
    NONE,
    GRABBED
}

// any scripts implementing this should also have the tag most likely
// or the layer
public interface IGrabbable
{
    // can pass something over if you need a hold state instead
    GrabState OnGrab(Transform grabbingTransform);

    void OnRelease();
}
