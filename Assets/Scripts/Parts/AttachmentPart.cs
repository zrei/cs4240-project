using UnityEngine;

[RequireComponent(typeof(GrabResponse))]
public class AttachmentPart : MonoBehaviour
{
    [Header("Attachment")]
    [SerializeField] private Transform attachmentPoint;
    [SerializeField] private float attachDistance = 0.1f;

    [Header("Joint Settings")]
    [SerializeField] private float jointBreakForce = 4000f;
    [SerializeField] private bool enableCollision = false;

    private Rigidbody _rb;
    private GrabResponse _grabResponse;
    private Transform _objectTransform;
    private bool _isAttached = false;

    private FixedJoint _joint;

    private CollisionInteraction m_CollisionInteraction;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _objectTransform = transform;
        _grabResponse = GetComponent<GrabResponse>();
        m_CollisionInteraction = attachmentPoint.GetComponentInChildren<CollisionInteraction>(true);

        if (m_CollisionInteraction)
        {
            m_CollisionInteraction.OnCollisionInteraction += OnCollideWithAttachedPoint;
        }
    }

    private void OnDestroy()
    {
        if (m_CollisionInteraction)
            m_CollisionInteraction.OnCollisionInteraction -= OnCollideWithAttachedPoint;
    }


    private void Update()
    {

        if (_grabResponse.IsBeingGrabbed && !_isAttached)
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

    private void OnCollideWithAttachedPoint()
    {
        if (_isAttached)
            return;
        AttachToPoint();
    }

    private void AttachToPoint()
    {
        if (m_CollisionInteraction)
            m_CollisionInteraction.OnCollisionInteraction -= OnCollideWithAttachedPoint;

        if (attachmentPoint != null)
        {
            transform.position = attachmentPoint.position;
            transform.rotation = attachmentPoint.rotation;
            
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
}
