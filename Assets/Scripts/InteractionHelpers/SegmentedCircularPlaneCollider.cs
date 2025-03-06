using UnityEngine;

public class SegmentedCircularPlaneCollider : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private LayerMask m_InteractionMask;

    public VoidEvent TriggeredEvent;

    public void Setup(Transform parent, float radius, float width, float height, float angle, string interactionLayerName, LayerMask interactionMask)
    {
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        gameObject.layer = LayerMask.NameToLayer(interactionLayerName);

        BoxCollider collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.center = new Vector3(0, 0, 0.25f * radius);
        collider.size = new Vector3(0.5f * width, height, 0.5f * radius);
        m_InteractionMask = interactionMask;

        transform.localRotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer & m_InteractionMask) > 0)
        {
            TriggeredEvent?.Invoke();
        }
    }
}
