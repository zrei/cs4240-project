using UnityEngine;

public enum Steps
{
    REMOVE_BAG,
    DISPOSE_BAG,
    REMOVE_ADHESIVE,
    DISPOSE_ADHESIVE,
    CLEAN_DIRTY_SKIN,
    ATTACH_ADHESIVE,
    ATTACH_CLEAN_BAG,
    CLEAN_SKIN,
    DISPOSE_WET_WIPES
}

// can also add substeps if needed
[CreateAssetMenu(fileName = "StepSO", menuName = "ScriptableObject/StepSO")]
public class StepSO : ScriptableObject
{
    public string m_StepInstruction;
    public Steps m_Step;
}