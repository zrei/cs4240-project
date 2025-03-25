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

    private Vector3 _initialPosition;
    private Quaternion _initialRotation;

    private FixedJoint _joint;

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

        AttachToConnector();
    }

    private void Update()
    {
        if (_isBeingGrabbed && _isAttached)
        {
            float distanceToAttach = Vector3.Distance(_objectTransform.position, attachmentPoint.position);
            Debug.Log("Distance to attachment point: " + distanceToAttach);
            
            if (distanceToAttach > attachDistance)
            {
                DetachFromConnector();
                Debug.Log("OnCompleteStep event fired");
                GlobalEvents.StepsEvents.OnCompleteStep?.Invoke();
            }
        }
    }

    private void AttachToConnector()
    {
        if (attachmentPoint != null)
        {
            transform.position = attachmentPoint.position;

            if (_joint != null)
            {
                Destroy(_joint);
            }

            Rigidbody connectorRb = attachmentPoint.GetComponent<Rigidbody>();
            if (connectorRb == null)
            {
                connectorRb = attachmentPoint.gameObject.AddComponent<Rigidbody>();
                connectorRb.isKinematic = true;
                connectorRb.interpolation = RigidbodyInterpolation.Interpolate;
            }

            _joint = gameObject.AddComponent<FixedJoint>();
            _joint.connectedBody = connectorRb;
            _joint.breakForce = 4000f;
            _joint.enableCollision = false;

            _rb.isKinematic = false;
            _rb.useGravity = false;
            _rb.mass = 1f;
            _rb.interpolation = RigidbodyInterpolation.Interpolate;

            _rb.constraints = RigidbodyConstraints.FreezeRotation;

            StartCoroutine(RemoveConstraintsAfterDelay(0.7f));

            _isAttached = true;
        }
    }

    public void OnGrabbed()
    {
        _isBeingGrabbed = true;
        
    }

    public void OnReleased()
    {
        _isBeingGrabbed = false;

        if (!_isAttached)
            _rb.isKinematic = false;
    }

    private IEnumerator RemoveConstraintsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (_isAttached)
        {
            _rb.constraints = RigidbodyConstraints.None;
        }
    }

    public void DetachFromConnector()
    {
        Debug.Log("Detach Called");
        if (_joint != null)
        {
            Destroy(_joint);
        }

        _rb.isKinematic = false;
        _rb.useGravity = true;

        _rb.linearDamping = 0.05f;
        _rb.angularDamping = 0.05f;

        _rb.constraints = RigidbodyConstraints.None;

        _isAttached = false;
    }

}
