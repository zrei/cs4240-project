using UnityEngine;

public class AdhesiveRemoval : MonoBehaviour
{
    // waiting for pinch?
    [SerializeField] private float m_MaxPullSpeed;
    [SerializeField] private float m_RequiredPullDistance;

    private Vector3 m_InitialPosition;
    private Vector3 m_PreviousPosition;
    private bool m_IsBeingPulled = false;
    private bool m_HasBeenRemoved = false;

    private void Start()
    {
        m_InitialPosition = transform.position;
    }

    // or something similar
    private void AttachToHand()
    {
        // transform.parent = playerHand.transform;
        m_IsBeingPulled = true;
    }

    private void Update()
    {
        if (m_HasBeenRemoved)
            return;

        if (m_IsBeingPulled)
        {
            float distanceMovedSoFarSquared = (transform.position - m_InitialPosition).sqrMagnitude;
            float speed = (transform.position - m_PreviousPosition).magnitude / Time.deltaTime;
        
            if (speed > m_MaxPullSpeed)
            {
                // play sound or something
            }

            if (distanceMovedSoFarSquared > m_RequiredPullDistance * m_RequiredPullDistance)
            {
                m_HasBeenRemoved = true;
                // handle
            }
        }

        m_PreviousPosition = transform.position;
    }
}
