using UnityEngine;

public class AdhesiveRemoval : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private OneHandFreeGrabWithEvents m_GrabNotifier;
    [SerializeField] private Rigidbody m_Rigidbody;
    [SerializeField] private Transform m_TrackedTransform;

    [Header("Settings")]
    [SerializeField] private float m_MaxPullSpeed;
    [SerializeField] private float m_RequiredPullDistance;

    private Vector3 m_InitialPosition;
    private Quaternion m_InitialRotation;
    private Vector3 m_PreviousPosition;
    private bool m_IsBeingPulled = false;
    private bool m_HasBeenRemoved = false;
    private void Start()
    {
        m_Rigidbody.useGravity = false;

        m_GrabNotifier.onObjectGrabbed += OnPinched;
        m_GrabNotifier.onObjectReleased += OnPinchStop;
    }

    private void OnDestroy()
    {
        m_GrabNotifier.onObjectGrabbed -= OnPinched;
        m_GrabNotifier.onObjectReleased -= OnPinchStop;
    }

    private void OnPinched(GameObject _)
    {
        m_InitialPosition = m_TrackedTransform.position;
        m_InitialRotation = m_TrackedTransform.rotation;
        m_IsBeingPulled = true;
    }

    private void OnPinchStop(GameObject _)
    {
        m_IsBeingPulled = false;

        if (!m_HasBeenRemoved)
        {
            m_TrackedTransform.position = m_InitialPosition;
            m_TrackedTransform.rotation = m_InitialRotation;
        }
    }

    private void Update()
    {
        if (m_HasBeenRemoved)
            return;

        if (m_IsBeingPulled)
        {
            float distanceMovedSoFarSquared = (m_TrackedTransform.position - m_InitialPosition).sqrMagnitude;
            float speed = (m_TrackedTransform.position - m_PreviousPosition).magnitude / Time.deltaTime;

            if (speed > m_MaxPullSpeed)
            {                
                Logger.Log(typeof(AdhesiveRemoval), "Exceeded speed", LogLevel.LOG);
                // play sound or something
            }

            if (distanceMovedSoFarSquared > m_RequiredPullDistance * m_RequiredPullDistance)
            {
                m_HasBeenRemoved = true;
                m_Rigidbody.useGravity = true;
                transform.parent = null;
                Logger.Log(typeof(AdhesiveRemoval), "Removed adhesive", LogLevel.LOG);
                GlobalEvents.StepsEvents.OnCompleteStep?.Invoke();
                // handle
            }
        }

        m_PreviousPosition = m_TrackedTransform.position;
    }
}
