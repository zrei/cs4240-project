using UnityEditor;
using UnityEngine;

public class SegmentedCircularPlaneCollider : MonoBehaviour
{
    public VoidEvent OnTriggerEnterEvent;
    public VoidEvent OnTriggerExitEvent;

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

        EditorUtility.SetDirty(this);
    }
#endif

    public void Init(LayerMask interactionMask)
    {
        m_InteractionMask = interactionMask;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Triggered by: {other.gameObject.name}");
        if (((1 << other.gameObject.layer) & m_InteractionMask) != 0)
        {
            OnTriggerEnterEvent?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"Triggered by: {other.gameObject.name}");
        if (((1 << other.gameObject.layer) & m_InteractionMask) != 0)
        {
            OnTriggerExitEvent?.Invoke();
        }
    }

    public void enable()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;
    }
}
