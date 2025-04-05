using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class GrabResponse : MonoBehaviour
{
    [SerializeField] private SoundCue m_GrabSound;
    [SerializeField] private SoundCue m_ReleaseSound;

    public bool IsBeingGrabbed { get; private set; } = false;
    public VoidEvent OnGrabbedEvent;
    public VoidEvent OnReleasedEvent;

    private HandGrabInteractable _handGrabInteractable;

    private void Start()
    {
        _handGrabInteractable = GetComponentInChildren<HandGrabInteractable>(true);
        _handGrabInteractable.WhenPointerEventRaised += OnPointerEventRaised;
    }

    private void OnDestroy()
    {
        _handGrabInteractable.WhenPointerEventRaised -= OnPointerEventRaised;
    }

    private void OnPointerEventRaised(PointerEvent pointerEvent)
    {
        switch (pointerEvent.Type)
        {
            case PointerEventType.Select:
                UponGrab();
                break;
            case PointerEventType.Unselect:
                UponRelease();
                break;
        }
    }

    private void UponGrab()
    {
        IsBeingGrabbed = true;
        OnGrabbedEvent?.Invoke();
        if (m_GrabSound != null)
        {
            m_GrabSound.ToggleSoundPlaying(true);
        }
    }

    private void UponRelease()
    {
        IsBeingGrabbed = false;
        OnReleasedEvent?.Invoke();
        if (m_ReleaseSound != null)
        {
            m_ReleaseSound.ToggleSoundPlaying(true);
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (GetComponentInChildren<HandGrabInteractable>() == null)
        {
            Debug.LogError("There is no HandGrabInteractable in the hierarchy for this grab response");
        }
    }
#endif
}
