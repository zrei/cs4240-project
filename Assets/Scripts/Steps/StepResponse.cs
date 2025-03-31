using UnityEngine;

public abstract class StepResponse : MonoBehaviour
{
    [SerializeField] private Steps m_Step;

    public Steps Step => m_Step;

    protected virtual void Awake()
    {
        GlobalEvents.StepsEvents.OnBeginStep += OnStepChange;
    }

    protected virtual void OnDestroy()
    {
        GlobalEvents.StepsEvents.OnBeginStep -= OnStepChange;
        GlobalEvents.StepsEvents.OnCompleteStep -= OnCompleteStep;
    }

    protected virtual void OnStepChange(StepSO stepSO)
    {
        if (m_Step != stepSO.m_Step)
            return;

        GlobalEvents.StepsEvents.OnBeginStep -= OnStepChange;
        GlobalEvents.StepsEvents.OnCompleteStep += OnCompleteStep;
        OnBeginStep();
    }

    protected abstract void OnBeginStep();

    protected virtual void OnCompleteStep()
    {
        GlobalEvents.StepsEvents.OnCompleteStep -= OnCompleteStep;
    }
}
