using UnityEngine;
using XRInteraction = UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRInteraction.Interactables.XRSimpleInteractable))]
public class StomaGazeStep : MonoBehaviour
{
    private XRInteraction.Interactables.XRSimpleInteractable _interactable;

    private void Start()
    {
        _interactable = GetComponent<XRInteraction.Interactables.XRSimpleInteractable>();
        _interactable.selectEntered.AddListener(OnGazeSelected);
        _interactable.hoverEntered.AddListener(OnHoverEntered);
    }

    private void OnDestroy()
    {
        _interactable.selectEntered.RemoveAllListeners();
        _interactable.hoverEntered.RemoveAllListeners();
    }

    private void OnGazeSelected(XRInteraction.SelectEnterEventArgs _)
    {
        GlobalEvents.StepsEvents.OnCompleteStep?.Invoke();
    }

    private void OnHoverEntered(XRInteraction.HoverEnterEventArgs _)
    {
        Debug.Log("Hover entered");
    }
}
