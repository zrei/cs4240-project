using Oculus.Interaction;
using UnityEngine;

public class PlayerHandsSetup : MonoBehaviour
{
    [SerializeField] private HandVisual m_LeftHandVisual;
    [SerializeField] private HandVisual m_RightHandVisual;

    private void Start()
    {
        m_LeftHandVisual.ForceOffVisibility = true;
        m_RightHandVisual.ForceOffVisibility = true;
    }
}