using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

public abstract class StepsSelectFilter : StepResponse, IXRSelectFilter, IXRHoverFilter
{
    bool IXRSelectFilter.canProcess { get; } = true;
    bool IXRHoverFilter.canProcess { get; } = true;

    private bool _canInteract = false;

    protected override void OnBeginStep()
    {
        _canInteract = true;
    }

    protected override void OnCompleteStep()
    {
        base.OnCompleteStep();
        _canInteract = false;
    }

    bool IXRSelectFilter.Process(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactor, UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable interactable)
    {
        if (!_canInteract)
        {
            HandleCannotInteract();
        }
        return _canInteract;
    }

    bool IXRHoverFilter.Process(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRHoverInteractor interactor, UnityEngine.XR.Interaction.Toolkit.Interactables.IXRHoverInteractable interactable)
    {
        if (!_canInteract)
        {
            HandleCannotInteract();
        }
        return _canInteract; 
    }

    protected abstract void HandleCannotInteract();

}
