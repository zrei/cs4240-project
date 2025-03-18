using UnityEngine;

public class ReturnToPosition : MonoBehaviour
{
    [Header("Height")]
    [SerializeField] private bool m_UseMinimumHeight = false;
    [Tooltip("Whether to calculate the height of the transform relative to its position when this script was enabled")]
    [SerializeField] private bool m_CalculateHeightRelativeToOriginalPosition = false;
    [SerializeField] private float m_MinimumHeight = -10f;

    [Header("Trigger")]
    [SerializeField] private bool m_UseTrigger = false;
    [SerializeField] private LayerMask m_AllowedLayers;

    private Vector3 m_InitialWorldPosition;

    public VoidEvent OnReposition;

    private void OnEnable()
    {
        m_InitialWorldPosition = transform.position;    
    }

    private void Update()
    {
        if (m_UseMinimumHeight)
        {
            if (CalculateHeight() < m_MinimumHeight)
                Reposition();
        }
    }

    private float CalculateHeight()
    {
        return m_CalculateHeightRelativeToOriginalPosition ? transform.position.y - m_InitialWorldPosition.y : transform.position.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & m_AllowedLayers) != 0)
        {
            Reposition();
        }
    }

    private void Reposition()
    {
        transform.position = m_InitialWorldPosition;
        OnReposition?.Invoke();
    }
}
