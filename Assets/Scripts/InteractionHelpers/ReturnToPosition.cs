using UnityEngine;

public class ReturnToPosition : ResetPart
{
    public VoidEvent OnReposition;

    protected override void Reset()
    {
        transform.position = m_InitialWorldPosition;
        transform.rotation = m_InitialRotation;
        if (m_Rigidbody)
        {
            m_Rigidbody.linearVelocity = Vector3.zero;
            m_Rigidbody.angularVelocity = Vector3.zero;
        }
            
        OnReposition?.Invoke();
    }
}
