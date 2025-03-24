using UnityEngine;
using UnityEngine.Events;
using XRInteraction = UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

/// <summary>
/// Interaction summary:
/// Begins with gravity turned off to ensure the object is accessible
/// Gravity is turned on once the object is grabbed
/// Gravity is turned off when the object is successfully attached
/// Gravity is turned off when object is repositioned due to falling too much
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
public class AttachmentPart : MonoBehaviour
{
    [Header("Attachment")]
    [SerializeField] private Transform attachmentPoint;
    [SerializeField] private float attachDistance = 0.1f; 

    [Header("Joint Settings")]
    [SerializeField] private float jointBreakForce = 4000f;
    [SerializeField] private bool enableCollision = false;

    private Rigidbody _rb;
    private XRInteraction.Interactables.XRGrabInteractable _grabInteractable;
    private Transform _objectTransform;
    private bool _isAttached = false;
    private bool _isBeingGrabbed = false;

    private FixedJoint _joint;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _objectTransform = transform;
        _grabInteractable = GetComponent<XRInteraction.Interactables.XRGrabInteractable>();
    }


    private void Update()
    {
        // Only check for attachment when being grabbed and not already attached
        if (_isBeingGrabbed && !_isAttached)
        {
            float distanceToAttach = Vector3.Distance(_objectTransform.position, attachmentPoint.position);
            
            // If close enough to attachment point, attach
            if (distanceToAttach < attachDistance)
            {
                AttachToPoint();
                Debug.Log("Object attached to attachment point!");
                GlobalEvents.StepsEvents.OnCompleteStep?.Invoke();
            }
        }
    }

    public void OnGrabbed()
    {
        Debug.Log("Object grabbed");
        _isBeingGrabbed = true;
        
        // If already attached, detach when grabbed
        if (_isAttached)
        {
            DetachFromPoint();
        }
    }

    public void OnReleased()
    {
        Debug.Log("Object released");
        _isBeingGrabbed = false;
    }


    private void OnAttach()
    {
        transform.parent = attachmentPoint;

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        // turn off gravity
        //m_Rigidbody.useGravity = false;
    }

    public void AttachToPoint()
    {
        if (attachmentPoint != null)
        {
            // Set position to attachment point
            transform.position = attachmentPoint.position;
            
            // Clean up any existing joint
            if (_joint != null)
            {
                Destroy(_joint);
            }
            
            Rigidbody attachPointRb = attachmentPoint.GetComponent<Rigidbody>();
            if (attachPointRb == null)
            {
                attachPointRb = attachmentPoint.gameObject.AddComponent<Rigidbody>();
                attachPointRb.isKinematic = true;
                attachPointRb.interpolation = RigidbodyInterpolation.Interpolate;
            }
            
            _joint = gameObject.AddComponent<FixedJoint>();
            _joint.connectedBody = attachPointRb;
            _joint.breakForce = jointBreakForce;
            _joint.enableCollision = enableCollision;
            
            _rb.isKinematic = false;
            _rb.useGravity = false;
            _rb.interpolation = RigidbodyInterpolation.Interpolate;
            
            // Mark as attached
            _isAttached = true;
            
            Debug.Log("Object firmly attached to point");
        }
    }

    public void DetachFromPoint()
    {
        Debug.Log("Detach Called");
        if (_joint != null)
        {
            Destroy(_joint);
        }
        
        _rb.isKinematic = false;
        _rb.useGravity = true;
        _rb.constraints = RigidbodyConstraints.None;
        
        _isAttached = false;
    }
}
