using UnityEngine;
using System.Collections;

/// <summary>
/// Part initially connected to a point that needs to be removed
/// by grabbing. Once removed, the complete step event is fired
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(GrabResponse))]
public class RemovalPart : MonoBehaviour
{
    [Header("Attachment")]
    [SerializeField] private Transform attachmentPoint;
    [SerializeField] private float attachDistance = 0.1f;

    [Header("Sound")]
    [SerializeField] private SoundCue m_RemovalSound;

    private FixedJoint _joint;

    private Rigidbody _rb;
    private Transform _objectTransform;
    public bool IsAttached { get; private set; } = true;
    public bool IsBeingGrabbed => _grabResponse != null && _grabResponse.IsBeingGrabbed;

    private GrabResponse _grabResponse;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _grabResponse = GetComponent<GrabResponse>();
        _objectTransform = transform;

        AttachToConnector();
    }

    private void Update()
    {
        if (IsBeingGrabbed && IsAttached)
        {
            float distanceToAttach = Vector3.Distance(_objectTransform.position, attachmentPoint.position);

            if (distanceToAttach > attachDistance)
            {
                DetachFromConnector();
                GlobalEvents.StepsEvents.OnCompleteStep?.Invoke();
            }
        }
    }

    public void AttachToConnector()
    {
        if (attachmentPoint != null)
        {
            transform.position = attachmentPoint.position;
            transform.rotation = attachmentPoint.rotation;

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

            IsAttached = true;
        }
    }
    private IEnumerator RemoveConstraintsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (IsAttached)
        {
            _rb.constraints = RigidbodyConstraints.None;
        }
    }

    private void DetachFromConnector()
    {
        if (m_RemovalSound != null)
        {
            m_RemovalSound.ToggleSoundPlaying(true);
        }

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

        IsAttached = false;
    }

}
