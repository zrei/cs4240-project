using UnityEngine;
using XR = UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
public class GrabResponse : MonoBehaviour
{
    [SerializeField] private SoundCue m_GrabSound;
    [SerializeField] private SoundCue m_ReleaseSound;

    public bool IsBeingHandGrabbed { get; private set; } = false;
    public bool IsBeingPinchGrabbed { get; private set; } = false;

    public bool IsBeingGrabbed => IsBeingHandGrabbed || IsBeingPinchGrabbed;

    private XR.Interactables.XRGrabInteractable _grabInteractable;
    private void Start()
    {
        _grabInteractable = GetComponent<XR.Interactables.XRGrabInteractable>();
        _grabInteractable.selectEntered.AddListener(OnSelectEnter);
        _grabInteractable.selectExited.AddListener(OnSelectExit);
        _grabInteractable.activated.AddListener(OnActivate);
        _grabInteractable.deactivated.AddListener(OnDeactivate);
    }

    private void OnDestroy()
    {
        _grabInteractable.selectEntered.RemoveAllListeners();
        _grabInteractable.selectExited.RemoveAllListeners();
        _grabInteractable.activated.RemoveAllListeners();
        _grabInteractable.deactivated.RemoveAllListeners();
    }

    private void OnSelectEnter(XR.SelectEnterEventArgs _)
    {
        IsBeingHandGrabbed = true;

        if (!IsBeingPinchGrabbed)
            UponGrab();
    }

    private void OnSelectExit(XR.SelectExitEventArgs _)
    {
        IsBeingHandGrabbed = false;

        if (!IsBeingPinchGrabbed)
            UponRelease();
    }

    private void OnActivate(XR.ActivateEventArgs _)
    {
        IsBeingPinchGrabbed = true;

        if (!IsBeingHandGrabbed)
            UponGrab();
    }

    private void OnDeactivate(XR.DeactivateEventArgs _)
    {
        IsBeingPinchGrabbed = false;

        if (!IsBeingHandGrabbed)
            UponRelease();
    }

    private void UponGrab()
    {
        if (m_GrabSound != null)
        {
            m_GrabSound.ToggleSoundPlaying(true);
        }
    }

    private void UponRelease()
    {
        if (m_ReleaseSound != null)
        {
            m_ReleaseSound.ToggleSoundPlaying(true);
        }
    }
}
