using TMPro;
using UnityEngine;

public class StepsUIDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_StepsText;

    private void Awake()
    {
        GlobalEvents.StepsEvents.OnBeginStep += SetStep;
    }

    private void OnDestroy()
    {
        GlobalEvents.StepsEvents.OnBeginStep -= SetStep;
    }

    private void SetStep(StepSO step)
    {
        m_StepsText.text = step.m_StepInstruction;
    }
}
