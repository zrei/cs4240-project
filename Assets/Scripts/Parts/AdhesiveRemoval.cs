using UnityEngine;
using UnityEngine.Events;
using XRInteraction = UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

[RequireComponent(typeof(Rigidbody), typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
public class AttachableObject : MonoBehaviour
{
    [Header("Attachment")]
    [SerializeField] private Transform attachmentPoint;
    [SerializeField] private float attachDistance = 0.1f;

    private Rigidbody _rb;
    private XRInteraction.Interactables.XRGrabInteractable _grabInteractable;
    private Transform _objectTransform;
    private bool _isAttached = true;
    private bool _isBeingGrabbed = false;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _objectTransform = transform;
        _grabInteractable = GetComponent<XRInteraction.Interactables.XRGrabInteractable>();
        
        _rb.isKinematic = true;
    }

    private void Update()
    {
        if (_isBeingGrabbed && _isAttached)
        {
            float distanceToAttach = Vector3.Distance(_objectTransform.position, attachmentPoint.position);
            Debug.Log("Distance to attachment point: " + distanceToAttach);
            
            if (distanceToAttach > attachDistance)
            {
                DetachFromPoint();
                Debug.Log("OnCompleteStep event fired");
            }
        }
    }

    public void OnGrabbed()
    {
        _isBeingGrabbed = true;
        
    }

    public void OnReleased()
    {
        _isBeingGrabbed = false;
    }


    public void DetachFromPoint()
    {
        Debug.Log("Detach Called on adhesive removal");

        transform.SetParent(null);
        
        _rb.isKinematic = false;
        _rb.useGravity = true;
        
        _rb.linearDamping = 0.05f;
        _rb.angularDamping = 0.05f;
        
        _rb.constraints = RigidbodyConstraints.None;
        
        _isAttached = false;
    }
}
