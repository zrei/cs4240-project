using TMPro;
using UnityEngine;

public class StepsUIDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_StepsText;

    private const string FINISHED_ALL_STEPS = "You've completed everything!";

    private void Awake()
    {
        GlobalEvents.StepsEvents.OnBeginStep += SetStep;
        GlobalEvents.StepsEvents.OnCompleteAllSteps += CompleteAllText;
    }

    private void OnDestroy()
    {
        GlobalEvents.StepsEvents.OnBeginStep -= SetStep;
        GlobalEvents.StepsEvents.OnCompleteAllSteps -= CompleteAllText;
    }

    private void SetStep(StepSO step)
    {
        m_StepsText.text = step.m_StepInstruction;
    }

    private void CompleteAllText()
    {
        m_StepsText.text = FINISHED_ALL_STEPS;
    }
}
