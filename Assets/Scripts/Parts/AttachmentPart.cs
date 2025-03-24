using UnityEngine;
using UnityEngine.Events;
using XRInteraction = UnityEngine.XR.Interaction.Toolkit;
using System.Collections;


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

        if (_isBeingGrabbed && !_isAttached)
        {
            float distanceToAttach = Vector3.Distance(_objectTransform.position, attachmentPoint.position);
            
            // If close enough attach
            if (distanceToAttach < attachDistance)
            {
                AttachToPoint();
                Debug.Log("Object attached to attachment point!");
            }
        }
    }

    public void OnGrabbed()
    {
        Debug.Log($"{gameObject.name} grabbed");
        _isBeingGrabbed = true;
        

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
        //m_Rigidbody.useGravity = false;
    }

    public void AttachToPoint()
    {
        if (attachmentPoint != null)
        {
            transform.position = attachmentPoint.position;
            
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
            
            _isAttached = true;
            
            Debug.Log($"{gameObject.name} firmly attached to point");
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
