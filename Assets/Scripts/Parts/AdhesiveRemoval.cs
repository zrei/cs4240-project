using UnityEngine;

[RequireComponent(typeof(RemovalPart))]
public class AdhesiveRemoval : MonoBehaviour
{
    [SerializeField] private float m_MaximumSpeed;

    private bool m_WasAttachedLastFrame = false;

    private RemovalPart m_RemovalPart;
    private Vector3 m_PreviousPosition;

    private void Start()
    {
        m_RemovalPart = GetComponent<RemovalPart>();
    }

    private void Update()
    {
        if (m_WasAttachedLastFrame && m_RemovalPart.IsBeingGrabbed)
        {
            float speed = (transform.position - m_PreviousPosition).sqrMagnitude;
            if (speed > m_MaximumSpeed * m_MaximumSpeed)
                HandleFastSpeed();
        }

        m_PreviousPosition = transform.position;
        m_WasAttachedLastFrame = m_RemovalPart.IsAttached;
    }

    private void HandleFastSpeed()
    {

    }
}
