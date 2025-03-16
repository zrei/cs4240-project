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

    private Vector3 m_InitialLocalPosition;
    private Quaternion m_InitialLocalRotation;
    private Vector3 m_InitialWorldPosition;
    private Vector3 m_PreviousPosition;
    private bool m_IsBeingPulled = false;
    private bool m_HasBeenRemoved = false;
    private void Start()
    {
        m_Rigidbody.useGravity = false;

        m_GrabNotifier.onObjectGrabbed += OnPinched;
        m_GrabNotifier.onObjectReleased += OnPinchStop;

        m_InitialLocalPosition = m_TrackedTransform.localPosition;
        m_InitialLocalRotation = m_TrackedTransform.localRotation;
    }

    private void OnDestroy()
    {
        m_GrabNotifier.onObjectGrabbed -= OnPinched;
        m_GrabNotifier.onObjectReleased -= OnPinchStop;
    }

    private void OnPinched(GameObject _)
    {
        Debug.Log("Pulled");
        m_InitialWorldPosition = m_TrackedTransform.position;
        m_IsBeingPulled = true;
    }

    private void OnPinchStop(GameObject _)
    {
        m_IsBeingPulled = false;

        if (!m_HasBeenRemoved)
        {
            m_TrackedTransform.localPosition = m_InitialLocalPosition;
            m_TrackedTransform.localRotation = m_InitialLocalRotation;
        }
    }

    private void Update()
    {
        if (m_HasBeenRemoved)
            return;

        if (m_IsBeingPulled)
        {
            float distanceMovedSoFarSquared = (m_TrackedTransform.position - m_InitialWorldPosition).sqrMagnitude;
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
                m_TrackedTransform.parent = null;
                Logger.Log(typeof(AdhesiveRemoval), "Removed adhesive", LogLevel.LOG);
                GlobalEvents.StepsEvents.OnCompleteStep?.Invoke();
            }
        }

        m_PreviousPosition = m_TrackedTransform.position;
    }
}
