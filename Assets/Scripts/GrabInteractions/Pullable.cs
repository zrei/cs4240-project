using UnityEngine;

public abstract class Pullable : MonoBehaviour, IGrabbable
{
    [Tooltip("Distance (squared) away from the origin that this must be pulled to activate the event")]
    [SerializeField] private float m_PullDistanceSquared;

    private Transform m_CurrentlyGrabbingTransform = null;

    public GrabState OnGrab(Transform grabbedTransform)
    {
        m_CurrentlyGrabbingTransform = grabbedTransform;
        return GrabState.GRABBED;
    }

    public void OnRelease()
    {
        m_CurrentlyGrabbingTransform = null;
    }

    private void Update()
    {
        if (m_CurrentlyGrabbingTransform)
        {
            float distance = (transform.position - m_CurrentlyGrabbingTransform.position).sqrMagnitude;
            if (distance >= m_PullDistanceSquared)
            {
                OnRelease();
                OnPull();
            }
        }
    }

    protected abstract void OnPull();
}