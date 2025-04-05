using Oculus.Interaction;

public abstract class HandGrabFilter : StepResponse, IGameObjectFilter
{
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

    bool IGameObjectFilter.Filter(UnityEngine.GameObject gameObject)
    {
        if (!_canInteract)
        {
            HandleCannotInteract();
        }

        return _canInteract;
    }

    protected abstract void HandleCannotInteract();

}
