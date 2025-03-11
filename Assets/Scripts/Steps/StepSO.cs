using UnityEngine;

public enum Steps
{
    REMOVE_BAG,
    REMOVE_ADHESIVE
}

// can also add substeps if needed
[CreateAssetMenu(fileName = "StepSO", menuName = "ScriptableObject/StepSO")]
public class StepSO : ScriptableObject
{
    public string m_StepInstruction;
    public Steps m_Step;
}