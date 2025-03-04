using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class GrabAndFollow : MonoBehaviour, IGrabbable
{
    [SerializeField] private bool m_EnablePhysicsWhenNotGrabbed = false;

    private Rigidbody m_Rb;
    private Transform m_OriginalParent;
    private void Start()
    {
        m_Rb = GetComponent<Rigidbody>();
        m_OriginalParent = transform.parent;
    }

    public GrabState OnGrab(Transform grabbedTransform)
    {
        transform.parent = grabbedTransform;
        transform.localPosition = Vector3.zero;
        //transform.localRotation = Quaternion.identity;
        m_Rb.isKinematic = true;
        return GrabState.GRABBED;
    }

    public void OnRelease()
    {
        transform.parent = m_OriginalParent;
        if (m_EnablePhysicsWhenNotGrabbed)
            m_Rb.isKinematic = false;
    }
}