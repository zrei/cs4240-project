using UnityEngine;
using XRInteraction = UnityEngine.XR.Interaction.Toolkit;

public class ReturnToPosition : MonoBehaviour
{
    [Header("Height")]
    [SerializeField] private bool m_UseMinimumHeight = false;
    [Tooltip("Whether to calculate the height of the transform relative to its position when this script was enabled")]
    [SerializeField] private bool m_CalculateHeightRelativeToOriginalPosition = false;
    [SerializeField] private float m_MinimumHeight = -10f;

    [Header("Trigger")]
    [SerializeField] private bool m_UseTrigger = false;
    [SerializeField] private LayerMask m_AllowedLayers;

    private Vector3 m_InitialWorldPosition;
    private Quaternion m_InitialRotation;

    public VoidEvent OnReposition;

    private Rigidbody m_Rigidbody;
    private XRInteraction.Interactables.XRGrabInteractable _grabInteractable;
    private bool m_IsBeingGrabbed = false;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        _grabInteractable = GetComponent<XRInteraction.Interactables.XRGrabInteractable>();
    }

    private void OnEnable()
    {
        m_InitialWorldPosition = transform.position;    
        m_InitialRotation = transform.rotation;

        if (_grabInteractable)
        {
            _grabInteractable.selectEntered.AddListener(OnGrabbed);
            _grabInteractable.selectExited.AddListener(OnReleased);
        }
        
    }

    private void OnDisable()
    {
        if (_grabInteractable)
        {
            _grabInteractable.selectEntered.RemoveAllListeners();
            _grabInteractable.selectExited.RemoveAllListeners();
        }

        m_IsBeingGrabbed = false;
    }

    private void Update()
    {
        if (!m_IsBeingGrabbed && m_UseMinimumHeight)
        {
            if (CalculateHeight() < m_MinimumHeight)
                Reposition();
        }
    }

    private void OnGrabbed(XRInteraction.SelectEnterEventArgs _)
    {
        m_IsBeingGrabbed = true;
    }

    private void OnReleased(XRInteraction.SelectExitEventArgs _)
    {
        m_IsBeingGrabbed = false;
    }

    private float CalculateHeight()
    {
        return m_CalculateHeightRelativeToOriginalPosition ? transform.position.y - m_InitialWorldPosition.y : transform.position.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_IsBeingGrabbed && ((1 << other.gameObject.layer) & m_AllowedLayers) != 0)
        {
            Reposition();
        }
    }

    private void Reposition()
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
