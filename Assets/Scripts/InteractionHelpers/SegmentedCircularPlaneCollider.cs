using UnityEngine;

public class SegmentedCircularPlaneCollider : MonoBehaviour
{
    public VoidEvent TriggeredEvent;

    private LayerMask m_InteractionMask = -1;

#if UNITY_EDITOR
    public void Setup(Transform parent, float radius, float width, float height, float angle, string interactionLayerName)
    {
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        gameObject.layer = LayerMask.NameToLayer(interactionLayerName);

        BoxCollider collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.center = new Vector3(0, 0, 0.25f * radius);
        collider.size = new Vector3(0.5f * width, height, 0.5f * radius);

        transform.localRotation = Quaternion.Euler(0f, angle, 0f);
    }
#endif

    public void Init(LayerMask interactionMask)
    {
        m_InteractionMask = interactionMask;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer & m_InteractionMask) > 0)
        {
            TriggeredEvent?.Invoke();
        }
    }
}
