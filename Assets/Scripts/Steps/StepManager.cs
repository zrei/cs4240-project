using UnityEngine;
using System.Collections.Generic;

public class StepManager : Singleton<StepManager>
{
    [SerializeField] private List<StepSO> m_Steps;

    private int m_CurrStepIndex;
    public Steps CurrStep => m_Steps[m_CurrStepIndex].m_Step;
    public int CurrStepNumber => m_CurrStepIndex + 1;
    public bool HasCompletedAllSteps => m_CurrStepIndex >= m_Steps.Count;

    private static string STEP_FORMAT_STRING = "Step {0}: {1}";

    protected override void HandleAwake()
    {
        base.HandleAwake();

        m_CurrStepIndex = 0;
        OnGoToStep();

        GlobalEvents.StepsEvents.OnCompleteStep += OnCompleteStep;
    }

    protected override void HandleDestroy()
    {
        base.HandleDestroy();

        GlobalEvents.StepsEvents.OnCompleteStep -= OnCompleteStep;
    }

    #region Step Handling
    private void OnCompleteStep()
    {
        ++m_CurrStepIndex;

        OnGoToStep();
    }

    private void OnGoToStep()
    {
        if (!HasCompletedAllSteps)
            GlobalEvents.StepsEvents.OnGoToStep?.Invoke(CurrStep);
    }
    #endregion

    #region Helper
    public string GetStepFormattedString()
    {
        return string.Format(STEP_FORMAT_STRING, CurrStepNumber, m_Steps[m_CurrStepIndex].m_StepInstruction);
    }
    #endregion
}