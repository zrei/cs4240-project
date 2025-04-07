using UnityEngine;
using XRInteraction = UnityEngine.XR.Interaction.Toolkit;

public class ReturnToPosition : ResetPart
{
    public VoidEvent OnReposition;

    protected override void Reset()
    {
        transform.position = m_InitialWorldPosition;
        transform.rotation = m_InitialRotation;
        if (m_Rigidbody)
        {
            m_Rigidbody.useGravity = false;
            m_Rigidbody.isKinematic = true;
        }
            
        OnReposition?.Invoke();
    }
}
