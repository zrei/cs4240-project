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
    }

    private void OnGazeSelected(XRInteraction.SelectEnterEventArgs _)
    {
        GlobalEvents.StepsEvents.OnCompleteStep?.Invoke();
    }
}
