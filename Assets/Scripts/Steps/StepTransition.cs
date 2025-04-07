using UnityEngine.Events;
using UnityEngine;

public class StepTransition : StepResponse
{
    [SerializeField] private UnityEvent TransitionEvents;

    protected override void OnBeginStep()
    {
        TransitionEvents?.Invoke();
    }
}