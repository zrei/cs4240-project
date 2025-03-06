using UnityEngine;

public abstract class SingleStepHandler : MonoBehaviour
{
    [SerializeField] private Steps m_Step;

    protected bool m_IsActive = false;

    private void Awake()
    {
        GlobalEvents.StepsEvents.OnGoToStep += OnGoToStep;
    }

    private void OnDestroy()
    {
        GlobalEvents.StepsEvents.OnGoToStep -= OnGoToStep;
    }

    private void OnGoToStep(Steps step)
    {
        m_IsActive = step == m_Step;
    }

    protected void OnCompleteStep()
    {
        m_IsActive = false;
        GlobalEvents.StepsEvents.OnCompleteStep?.Invoke();
    }
}
