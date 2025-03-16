using UnityEngine;

public class AttachmentPart : MonoBehaviour
{
    [SerializeField] private LayerMask m_AttachmentColliderMask;
    [SerializeField] private Transform m_AttachPoint;
    [SerializeField] private Rigidbody m_Rigidbody;

    private void OnCollisionEnter(Collision collision)
    {
        // complete the step, which should deactivate the grab component automatically
        GlobalEvents.StepsEvents.OnCompleteStep?.Invoke();

        // attach to another point
        transform.parent = m_AttachPoint;
        // reset position
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        // turn off gravity
        m_Rigidbody.useGravity = false;
    }
}
