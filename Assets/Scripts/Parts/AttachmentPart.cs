using UnityEngine;

/// <summary>
/// Interaction summary:
/// Begins with gravity turned off to ensure the object is accessible
/// Gravity is turned on once the object is grabbed
/// Gravity is turned off when the object is successfully attached
/// Gravity is turned off when object is repositioned due to falling too much
/// </summary>
public class AttachmentPart : MonoBehaviour
{
    [SerializeField] private Transform m_AttachPoint;
    [SerializeField] private Rigidbody m_Rigidbody;
    [SerializeField] private OneHandFreeGrabWithEvents m_OneHandFreeGrabWithEvents;
    [SerializeField] private ReturnToPosition m_ReturnToPosition;
    [SerializeField] private CollisionInteraction m_CollisionInteraction;

    private void Start()
    {
        m_Rigidbody.useGravity = false;
        m_ReturnToPosition.OnReposition += OnReposition;
        m_CollisionInteraction.OnCollisionInteraction += OnAttach;
    }

    private void OnDestroy()
    {
        m_ReturnToPosition.OnReposition -= OnReposition;
        m_CollisionInteraction.OnCollisionInteraction -= OnAttach;
    }

    private void OnEnable()
    {
        m_OneHandFreeGrabWithEvents.onObjectGrabbed += OnGrabbed;
    }

    private void OnDisable()
    {
        m_OneHandFreeGrabWithEvents.onObjectGrabbed -= OnGrabbed;
    }

    private void OnGrabbed(GameObject _)
    {
        m_Rigidbody.useGravity = true;
    }

    private void OnReposition()
    {
        m_Rigidbody.useGravity = false;
    }

    private void OnAttach()
    {
        // attach to another point
        transform.parent = m_AttachPoint;
        // reset position
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        // turn off gravity
        m_Rigidbody.useGravity = false;
    }
}
